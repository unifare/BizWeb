using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UniOrm
{
    public abstract class PrivateReflectionDynamicObjectBase : DynamicObject
    {
#if NET45
        private static readonly Type[] _emptyTypes = new Type[0];
#endif

        // We need to virtualize this so we use a different cache for instance and static props
        protected abstract IDictionary<Type, IDictionary<string, IProperty>> PropertiesOnType { get; }

        protected abstract Type TargetType { get; }

        public abstract object Instance { get; }

        protected abstract BindingFlags BindingFlags { get; }

        public abstract object RealObject { get; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            IProperty prop = GetProperty(binder.Name);

            // Get the property value
            result = prop.GetValue(Instance, index: null);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = result.AsDynamic();

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            IProperty prop = GetProperty(binder.Name);

            // Set the property value.  Make sure to unwrap it first if it's one of our dynamic objects
            prop.SetValue(Instance, Unwrap(value), index: null);

            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            IProperty prop = GetIndexProperty();
            result = prop.GetValue(Instance, indexes);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = result.AsDynamic();

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            IProperty prop = GetIndexProperty();
            prop.SetValue(Instance, Unwrap(value), indexes);

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            for (int i = 0; i < args.Length; i++)
            {
                args[i] = Unwrap(args[i]);
            }

            var typeArgs = GetGenericMethodArguments(binder);

            result = InvokeMethodOnType(TargetType, Instance, binder.Name, args, typeArgs);

            // Wrap the sub object if necessary. This allows nested anonymous objects to work.
            result = result.AsDynamic();

            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            result = binder.Type.GetTypeInfo().IsInstanceOfType(RealObject) ? RealObject : Convert.ChangeType(RealObject, binder.Type);

            return true;
        }

        public override string ToString()
        {
            Debug.Assert(Instance != null);

            return Instance.ToString();
        }

        private IProperty GetIndexProperty()
        {
            // The index property is always named "Item" in C#
            return GetProperty("Item");
        }

        private IProperty GetProperty(string propertyName)
        {
            // Get the list of properties and fields for this type
            IDictionary<string, IProperty> typeProperties = GetTypeProperties(TargetType);

            // Look for the one we want
            if (typeProperties.TryGetValue(propertyName, out IProperty property))
                return property;

            // The property doesn't exist

            // Get a list of supported properties and fields and show them as part of the exception message
            // For fields, skip the auto property backing fields (which name start with <)
            var propNames = typeProperties.Keys.Where(name => name[0] != '<').OrderBy(name => name);

            throw new MissingMemberException(
                $"The property {propertyName} doesn\'t exist on type {TargetType}. Supported properties are: {string.Join(", ", propNames)}");
        }

        private IDictionary<string, IProperty> GetTypeProperties(Type type)
        {
            // First, check if we already have it cached
            if (PropertiesOnType.TryGetValue(type, out IDictionary<string, IProperty> typeProperties))
                return typeProperties;

            // Not cached, so we need to build it
            typeProperties = new Dictionary<string, IProperty>();

            // First, recurse on the base class to add its fields
            if (type.GetTypeInfo().BaseType != null)
            {
                foreach (IProperty prop in GetTypeProperties(type.GetTypeInfo().BaseType).Values)
                {
                    typeProperties[prop.Name] = prop;
                }
            }

            // Then, add all the properties from the current type
            foreach (PropertyInfo prop in type.GetTypeInfo().GetProperties(BindingFlags))
            {
                if (prop.DeclaringType == type)
                {
                    typeProperties[prop.Name] = new Property(prop);
                }
            }

            // Finally, add all the fields from the current type
            foreach (FieldInfo field in type.GetTypeInfo().GetFields(BindingFlags))
            {
                if (field.DeclaringType == type)
                {
                    typeProperties[field.Name] = new Field(field);
                }
            }

            // Cache it for next time
            PropertiesOnType[type] = typeProperties;

            return typeProperties;
        }

        private static bool ParametersCompatible(MethodInfo method, ref object[] passedArguments, bool isExtense  )
        {
            Debug.Assert(method != null);
            Debug.Assert(passedArguments != null);
            List<object> newargs = new List<object>();
            var parametersOnMethod = method.GetParameters();
            var sartindex = 0;
            if (isExtense)
            {
                sartindex = 1;
                //if (parametersOnMethod.Length-1 != passedArguments.Length)
                //{
                //    return false;
                //}
            }


            for (int i = sartindex; i < parametersOnMethod.Length; ++i)
            {
                bool isMisstype = false;
                var parameterType = parametersOnMethod[i].ParameterType.GetTypeInfo();

                if (isExtense)
                {
                    if (i >= passedArguments.Length)
                    {
                        if (parametersOnMethod[i].HasDefaultValue)
                        { 
                            newargs.Add(Type.Missing);
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        ref var argument = ref passedArguments[i - 1];

                        if (argument == null && parameterType.IsValueType)
                        {
                            // Value types can not be null.
                            return false;
                        }

                        if (!parameterType.IsInstanceOfType(argument))
                        {
                            // Parameters should be instance of the parameter type.
                            if (parameterType.IsByRef)
                            {
                                var typePassedByRef = parameterType.GetElementType().GetTypeInfo();

                                Debug.Assert(typePassedByRef != null);

                                if (typePassedByRef.IsValueType && argument == null)
                                {
                                    return false;
                                }

                                if (argument != null)
                                {
                                    var argumentType = argument.GetType().GetTypeInfo();
                                    var argumentByRefType = argumentType.MakeByRefType().GetTypeInfo();
                                    if (parameterType != argumentByRefType)
                                    {
                                        try
                                        {
                                            argument = Convert.ChangeType(argument, typePassedByRef.AsType());
                                        }
                                        catch (InvalidCastException)
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            else if (argument == null)
                            {

                            }
                            else
                            {
                                if (parametersOnMethod[i].HasDefaultValue)
                                {
                                    if (i == parametersOnMethod.Length - 1)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        isMisstype = true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        if (isMisstype)
                        {
                            newargs.Add(Type.Missing);
                        }
                        else
                        {
                            newargs.Add(argument);
                        }
                    }
                }
                else
                {
                    if (i >= passedArguments.Length)
                    {
                        if (parametersOnMethod[i].HasDefaultValue)
                        { 
                            newargs.Add(Type.Missing);
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        ref var argument = ref passedArguments[i];

                        if (argument == null && parameterType.IsValueType)
                        {
                            // Value types can not be null.
                            return false;
                        }

                        if (!parameterType.IsInstanceOfType(argument))
                        {
                            // Parameters should be instance of the parameter type.
                            if (parameterType.IsByRef)
                            {
                                var typePassedByRef = parameterType.GetElementType().GetTypeInfo();

                                Debug.Assert(typePassedByRef != null);

                                if (typePassedByRef.IsValueType && argument == null)
                                {
                                    return false;
                                }

                                if (argument != null)
                                {
                                    var argumentType = argument.GetType().GetTypeInfo();
                                    var argumentByRefType = argumentType.MakeByRefType().GetTypeInfo();
                                    if (parameterType != argumentByRefType)
                                    {
                                        try
                                        {
                                            argument = Convert.ChangeType(argument, typePassedByRef.AsType());
                                        }
                                        catch (InvalidCastException)
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            else if (argument == null)
                            {
                            }
                            else
                            {
                                if (parametersOnMethod[i].HasDefaultValue)
                                {
                                    if (i == parametersOnMethod.Length - 1)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        isMisstype = true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }

                        }

                        if (isMisstype)
                        {
                            newargs.Add(Type.Missing);
                        }
                        else
                        {
                            newargs.Add(argument);
                        }
                    }
                }

            }
            passedArguments = newargs.ToArray();
            return true;
        }

        private static object InvokeMethodOnType(Type type, object target, string name, object[] args, Type[] typeArgs)
        {
            Debug.Assert(type != null);
            Debug.Assert(args != null);
            Debug.Assert(typeArgs != null);

            const BindingFlags allMethods =
                BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Instance | BindingFlags.Static;

            MethodInfo method = null;
            Type currentType = type;

            while (method == null && currentType != null)
            {
                var methods = currentType.GetTypeInfo().GetMethods(allMethods);

                MethodInfo candidate;
                for (int i = 0; i < methods.Length; ++i)
                {
                    candidate = methods[i];

                    if (candidate.Name == name)
                    {
                        // Check if the method is called as a generic method.
                        if (typeArgs.Length > 0 && candidate.ContainsGenericParameters)
                        {
                            var candidateTypeArgs = candidate.GetGenericArguments();
                            if (candidateTypeArgs.Length == typeArgs.Length)
                            {
                                candidate = candidate.MakeGenericMethod(typeArgs);
                            }
                        }

                        if (ParametersCompatible(candidate, ref args,false))
                        {
                            method = candidate;
                            break;
                        }
                    }
                }
                if (method == null)
                {
                    methods = currentType.GetExtensionMethods();
                    for (int i = 0; i < methods.Length; ++i)
                    {
                        candidate = methods[i];

                        if (candidate.Name == name)
                        {
                            // Check if the method is called as a generic method.
                            if (typeArgs.Length > 0 && candidate.ContainsGenericParameters)
                            {
                                var candidateTypeArgs = candidate.GetGenericArguments();
                                if (candidateTypeArgs.Length == typeArgs.Length)
                                {
                                    candidate = candidate.MakeGenericMethod(typeArgs);
                                }
                            }

                            if (ParametersCompatible(candidate, ref args,true))
                            {
                                method = candidate;
                                break;
                            }
                        }
                    }
                }
                if (method == null)
                {
                    // Move up in the type hierarchy.
                    currentType = currentType.GetTypeInfo().BaseType;
                }
            }
         

            if (method == null)
            {
               
                throw new MissingMethodException($"Method with name '{name}' not found on type '{type.FullName}'.");
            }
            if (method.CustomAttributes.Any(p => p.AttributeType == typeof(System.Runtime.CompilerServices.ExtensionAttribute)))
            {
                var newargs = new List<object>();
                newargs.Add(target);
                newargs.AddRange(args);
                return method.Invoke(null, newargs.ToArray());
            }
            else
            {
                return method.Invoke(target, args);
            }
           
        }
        
        private static object Unwrap(object o)
        {
            // If it's a wrap object, unwrap it and return the real thing
            if (o is PrivateReflectionDynamicObjectBase wrappedObj)
                return wrappedObj.RealObject;

            // Otherwise, return it unchanged
            return o;
        }

        private static Type[] GetGenericMethodArguments(InvokeMemberBinder binder)
        {
            var csharpInvokeMemberBinderType = binder
                    .GetType().GetTypeInfo()
                    .GetInterface("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder")
                    .GetTypeInfo();

            var typeArgsList = (IList<Type>)csharpInvokeMemberBinderType.GetProperty("TypeArguments").GetValue(binder, null);

            Type[] typeArgs;
            if (typeArgsList.Count == 0)
            {
#if NET45
                typeArgs = _emptyTypes;
#else
                typeArgs = Array.Empty<Type>();
#endif
            }
            else
            {
                typeArgs = typeArgsList.ToArray();
            }

            return typeArgs;
        }
    }


    public static class TypeExtension
    {
        /// <summary>
        /// This Methode extends the System.Type-type to get all extended methods. It searches hereby in all assemblies which are known by the current AppDomain.
        /// </summary>
        /// <remarks>
        /// Insired by Jon Skeet from his answer on http://stackoverflow.com/questions/299515/c-sharp-reflection-to-identify-extension-methods
        /// </remarks>
        /// <returns>returns MethodInfo[] with the extended Method</returns>

        public static MethodInfo[] GetExtensionMethods(this Type t)
        {
            List<Type> AssTypes = new List<Type>();

            foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssTypes.AddRange(item.GetTypes());
            }

            var query = from type in AssTypes
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where method.GetParameters()[0].ParameterType == t
                        select method;
            return query.ToArray<MethodInfo>();
        }

        /// <summary>
        /// Extends the System.Type-type to search for a given extended MethodeName.
        /// </summary>
        /// <param name="MethodeName">Name of the Methode</param>
        /// <returns>the found Methode or null</returns>
        public static MethodInfo GetExtensionMethod(this Type t, string MethodeName)
        {
            var mi = from methode in t.GetExtensionMethods()
                     where methode.Name == MethodeName
                     select methode;
            if (mi.Count<MethodInfo>() <= 0)
                return null;
            else
                return mi.First<MethodInfo>();
        }
    }
}

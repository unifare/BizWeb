using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace UniOrm.Model
{
    public class TypeDefinition
    {
        public string VersionNum { get; set; }
        public long Id { get; set; }
        public string ClassName { get; set; }
        public string AliName { get; set; }

        //
        // Summary:
        //     Gets a combination of System.Reflection.GenericParameterAttributes flags that
        //     describe the covariance and special constraints of the current generic type parameter.
        //
        // Returns:
        //     A bitwise combination of System.Reflection.GenericParameterAttributes values
        //     that describes the covariance and special constraints of the current generic
        //     type parameter.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The current System.Type object is not a generic type parameter. That is, the
        //     System.Type.IsGenericParameter property returns false.
        //
        //   T:System.NotSupportedException:
        //     The invoked method is not supported in the base class.
        public virtual GenericParameterAttributes GenericParameterAttributes { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the System.Type can be accessed by code outside
        //     the assembly.
        //
        // Returns:
        //     true if the current System.Type is a public type or a public nested type such
        //     that all the enclosing types are public; otherwise, false.
        public bool IsVisible { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the System.Type is not declared public.
        //
        // Returns:
        //     true if the System.Type is not declared public and is not a nested type; otherwise,
        //     false.
        public bool IsNotPublic { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the System.Type is declared public.
        //
        // Returns:
        //     true if the System.Type is declared public and is not a nested type; otherwise,
        //     false.
        public bool IsPublic { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether a class is nested and declared public.
        //
        // Returns:
        //     true if the class is nested and declared public; otherwise, false.
       
        //
        // Summary:
        //     Gets the attributes associated with the System.Type.
        //
        // Returns:
        //     A System.Reflection.TypeAttributes object representing the attribute set of the
        //     System.Type, unless the System.Type represents a generic type parameter, in which
        //     case the value is unspecified.
        public TypeAttributes Attributes { get; set; }
 
        //
        // Summary:
        //     Gets a value indicating whether the System.Type is nested and visible only to
        //     classes that belong to either its own family or to its own assembly.
        //
        // Returns:
        //     true if the System.Type is nested and visible only to classes that belong to
        //     its own family or to its own assembly; otherwise, false.
        public bool IsNestedFamORAssem { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the fields of the current type are laid out automatically
        //     by the common language runtime.
        //
        // Returns:
        //     true if the System.Type.Attributes property of the current type includes System.Reflection.TypeAttributes.AutoLayout;
        //     otherwise, false.
        public bool IsAutoLayout { get; set; }
         
        //
        // Summary:
        //     Gets the namespace of the System.Type.
        //
        // Returns:
        //     The namespace of the System.Type; null if the current instance has no namespace
        //     or represents a generic parameter.
        public   string Namespace { get; set; }
       
        //
        // Summary:
        //     Gets the assembly-qualified name of the type, which includes the name of the
        //     assembly from which this System.Type object was loaded.
        //
        // Returns:
        //     The assembly-qualified name of the System.Type, which includes the name of the
        //     assembly from which the System.Type was loaded, or null if the current instance
        //     represents a generic type parameter.
        public   string AssemblyQualifiedName { get; set; }
  
        public   string FullName { get; set; }
 
        
        public   Guid GUID { get; set; }
 
        public bool IsValueType { get; set; }
    
        public bool IsInterface { get; set; }
      
     
        public virtual bool IsGenericType { get; set; }
 
        public bool IsArray { get; set; }
      
        
         
        public bool IsSpecialName { get; set; }
      
        public virtual bool IsEnum { get; set; }
      
        public bool IsSealed { get; set; }
       
        public bool IsAbstract { get; set; }
      
        public bool IsClass { get; set; }


        public DateTime? AddTime { get; set; }

    }
}

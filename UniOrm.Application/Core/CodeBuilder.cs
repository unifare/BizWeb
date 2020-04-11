using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection; 
using System.Linq;
using Fasterflect;

namespace UniOrm.Core
{
    public enum StatmentType
    {
        Bool,
        Retun,
        Void,
        If,
        Else,

        For,
        Foreach,
        Swith,
        Case
    }
    public class CodeStatment
    {
        public StatmentType StatmentType { get; set; }
        public MethodInfo Methed { get; set; }
        public string MethedName { get; set; }
        public List<object> Paremters { get; set; }
        public string TypeName { get; set; }
        public object Instance { get; set; }
        public object Result { get; set; }
        public virtual object Run()
        {
            if (Instance == null)
            {
                var t = CodeBuilder.GetTypeFromCache(TypeName);
                if (t == null)
                {
                    //fromsql
                }
                return t.CallMethod(MethedName, Paremters);
            }
            else
            {
                return Instance.CallMethod(MethedName, Paremters);
            }
        }
    }
    public class IfStatment : CodeStatment
    {
        public IfStatment()
        {
            StatmentType = StatmentType.If;
        }
        public List<CodeStatment> TrueStatments { get; set; }
        public List<CodeStatment> Else { get; set; }
        public CodeStatment BoolStatement { get; set; }
    }
    public class ForStatment : CodeStatment
    {
        public ForStatment()
        {
            StatmentType = StatmentType.For;
        }
        public CodeStatment InitStatment { get; set; }
        public int Length { get; set; }
        public List<CodeStatment> AfterBody { get; set; }
        public List<CodeStatment> RunBody { get; set; }
    }
    public class SwitchStatment : CodeStatment
    {
        public SwitchStatment()
        {
            StatmentType = StatmentType.Swith;
        }
        public object Tocken { get; set; }

        public Dictionary<string, List<CodeStatment>> Body { get; set; }

    }
    public class CodeSection
    {
        public List<CodeStatment> CodeStatments { get; set; }
    }
    public class FunctionCode
    {
        public Dictionary<string, object> LocalKeyValues { get; set; }
        public List<object> InParamters { get; set; }
        public List<CodeSection> CodeSections { get; set; }
        public object RetrunObject { get; set; }
    }

    public static class CodeBuilder
    {
        public static Type GetTypeFromCache(string TypeName)
        {
            if(APP.Types.ContainsKey(  TypeName))
            {
              return  APP.Types[TypeName];
            }
            return null;
        }

        public static object RunCodeStatment(string TypeName, object Instance, string methodName, params object[] objects)
        {

            if (Instance == null)
            {
                var t = GetTypeFromCache(TypeName);
                if (t == null)
                {
                    //fromsql
                }
                return t.CallMethod(methodName, objects);
            }
            else
            {
                return Instance.CallMethod(methodName, objects);
            }

        }
        public static CodeSection StartCodeStatment(StatmentType statmentType, string TypeName, object Instance, string methodName, params object[] objects)
        {
            var codeSection = new CodeSection();
            var paralist = new List<object>();
            if (objects != null)
            {
                paralist = objects.ToList();
            }
            var CodeStatment = new CodeStatment()
            {
                Instance = Instance,
                MethedName = methodName,
                Paremters = paralist,
                StatmentType = statmentType
            };
            codeSection.CodeStatments.Add(CodeStatment);
            return codeSection;
        }

        public static CodeSection StartCodeStatment(this CodeSection codeSection, StatmentType statmentType, string TypeName, object Instance, string methodName, params object[] objects)
        {
            var paralist = new List<object>();
            if (objects != null)
            {
                paralist = objects.ToList();
            }
            var CodeStatment = new CodeStatment()
            {
                Instance = Instance,
                MethedName = methodName,
                Paremters = paralist,
                StatmentType = statmentType
            };
            codeSection.CodeStatments.Add(CodeStatment);
            return codeSection;
        }
    }
}

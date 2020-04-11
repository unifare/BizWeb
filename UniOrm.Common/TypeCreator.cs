/*
 * ************************************
 * file:	    TypeCreator.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/4 17:12:30
 * description:	
 * ************************************
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace UniOrm
{
    public class TypeCreator
    {
        public StringBuilder Script = new StringBuilder();

        public string ClassName { get; set; } = string.Empty;
        public List<string> NameSpaces { get; set; } = new List<string>();
        public List<string> Methods { get; set; } = new List<string>();
        public Dictionary<string, TypeSection> Properity { get; set; } = new Dictionary<string, TypeSection>();
        public static string   DBRunReferenceTemplate= @"using System; 
                             public class {0}
        {
           {1} 
           
        }
            ";
        public static TypeCreator NewClassBulder(string name)
        {
            var typ = new TypeCreator();
            typ.ClassName = name;
            return typ;
        }
        
        public string ToCodeString()
        {
            var pbuiler = new StringBuilder();
            foreach(var p in Properity)
            {
                var nullSynex = "";
                if (p.Value.isNull)
                {
                    if (p.Value.ObjType == typeof(int)
                        || p.Value.ObjType == typeof(decimal)
                        || p.Value.ObjType == typeof(double)
                        || p.Value.ObjType == typeof(float)
                        || p.Value.ObjType == typeof(bool)
                        || p.Value.ObjType == typeof(DateTime)
                        )
                    {
                        nullSynex = "?";
                    } 
                }
                var   typename = "";
                if(p.Value.ObjType== typeof(int))
                {
                    typename = " int ";
                }
                else if (p.Value.ObjType == typeof(System.Int64))
                {
                    typename = " long ";
                }
                else if (p.Value.ObjType == typeof(string))
                {
                    typename = " string ";
                }
                else if (p.Value.ObjType == typeof(decimal))
                {
                    typename = " decimal ";
                }

                else if (p.Value.ObjType == typeof(double))
                {
                    typename = " double ";
                }
                else if (p.Value.ObjType == typeof(float))
                {
                    typename = " float ";
                }
                else if (p.Value.ObjType == typeof(bool))
                {
                    typename = " bool ";
                }
                else if (p.Value.ObjType == typeof(DateTime))
                {
                    typename = " DateTime ";
                }
                pbuiler.Append("public " + typename + nullSynex+" " + p.Value.TypeName +" {get;set;}");
                pbuiler.AppendLine(" ");
                //if (p.Value.Defaultvalue != null)
                //{
                //    if(p.Value.ObjType==typeof(DateTime))
                //    {
                //         pbuiler.Append(" = Convert.ToDateTime(\"" +   p.Value.Defaultvalue.ToString()+"\"); \r\n");

                //    }
                //    else if(p.Value.ObjType == typeof(string))
                //    {
                //        pbuiler.Append(" = \"" + p.Value.Defaultvalue.ToString() + "\";  \r\n");
                //    }
                //    else
                //    {
                //        pbuiler.Append(" = " + p.Value.Defaultvalue.ToString().ToLower() + ";  \r\n");
                //    }
                //}
            }
           var outstring= @"   using System;
                 public class " + ClassName + @"
            {
            " + pbuiler.ToString() + @"
            }";
            return outstring;
        }
        public override string ToString()
        {
            return ToCodeString();
        }
        public TypeCreator AddProperityName(string name, Type type, bool isnull, object Defaultvalue)
        {
            if (!Properity.ContainsKey(name))
            {
                Properity.Add(name, new TypeSection() { isNull=isnull, Defaultvalue = Defaultvalue, TypeName = name, ObjType = type });
            }
            return this;
        }


        public TypeCreator AddNameSpace(string name )
        {
            if (!NameSpaces.Contains(name))
            {
                NameSpaces.Add(name);
            }
            return this;
        }

    }

    public class TypeSection
    {
        public string TypeName { get; set; } = string.Empty;
        public Type ObjType { get; set; }
        public object Defaultvalue { get; set; }
        public bool isNull { get; set; } 

    }
} 

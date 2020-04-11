using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection; 

namespace UniOrm.Model.DataService
{
    public class AssemblyInjection : IAssemblyInjection
    {
        ISysDatabaseService Dbclient;
        public AssemblyInjection(ISysDatabaseService dbclient)
        {
            Dbclient = dbclient;
        }


        public void ResgiterAllDll(string[] filepaths)
        {
            foreach (var f in filepaths)
            {
                Assembly asm = Assembly.LoadFrom(f);
                var fullname = asm.FullName;
                var verunmer = asm.GetName().Version.ToString();

                var oldasscon = Dbclient.GetSimpleCode<AssemblyACon>(new { Name = asm.FullName }).FirstOrDefault();
                if (oldasscon == null)
                {
                    oldasscon = new AssemblyACon();
                    oldasscon.AddTime = DateTime.Now;
                    AssemblyDescriptionAttribute asmdis = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyDescriptionAttribute));
                    AssemblyCopyrightAttribute asmcpr = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyCopyrightAttribute));
                    AssemblyCompanyAttribute asmcpn = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyCompanyAttribute));
                    string s = string.Format("Description:{0} Copyright:{1} Company:{2} ", asmdis != null ? asmdis.Description : string.Empty, asmcpr != null ? asmcpr.Copyright : string.Empty, asmcpn != null ? asmcpn.Company : string.Empty);
                    oldasscon.Description = s;
                    oldasscon.DllPath = f;
                    oldasscon.IsVirtual = false;
                    oldasscon.Name = asm.FullName;
                    oldasscon.NameSpace = asm.FullName;
                    oldasscon.VersionNum = verunmer;
                    Dbclient.InsertCode(oldasscon);
                }
                else
                {
                    oldasscon.AddTime = DateTime.Now;
                    AssemblyDescriptionAttribute asmdis = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyDescriptionAttribute));
                    AssemblyCopyrightAttribute asmcpr = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyCopyrightAttribute));
                    AssemblyCompanyAttribute asmcpn = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(asm, typeof(AssemblyCompanyAttribute));
                    string s = string.Format("Description:{0} Copyright:{1} Company:{2} ", asmdis != null ? asmdis.Description : string.Empty, asmcpr != null ? asmcpr.Copyright : string.Empty, asmcpn != null ? asmcpn.Company : string.Empty);
                    oldasscon.Description = s;
                    oldasscon.DllPath = f;
                    oldasscon.IsVirtual = false;
                    oldasscon.NameSpace = asm.FullName;
                    oldasscon.VersionNum = verunmer;
                    Dbclient.UpdateSimpleCode(oldasscon);
                }
                var alltypes = asm.GetTypes().Where(p => p.IsAbstract == false
                                && p.IsPublic == true
                                //&& p.Name != "ToString" && p.Name != "GetType"
                                && !p.Name.StartsWith("AspNetCore"));
                foreach (var t in alltypes)
                {
                    var oldtype = Dbclient.GetSimpleCode<TypeDefinition>(new { FullName = t.FullName }).FirstOrDefault();
                    if (oldtype == null)
                    {
                        oldtype = new TypeDefinition();
                        oldtype.AddTime = DateTime.Now;
                        oldtype.ClassName = t.Name;
                        oldtype.FullName = t.FullName;
                        oldtype.GUID = t.GUID;
                        oldtype.IsAbstract = t.IsAbstract;
                        oldtype.IsArray = t.IsArray;
                        oldtype.IsAutoLayout = t.IsAutoLayout;
                        oldtype.IsClass = t.IsClass;
                        oldtype.IsEnum = t.IsEnum;
                        oldtype.IsGenericType = t.IsGenericType;
                        oldtype.IsInterface = t.IsInterface;
                        oldtype.IsNestedFamORAssem = t.IsNestedFamORAssem;
                        oldtype.IsNotPublic = t.IsNotPublic;
                        oldtype.IsPublic = t.IsPublic;
                        oldtype.IsSealed = t.IsSealed;
                        oldtype.IsSpecialName = t.IsSpecialName;
                        oldtype.IsValueType = t.IsValueType;
                        oldtype.IsVisible = t.IsVisible;
                        oldtype.Namespace = t.Namespace;
                        oldtype.VersionNum = verunmer;
                        oldtype.AliName = t.FullName;
                        var reInt = Dbclient.InsertCode(oldtype);

                        oldtype.Id = reInt;
                    }
                    else
                    {
                        oldtype.AddTime = DateTime.Now;
                        oldtype.VersionNum = verunmer;
                        oldtype.ClassName = t.Name;
                        oldtype.GUID = t.GUID;
                        oldtype.IsAbstract = t.IsAbstract;
                        oldtype.IsArray = t.IsArray;
                        oldtype.IsAutoLayout = t.IsAutoLayout;
                        oldtype.IsClass = t.IsClass;
                        oldtype.IsEnum = t.IsEnum;
                        oldtype.IsGenericType = t.IsGenericType;
                        oldtype.IsInterface = t.IsInterface;
                        oldtype.IsNestedFamORAssem = t.IsNestedFamORAssem;
                        oldtype.IsNotPublic = t.IsNotPublic;
                        oldtype.IsPublic = t.IsPublic;
                        oldtype.IsSealed = t.IsSealed;
                        oldtype.IsSpecialName = t.IsSpecialName;
                        oldtype.IsValueType = t.IsValueType;
                        oldtype.IsVisible = t.IsVisible;
                        oldtype.Namespace = t.Namespace;
                        oldtype.AliName = t.FullName;
                        Dbclient.UpdateSimpleCode(oldasscon);
                    }

                    var methodinfos = t.GetMethods().Where(p => p.IsAbstract == false && p.IsVirtual == false
                                && p.MemberType == MemberTypes.Method && p.IsPublic == true
                                && p.Name != "ToString" && p.Name != "GetType"
                                && !p.Name.StartsWith("get")
                                && !p.Name.StartsWith("set"));
                    foreach (var m in methodinfos)
                    {
                        string parameterInfo, retuentype, mfullname;
                        NewMethod(t, m, out parameterInfo, out retuentype, out mfullname);
                        var oldmethod = Dbclient.GetSimpleCode<MethodDefinition>(new { FullName = mfullname }).FirstOrDefault();
                        if (oldmethod == null)
                        {
                            oldmethod = new MethodDefinition();
                            oldmethod.AddTime = DateTime.Now;
                            oldmethod.BelongTypeId = oldtype.Id;
                            oldmethod.FullName = mfullname;
                            oldmethod.IsPrivate = m.IsPrivate;
                            oldmethod.IsPublic = m.IsPublic;
                            oldmethod.IsStatic = m.IsStatic;
                            oldmethod.MemberType = m.MemberType;
                            oldmethod.Name = m.Name;
                            oldmethod.ParameterInfo = parameterInfo;
                            oldmethod.ReturnType = retuentype;
                            oldmethod.VersionNum = verunmer;
                            Dbclient.InsertCode(oldmethod);
                        }
                        else
                        {
                            oldmethod.AddTime = DateTime.Now;
                            oldmethod.BelongTypeId = oldtype.Id;
                            oldmethod.FullName = mfullname;
                            oldmethod.IsPrivate = m.IsPrivate;
                            oldmethod.IsPublic = m.IsPublic;
                            oldmethod.IsStatic = m.IsStatic;
                            oldmethod.MemberType = m.MemberType;
                            oldmethod.Name = m.Name;
                            oldmethod.ParameterInfo = parameterInfo;
                            oldmethod.ReturnType = retuentype;
                            oldmethod.VersionNum = verunmer;
                            Dbclient.UpdateSimpleCode(oldmethod);
                        }
                    }

                }

            }
        }

        private static void NewMethod(Type t, MethodInfo m, out string parameterInfo, out string retuentype, out string mfullname)
        {
            var allparas = m.GetParameters();
            parameterInfo = string.Empty;
            retuentype = m.ReturnType.FullName;
            StringBuilder sb = new StringBuilder();
            if (allparas != null && allparas.Length > 0)
            {
                foreach (var pa in allparas)
                {
                    sb.Append(string.Format("[{0} {1}],", pa.ParameterType, pa.Name));
                }
                parameterInfo = sb.ToString().TrimEnd(',');
            }

            mfullname = string.Concat(retuentype, " ", t.FullName, ".", m.Name, string.Format("({0})", parameterInfo));
        }

        public static string GetMethodFullName(MethodInfo m)
        {
            string mfullname = string.Empty;
            var allparas = m.GetParameters();
            var parameterInfo = string.Empty;
            var retuentype = m.ReturnType.FullName;
            StringBuilder sb = new StringBuilder();
            if (allparas != null && allparas.Length > 0)
            {
                foreach (var pa in allparas)
                {
                    sb.Append(string.Format("[{0} {1}],", pa.ParameterType, pa.Name));
                }
                parameterInfo = sb.ToString().TrimEnd(',');
            }

            mfullname = string.Concat(retuentype, " ", m.ReflectedType.FullName, ".", m.Name, string.Format("({0})", parameterInfo));
            return mfullname;
        }
    }
}

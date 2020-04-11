using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; 
using System.IO;
using System.Reflection; 

namespace UniOrm.Common
{
    public enum ModuleName
    {
        Other,
        Security,
        WebUtility,

    }

    public class DefaultModuleManager
    {
        public ModuleCollection RegistedModules { get; set; } = new ModuleCollection();

        public Guid Guid { get; set; }
        public DefaultModuleManager()
        {
            var dlls = GetAllPluginDlls(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var dll in dlls)
            {
                var ass = Assembly.LoadFrom(dll);
                var alltypes = ass.GetTypes().Where(p => p.IsSubclassOf(typeof(ModuleBase)));
                foreach (var item in alltypes)
                {
                    RegistedModules.Add((IModule)Activator.CreateInstance(item));
                }

            }
            Guid = new Guid();

        }

        /// <summary>
        /// 扫描后端
        /// </summary>
        /// <param name="filePath">bin目录</param>
        private static string[] GetAllPluginDlls(string dlldir)
        {

            return Directory.GetFiles(dlldir, "*Plugin.dll");
        }

        //public RequireItemCollection TotalRequireItems
        //{
        //    get
        //    {
        //        var cachemodel = APPCommon.RuntimeCache.GetOrCreate(Guid, entry =>
        //       {
        //           RequireItemCollection totalRequireItems = new RequireItemCollection();
        //           foreach (var m in RegistedModules)
        //           {
        //               foreach (var mc in m.RequireItems)
        //               {
        //                   if (totalRequireItems.FirstOrDefault(prop => prop.Name == mc.Name && prop.version == mc.version) != null)
        //                   {
        //                       totalRequireItems.Add(mc);
        //                   }

        //               }
        //           }
        //           APPCommon.RuntimeCache.Set(Guid, totalRequireItems);
        //           return totalRequireItems;
        //       });

        //        return cachemodel;
        //    }
        //}
         
        public IModule GetModule(ModuleName? name = null, string othername = null)
        {
            if (name == null)
            {
                return RegistedModules.FirstOrDefault(p => p.ModuleName == othername);
            }
            else
            {
                if (name != ModuleName.Other)
                {
                    return RegistedModules.FirstOrDefault(p => p.ModuleName == name.ToString());
                }
                else
                {
                    return RegistedModules.FirstOrDefault(p => p.ModuleName == othername);
                }
            }
            //loger
        }
        public IModule AddModule(IModule module)
        {
            RegistedModules.Add(module);
            return module;
        }
    }
}

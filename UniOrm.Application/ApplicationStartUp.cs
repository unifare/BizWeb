 
using Microsoft.Extensions.DependencyInjection; 
using System;
using System.Collections.Generic;
using System.Text;
using FlunentMigratorFactory;
using System.IO;
using System.Reflection; 
using Microsoft.Extensions.Caching.Memory;
using UniOrm.Core;
using UniOrm;
using UniOrm.Common; 
using UniOrm.DataMigrationiHistrory;
using SqlSugar;
using Autofac;

namespace UniOrm.Application
{
    public static class ApplicationStartUp
    {
        private static IServiceProvider autofacServiceProvider;


        static bool s_isInit = false;
        private static readonly string loggerName = "AConStateStartUp";

        static ApplicationStartUp()
        {

        }

        //internal static string sysconstring = "Data Source = ./config/aconstate.db";
        //internal static string codeormtype = "sqlsugar";
        /// <summary>
        /// 扫描后端
        /// </summary>
        /// <param name="filePath">bin目录</param>
        private static string[] GetAllPluginDlls(string dlldir)
        {

            return Directory.GetFiles(dlldir, "*Plugin.dll");
        }
   

        private static List<Type> ReadTypeFromConfig(List<RegestedModel> regestedModels)
        {
            var listtypedModels = new List<Type>();
            foreach (var m in regestedModels)
            {
                var assembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, m.Dll));
                foreach (var ty in m.TypeNames)
                {
                    var t = assembly.GetType(ty);
                    if (t == null)
                    {
                        Logger.LogWarn(loggerName, "InitAutofac -> Load Type {0} not in {1}", ty, m);
                        continue;
                    }
                    listtypedModels.Add(t);
                }

            }
            return listtypedModels;
        }
       
      
        public static IServiceProvider InitAutofac(this IServiceCollection services,   IEnumerable<Assembly> modulesAssembly)
        {
            if (s_isInit)
            {
                return autofacServiceProvider;
            }
            //services.AddAutofac(); 
            //APP.Resover.RegistAutofacModule();

            APP.RegisterAutofacModuleTypes();
            //APP.RegisterAutofacAssemblies(modulesAssembly);





            ///////////using /////////////////////
            using (var scope = APP.Resover.Resovertot.BeginLifetimeScope())
            {
                var memoryCache = scope.Resolve<IMemoryCache>();
                APP.RuntimeCache = new RuntimeCache(memoryCache);
                autofacServiceProvider = scope.Resolve<IServiceProvider>();
                //var systemResover = new AutofacResover() { Container = container };
                //APP.Builder.RegisterInstance<IResover>(systemResover); 
            }
           

            //  APP.Container = systemResover;
            return autofacServiceProvider;
        }
    }
}

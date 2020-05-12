using Fasterflect;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UniOrm;
using UniOrm.Common;
using Autofac;
using System.IO;
using System.Collections.Concurrent;
using UniOrm.Model;
using UniOrm.Core;
using UniOrm.Application;
using UniOrm.Model.DataService;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using RazorLight;
using RazorLight.Razor;
using UniOrm.Common.RazorPage;

namespace UniOrm
{
    public partial class APP
    {
        public static IConfiguration Configuration
        {
            get
            {
                return APPCommon.Configuration;
            }
            set
            {
                APPCommon.Configuration = value;
            }
        }

        public static ServiceProvider ApplicationServices
        {
            get
            {
                return APPCommon.ApplicationServices;
            }
            set
            {
                APPCommon.ApplicationServices = value;
            }
        }
        public static RuntimeCache RuntimeCache;
        public static ContainerBuilder Builder
        {
            get
            {
                return APPCommon.Builder;
            }
            set
            {
                APPCommon.Builder = value;
            }
        }

        public static IResover Container;
        public static Dictionary<string, Assembly> Dlls
        {
            get
            {
                return APPCommon.Dlls;
            }
            set
            {
                APPCommon.Dlls = value;
            }
        }
        public static List<string> DynamicReferenceDlls
        {
            get
            {
                return APPCommon.DynamicReferenceDlls;
            }
            set
            {
                APPCommon.DynamicReferenceDlls = value;
            }
        }
        public static Dictionary<string, RuntimeCode> RuntimeCodes = new Dictionary<string, RuntimeCode>();
        public static Dictionary<string, Type> Types
        {
            get
            {
                return APPCommon.Types;
            }
            set
            {
                APPCommon.Types = value;
            }
        }
        public static List<ComposeEntity> Composeentitys = new List<ComposeEntity>();
        public static List<ComposeTemplate> ComposeTemplates = new List<ComposeTemplate>();
        public static List<AConFlowStep> AConFlowSteps = new List<AConFlowStep>();
        public static DefaultModuleManager ModuleManager
        {
            get
            {
                return APPCommon.ModuleManager;
            }
            set
            {
                APPCommon.ModuleManager = value;
            }
        }
       public static RazorLightEngine Razorengine
        {
            get
            {
              
                return APPCommon.Razorengine;
            }
        }


        public static Dictionary<string, MethodInfo> MethodInfos
        {
            get
            {
                return APPCommon.MethodInfos;
            }
            set
            {
                APPCommon.MethodInfos = value;
            }
        }
        //public  List<RuntimeModel> RuntimeModels = new List<RuntimeModel>();
        public static ConcurrentDictionary<string, object> StepResults = new ConcurrentDictionary<string, object>();
        readonly static string logName = "ACon.Unver.Manager";
        static APP()
        {
            //var allModules = ModuleManager.RegistedModules;
            //foreach (var m in allModules)
            //{
            //    m.Init();
            //}
        }

        public static DefaultUser LoginDefaultUser(string username, string password)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                var strResult = BitConverter.ToString(result);
                password = strResult.Replace("-", "");

            }
            return Container.Resolve<ISysDatabaseService>().GetDefaultUser(username, password);
        }
      

        public static AdminUser LoginAdmin(string username, string password)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                var strResult = BitConverter.ToString(result);
                password = strResult.Replace("-", "");

            }
            return Container.Resolve<ISysDatabaseService>().GetAdminUser(username, password);
        }
        public static string GetDicstring(string key)
        {
            var item = APPCommon. AppConfig.SystemDictionaries.FirstOrDefault(p => p.KeyName == key);
            if (item != null)
            {
                return item.Value;
            }
            return string.Empty;
        }
        public static IGodWorker GetWorker()
        {
            return Container.Resolve<IGodWorker>();
        }



        public static void RegisterAutofacModule(Autofac.Module moudle)
        {

            APPCommon.RegisterAutofacModule(moudle);
        }

        public static void RegisterAutofacAssemblies(IEnumerable<Assembly> modulesAssembly)
        {
            APPCommon.RegisterAutofacAssemblies(modulesAssembly); 
        }
        public static void RegisterAutofacModuleTypes()
        {
            var allModules = ModuleManager.RegistedModules;
            foreach (var m in allModules)
            {
                m.RegisterAutofacTypes();
                var mms = m.GetAutofacModules();
                foreach (var item in mms)
                {
                    Builder.RegisterModule(item);
                }
            }
            //loger
        }


        public static void ClearCache()
        {

            RuntimeCache.Clear();
        }
        public static RuntimeCode FindOrAddRumtimeCode(string guid)
        {
            if (RuntimeCodes.ContainsKey(guid))
            {
                return RuntimeCodes[guid];
            }
            return null;
        }

        public static MethodInfo GetMethodFromConfig(bool IsPlugin, string libname, string typename, string methodName)
        {

            return APPCommon.GetMethodFromConfig(IsPlugin, libname, typename, methodName);
        }
        public static void ConfigureSiteAllModulesServices(IServiceCollection services)
        {
            var allModules = ModuleManager.RegistedModules;
            foreach (var m in allModules)
            {
                m.ConfigureSiteServices(services);
            }
        }

        public static void Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var allModules = ModuleManager.RegistedModules;
            foreach (var m in allModules)
            {
                m.Startup(configuration);
            }
        }


        public static void ConfigureSiteAllModules(IApplicationBuilder app)
        {
            var allModules = ModuleManager.RegistedModules;
            foreach (var m in allModules)
            {
                var mms = m.GetAutofacModules();
                foreach (var item in mms)
                {
                    Builder.RegisterModule(item);
                }
            }
        }


        public static object OpenDBSession(object dbFactory, string ormname, string connectionstring)
        {

            var adp = dbFactory.GetIndexer(ormname);
            //  .GetIndexer(ormname);

            if (!string.IsNullOrEmpty(connectionstring))
            {
                return adp.CallMethod("CreateDefaultInstance", connectionstring);
            }
            else
            {
                return adp.CallMethod("CreateDefaultInstance");
            } 
        }

    

        public static void InitDbMigrate()
        {
            var allModules = ModuleManager.RegistedModules;
            foreach (var m in allModules)
            {
                m.EnsureDaContext();
            }
        }

        public static object GetData( string ssql, object inParamters)
        {

            return APPCommon.GetData(  ssql, inParamters);

        }
        //public static object GetData<T>(  string ssql, params object[] inParamters)
        //{
        //    Type t = Type.GetType("AConState.DataService.ICodeService");
        //    Container.Resolve(t).CallMethod("GetSimpleCode", inParamters);
        //    //var product = CSScript.CreateFunc<int>(@"int Product(int a, int b)
        //    //                             {
        //    //                                 return a * b;
        //    //                             }");
        //    return null;
        //}
        public object Invoke<T>(params string[] commands)
        {
            var t = typeof(T);

            //t.
            return null;
        }

        public static void SetServiceProvider()
        {
            var allModules = ModuleManager.RegistedModules;
            foreach (var m in allModules)
            {
                m.SetServiceProvider(ApplicationServices);
            }
        }
    }
}

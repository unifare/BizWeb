using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using UniOrm.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniOrm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;

namespace UniOrm.Common
{
    public abstract class ModuleBase : IModule
    {
        private readonly string LoggerName = "ModuleBase";
        public virtual ContainerBuilder Builder
        {

            get
            {
                return APPCommon.Builder;
            }

        }
        public virtual IResover Container
        {

            get
            {
                return APPCommon.Container;
            }

        }

        public virtual string ModuleStaticPath { get; } = "wwwroot";
        public virtual string ModuleStaticName { get; } = "";
        public virtual Dictionary<string, string> OtherMapPath { get; }
        public virtual string Theme { get; }
        public static ServiceProvider ServiceProvider { get; set; }
        public static IConfiguration Configuration { get; set; }
        public virtual void Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public abstract void RegisterAutofacTypes();


        public virtual void EnsureDaContext()
        {
            //Init();
        }

        public abstract string DllPath { get; set; }
        public abstract string ModuleName { get; }
        public static Dictionary<string, Type> Types = new Dictionary<string, Type>();
        public RequireItemCollection RequireItems { get; set; }

        /// <summary>
        /// 扫描后端
        /// </summary>
        /// <param name="filePath">bin目录</param>
        private static string[] ScanBack(string dlldir)
        {
            return Directory.GetFiles(dlldir, "*.dll");
        }

        public virtual void SetServiceProvider(ServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public ModuleBase()
        {
            OtherMapPath = new Dictionary<string, string>();
            Types = new Dictionary<string, Type>();
            Init();
        }
        public AppConfig GetAppConfig()
        {
            return APPCommon.AppConfig;
        }

        public abstract List<Autofac.Module> GetAutofacModules();

        public virtual string GetDBTablePrefixConfig()
        {
            return APPCommon.AppConfig.UsingDBConfig.DefaultDbPrefixName;
        }

        //public string configPath = System.Reflection.Assembly.GetCallingAssembly().Location.ToString() + ".config";
        public Configuration MyConfiguration
        {
            get
            {
                var exname  =  this.GetType().Assembly.Location.ToString() + ".config";

               // var exname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ModuleName + ".dll.config");
                Logger.LogDebug(LoggerName, "load configFilePath is {0}", exname);
                if (!File.Exists(exname))
                {
                    return null;
                }
                else
                {
                    return ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap()
                    {
                        ExeConfigFilename = exname
                    }, ConfigurationUserLevel.None);
                }
            }
        }

        public string GetModuleConfig(string key)
        {
            return MyConfiguration.AppSettings.Settings[key] == null ? "" : MyConfiguration.AppSettings.Settings[key].Value;
        }

        public virtual bool Init()
        {
            SetModuleAppConfig();
            var configFileDir = GetModuleConfig("IsUsingAppDB").ToBool();
            MouduleDbConfig = ModuleAppConfig.UsingDBConfig;
            DB = new DB(ModuleAppConfig.UsingDBConfig);
            return true;
        }

        private void SetModuleAppConfig()
        {
            var configFileDir = GetModuleConfig("configFileDir");
            var configFilePath = GetModuleConfig("configFile");
            Logger.LogDebug(LoggerName, "configFilePath is {0}", configFilePath);
            if (!string.IsNullOrEmpty(configFileDir))
            {
                var phyDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileDir);
                if (!Directory.Exists(phyDir))
                {
                    Directory.CreateDirectory(phyDir);
                }

            }
            configFilePath = configFileDir + configFilePath;

            var configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFilePath);
            Logger.LogDebug(LoggerName, "configpath is {0}", configpath);


            var configroot = JToken.Parse(File.ReadAllText(configpath));
            ModuleAppConfig = JsonConvert.DeserializeObject<AppConfig>(configroot[ModuleName].ToString());
        }

        public abstract AppConfig ModuleAppConfig { get; set; }
        public virtual void ConfigureSite(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ModuleName)),
                RequestPath = ModuleName
            });
            //app.UseStaticFiles(new StaticFileOptions
            //{0\5'\
            //    FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output")),
            //    RequestPath = ""
            //});
        }

        public abstract List<Type> ModelType();

        public abstract List<string> ModelTypeStrings();

        public abstract DbConnectionConfig MouduleDbConfig { get; set; }

        public abstract List<Type> FunctionalTypes { get; set; }
        public virtual bool MigrateDB()
        {
            return true;
        }
        public DB DB { get; set; }

        public abstract void ConfigureRouter(IRouteBuilder routeBuilder);

        public abstract void ConfigureSiteServices(IServiceCollection services);

    }
}

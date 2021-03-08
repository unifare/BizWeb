using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection; 
using SqlSugar;
using Autofac;
namespace UniOrm.Common.Core
{
    public class CoreManager
    {
        public static IContainer Container { get; set; }
        public static ContainerBuilder builder { get; set; } = new ContainerBuilder();
        public static IConfiguration Config { get; set; }
        public static InterfaceInfo StartupInterface {get;set;}
        static CoreManager()
        {
            Config = new ConfigurationBuilder()
              .Add(new JsonConfigurationSource { Path = "/config/system_default.json", ReloadOnChange = true })
              .Build();
        }
        public static void ResgiteCore()
        {
            //Container.Options.DefaultScopedLifestyle= SetCurrentScope.
            builder.RegisterInstance<IFunction>( new BasicFunction() );
            builder.RegisterInstance<ISqlSugarClient>( DB.UniClient );
            ResgiteStartupCode();
            Container=builder.Build();
        }

        public static void ResgiteStartupCode()
        { 
            foreach(var i in SysAppConfig.Interfaces)
            {
                i.InterfaceType=Type.GetType(i.InterfaceTypeName);
                i.ImplementType = Type.GetType(i.ImplementTypeName);
            }
            StartupInterface = SysAppConfig.Interfaces.FirstOrDefault(p => p.Name == SysAppConfig.StarupKey);
            if (StartupInterface == null)
            {
                return;
            }
            //builder.RegisterInstance(StartupInterface.InterfaceType  ).As(StartupInterface.ImplementType);
        }


        private static AppConfigSystem _appConfig;
        public static AppConfigSystem SysAppConfig
        {
            get
            {
                if (_appConfig == null)
                { 
                    _appConfig = GetConfigObj<AppConfigSystem>( "App" );
                }
                return _appConfig;
            }
        }
        public static T Resolver<T>() where T:class
        {
           return Container.Resolve<T>( );
        }
        public static object Resolver (string TypeString)  
        {
            var type= Type.GetType(TypeString);
            return Container.Resolve(type);
        }
        public static T GetConfigValue<T>(string key) 
        {
           return Config.GetValue<T>(key); 
        }

        public static T GetConfigObj<T>(string key) where T : class, new()
        { 
            T appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(Config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;

            return appconfig;
        }
    }


}

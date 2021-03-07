 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniOrm.Common;

namespace UniOrm.Common
{
    public interface IModule
    { 
        Resover Resover { get; }
        string DllPath { get; set; }
        string ModuleName { get; }
        string ModuleStaticPath { get; } 
        string ModuleStaticName { get; } 
        Dictionary<string,string> OtherMapPath { get;   }

        string Theme { get; } 
        bool Init();
        AppConfig ModuleAppConfig { get; set; }
        void EnsureDaContext();
        List<Type> ModelType();
        void Startup(IConfiguration configuration);
        DB DB { get; set; }
        List<string> ModelTypeStrings();

        DbConnectionConfig MouduleDbConfig { get; set; }
        List<Type> FunctionalTypes { get; set; }
        string GetDBTablePrefixConfig();
        bool MigrateDB();

        void SetServiceProvider(ServiceProvider serviceProvider);

        //List<Module> GetAutofacModules();

        void RegisterAutofacTypes();
        //RequireItemCollection RequireItems { get; set; }

        //DbMigrationUnit ModuleMigrationUnit { get; set; }

        void ConfigureSiteServices(IServiceCollection services);
        void ConfigureSite(IApplicationBuilder app, IWebHostEnvironment env);
        void ConfigureRouter(IRouteBuilder routeBuilder);
    }
}

using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniOrm.Application;
using UniOrm;
using UniOrm.Startup.Web;
using static IdentityModel.OidcConstants;
using Microsoft.Extensions.Hosting;
using Autofac;
using Microsoft.Extensions.Caching.Memory;
using UniOrm.Common;
using Autofac.Extensions.DependencyInjection;

namespace UniNote
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Logger.LogInfo("Startup", "Startup is starting");
            Configuration = configuration;
            Configuration.Startup();
        }

        public IConfiguration Configuration { get; }
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //APPCommon.Resover.Container
            containerBuilder.RegisterModule<AutofacModule>();

        }
    
        public void ConfigureServices(IServiceCollection services)
        {
            Logger.LogInfo("Startup", "ConfigureServices is starting");
            services.ConfigureServices();
            Logger.LogInfo("Startup", "ConfigureServices is end");
            // ApplicationStartUp.EnsureDaContext(typeof(MigrationVersion1).Assembly);

        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            APP.Resover.Resovertot = app.ApplicationServices.GetAutofacRoot();


            var memoryCache = APP.Resover.Resovertot.Resolve<IMemoryCache>();
            APP.RuntimeCache = new RuntimeCache(memoryCache);
            //autofacServiceProvider = scope.Resolve<IServiceProvider>();
            //var systemResover = new AutofacResover() { Container = container };
            //APP.Builder.RegisterInstance<IResover>(systemResover); 

            Logger.LogInfo("Startup", "Configure is starting");
            app.ConfigureSite(env);
            Logger.LogInfo("Startup", "Configure is end");
        }
    }
}

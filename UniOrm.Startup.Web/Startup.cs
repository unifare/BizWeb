using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniOrm.Application;
using UniOrm.Common;
using UniOrm.Startup.Web;
using Autofac.Extensions.DependencyInjection;
namespace UniOrm.Startup.Web
{
    public class Startup : UniOrm.Common.Core.IStartUp
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
        //public ILifetimeScope AutofacContainer { get; private set; }
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
            app.ConfigureSite(env, lifetime);
            Logger.LogInfo("Startup", "Configure is end");
        }

    }
}

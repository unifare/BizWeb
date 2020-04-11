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

namespace UniNote
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Logger.LogInfo("Startup", "Startup is starting");
            Configuration = configuration; 
            Configuration.Startup();
        }

        public IConfiguration Configuration { get; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Logger.LogInfo("Startup", "ConfigureServices is starting");
            var serv = services.ConfigureServices();
            Logger.LogInfo("Startup", "ConfigureServices is end");
            // ApplicationStartUp.EnsureDaContext(typeof(MigrationVersion1).Assembly);
            return serv;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {  
            Logger.LogInfo("Startup", "Configure is starting");
            app.ConfigureSite(env);
            Logger.LogInfo("Startup", "Configure is end");
        }
    }
}

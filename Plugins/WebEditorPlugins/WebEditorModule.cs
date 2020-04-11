using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UEditor.Core;
using UniOrm;
using UniOrm.Common;
using static IdentityModel.OidcConstants;

namespace WebEditorPlugins
{
    public class WebEditorModule : ModuleBase
    {
        public override string ModuleName { get; } = nameof(WebEditorModule);
        public override string DllPath { get; set; }
        public override AppConfig ModuleAppConfig { get; set; }
        public override DbConnectionConfig MouduleDbConfig { get; set; }
        public WebEditorModule()
        {
        }
        public OauthClientModel OauthClient { get; set; }
        public override void EnsureDaContext()
        {
            
        }

        public override bool Init()
        { 
            return true;
        }


        public override bool MigrateDB()
        {
            return true;
        }

        public override List<Type> ModelType()
        {
            return null;
        }

        public override List<Type> FunctionalTypes { get; set; }

        public override List<string> ModelTypeStrings()
        {
            return new List<string>();
        }

        public override List<Autofac.Module> GetAutofacModules()
        {
            return new List<Autofac.Module>();
        }



        public override void RegisterAutofacTypes()
        {
            Builder.RegisterInstance<WebEditorModule>(this); 
        
        }

        public override void ConfigureSite(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot")),
                RequestPath = ""
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "upload")),
                RequestPath = "/upload",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });
        }

        public override void ConfigureRouter(IRouteBuilder routeBuilder)
        {

        }



        public override void ConfigureSiteServices(IServiceCollection services)
        {
            services.AddUEditorService(   );
        }
    }
}

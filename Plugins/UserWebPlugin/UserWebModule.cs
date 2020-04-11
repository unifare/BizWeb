using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using UniOrm;
using UniOrm.Common;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Autofac;
using static IdentityModel.OidcConstants;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace UniOrm.UserWebPlugin
{
    public class UserWebModule : ModuleBase
    {
        public override string ModuleName { get; } = nameof(UserWebModule);
        public override string DllPath { get; set; }
        public override AppConfig ModuleAppConfig { get; set; }
        public override DbConnectionConfig MouduleDbConfig { get; set; }
        public UserWebModule()
        {
        }
        public OauthClientModel OauthClient { get; set; }
        public override void EnsureDaContext()
        {
            //UniOrm.Common.DbMigrationHelper.EnsureDaContext(
            //     APPCommon.AppConfig.UsingDBConfig.Connectionstring,
            //     (int)APPCommon.AppConfig.UsingDBConfig.DBType,
            //     typeof(UserWebModule).Assembly);
        }

        public override bool Init()
        {
            base.Init();
            return true;
        }


        public override bool MigrateDB()
        {
            return true;
        }

        public override List<Type> ModelType()
        {
            throw new NotImplementedException();
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
            Builder.RegisterInstance< UserWebModule>(this); 
         
        }

        public override void ConfigureSite(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserWebStatic")),
                RequestPath = "/UserWebStatic"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Userweb")),
                RequestPath = "/Userweb"
            });
        }

        public override void ConfigureRouter(IRouteBuilder routeBuilder)
        {

        }



        public override void ConfigureSiteServices(IServiceCollection services)
        {

        }
    }
}

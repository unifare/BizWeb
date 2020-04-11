using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.JDPay;
using Essensoft.AspNetCore.Payment.LianLianPay;
using Essensoft.AspNetCore.Payment.QPay;
using Essensoft.AspNetCore.Payment.UnionPay;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using Microsoft.AspNetCore.Routing;

namespace AlipayPlugin
{
    public class AlipayModule : ModuleBase
    {

        public override string ModuleName { get; } = nameof(AlipayModule);
        public override string DllPath { get; set; }
        public override AppConfig ModuleAppConfig { get; set; }
        public override DbConnectionConfig MouduleDbConfig { get  ; set ; }
        public AlipayModule()
        { 
        }

        public override void EnsureDaContext()
        {
            UniOrm.Common. DbMigrationHelper.EnsureDaContext(
                APPCommon.AppConfig.UsingDBConfig.Connectionstring,
                (int)APPCommon.AppConfig.UsingDBConfig.DBType, 
                typeof(AlipayModule).Assembly);
        }

        public override bool Init()
        {
            base.Init();
            return true;
        }
        public override void SetServiceProvider(ServiceProvider serviceProvider)
        {

            ServiceProvider = serviceProvider;
        }

        public override bool MigrateDB()
        {
            throw new NotImplementedException();
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

        public override List<Autofac. Module> GetAutofacModules()
        {
            return new List<Autofac.Module>();
        }
        public override void RegisterAutofacTypes()
        {

        }
        public override void ConfigureSiteServices(IServiceCollection services)
        { 
            // 引入Payment 依赖注入
            services.AddAlipay();
            services.AddJDPay();
            services.AddQPay();
            services.AddUnionPay();
            services.AddWeChatPay();
            services.AddLianLianPay();

            // 在 appsettings.json 中 配置选项
            services.Configure<AlipayOptions>(Configuration.GetSection("AlipayPlugin:Alipay"));
            services.Configure<JDPayOptions>(Configuration.GetSection("AlipayPlugin:JDPay"));
            services.Configure<QPayOptions>(Configuration.GetSection("AlipayPlugin:QPay"));
            services.Configure<UnionPayOptions>(Configuration.GetSection("AlipayPlugin:UnionPay"));
            services.Configure<WeChatPayOptions>(Configuration.GetSection("AlipayPlugin:WeChatPay"));
            services.Configure<LianLianPayOptions>(Configuration.GetSection("AlipayPlugin:LianLianPay"));
          
            //services.AddWebEncoders(opt =>
            //{
            //    opt.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            //});
        }

        public override void ConfigureSite(IApplicationBuilder app, IHostingEnvironment env)
        {

        }
       

        public override void ConfigureRouter(IRouteBuilder routeBuilder)
        {
            
        }
    }
}

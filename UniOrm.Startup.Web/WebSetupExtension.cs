using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.Configuration;
using UniOrm.Common;
using Newtonsoft.Json.Serialization;
using UniOrm;
using UniOrm.Application;
using Microsoft.AspNetCore.Authentication.Cookies;
using Swashbuckle.AspNetCore.Swagger; 
using Microsoft.IdentityModel.Tokens;
using UniOrm.Core;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using static IdentityModel.OidcConstants;
using MediatR;
using System.Threading.Tasks;
using NetCoreCMS.Framework.Core.App;
using Microsoft.AspNetCore;
using System.Threading;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection.Extensions; 
using Microsoft.Extensions.Hosting; 
using Microsoft.AspNetCore.Mvc.TagHelpers;  
using Microsoft.OpenApi.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using UniOrm.Startup.Web.DynamicController;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UniOrm.Model;
using UniOrm.Common.ReflectionMagic;
using UniOrm.Common.Middlewares;
using Microsoft.AspNetCore.Mvc.Razor;
using UniOrm.Startup.Web.Views;
using SimpleInjector;
using UniOrm.Common.Core;
using Autofac.Extensions.DependencyInjection;

namespace UniOrm.Startup.Web
{
    public static class WebSetup
    {
        private static IWebHost nccWebHost;
        //private static Thread starterThread = new Thread(StartApp);
        private const string LoggerName = "WebSetup";

        public static IConfiguration Configuration = null;

        public static void StartApp(string[] argsObj)
        {
            CoreManager.ResgiteCore();
            var fun = CoreManager.Resolver<IFunction>();
            UniOrm.ApplicationManager.Load().StartApp(argsObj, CreateWebHostBuilder);
        }

        private static string[] ScanBack(string dlldir)
        {
            return Directory.GetFiles(dlldir, "*.json");
        }
        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
           var ee= Type.GetType(CoreManager.StartupInterface.ImplementTypeName);
            var webHostBuilder = Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //这里是Autofac的引用声明
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.ConfigureAppConfiguration(builder =>
                   {
                       builder.AddJsonFile("config/System.json");
                       var alljsons = ScanBack(AppDomain.CurrentDomain.BaseDirectory);
                       foreach (var json in alljsons)
                       {
                           builder.AddJsonFile(json);
                       }

                   }).UseStartup (ee);
               });

            return webHostBuilder;
        }
        public static async Task RestartAppAsync()
        {
            await NetCoreCmsHost.StopAppAsync(nccWebHost);
        }

        public static async Task ShutdownAppAsync()
        {
            await NetCoreCmsHost.ShutdownAppAsync(nccWebHost);
        }
        public static void Startup(this IConfiguration configuration)
        {
            Configuration = configuration;
            APP.Startup(configuration);
        }
    
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(this IServiceCollection services )
        {
          
            APP.ConfigureSiteAllModulesServices(services);
            services.AddMediatR(typeof(WebSetup).Assembly); 
           
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICompiler, Compiler>()
               .AddSingleton<DynamicActionProvider>()
               .AddSingleton<DynamicChangeTokenProvider>()
               .AddSingleton<IActionDescriptorProvider>(provider => provider.GetRequiredService<DynamicActionProvider>())
               .AddSingleton<IActionDescriptorChangeProvider>(provider => provider.GetRequiredService<DynamicChangeTokenProvider>());

          
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromSeconds(60 * 60);
                o.Cookie.HttpOnly = true;
            });

            // 配置授权

            var appConfig = APPCommon.AppConfig;
            var signingkey = appConfig.GetDicstring("JWT.IssuerSigningKey");
            var backendfoldername = appConfig.GetDicstring("backend.foldername");
            var AuthorizeCookiesName = appConfig.GetDicstring("AuthorizeCookiesName");
            var OdicCookiesName = appConfig.GetDicstring("OdicCookiesName");
            var identityserver4url = appConfig.GetDicstring("Identityserver4.url");
            var Identityserver4ApiResouceKey = appConfig.GetDicstring("Identityserver4.ApiResouceKey");
            var idsr4_ClientId = appConfig.GetDicstring("idsr4_ClientId");
            var idsr4_ClientSecret = appConfig.GetDicstring("idsr4_ClientSecret");
            var idsr4_ReponseType = appConfig.GetDicstring("idsr4_ReponseType");
            var OauthClientConfig_scopes = appConfig.GetDicstring("OauthClientConfig_scopes");
            var IsUsingIdentityserverClient = Convert.ToBoolean(appConfig.GetDicstring("IsUsingIdentityserverClient"));
            var IsUsingIdentityserver4 = Convert.ToBoolean(appConfig.GetDicstring("IsUsingIdentityserver4"));
            var isAllowCros = Convert.ToBoolean(appConfig.GetDicstring("isAllowCros"));
            var AllowCrosUrl = appConfig.GetDicstring("AllowCrosUrl");
            var IsUserAutoUpdatedb = Convert.ToBoolean(appConfig.GetDicstring("IsUserAutoUpdatedb"));
            var isEnableSwagger = Convert.ToBoolean(appConfig.GetDicstring("isEnableSwagger")); ;
            //services.AddMvcCore().AddAuthorization().AddJsonFormatters(); 
            var IsUsingLocalIndentity = Convert.ToBoolean(appConfig.GetDicstring("IsUsingLocalIndentity"));
            // var IsUsingDB = Convert.ToBoolean(appConfig.GetDicstring("IsUsingDB"));
            var IsUsingCmsGlobalRouterFilter = Convert.ToBoolean(appConfig.GetDicstring("IsUsingCmsGlobalRouterFilter"));
            if (IsUserAutoUpdatedb)
            {
                InitDbMigrate();
            }

            APP.InitDbMigrate();
            APPCommon.AppConfig.LoadDBDictionary();
            APPCommon.LoadLocalLangs();
            if (isEnableSwagger)
            {
                //注册Swagger生成器，定义一个和多个Swagger 文档
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo()
                    {
                        Version = "v1",
                        Title = " API",
                        Description = "A simple example ASP.NET Core Web API",
                        Contact = new OpenApiContact
                        {
                            Name = "Oliver Wa",
                            Email = string.Empty,
                            Url = new Uri("http://www.66wave.com/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "许可证名字",
                            Url = new Uri("http://www.66wave.com/")
                        }
                    });
                });
            }
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Adult1", policy =>
            //        {
            //        });
            //    options.AddPolicy("Adult2", policy =>
            //    {
            //    });
            //});
            SetupIdentity(services, appConfig, signingkey, backendfoldername, AuthorizeCookiesName, OdicCookiesName, identityserver4url, Identityserver4ApiResouceKey, idsr4_ClientId, idsr4_ClientSecret, OauthClientConfig_scopes, IsUsingIdentityserverClient, IsUsingIdentityserver4, IsUsingLocalIndentity, idsr4_ReponseType);
            if (isAllowCros)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("allow_all", bb =>
                    {

                        if (AllowCrosUrl == "*")
                        {
                            bb = bb.AllowAnyOrigin();
                        }
                        else
                        {
                            var allusrs = AllowCrosUrl.Split(',');
                            bb = bb.WithOrigins(allusrs);
                        }

                        bb.AllowAnyOrigin();
                    });
                });
            }
            // _ = services.AddMvc(o =>    _是什么 IMvcBuiler
            services.AddRazorPages(c =>
            {
                 //c.
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new TemplateViewLocationExpander(Configuration));
            });
            services.AddControllersWithViews(o =>
            {
                o.Filters.Add<WorkAuthorzation>(); // 添加身份验证过滤器

                if (IsUsingCmsGlobalRouterFilter)
                {
                    o.Filters.Add<GlobalActionFilter>();
                    o.Filters.Add<WorkAuthorzation>();
                }
            })
            .AddRazorRuntimeCompilation(options =>
            {
                //var libraryPath = Path.GetFullPath(
               //          Path.Combine(AppDomain.CurrentDomain.BaseDirectory , "UniOrm.Startup.Web"));
                options.FileProviders.Add(new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory));

            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                CustomizedDateTimeConverer converer = new CustomizedDateTimeConverer();
                //然后需要使用到的dto对象或者实体对象，打上这个特性即可，如下所示： [JsonConverter(typeof(CustomizedDateTimeConverer), “yyyy - MM - dd HH: mm:ss”)]
                options.SerializerSettings.Converters.Add(converer);

            })
            .AddRazorPagesOptions(
               options =>
               {
                   options.RootDirectory = "/Pages";


                   //url重写
                   //options.Conventions.AddPageRoute("/Post", "Post/{year}/{month}/{day}");
                   //以下示例将 URL www.domain.com/product 映射到Razor 页面 “extras”文件夹“products.cshtml”文件：
                   //options.Conventions.AddPageRoute("/extras/products", "product");
                   //最后一个例子说明将所有请求映射到单个文件。如果站点内容存储在特定位置（数据库，Markdown文件），并且由单个文件（例如 “index.cshtml” ）
                   //负责根据 URL 定位内容，然后将其处理为HTML，则可以执行此操作：
                   //options.Conventions.AddPageRoute("/index", "{*url}");
                   //默认关闭 防止跨站请求伪造（CSRF / XSRF）攻击
                   options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
               })
            .ConfigureApplicationPartManager(m =>
            {
                //多个项目中分离Asp.Net Core Mvc的Controller和Areas
                //var homeType = typeof(Web.Controllers.Areas.HomeController);
                //var controllerAssembly = homeType.GetTypeInfo().Assembly;
                //var feature = new ControllerFeature();
                //m.ApplicationParts.Add(new AssemblyPart(controllerAssembly));
                //m.PopulateFeature(feature);
                //services.AddSingleton(feature.Controllers.Select(t => t.AsType()).ToArray());
            })
            .InitializeTagHelper<FormTagHelper>((helper, context) => helper.Antiforgery = false);
            
       
            var asses = AppDomain.CurrentDomain.GetAssemblies();
           // services.AddControllers().AddControllersAsServices(); //控制器当做实例创建
          //  services.InitAutofac(asses);
            // APPCommon.Builder.RegisterType<IHttpContextAccessor, HttpContextAccessor>(); 
            appConfig.ResultDictionary = appConfig.ResultDictionary;
            APP.ApplicationServices = services.BuildServiceProvider();
            APP.SetServiceProvider();
        }

        public static void BuildAllDynamicActions(DynamicActionProvider dynamicActionProvider, DynamicChangeTokenProvider dynamicChangeToken)
        {
             HandelBuildAllDynamicActions(  dynamicActionProvider,   dynamicChangeToken);
             
        }

        private static void HandelBuildAllDynamicActions(DynamicActionProvider dynamicActionProvider, DynamicChangeTokenProvider dynamicChangeToken)
        {

            var allactios = DB.UniClient.Queryable<AConMvcClass>().Where(p => p.IsEanable != null && p.IsEanable == true).ToList();
            try
            {
                foreach (var c in allactios)
                {
                    dynamicActionProvider.AddControllers( AConMvcCompileClass.ToCompileClass(c) );
                }
                dynamicChangeToken.NotifyChanges();
            }
            catch
            {

            }

        }

        private static void SetupIdentity(IServiceCollection services, AppConfig appConfig, string signingkey, string backendfoldername, string AuthorizeCookiesName, string OdicCookiesName, string identityserver4url, string Identityserver4ApiResouceKey, string idsr4_ClientId, string idsr4_ClientSecret, string OauthClientConfig_scopes, bool IsUsingIdentityserverClient, bool IsUsingIdentityserver4, bool IsUsingLocalIndentity ,string idsr4_ReponseType)
        {
            if (IsUsingLocalIndentity)
            {
              var s=  services.AddAuthentication(
                    options =>
                    {
                        if (IsUsingIdentityserverClient == false || IsUsingIdentityserver4 == false)
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        }
                    }
                )
                 .AddCookie(UserAuthorizeAttribute.CustomerAuthenticationScheme, option =>
                 {
                     option.LoginPath = new PathString("/account/login");
                     option.AccessDeniedPath = new PathString("/Error/Forbidden");
                 })
                .AddCookie(AdminAuthorizeAttribute.CustomerAuthenticationScheme, option =>
                {
                    option.LoginPath = new PathString("/" + backendfoldername + "/Admin/Signin");
                    option.AccessDeniedPath = new PathString("/Error/Forbidden");
                });
               
                s.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    if (IsUsingIdentityserver4)
                    {
                        //options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);
                        options.Authority = identityserver4url; // IdentityServer的地址
                        options.RequireHttpsMetadata = false; // 不需要Https 
                        options.Audience = Identityserver4ApiResouceKey; // 和资源名称相对应 
                    }
                    else
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingkey)),//秘钥
                            ValidateIssuer = true,
                            ValidIssuer = appConfig.GetDicstring("JWT.Issuer"),
                            ValidateAudience = true,
                            ValidAudience = appConfig.GetDicstring("JWT.Audience"),
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.FromMinutes(5)
                        };
                    }
                    options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(1);
                    // 我们要求 Token 需要有超时时间这个参数
                    options.TokenValidationParameters.RequireExpirationTime = true;
                    //};
                });
                
            }
            if (IsUsingIdentityserver4 && !IsUsingLocalIndentity)
            {
                services.AddMvcCore().AddAuthorization();
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                   {
                       options.Authority = identityserver4url; // IdentityServer的地址
                       options.RequireHttpsMetadata = false; // 不需要Https 
                       options.Audience = Identityserver4ApiResouceKey; // 和资源名称相对应 
                       options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(1);
                       options.TokenValidationParameters.RequireExpirationTime = true;
                   });

            }
            if (IsUsingIdentityserverClient)
            {
                services.AddAuthentication(options =>
                {
                    // 使用cookie来本地登录用户（通过DefaultScheme = "Cookies"）
                    options.DefaultScheme = AuthorizeCookiesName;
                    // 设置 DefaultChallengeScheme = "oidc" 时，表示我们使用 OIDC 协议
                    options.DefaultChallengeScheme = OdicCookiesName;
                })
                    // 我们使用添加可处理cookie的处理程序
                    .AddCookie(AuthorizeCookiesName)
                    // 配置执行OpenID Connect协议的处理程序

                    .AddOpenIdConnect(OdicCookiesName, options =>
                    {
                        // 
                        options.SignInScheme = AuthorizeCookiesName;
                        // 表明我们信任IdentityServer客户端
                        options.Authority = identityserver4url;
                        // 表示我们不需要 Https
                        options.RequireHttpsMetadata = false;
                        // 用于在cookie中保留来自IdentityServer的 token，因为以后可能会用
                        options.SaveTokens = true;
                        try
                        {
                            options.ClientId = idsr4_ClientId; // "mvc_client";
                            options.ClientSecret = idsr4_ClientSecret;
                            options.CallbackPath = "/public/callback.html";
                            //options.TokenEndpoint = "/Admin/Signin";
                            options.ResponseType = idsr4_ReponseType;
                        }
                        catch (Exception exp)
                        {
                            Logger.LogError(LoggerName, "exp: " + exp.Message + ",------------->" + LoggerHelper.GetExceptionString(exp));
                        }
                        options.Scope.Clear();
                        var allscopes = OauthClientConfig_scopes.Split(',');
                        foreach (var ss in allscopes)
                        {
                            options.Scope.Add(ss);
                        }

                    })
                 ;

            }


        }

        //public static string GetDicstring(AppConfig appConfig, string key)
        //{
        //    var item = appConfig.SystemDictionaries.FirstOrDefault(p => p.KeyName == key);
        //    if (item != null)
        //    {
        //        return item.Value;
        //    }
        //    return string.Empty;
        //}

        public static void InitDbMigrate()
        {
            Application.DbMigrationHelper.EnsureDaContext(APPCommon.AppConfig.UsingDBConfig);
        }
        public static void ConfigureSite(this IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(OnStart);//1:应用启动时加载配置,2:应用启动后注册服务中心
            lifetime.ApplicationStopped.Register(UnRegService);//应用停止后从服务中心注销
            //app.UseSimpleInjector(APPCommon.Resover.Container);
            app.UseDeveloperExceptionPage();
            var appConfig = APPCommon.AppConfig;
            var UserDefaultStaticalDir = appConfig.GetDicstring("UserDefaultStaticalDir");
            var webroot = Path.Combine(Directory.GetCurrentDirectory(), UserDefaultStaticalDir);
            if (!Directory.Exists(webroot))
            {
                Directory.CreateDirectory(webroot);
            }
            app.UseStaticFiles();

            CreatUserSpaceDirectory(app);
            APP.ConfigureSiteAllModules(app);

            //app.UseCookiePolicy(); //是否启用cookie隐私

            var isEnableSwagger = Convert.ToBoolean(appConfig.GetDicstring("isEnableSwagger")); ;
            if (isEnableSwagger)
            {
                //启用中间件服务生成Swagger作为JSON终结点
                app.UseSwagger();
                //启用中间件服务对swagger-ui，指定Swagger JSON终结点
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }
            app.UsePhp(new PhpRequestOptions() { RootPath = APPCommon.UserUploadBaseDir, ScriptAssembliesName = new string[] { "UniOrm.PHP" } }) ;
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization(); 
            var isAllowCros = Convert.ToBoolean(appConfig.GetDicstring("isAllowCros"));
            if (isAllowCros)
            {
                app.UseCors("allow_all");
            }
            var AreaName = appConfig.GetDicstring("AreaName");

            //url 重写。。。
            // var rewrite = new RewriteOptions()
            //.AddRewrite("first", "home/index", 301);
            // app.UseRewriter(rewrite);

            //用户session服务
            app.UseSession();

            APPCommon.ConfigureSite(app, env);
             app.UseTheme();//启用theme中间件
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(name: "areas", "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute("areaRoute", "{"+ AreaName + ":exists}/{controller}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                       "factory", "/fact/{action}", new { controller = "Fact", action = "Index" });
                endpoints.MapControllerRoute(
                "cmsinstall", "/CmsInstall/{action}", new { controller = "CmsInstall", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                   "all", @"{**path}", new { controller = "FactoryBuilder", action = "Index" });

               endpoints.MapRazorPages();
            });

           // APPCommon.Resover.Container.Verify();
           // BuildAllDynamicActions();
        }
        private static void OnStart()
        {
            LoadAppConfig();
            RegService();
        }
        private static void LoadAppConfig()
        {
            //加载应用配置
            Console.WriteLine("ApplicationStarted:LoadAppConfig");
        }

        private static void RegService()
        {
            //先判断是否已经注册过了
            //this code is called when the application stops
            Console.WriteLine("ApplicationStarted:RegService");
        }
        private static void UnRegService()
        {
            //this code is called when the application stops
            Console.WriteLine("ApplicationStopped:UnRegService");
        }
        private static void CreatUserSpaceDirectory(IApplicationBuilder app)
        {
            var filedir = Path.Combine(APPCommon.UserUploadBaseDir, "wwwroot");
            if (!Directory.Exists(filedir))
            {
                Directory.CreateDirectory(filedir);
            }
            app.UseStaticFiles(new StaticFileOptions
            {

                FileProvider = new PhysicalFileProvider(filedir),
                RequestPath = "/w"
            });
        }
    }
}

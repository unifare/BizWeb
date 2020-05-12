using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UniOrm;
using UniOrm.Common;
using Autofac;
using System.IO;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using UniOrm.Model;
using SqlSugar;
using System.Threading.Tasks;
using SqlKata;
using SqlKata.Execution;
using SqlKata.Extensions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using RazorLight;
using UniOrm.Common.RazorPage;
using System.Dynamic;

namespace UniOrm
{
    public partial class APPCommon
    {
        public static HttpClient Client { get; set; } = new HttpClient();
        public static IConfiguration Configuration { get; set; }
        public static ServiceProvider ApplicationServices;
        public static ContainerBuilder Builder = new ContainerBuilder();
        public static IResover Container;
        public static Dictionary<string, Assembly> Dlls = new Dictionary<string, Assembly>();
        public static List<string> DynamicReferenceDlls = new List<string>();
        public static Dictionary<string, Type> Types = new Dictionary<string, Type>();
        public static Dictionary<string, MethodInfo> MethodInfos = new Dictionary<string, MethodInfo>();
        //public  List<RuntimeModel> RuntimeModels = new List<RuntimeModel>();
        public static ConcurrentDictionary<string, object> StepResults = new ConcurrentDictionary<string, object>();
        readonly static string logName = "UniOrm.APPCommon";
        public static RuntimeCache RuntimeCache;
        public static DefaultModuleManager ModuleManager { get; set; } = new DefaultModuleManager();
        public static List<LocalLangs> Langs = new List<LocalLangs>();
        private static AppConfig _AppConfig;
        public const string AreaName = "sd23nj";
        private static string _appBaseDir = string.Empty;
        public static string AppBaseDir
        {
            get
            {
                if (string.IsNullOrEmpty(_appBaseDir))
                {
                    _appBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                }
                return _appBaseDir;
            }

        }

        public static async Task LoadLocalLangs()
        {
            Langs = await DB.UniClient.Queryable<LocalLangs>().ToListAsync();

        }
        public static async Task<LocalLangs> GetLoalLang(string key, string langname = "zh_CN", int lang = 0)
        {
         
            LocalLangs lg = new LocalLangs();
            if (!string.IsNullOrEmpty(langname))
            {
                var cachelang = Langs.FirstOrDefault(p => p.Name == key && string.Compare(p.LangName, langname, true) == 0);

                if (cachelang != null)
                {
                    lg = cachelang;
                }
                else
                {
                    lg = await DB.UniClient.Queryable<LocalLangs>().Where(p => p.Name == key && SqlFunc.ToLower(p.LangName) == SqlFunc.ToLower("JACK")).FirstAsync();
                }
            }
            else
            {
                var cachelang = Langs.FirstOrDefault(p => p.Name == key && Convert.ToInt32(p.Lang) == lang);

                if (cachelang != null)
                {
                    lg = cachelang;
                }
                else
                {
                    lg = await DB.UniClient.Queryable<LocalLangs>().Where(p => p.Name == key && Convert.ToInt32(p.Lang) == lang).FirstAsync();
                }
            }
            return lg;
        }

        //public static async Task<LocalLangs> GetLoalLang(string key, int lang =0)
        //{
        //    var lg = await DB.UniClient.Queryable<LocalLangs>().Where(p => p.Name == key && Convert.ToInt32( p.Lang )== lang).FirstAsync();
        //    return lg;
        //}
        private static string _userUploadBaseDir = string.Empty;
        public static string UserUploadBaseDir
        {
            get
            {
                if (string.IsNullOrEmpty(_userUploadBaseDir))
                {
                    _userUploadBaseDir = Path.Join(APPCommon.AppBaseDir, AppConfig.UserSpaceDir);
                    if (!Directory.Exists(_userUploadBaseDir))
                    {
                        Directory.CreateDirectory(_userUploadBaseDir);
                    }
                }
                return _userUploadBaseDir;
            }

        }

        public static string GetWTableName(string tableName)
        {
            return AppConfig.UsingDBConfig.DefaultDbPrefixName + tableName;
        }

        public static AppConfig AppConfig
        {
            get
            {
                if (_AppConfig == null)
                {
                    var configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config"+Path.DirectorySeparatorChar+"System.json");
                    var configroot = JToken.Parse(File.ReadAllText(configpath));
                    _AppConfig = JsonConvert.DeserializeObject<AppConfig>(configroot["App"].ToString());
                    //if( _AppConfig!=null)
                    //{
                    //    _AppConfig.LoadDBDictionary();
                    //}
                }
                return _AppConfig;
            }
        }
      

        static APPCommon()
        {

        }

        public static void RegisterAutofacModule(Autofac.Module moudle)
        {

            Builder.RegisterModule(moudle);
        }

        public static void RegisterAutofacAssemblies(IEnumerable<Assembly> modulesAssembly)
        {
            if (modulesAssembly != null)
            {
                foreach (var m in modulesAssembly)
                {
                    Builder.RegisterAssemblyModules(m);
                }
            }
        }

        public static void ResponseUnAuth(ActionExecutingContext action, string returnurl)
        {
            if (string.IsNullOrEmpty(returnurl))
            {
                action.Result = new ContentResult()
                {
                    ContentType = "text/html",
                    StatusCode = 401
                };
            }
            else
            {
                action.Result = new RedirectResult(returnurl);
            }
          
        }

        public static MethodInfo GetMethodFromConfig(bool IsPlugin, string libname, string typename, string methodName)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            //if (!IsPlugin)
            //{
            //    dir = Path.Combine(dir, "Plugins");
            //}
            var filepath = Path.Combine(dir, libname);
            Assembly assembley;
            if (!Dlls.ContainsKey(filepath))
            {
                assembley = Assembly.LoadFrom(filepath);
                Dlls.Add(filepath, assembley);
            }
            else
            {
                assembley = Dlls[filepath];
            }
            Type type;
            var alltypename = typename;
            if (!Types.ContainsKey(alltypename))
            {
                type = assembley.GetType(alltypename);
                Types.Add(alltypename, type);
            }
            else
            {
                type = Types[alltypename];
            }
            MethodInfo method;
            var allMethodName = string.Concat(alltypename, ".", methodName);
            if (!MethodInfos.ContainsKey(allMethodName))
            {
                //if( cons.IsStatic)
                //{
                method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
                // }

                MethodInfos.Add(allMethodName, method);
            }
            else
            {
                method = MethodInfos[allMethodName];
            }

            return method;
        }

        public static void ConfigureSite(IApplicationBuilder app, IHostingEnvironment env)
        {
            var allModules = ModuleManager.RegistedModules;
            foreach (var m in allModules)
            {
                m.ConfigureSite(app, env);
            }
        }
    
        public static void ConfigureRouter(IRouteBuilder routeBuilder)
        {
            var allModules = ModuleManager.RegistedModules;
            foreach (var m in allModules)
            {
                m.ConfigureRouter(routeBuilder);
            }
        }
        private static RazorLightEngine razorengine = null;

        public static RazorLightEngine Razorengine
        {
            get
            {
                if (razorengine == null)
                {
                    InitRazorEngine();
                }
                return razorengine;
            }
        }

        public static void InitRazorEngine()
        {
            var project = new UniRazorProject();
            razorengine = new RazorLightEngineBuilder()
                .AddDefaultNamespaces(APPCommon.AppConfig.RazorNamesapace.ToArray())
                .UseProject(project)
                .UseMemoryCachingProvider().
                Build();
        }

        public static async Task<string> RenderRazorKey(string templateKey, object model = null, ExpandoObject viewBag = null)
        {
            var engine = Razorengine;
            string result = string.Empty;
            if (model != null)
            {
                if (viewBag != null)
                {
                    result = await engine.CompileRenderAsync(templateKey, model, viewBag);
                }
                else
                {
                    result = await engine.CompileRenderAsync(templateKey, model);
                }
            }
            else
            {
                result = await engine.CompileRenderAsync(templateKey, new { });
            }

            return result;
        }



        public static async Task<ContentResult> View(string templateKey, object model=null ,ExpandoObject viewBag=null)
        { 
            
            return new ContentResult()
            {
                Content = await RenderRazorKey(templateKey, model, viewBag),
                ContentType = "text/html",
                StatusCode = 200
            }; ;
        }

        public static string ToSourceCode(AConMvcClass systemHtml)
        {
            var strinbuilder = systemHtml.UsingNameSpance + "\r\n" + systemHtml.ClassAttrs + "\r\n" +
                        " public partial class ";
            if (systemHtml.IsSelfDefine == true)
            {
                strinbuilder += systemHtml.ClassName;
            }
            else
            {
                strinbuilder += systemHtml.ClassName + "Controller";

                if (!string.IsNullOrEmpty(systemHtml.InhiredClass))
                {
                    strinbuilder += ":" + systemHtml.InhiredClass;
                }
                else if (systemHtml.IsController == true)
                {
                    strinbuilder += ": Microsoft.AspNetCore.Mvc.Controller";
                }
            }
            return strinbuilder += "\r\n{ \r\n" + systemHtml.ActionCode + "\r\n}";

        }

        public static List<dynamic> GetData(  string ssql, object  inParamters)
        {
            if (inParamters != null )
            { 
                return DB.UniClient.Ado.SqlQuery<dynamic>(ssql, inParamters);
            }
            else
            {
                return DB.UniClient.Ado.SqlQuery<dynamic>(ssql);
            }

        }

        public static IEnumerable<dynamic> GetKataData(Query inQuery, object inParamters)
        { 
            return inQuery.Get(); 
        }
    }
}

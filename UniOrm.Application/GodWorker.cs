using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.IO;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using UniOrm.Model.DataService;
using UniOrm.Core;
using UniOrm.Common;
using UniOrm;
using UniOrm.Model;
using RazorLight;
using CSScriptLib;
using System.Security.Claims; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Microsoft.Extensions.Configuration;
namespace UniOrm.Application
{
    public class RutimeGlobal
    {
        public RutimeGlobal()
        {
            __P = new List<object>();
        }
        public System.Collections.Generic.List<object> __P;
    }


    public class GodWorker : IGodWorker
    {
        public Dictionary<string, object> WorkerResouceInfos { get; set; }
        public string WorkerName { get; set; }
        readonly static object lockobj = new object();
        static string logName = "AConState.Application.GodMaker";
        //public static Dictionary<string, RuntimeModel> RuntimeModels = new Dictionary<string, RuntimeModel>();

        HttpContext httpContext { get; set; }
        // public DefaultModuleManager ModuleManager { get; set; }
        ISysDatabaseService CodeService;
        IConfiguration Config { get; set; }
        ISqlSugarClient DbFactory;
        public AppConfig appConfig { get; }
        public GodWorker(ISysDatabaseService codeService, ISqlSugarClient dbFactory, IConfiguration config)
        {
            WorkerName = Guid.NewGuid().ToString("N");
            DbFactory = dbFactory;
            CodeService = codeService;
            Config = config;
            appConfig= config.GetSection("App").Get<AppConfig>();
            //ModuleManager = new DefaultModuleManager();
        }


        public async Task Run(params object[] parameters)
        {

            var st = new System.Diagnostics.StackTrace();
            var lastMethod = st.GetFrame(1).GetMethod();
            //var appconfigstring = Config.GetValue<AppConfig>("App").AppConfigs;
 
            if (appConfig.AppType == "aspnetcore")
            {

                ComposeEntity cons = null;

                cons = FindComposity(appConfig, null);
                if (cons == null)
                {
                    Logger.LogError(logName, "Run -> Find composity returns null ");
                    return;
                }

                var newrunmodel = new RuntimeStepModel(Config)
                {
                    ComposeEntity = cons,
                    HashCode = cons.GetHash()
                };
                newrunmodel.Res["__actioncontext"] = parameters[0];
                newrunmodel.Res["__httpcontext"] = parameters[0].GetProp("HttpContext");
                httpContext = newrunmodel.Res["__httpcontext"] as HttpContext;
                newrunmodel.Res["__session"] = httpContext.Session;
                newrunmodel.Res["__db"] = DB.Kata;
                var pagetool = new RazorTool();
                pagetool.ActionContext = parameters[0] as ActionExecutingContext;
                pagetool.ResouceInfos = newrunmodel.Res;
                pagetool.Funs = newrunmodel.Funtions;
                newrunmodel.Res["__page"] = pagetool;
                newrunmodel.Res["__config"] = appConfig;

                if (!string.IsNullOrEmpty(cons.Templateid))
                {
                    newrunmodel.ComposeTemplate = FindComposeTemplet(cons.Templateid);
                }

                await RunComposity(parameters[0].GetHashCode(), httpContext,newrunmodel, DbFactory, CodeService, Config);


            }
            //var context = parameters[0] as ActionExecutingContext;
            //var resp = context.HttpContext.Response;
            //resp.ContentType = "text/html";

            //using (StreamWriter sw = new StreamWriter(resp.Body))
            //{
            //    sw.Write("Write a string to response in WriteResponseWithoutReturn!");
            //} 
        }

        //public void GetRequestHash(ActionExecutingContext actionExecutingContext)
        //{
        //    //actionExecutingContext.HttpContext.req
        //}

        private async static Task RunComposity(int requsetHash, HttpContext httpContext, RuntimeStepModel newrunmodel, ISqlSugarClient dbFactory, ISysDatabaseService codeService, IConfiguration config)
        {
            var cons = newrunmodel.ComposeEntity;
            if (cons.RunMode == RunMode.Coding)
            {
                if (newrunmodel.ComposeTemplate != null)
                {
                    //TODO :add template
                }
                //Manager.RuntimeModels.Add(newrunmodel);
                else
                { 
                    var steps = FindSteps(cons.Guid, codeService);

                    foreach (var s in steps)
                    {
                        object rebject = null;
                        object DynaObject = null; 
                     
                        if (s.IsUsingAuth.ToBool()   )
                        {
                            await httpContext.AuthenticateAsync();
                            if (httpContext.User.Identity.Name != s.UserName|| !httpContext.User.Identity.IsAuthenticated)
                            { 
                                APPCommon.ResponseUnAuth((ActionExecutingContext)newrunmodel.Res["__actioncontext"], s.LoginUrl);
                                 
                                return;
                            }
                        }
                        
                        var cacheKey = string.Concat(cons.Guid, "_", s.ExcuteType, "_", s.FlowStepType, "_", s.Guid, "_", s.ArgNames);
                        object stepResult = APP.RuntimeCache.GetOrCreate(cacheKey, entry =>
                          {
                              object newobj = null;
                              APP.RuntimeCache.Set(cacheKey, newobj);
                              return newobj;
                          });
                        
                        if (s.IsUsingCache && stepResult != null)
                        {
                            rebject = stepResult;
                        }
                        else
                        {
                            if (!s.IsUsingCache || stepResult == null)
                            {
                                switch (s.ExcuteType)
                                {
                                    case ExcuteType.Syn:
                                        switch (s.FlowStepType)
                                        {
                                            case FlowStepType.Declare:
                                                {
                                                    lock (lockobj)
                                                    {
                                                        //root.Usings[2].Name.ToString()
                                                        // var rebject2 = Manager.GetData(spec.InParamter1, spec.InParamter2);
                                                        var runcode = APP.FindOrAddRumtimeCode(s.Guid);
                                                        var so_default = ScriptOptions.Default;
                                                        if (runcode == null)
                                                        {
                                                            runcode = new RuntimeCode()
                                                            {
                                                                StepGuid = s.Guid,
                                                                CodeLines = s.ProxyCode,

                                                            };
                                                            List<string> dlls = new List<string>();

                                                            var isref = false;
                                                            string dllbase = AppDomain.CurrentDomain.BaseDirectory;


                                                            if (!string.IsNullOrEmpty(s.TypeLib))
                                                            {
                                                                var dllfile = dllbase + s.TypeLib;
                                                                if (APP.DynamicReferenceDlls.Contains(dllfile))
                                                                {
                                                                    isref = false;
                                                                }
                                                                else
                                                                {
                                                                    APP.DynamicReferenceDlls.Add(dllfile);
                                                                    isref = true;
                                                                    dlls.Add(dllfile);
                                                                }
                                                            }
                                                            if (!string.IsNullOrEmpty(s.ReferenceDlls))
                                                            {
                                                                isref = true;
                                                                string[] dllnams = s.ReferenceDlls.Split(',');
                                                                foreach (var n in dllnams)
                                                                {
                                                                    APP.DynamicReferenceDlls.Add(dllbase + n);
                                                                }

                                                                dlls.AddRange(dllnams);
                                                            }
                                                            if (isref)
                                                            {
                                                                so_default = so_default.WithReferences(dlls.ToArray());
                                                            }
                                                            so_default = so_default.WithReferences(Assembly.GetExecutingAssembly());

                                                            var state = CSharpScript.Create<object>(s.ProxyCode, so_default, typeof(Dictionary<string, object>));
                                                          
                                                            runcode.Script = state;
                                                            APP.RuntimeCodes.Add(s.Guid, runcode);
                                                        }
                                                        if (!string.IsNullOrEmpty(s.ReferenceDlls))
                                                        {
                                                            string dllbase = AppDomain.CurrentDomain.BaseDirectory;

                                                        }
                                                        rebject = runcode.Script.RunAsync(newrunmodel.Res).Result.ReturnValue;

                                                    }
                                                }
                                                break;
                                            case FlowStepType.GetData:
                                                { 
                                                    DynaObject = HandleGetData(httpContext, newrunmodel, dbFactory,  s);
                                                }
                                                break;
                                            case FlowStepType.CallMethod:
                                                {
                                                    var methodsub = APP.GetMethodFromConfig(s.IsBuildIn.Value, s.TypeLib, s.TypeFullName, s.MethodName);
                                                    var objParams = new List<object>();
                                                    if (!string.IsNullOrEmpty(s.ArgNames))
                                                    {
                                                        objParams = newrunmodel.GetPoolResuce(s.ArgNames.Split(','));
                                                    }

                                                    else
                                                    {
                                                        objParams = null;
                                                    }
                                                    try
                                                    {
                                                        if (methodsub.IsStatic)
                                                        {

                                                            DynaObject = methodsub.Invoke(null, objParams.ToArray());
                                                        }
                                                        else
                                                        {
                                                            var instance = newrunmodel.Res[s.InstanceName];
                                                            DynaObject = methodsub.Invoke(instance, objParams.ToArray());
                                                        }
                                                    }
                                                    catch (Exception exp)
                                                    {
                                                        Logger.LogError(logName, "Run -> FlowStepType.CallMethod error,composity:{0},step:{1},-------------exception:{2}", cons.Id, s.Guid, LoggerHelper.GetExceptionString(exp));
                                                        break;
                                                    }

                                                }
                                                break;
                                            case FlowStepType.Text:
                                                {
                                                    rebject = s.OutPutText;
                                                }
                                                break;
                                            case FlowStepType.Function:
                                                {
                                                    DynaObject = DealTheFunction(newrunmodel, s,httpContext);
                                                }
                                                break;
                                            case FlowStepType.RazorText:
                                                try
                                                {
                                                    rebject = stepResult = await HandleRazorText(newrunmodel, s,httpContext, s.ProxyCode);
                                                }
                                                catch (Exception exp)
                                                {
                                                    Logger.LogError(logName, "parser RazorText wrong: " + exp.Message + "-------" + LoggerHelper.GetExceptionString(exp));
                                                }
                                                break;
                                            case FlowStepType.RazorFile:
                                                try
                                                { 
                                                    var filePath = s.ProxyCode;
                                                    string template = File.ReadAllText(Path.Combine(APPCommon.UserUploadBaseDir, filePath));
                                                    rebject = stepResult = await HandleRazorText(newrunmodel, s, httpContext, template);
                                                }
                                                catch (Exception exp)
                                                {
                                                    Logger.LogError(logName, "parser RazorFile wrong: " + exp.Message + "-------" + LoggerHelper.GetExceptionString(exp));
                                                }
                                                break;

                                        }
                                        break;
                                }
                                if (rebject == null)
                                {
                                    rebject = MagicExtension.BackToInst(DynaObject);

                                }
                                if (s.IsUsingCache)
                                {
                                    APP.RuntimeCache.Set(cacheKey, rebject);
                                }
                            }
                        }
                       
                        if (!string.IsNullOrEmpty(s.StorePoolKey) && rebject != null)
                        {
                            newrunmodel.SetComposityResourceValue(s.StorePoolKey, rebject);
                        }

                    }
                    await CheckAndRunNextRuntimeComposity(requsetHash, httpContext, newrunmodel, dbFactory, codeService, config);
                }

                //Manager.RuntimeModels.Remove(newrunmodel);
            }
        }

        private static object DealTheFunction(RuntimeStepModel newrunmodel, AConFlowStep s ,HttpContext httpContext)
        {
            object rebject;
            var defaultNamespace = @"using UniOrm;
using UniOrm.Application;
using UniOrm.Common;
using UniOrm.Model;
using UniOrm.Startup.Web ;
using System;
using System.Web;
using System.IO;
using System.Text;
using System.Text.Encodings;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic ;
using System.Diagnostics ;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;
using System.Configuration ;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Drawing;
using System.Drawing.Printing;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Npgsql;
//using PetaPoco.SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlSugar;
using CSScriptLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Numerics ;
using Peachpie.AspNetCore.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;";
            var functionName = "FunctionName";
            var selfnamespace = s.OutPutText;

            var allcode =
                defaultNamespace + "\r\n"
               + selfnamespace + "\r\n"
               + "public object " + functionName + "(object __param){ \r\n"
               + "var __model=__param.AsDynamic();"
               + "var Page=__model.Res[\"__page\"];"
               + s.ProxyCode
               + "\r\n}";


            var runobj = CSScript.Evaluator.ReferenceAssembly(Assembly.GetExecutingAssembly())
                 .CreateDelegate(allcode);
            newrunmodel.Funtions.Add(s.StorePoolKey, runobj); 
            var pagetool = newrunmodel.Res["__page"] as RazorTool;
            pagetool.Funs = newrunmodel.Funtions; 
            rebject = runobj(newrunmodel);
            return rebject;
        }

        private static async Task<string> HandleRazorText(RuntimeStepModel newrunmodel, AConFlowStep s, HttpContext httpContext, string template)
        {
            var engine = APP.Razorengine;
            //string template = s.ProxyCode;
            string stepResult = "";
            if (string.IsNullOrEmpty(template))
            {

            }
            else
            {
                var stringbuilder = new StringBuilder();
                stringbuilder.Append(@"@using UniOrm 
@using UniOrm.Application
@using UniOrm.Common
@using UniOrm.Model
@using UniOrm.Startup.Web 
@using System
@using System.Web
@using System.IO
@using System.Text
@using System.Text.Encodings
@using System.Text.RegularExpressions
@using System.Collections.Generic
@using System.Diagnostics
@using System.Linq
@using System.Security.Claims
@using System.Threading
@using System.Threading.Tasks
@using System.Reflection
@using System.Dynamic 
@using System.Diagnostics 
@using System.Linq.Expressions
@using System.Xml
@using System.Xml.Linq
@using System.Configuration 
@using System.Data
@using System.Data.SqlClient
@using System.Data.Common
@using System.Data.OleDb
@using System.Globalization
@using System.Net
@using System.Net.Http
@using System.Net.Http.Headers
@using System.Net.Mail
@using System.Net.Security
@using System.Net.Sockets
@using System.Net.WebSockets
@using System.Drawing
@using System.Drawing.Printing
@using Microsoft.Data.Sqlite
@using MySql.Data.MySqlClient
@using Npgsql
@using SqlKata.Compilers
@using SqlKata.Execution
@using SqlSugar
@using CSScriptLib;
@using Newtonsoft.Json
@using Newtonsoft.Json.Linq
@using System.Numerics 
@using Microsoft.AspNetCore.Authentication
@using Microsoft.AspNetCore.Authorization
@using Microsoft.Extensions.DependencyInjection
@using Microsoft.AspNetCore.Mvc");


                if (!string.IsNullOrEmpty(s.ReferenceDlls))
                {
                    string[] dllnams = s.ReferenceDlls.Split(',');
                    foreach (var n in dllnams)
                    {
                        var rootname = n.TrimEnd(".dll".ToCharArray());
                        engine.Options.Namespaces.Add(n.TrimEnd(".dll".ToCharArray()));
                        stringbuilder.AppendLine(rootname);
                    }
                }
                stringbuilder.AppendLine("\r\n@{ DisableEncoding = true;  ");
                stringbuilder.AppendLine("\r\n var Page = new RazorTool();  ");
                stringbuilder.AppendLine("\r\n Page.Step=Model.Step; ");
                stringbuilder.AppendLine("\r\n Page.ResouceInfos=Model.Item as Dictionary<string, object>; ");
                stringbuilder.AppendLine("\r\n Page.Funs=Model.Funs as Dictionary<string, MethodDelegate>; ");
                stringbuilder.AppendLine("\r\n  }");
                var objParams = new List<object>();
                if (!string.IsNullOrEmpty(s.ArgNames))
                {
                    objParams = newrunmodel.GetPoolResuce(s.ArgNames.Split(','));
                }
                dynamic modelArg = null;
                var module = APPCommon.ModuleManager.GetModule(null, s.ModuleName);
                if (module == null)
                {

                    modelArg = new { Step = s, Module = new { }, Item = newrunmodel.Res, Funs = newrunmodel.Funtions };

                }
                else
                {

                    modelArg = new { Step = s, Module = module.AsDynamic(), Item = newrunmodel.Res, Funs = newrunmodel.Funtions };

                }
                var cachekey2 = template.ToMD5();
                var cacheResult = engine.Handler.Cache.RetrieveTemplate(cachekey2);
                template = stringbuilder.AppendLine("\r\n").Append(template).ToString();
                if (cacheResult.Success)
                {
                    stepResult = await engine.RenderTemplateAsync(cacheResult.Template.TemplatePageFactory(), modelArg);
                }
                else
                {
                    stepResult = await engine.CompileRenderStringAsync(cachekey2, template, modelArg);
                }

            }

            return stepResult;
        }

        private static object HandleGetData(HttpContext httpContext, RuntimeStepModel newrunmodel, ISqlSugarClient dbFactory, AConFlowStep s)
        {
            object dynaObject; 
            var objParams2 = new List<object>();
            if (!string.IsNullOrEmpty(s.ArgNames))
            {
                var args = s.ArgNames.Split(',');
                foreach (var aarg in args)
                {
                    object obj = aarg;
                    if (aarg.StartsWith("&"))
                    {
                        obj = newrunmodel.Resuce(aarg);

                    }
                    objParams2.Add(obj);
                }
            }
            if (objParams2 == null || objParams2.Count == 0)
            {
                dynaObject = APP.GetData( s.InParamter1);
            }
            else
            {
                dynaObject = APP.GetData( s.InParamter1, objParams2.ToArray());
            }

            return dynaObject;
        }

        private static async Task CheckAndRunNextRuntimeComposity(int requsetHash, HttpContext httpContext, RuntimeStepModel newrunmodel, ISqlSugarClient dbFactory, ISysDatabaseService codeService, IConfiguration config)
        {
            var resouce = newrunmodel.Resuce(newrunmodel.NextRunTimeKey);
            if (resouce != null)
            {
                var guid = (string)resouce;
                if (string.IsNullOrEmpty(guid))
                {
                    //newrunmodel.ResouceInfos.Remove(newrunmodel.NextRunTimeKey);
                    return;
                }
                var nextcon = codeService.GetConposity(guid).FirstOrDefault();
                if (nextcon == null)
                {
                    nextcon = new ComposeEntity()
                    {
                        Guid = guid,
                        RunMode = RunMode.Coding
                    };
                    var reint = codeService.InsertCode(nextcon);

                }
                else
                {
                    var nextRnmodel = new RuntimeStepModel(config)
                    {
                        ParentRuntimeModel = newrunmodel,
                        Res= newrunmodel.Res,
                        ComposeEntity = nextcon,
                        HashCode = nextcon.GetHash()
                    };
                    nextRnmodel.Res.Remove(newrunmodel.NextRunTimeKey);
                    await RunComposity(requsetHash, httpContext, nextRnmodel, dbFactory, codeService, config);

                }
            }
        }

        #region Utilies




        private ComposeEntity FindComposity(AppConfig appconfig, string allname)
        {
            lock (lockobj)
            {
                ComposeEntity cons = null;
                if (APPCommon.AppConfig.IsUseGloableCahe)
                {
                    cons = APP.Composeentitys.FirstOrDefault(p => p.Guid == appconfig.StartUpCompoistyID);
                    if (cons == null)
                    {
                        cons = CodeService.GetConposity(appconfig.StartUpCompoistyID, allname).FirstOrDefault();
                        if (cons != null)
                        {
                            APP.Composeentitys.Add(cons);
                        }
                    }

                }
                else
                {
                    cons = CodeService.GetConposity(appconfig.StartUpCompoistyID, allname).FirstOrDefault();
                }

                if (cons == null)
                {
                    cons = new ComposeEntity()
                    {
                        Name = allname,
                        RunMode = RunMode.Coding, Guid = appconfig.StartUpCompoistyID
                    };
                    var reint = CodeService.InsertCode(cons);
                    APP.Composeentitys.Add(cons);
                }

                return cons;
            }
        }
        private ComposeTemplate FindComposeTemplet(string tid)
        {
            lock (lockobj)
            {
                var cons = APP.ComposeTemplates.FirstOrDefault(p => p.Guid == tid);
                if (cons == null)
                {
                    cons = CodeService.GetSimpleCodeLinq<ComposeTemplate>(p => p.Guid == tid).FirstOrDefault();
                    if (cons != null)
                    {
                        APP.ComposeTemplates.Add(cons);
                    }
                }


                return cons;
            }
        }
        private static IEnumerable<AConFlowStep> FindSteps(string ComId, ISysDatabaseService codeService)
        {
            IEnumerable<AConFlowStep> cons = null;
            if (APPCommon.AppConfig.IsUseGloableCahe)
            {
                cons = APP.AConFlowSteps.Where(p => p.AComposityId == ComId);
                if (cons == null || cons.Count() == 0)
                {
                    cons = codeService.GetAConStateSteps(ComId).OrderBy(p => p.StepOrder).ToList();
                    if (cons != null)
                    {
                        APP.AConFlowSteps.AddRange(cons);
                    }
                }
            }
            else
            {
                cons = codeService.GetAConStateSteps(ComId).OrderBy(p => p.StepOrder).ToList();
            }

            if (cons == null)
            {
                Logger.LogError(logName, "FindStep -> FindStep null");

            }

            return cons;

        }

        public TypeDefinition G(string typeName)
        {
            return CodeService.GetTypeDefinition(typeName);
        }
        #endregion
    }
}

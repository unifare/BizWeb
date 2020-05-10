using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using UniOrm;
using UniOrm.Model;
using UniOrm.Model.DataService;
using UniOrm.Startup.Web;

namespace UniOrm.Startup.Web.Controllers
{

    [AdminAuthorize] 
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public partial class FactController : ControllerBase
    { 
        ISysDatabaseService m_codeService;
        public FactController(ISysDatabaseService codeService )
        {

            m_codeService = codeService; 
            //var ss = HttpContext.Session["admin"] ?? "";
            //if( )
        }

        [HttpGet]
        public IEnumerable<ComposeEntity> GetAllCompose()
        {
            var allpos = m_codeService.GetSimpleCode<ComposeEntity>(new { IsBuildIn = false });
            return allpos;
        }

        [HttpGet]
        public IEnumerable<TrigerRuleInfo> GetAllTrigers()
        {
            var allpos = m_codeService.GetSimpleCode<TrigerRuleInfo>(null);
            return allpos;
        }
        
        [HttpGet]
        public IEnumerable<AconFunction> GetAllFunctions()
        {
            var allpos = m_codeService.GetSimpleCode<AconFunction>(null);
            return allpos;
        }
        // GET api/values
        [HttpDelete]
        public bool DeleteSetp([FromBody] string id)
        {
            var oldobj = m_codeService.GetSimpleCodeLinq<AConFlowStep>(p => p.Guid == id).FirstOrDefault();
            if (oldobj == null)
            {
                return false;
            }
            var allpos = m_codeService.DeleteSimpleCode<AConFlowStep>(oldobj);
            return allpos;
        }

        [HttpDelete]
        public bool DeletTriger([FromBody] int id)
        {
            var oldobj = m_codeService.GetSimpleCodeLinq<TrigerRuleInfo>(p => p.Id == id).FirstOrDefault();
            if (oldobj == null)
            {
                return false;
            }
            var allpos = m_codeService.DeleteSimpleCode<TrigerRuleInfo>(oldobj);
            return allpos;
        }

        // GET api/values
        [HttpPost]
        public int ToInsertStep([FromBody]AConFlowStep id)
        {
            var model = id;
            EnsureData(model);
            var allpos = m_codeService.InsertCode<AConFlowStep>(model);
            return allpos;
        }

        private static void EnsureData(AConFlowStep model)
        {
            model.ArgNames = model.ArgNames?.Trim();
            model.AComposityId = model.AComposityId.Trim();
            model.MethodName = model.MethodName?.Trim();
            model.StorePoolKey = model.StorePoolKey?.Trim();
        }

        [HttpPost]
        public object UpdateDLL()
        {
            var remsg = string.Empty;
            if (Request.Form.Files.Count == 0)
            {
                remsg = ("未检测到文件");
                return new { isok = false, msg = remsg };
            }
            var dirName = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var file in Request.Form.Files)
            {
                file.UploadSaveSingleFile(dirName);
            }


            return new { isok = true, msg = remsg };
        }

        [HttpPost]
        public int ToUpateStep([FromBody]AConFlowStep id)
        {
            var model = id;
            EnsureData(model);
            var allpos = m_codeService.UpdateSimpleCode(model);
            return allpos;
        }
        [HttpGet]
        public object GetDefaultDlls()
        {
            //var defaultdll = "BasicPlugin.dll";
            //string comp = Request.Query["comid"];
            //var model = id;
            //model.AComposityId = comp;
            var dllfile = AppDomain.CurrentDomain.BaseDirectory;
            var allplugindll = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*Plugin.dll", SearchOption.AllDirectories);
            var rdlls = new List<string>();
            foreach (var item in allplugindll)
            {
                FileInfo fi = new FileInfo(item);
                rdlls.Add(fi.Name);
            }
            //Assembly asm = Assembly.LoadFrom(dllfile);
            //var alltypes = asm.GetTypes().Select<Type, string>(p => p.FullName);
            //var dtyp

            return new { dlls = rdlls };

        }
        [HttpGet]
        public object GetAllPluginReflections()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            //if (!Directory.Exists(dir))
            //{
            //    Directory.CreateDirectory(dir);
            //}
            var alldll = Directory.GetFiles(dir, "*Plugin.dll");
            //foreach (var dll in alldll)
            //{
            //    var defaultdll = dll;
            //    //string comp = Request.Query["comid"];
            //    //var model = id;
            //    //model.AComposityId = comp;
            //    var dllfile = Path.Combine(dir + defaultdll);
            //    Assembly asm = Assembly.LoadFrom(dllfile);
            //    var alltypes = asm.GetTypes().Select<Type, string>(p => p.FullName);

            //    foreach (var ty in alltypes)
            //    {  dynamic dd = new ExpandoObject();
            //        dd.dllfile = dllfile;
            //        dd.typename= ty.na
            //    }
            //    var dlls = new string[] { defaultdll };
            var list = new List<string>();

            //}
            foreach (var ty in alldll)
            {
                var file = new FileInfo(ty);
                list.Add(file.Name);
            }

            return new { dlls = list };

        }

        // GET api/values
        [HttpGet]
        public IEnumerable GetTypes()
        {
            // var defaultdll = "BasicPlugin.dll";
            string comp = Request.Query["ty"];
            var dll = Request.Query["dll"];
            //var model = id;
            //model.AComposityId = comp;
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var dllfile = Path.Combine(dir, dll);
            //if (comp == "p")
            //{
            //    dir = Path.Combine(dir, "Plugins");
            //    dllfile = Path.Combine(dir, dll);
            //}
            Assembly asm = null;
            if (!APP.Dlls.ContainsKey(dllfile))
            {
                asm = Assembly.LoadFrom(dllfile);
                APP.Dlls.Add(dllfile, asm);
            }
            else
            {
                asm = APP.Dlls[dllfile];
            }
            var list = new List<dynamic>();
            var alltypes = asm.GetTypes();
            foreach (var ty in alltypes)
            {

                dynamic dd = new ExpandoObject();
                dd.dllfile = dll;
                dd.FullName = ty.FullName;
                dd.Name = ty.Name;
                list.Add(dd);
            }
            return list;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable GetMothod(string id)
        {
            string fullName = Request.Query["FullName"].UrlDecode();
            //var model = id;
            //model.AComposityId = comp;
            var dllfile = Request.Query["id"].UrlDecode();
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            dllfile = Path.Combine(dir, dllfile);
            Assembly asm = null;
            if (!APP.Dlls.ContainsKey(dllfile))
            {
                asm = Assembly.LoadFrom(dllfile);
                APP.Dlls.Add(dllfile, asm);
            }
            else
            {
                asm = APP.Dlls[dllfile];
            }
            FileInfo fi = new FileInfo(id);
            bool isbuilin = true;
            //if (string.Compare(fi.Directory.Name, "Plugins", true) == 0)
            //{
            //    isbuilin = false;
            //}
            var alltype = asm.GetTypes().Where(p => p.FullName == fullName).FirstOrDefault();//.Select<Type, string>(p => p.FullName);
            if (alltype != null)
            {
                var allmethod = from m in alltype.GetMethods()
                                select new { methodname = m.Name, IsBuildIn = isbuilin, typename = m.ReflectedType.FullName, dll = dllfile, TypeLib = fi.Name };
                return allmethod;
            }

            return null;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<AConFlowStep> GetAConFlowStep()
        {

            var allFlowSteps = m_codeService.GetSimpleCode<AConFlowStep>(null);
            return allFlowSteps;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<AConFlowStep> GetAConFlowStepByStepID(int id)
        {

            var allFlowSteps = m_codeService.GetSimpleCodeLinq<AConFlowStep>(p => p.Id == id);
            return allFlowSteps;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<AConFlowStep> GetAConFlowStepByComposeID(string id)
        {

            var allFlowSteps = m_codeService.GetSimpleCodeLinq<AConFlowStep>(p => p.AComposityId == id)
                .OrderBy(p => p.StepOrder);
            return allFlowSteps;
        }
        [HttpGet]
        public IEnumerable<dynamic> GetAllModuleNames(string id)
        {
            var list = new List<dynamic>();
            foreach (var item in APPCommon.ModuleManager.RegistedModules)
            {
                list.Add(new { id = item.ModuleName, value = item.ModuleName });
            }
            return list;
        }


        [HttpPost]
        public int AddTriger([FromBody]TrigerRuleInfo id)
        {
            id.AddTime = DateTime.Now;
            id.Rule = id.Rule?.Trim();
            id.ComposityId = id.ComposityId?.Trim();
            if (string.IsNullOrEmpty(id.HttpMethod))
            {
                id.HttpMethod = "GET";
            }

            var allFlowSteps = m_codeService.InsertCode<TrigerRuleInfo>(id);
            return allFlowSteps;
        }

        [HttpPost]
        public int UpdateTriger([FromBody]TrigerRuleInfo id)
        {
            id.Rule = id.Rule?.Trim();
            id.HttpMethod = id.HttpMethod?.Trim();
            id.ComposityId = id.ComposityId?.Trim();

          //var  ww= m_codeService.GetSimpleCode<ComposeEntity>(new { Guid = id.ComposityId }).FirstOrDefault();

          //  if( ww!=null)
          //  {
          //      ww.Guid== 
          //  }
            var allFlowSteps = m_codeService.UpdateSimpleCode(id);

            return allFlowSteps;
        }

        
        [HttpPost]
        public int AddCompose([FromBody]ComposeEntity id)
        {
            id.AddTime = DateTime.Now;
            id.Guid = id.Guid?.Trim(); 
            var allFlowSteps = m_codeService.InsertCode<ComposeEntity>(id);
            return allFlowSteps;
        }

        [HttpPost]
        public int UpdateCache()
        {
            APP.Dlls.Clear();
            APP.Types.Clear();
            APP.MethodInfos.Clear();
            APP.RuntimeCodes.Clear();
            APP.Composeentitys.Clear();
            APP.DynamicReferenceDlls.Clear();
            APP.AConFlowSteps.Clear();
            APP.ComposeTemplates.Clear();
            APP.ClearCache();
            //var razorlist = m_codeService.GetSimpleCode<SystemHtml>(null);
            //foreach(var ss in razorlist)
            //{
            //    if(APP.Razorengine.Handler.Cache.Contains(ss.Name))
            //    {
            //        APP.Razorengine.Handler.Cache.Remove(ss.Name);
            //    }
            //}
            APP.InitRazorEngine();
            return 0;
        }
        [HttpPost]
        public int UpdateCompose([FromBody]ComposeEntity id)
        {
            id.AddTime = DateTime.Now;
            id.Guid = id.Guid?.Trim();
            id.TrigeMethod = id.TrigeMethod?.Trim();

            var ce = m_codeService.GetSimpleCode<ComposeEntity>(new { AComposityId = id.Id }).FirstOrDefault() ;
            var oldguid = string.Empty;
            if( ce!=null)
            {
                oldguid = ce.Guid;
            }
            if(string.IsNullOrEmpty(oldguid ))
            {
                return -1;
            }

            var ww = m_codeService.GetSimpleCode<AConFlowStep>(new { AComposityId = oldguid }) ;
            if (ww.Any())
            {
                foreach (var ss in ww)
                {
                    ss.AComposityId = id.Guid;
                    m_codeService.UpdateSimpleCode<AConFlowStep>(ss);
                }
            }
            var allFlowSteps = m_codeService.UpdateSimpleCode(id);
            return allFlowSteps;
        }

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        [HttpDelete]
        public bool RemoveComposity([FromBody]int id)
        {
            var oldobj = m_codeService.GetSimpleCodeLinq<ComposeEntity>(p => p.Id == id).FirstOrDefault();
            if (oldobj == null)
            {
                return false;
            }
            var ww = m_codeService.GetSimpleCode<AConFlowStep>(new { AComposityId = oldobj.Guid });
            if (ww.Any())
            {
                foreach (var ss in ww)
                { 
                    m_codeService.DeleteSimpleCode<AConFlowStep>(ss);
                }
            }
            var allpos = m_codeService.DeleteSimpleCode<ComposeEntity>(oldobj);
            return allpos; 
        }

    }
}

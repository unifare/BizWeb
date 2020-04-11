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

namespace UniNote.WebClient.Controllers
{

    [AdminAuthorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class FactController : ControllerBase
    {
        IHostingEnvironment _hostingEnvironment;
        ISysDatabaseService m_codeService;
        public FactController(ISysDatabaseService codeService, IHostingEnvironment hostingEnvironment)
        {

            m_codeService = codeService;
            _hostingEnvironment = hostingEnvironment;

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
            var allpos = m_codeService.DeleteSimpleCode(oldobj);
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
            var allpos = m_codeService.DeleteSimpleCode(oldobj);
            return allpos;
        }

        // GET api/values
        [HttpPost]
        public int ToInsertStep([FromBody]AConFlowStep id)
        {
            var model = id;
            var allpos = m_codeService.InsertCode<AConFlowStep>(model);
            return allpos;
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
            if (string.IsNullOrEmpty(id.HttpMethod))
            {
                id.HttpMethod = "GET";
            }

            var allFlowSteps = m_codeService.InsertCode<TrigerRuleInfo>(id);
            return allFlowSteps;
        }
        [HttpPost]
        public int UpateTriger([FromBody]TrigerRuleInfo id)
        {
            var allFlowSteps = m_codeService.UpdateSimpleCode(id);
            return allFlowSteps;
        }
        [HttpPost]
        public int UpdateTriger([FromBody]TrigerRuleInfo id)
        {
            id.AddTime = DateTime.Now;
            var allFlowSteps = m_codeService.InsertCode<TrigerRuleInfo>(id);
            return allFlowSteps;
        }
        [HttpPost]
        public int AddCompose([FromBody]ComposeEntity id)
        {
            id.AddTime = DateTime.Now;
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
            return 0;
        }
        [HttpPost]
        public int UpdateCompose([FromBody]ComposeEntity id)
        {
            id.AddTime = DateTime.Now;
            var allFlowSteps = m_codeService.UpdateSimpleCode(id);
            return allFlowSteps;
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

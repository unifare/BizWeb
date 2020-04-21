using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniOrm;
using UniOrm.Application;
using UniOrm.Startup.Web;
using SqlSugar;
using UniOrm.Model;

namespace UniNote.WebClient.Controllers
{
    [Area("sd23nj")] 
    [Route("sd23nj/[controller]/[action]")]
    [AdminAuthorize]
    public class LangsController : Controller
    {
        ISqlSugarClient dbFactory;
        public LangsController(ISqlSugarClient _dbFactory)
        {
            dbFactory = _dbFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult AddLangs(string Name, string Value, string Guid, string LangName)
        {
            LocalLangs localLangs = new LocalLangs();
            SetLangValues(Name, Value, Guid, LangName, localLangs);
            var allnames = dbFactory.Insertable<LocalLangs>(localLangs).ExecuteCommand();
            return Json(new { isok = true, msg = "" });
        }

        public IActionResult DelAllLangs(string Name)
        {
            var allnames = dbFactory.Deleteable<LocalLangs>().Where(p => p.Name == Name).ExecuteCommand();
            return Json(new { isok = allnames>=0, msg = "" });
        }

        public IActionResult DelLangs(long Id)
        { 
            var allnames = dbFactory.Deleteable<LocalLangs>( ).Where(p=>p.Id==Id).ExecuteCommand();
            return Json(new { isok = true, msg = "" });
        }

        public IActionResult SaveLangs(long Id,string Name, string Value, string Guid, string LangName)
        { 

            var oldname = dbFactory.Queryable<LocalLangs>().Where(p => p.Id == Id).First();
            if (oldname != null)
            {
                SetLangValues(Name, Value, Guid, LangName, oldname);
                dbFactory.Updateable<LocalLangs>(oldname).ExecuteCommand();
                return Json(new { isok = true, msg = "" });
            }
           
            return Json(new { isok = false, msg = "id:{id} not existed" });
        }

        private static void SetLangValues(string name, string value, string guid, string LangName, LocalLangs localLangs)
        {
            localLangs.Guid = guid;
            localLangs.Name = name;
            localLangs.Value = value;
            localLangs.LangName = LangName.ToLower();
            switch (localLangs.LangName)
            {
                case "en":
                    localLangs.Lang = ReportLanguageType.English;
                    break;
                case "tw":
                    localLangs.Lang = ReportLanguageType.TraditionalChinese;
                    break;
                case "zh":
                    localLangs.Lang = ReportLanguageType.SimpleChinese;
                    break;
            }
        }

        public IActionResult GetAllLangs()
        {
            var allnames = dbFactory.Queryable<LocalLangs>().Select("Name").Distinct().ToList();
            return Json(allnames);
        }
        public IActionResult GetAllLangsByName(string keyname)
        {
            var allnames = dbFactory.Queryable<LocalLangs>().Where(p=>p.Name== keyname).ToList();
            return Json(allnames);
        }
    }
}

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

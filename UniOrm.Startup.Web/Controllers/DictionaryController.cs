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
using UniOrm.Model;
using UniOrm.Model.DataService;

namespace UniOrm.Startup.Web.Controllers
{
    [Area(APPCommon.AreaName)]
    [Route(APPCommon.AreaName+"/[controller]/[action]")]
    [AdminAuthorize]
    public partial class DictionaryController : Controller
    {
        ISysDatabaseService m_codeService;
        public DictionaryController(ISysDatabaseService codeService)
        {
            m_codeService = codeService;
        }

        public IActionResult Index()
        {
            return View();
        }




        public IActionResult VueGrid2()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Signin()
        {
            return View();
        }

        public IActionResult SaveKeyvalue(int? id, string key, string value)
        {
            var recode = new  SystemDictionary();
            if (id == null)
            {
                recode = new SystemDictionary() { AddTime = DateTime.Now, IsSystem = false, KeyName = key, Value = value, SystemDictionarytype =  SystemDictionarytype.String };
                APPCommon.AppConfig.SystemDictionaries.Add(  recode);
               var result = m_codeService.InsertCode< SystemDictionary>(recode);

            }
            else
            {
                //var sd = new SystemDictionary();// { AddTime = DateTime.Now, IsSystem = false, KeyName = key, Value = value, SystemDictionarytype = SystemDictionarytype.String };
                recode = m_codeService.GetSimpleCode< SystemDictionary>(new { Id = id }).FirstOrDefault();
                if (recode == null)
                {
                    recode = new  SystemDictionary() { AddTime = DateTime.Now, IsSystem = false, KeyName = key, Value = value, SystemDictionarytype = SystemDictionarytype.String };
                    APPCommon.AppConfig.SystemDictionaries.Add(recode);
                    var result = m_codeService.InsertCode< SystemDictionary>(recode);
                }
                else
                {
                    recode.KeyName = key;
                    recode.Value = value;
                    m_codeService.UpdateSimpleCode(recode);
                    var sindex = APPCommon.AppConfig.SystemDictionaries.FindIndex(p => p.Id == recode.Id);
                    APPCommon.AppConfig.SystemDictionaries[sindex].KeyName = key;
                    APPCommon.AppConfig.SystemDictionaries[sindex].Value = value;

                }
            }
           

            return new JsonResult(new { result = recode });
        }
        public IActionResult DelKey(string key)
        {
            var result = true;
            //var sd = new SystemDictionary();// { AddTime = DateTime.Now, IsSystem = false, KeyName = key, Value = value, SystemDictionarytype = SystemDictionarytype.String };
            var recode = m_codeService.GetSimpleCode<SystemDictionary>(new { KeyName = key }).FirstOrDefault();
            if (recode != null)
            {
                result = m_codeService.DeleteSimpleCode<SystemDictionary>(recode);
            }
            return new JsonResult(new { isok = result });
        }


        public IActionResult GetAll(string key, int pIndex, int pagesize)
        {

            var result = m_codeService.GetSimpleCodePage<SystemDictionary>(null, pIndex, pagesize);
            return new JsonResult(new { result = result });
            

        }
        



    }
}

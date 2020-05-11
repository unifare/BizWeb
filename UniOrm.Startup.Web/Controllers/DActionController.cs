using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  
using SqlSugar;
using UniOrm.Model;
using UniOrm;
using UniOrm.Startup.Web.DynamicController;
using UniOrm.Common.ReflectionMagic;

namespace UniOrm.Startup.Web.Controllers
{
    [Area(APPCommon.AreaName)]
    [Route(APPCommon.AreaName +"/[controller]/[action]")]
    [AdminAuthorize]
    public partial class DActionController : Controller
    {
        private readonly ILogger<AconFunction> _logger;
        private readonly ISqlSugarClient DbFactory;
        private readonly DynamicActionProvider _actionProvider;
        private readonly DynamicChangeTokenProvider _dynamicChangeTokenProvider;
        public DActionController(ILogger<AconFunction> logger, ISqlSugarClient _dbFactory
            , DynamicActionProvider actionProvider, DynamicChangeTokenProvider dynamicChangeTokenProvider)
        {
            _logger = logger;
            DbFactory = _dbFactory;
            _actionProvider = actionProvider;
            _dynamicChangeTokenProvider = dynamicChangeTokenProvider;
        }

        public IActionResult DActionList()
        {
            return View();
        }
        public async Task<IActionResult> UpdateItem([FromBody]AConMvcClass systemHtml)
        {
            if (string.IsNullOrEmpty(systemHtml.Guid))
            {
                systemHtml.Guid = Guid.NewGuid().ToString("D");
            } 
            var aConMvcCompileClass = AConMvcCompileClass.ToCompileClass(systemHtml);
            var result = await DbFactory.Queryable<AConMvcClass>().Where(p => p.Id == systemHtml.Id).FirstAsync();
            if (result != null)
            {

                _actionProvider.RemoveController(aConMvcCompileClass);
                _dynamicChangeTokenProvider.NotifyChanges();

                result = systemHtml;
            }

            var reint = await DbFactory.Updateable(result).ExecuteCommandAsync();
            if (systemHtml.IsEanable.ToBool() == true)
            {
                try
                {

                    _actionProvider.AddControllers(aConMvcCompileClass);
                    _dynamicChangeTokenProvider.NotifyChanges();
                    return Json(new { isok = true });
                }
                catch (Exception ex)
                {
                    return Json(new { isok = false, err = ex.Message });
                }
            }
            return Json(new { isok = reint >= 0 });
        }

        public async Task<IActionResult> AddItem(
          [FromBody]AConMvcClass systemHtml   )
        {
            systemHtml.Addtime = DateTime.Now;

            if(string.IsNullOrEmpty( systemHtml.Guid))
            {
                systemHtml.Guid = Guid.NewGuid().ToString("D");
            }

            var oldmvc=  DbFactory.Queryable<AConMvcClass>().Where(p =>  p.ClassName.ToLower() == systemHtml.ClassName.ToLower()).FirstAsync();
            if( oldmvc!=null)
            {
                return Json(new { isok = false, err = $"has the same class {systemHtml.ClassName }"  });
            }

            var reint = await DbFactory.Insertable(systemHtml).ExecuteCommandAsync();
            if( systemHtml.IsEanable.ToBool()==true )
            {
                try
                { 
                    var aConMvcCompileClass = AConMvcCompileClass. ToCompileClass(systemHtml); 
                    _actionProvider.AddControllers(aConMvcCompileClass);
                    _dynamicChangeTokenProvider.NotifyChanges();
                    return Json(new { isok = true });
                }
                catch (Exception ex)
                {
                    return Json(new { isok = false, err = ex.Message });
                }
            }
            return Json(new { isok = reint > 0 });
        }

        private static AConMvcCompileClass ToCompileClass(AConMvcClass systemHtml, string source)
        {
            return new AConMvcCompileClass()
            {
                Guid = systemHtml.Guid,
                ActionCode = systemHtml.ActionCode
             ,
                AllSourceCode = source
             ,
                Addtime = systemHtml.Addtime
             ,
                ClassAttrs = systemHtml.ClassAttrs
             ,
                ClassName = systemHtml.ClassName
             ,
                ExReferenceName = systemHtml.ExReferenceName
             ,
                IsEanable = systemHtml.IsEanable
             ,
                Name = systemHtml.Name
             ,
                Id = systemHtml.Id
             ,
                UrlRule = systemHtml.UrlRule
             ,
                UsingNameSpance = systemHtml.UsingNameSpance
             ,
                VersionNum = systemHtml.VersionNum
            };
        }


        public async Task<IActionResult> DelItem(long Id)
        { 
            var reint = await DbFactory.Deleteable<AConMvcClass>(p => p.Id == Id).ExecuteCommandAsync();
            return Json(new { isok = reint>=0});
        }

        public async Task<IActionResult> GetAllLIst(int pageindex, int pagesize)
        {
            var toalnumber = 0;
            if( pagesize<=0)
            {
                pagesize = 100;
            }
            var list= await  DbFactory.Queryable<AConMvcClass>().OrderBy(p=>p.Id,OrderByType.Desc).ToPageListAsync(pageindex, pagesize, toalnumber);
            return Json(new { isok = true, data = list, num = toalnumber });
        }
 
    }
}

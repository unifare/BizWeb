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

            var result = await DbFactory.Queryable<AConMvcClass>().Where(p => p.Id == systemHtml.Id).FirstAsync();
            if(result != null)
            {
                result = systemHtml;
            }
           var reint= await  DbFactory.Updateable(result).ExecuteCommandAsync();
            if (systemHtml.IsEanable.ToBool() == true)
            {
                try
                {

                    var source = APPCommon.ToSourceCode(systemHtml);
                    _actionProvider.AddControllers(source);
                    _dynamicChangeTokenProvider.NotifyChanges();
                    return Json(new { isok = true });
                }
                catch (Exception ex)
                {
                    return Json(new { isok = false, err = ex.Message });
                }
            }
            return Json(new { isok = reint>=0  });
        }

        public async Task<IActionResult> AddItem(
          [FromBody]AConMvcClass systemHtml   )
        {
            systemHtml.Addtime = DateTime.Now;
            var reint = await DbFactory.Insertable(systemHtml).ExecuteCommandAsync();
            if( systemHtml.IsEanable.ToBool()==true )
            {
                try
                {
                   
                    var source = APPCommon.ToSourceCode(systemHtml);
                    _actionProvider.AddControllers(source);
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

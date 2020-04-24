using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BizWeb.Models;
using UniNote.WebClient.Controllers;
using SqlSugar;
using UniOrm.Model;
using UniOrm;

namespace BizWeb.Controllers
{
    [Area("sd23nj")]
    [Route("sd23nj/[controller]/[action]")]
    [AdminAuthorize]
    public class HtmlController : Controller
    {
        private readonly ILogger<HtmlController> _logger;
        private readonly ISqlSugarClient DbFactory;
        public HtmlController(ILogger<HtmlController> logger, ISqlSugarClient _dbFactory )
        {
            _logger = logger;
            DbFactory = _dbFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UpdateItem(long Id, string  Value)
        {

            var result = await DbFactory.Queryable<SystemHtml>().Where(p => p.Id == Id).FirstAsync();
            if(result != null)
            {
                result.Value = Value;
            }
           var reint= await  DbFactory.Updateable<SystemHtml>(result).ExecuteCommandAsync();
            return Json(new { isok = reint>=0  });
        }

        public async Task<IActionResult> AddItem(long Id, string Name, string Value)
        {

            SystemHtml systemHtml = new SystemHtml()
            {
                Name = Name,
                Value = Value,
                AddTime = DateTime.Now,
                LastUpdateTime = DateTime.Now

            };
            var reint = await DbFactory.Insertable<SystemHtml>(systemHtml).ExecuteCommandAsync();
            return Json(new { isok = reint > 0 });
        }

        public async Task<IActionResult> DelItem(long Id)
        { 
            var reint = await DbFactory.Deleteable<SystemHtml>(p => p.Id == Id).ExecuteCommandAsync();
            return Json(new { isok = reint>=0});
        }

        public async Task<IActionResult> GetAllLIst(int pageindex, int pagesize)
        {
            var toalnumber = 0;
            if( pagesize<=0)
            {
                pagesize = 100;
            }
            var list= await  DbFactory.Queryable<SystemHtml>().OrderBy(p=>p.Id,OrderByType.Desc).ToPageListAsync(pageindex, pagesize, toalnumber);
            return Json(new { isok = true, data = list, num = toalnumber });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

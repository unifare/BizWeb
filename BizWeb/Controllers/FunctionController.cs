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
    public class FunctionController : Controller
    {
        private readonly ILogger<AconFunction> _logger;
        private readonly ISqlSugarClient DbFactory;
        public FunctionController(ILogger<AconFunction> logger, ISqlSugarClient _dbFactory )
        {
            _logger = logger;
            DbFactory = _dbFactory;
        }

        public IActionResult FunctionList()
        {
            return View();
        }
        public async Task<IActionResult> UpdateItem(long Id, string FunctionCode, string FunctionMemo, string NnameSpaces, string ReferanceList)
        {

            var result = await DbFactory.Queryable<AconFunction>().Where(p => p.Id == Id).FirstAsync();
            if(result != null)
            {
                result.FunctionNameSpace = NnameSpaces;
                result.ReferanceList = ReferanceList;
                result.FunctionMemo = FunctionMemo;
                result.FunctionCode = FunctionCode;
            }
           var reint= await  DbFactory.Updateable(result).ExecuteCommandAsync();
            return Json(new { isok = reint>=0  });
        }

        public async Task<IActionResult> AddItem(long Id, string FunctionName, string FunctionMemo, string FunctionCode, string FunctionNameSpace, string ReferanceList)
        {

            var systemHtml = new AconFunction()
            {
                FunctionName = FunctionName,
                FunctionCode = FunctionCode,
                FunctionMemo= FunctionMemo,
                Guid = Guid.NewGuid().ToString(),
                FunctionNameSpace = FunctionNameSpace,
                ReferanceList = ReferanceList,
                AddTime = DateTime.Now

            };
            var reint = await DbFactory.Insertable(systemHtml).ExecuteCommandAsync();
            return Json(new { isok = reint > 0 });
        }

        public async Task<IActionResult> DelItem(long Id)
        { 
            var reint = await DbFactory.Deleteable<AconFunction>(p => p.Id == Id).ExecuteCommandAsync();
            return Json(new { isok = reint>=0});
        }

        public async Task<IActionResult> GetAllLIst(int pageindex, int pagesize)
        {
            var toalnumber = 0;
            if( pagesize<=0)
            {
                pagesize = 100;
            }
            var list= await  DbFactory.Queryable<AconFunction>().OrderBy(p=>p.Id,OrderByType.Desc).ToPageListAsync(pageindex, pagesize, toalnumber);
            return Json(new { isok = true, data = list, num = toalnumber });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

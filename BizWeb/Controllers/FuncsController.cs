using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlSugar;
using UniOrm;
using UniOrm.Model;

namespace UniNote.WebClient.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    [Authorize] 
    public class FuncsController : ControllerBase
    {

        private readonly ILogger<AconFunction> _logger;
        ISqlSugarClient dbFactory;
        public FuncsController(ISqlSugarClient _dbFactory, ILogger<AconFunction> logger )
        {
            dbFactory = _dbFactory;
        } 

            public async Task<object> UpdateItem(long Id, string FunctionCode, string FunctionMemo, string NnameSpaces, string ReferanceList)
            {

                var result = await dbFactory.Queryable<AconFunction>().Where(p => p.Id == Id).FirstAsync();
                if (result != null)
                {
                    result.FunctionNameSpace = NnameSpaces;
                    result.ReferanceList = ReferanceList;
                    result.FunctionMemo = FunctionMemo;
                    result.FunctionCode = FunctionCode;
                }
                var reint = await dbFactory.Updateable(result).ExecuteCommandAsync();
                return  new { isok = reint >= 0 } ;
            }

            public async Task<object> AddItem(long Id, string FunctionName, string FunctionMemo, string FunctionCode, string FunctionNameSpace, string ReferanceList)
            {

                var systemHtml = new AconFunction()
                {
                    FunctionName = FunctionName,
                    FunctionCode = FunctionCode,
                    FunctionMemo = FunctionMemo,
                    Guid = Guid.NewGuid().ToString(),
                    FunctionNameSpace = FunctionNameSpace,
                    ReferanceList = ReferanceList,
                    AddTime = DateTime.Now

                };
                var reint = await dbFactory.Insertable(systemHtml).ExecuteCommandAsync();
                return  new { isok = reint > 0 } ;
            }

            public async Task<object> DelItem(long Id)
            {
                var reint = await dbFactory.Deleteable<AconFunction>(p => p.Id == Id).ExecuteCommandAsync();
                return new { isok = reint >= 0 } ;
            }

            public async Task<object> GetAllLIst(int pageindex, int pagesize)
            {
                var toalnumber = 0;
                if (pagesize <= 0)
                {
                    pagesize = 100;
                }
                var list = await dbFactory.Queryable<AconFunction>().OrderBy(p => p.Id, OrderByType.Desc).ToPageListAsync(pageindex, pagesize, toalnumber);
                return new { isok = true, data = list, num = toalnumber };
            }
        } 
}

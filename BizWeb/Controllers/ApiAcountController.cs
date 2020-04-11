using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniOrm;
using UniOrm.Startup.Web; 
using UniOrm.Common;
using SqlSugar;
namespace UniNote.Controllers
{

    public class ApiAcountController : Controller
    {
        ISqlSugarClient dbFactory;
        IAuthorizeHelper authorizeHelper;
        public ApiAcountController(ISqlSugarClient _dbFactory, IAuthorizeHelper _authorizeHelper)
        {
            dbFactory = _dbFactory;
            authorizeHelper = _authorizeHelper;
        }



        public IActionResult Privacy()
        {
            return View();
        }


        public async Task<object> SignOut(string Token)
        {
            var respone = await authorizeHelper.Logout(HttpContext,Token);
            return respone;
        }



        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login(string userName, string password, string refreshToken)
        {
            var response = await authorizeHelper.LoginToIds4Async(HttpContext, userName, password, refreshToken);

            return new { response };

        }


    }
}

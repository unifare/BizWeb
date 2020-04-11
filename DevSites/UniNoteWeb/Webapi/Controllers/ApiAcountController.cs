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
using UniNote.Web.Model; 
using UniNote.WebApi;
using UniOrm.Common;

namespace UniNote.Controllers
{
  
    public class ApiAcountController : Controller
    {
        IDbFactory dbFactory;
        IAuthorizeHelper authorizeHelper;
        public ApiAcountController(IDbFactory _dbFactory, IAuthorizeHelper _authorizeHelper)
        {
            dbFactory = _dbFactory;
            authorizeHelper = _authorizeHelper;
        }

        

        public IActionResult Privacy()
        {
            return View();
        }


        public async Task<RedirectResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }

       

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login(  string userName, string password)
        {
            var response = await authorizeHelper.LoginToIds4Async(HttpContext, userName, password);
           
            return new {  response };

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

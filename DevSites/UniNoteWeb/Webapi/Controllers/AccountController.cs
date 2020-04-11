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

namespace UniNote.Controllers
{
    [UserAuthorize]
    public class AccountController : Controller
    {
        IDbFactory dbFactory;
        public AccountController(IDbFactory _dbFactory)
        {
            dbFactory = _dbFactory;
        }

        public IActionResult Index()
        {
            var ss = dbFactory.EFCore<pigcms_adma>().CreateDefaultInstance();

            var qlist = ss.From<pigcms_adma>();
            var allist = qlist.ToList<pigcms_adma>();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public async Task<RedirectResult> SignOut( )
        { 
            await HttpContext.SignOutAsync(UserAuthorizeAttribute.CustomerAuthenticationScheme);
            return Redirect( "~/");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login(string userName, string password)
        {
            var user = APP.LoginDefaultUser(userName, password);
            // var user = _userService.Login(userName, password);
            if (user != null)
            {
                //var authenticationType = AdminAuthorizeAttribute.CustomerAuthenticationScheme;
                //var identity = new ClaimsIdentity(authenticationType);
                //var claims = new Claim[]
                //   {
                //           new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                //           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                //   };
                //identity.AddClaims(claims);
                //await HttpContext.SignInAsync(authenticationType, new ClaimsPrincipal(identity));
                //return new { isok = true, msg = "" };

                var authenticationType = UserAuthorizeAttribute.CustomerAuthenticationScheme;
                var identity = new ClaimsIdentity(authenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                await HttpContext.SignInAsync(authenticationType, new ClaimsPrincipal(identity));
                return new { isok = true, msg = "" };

            }
            return new { isok = false, msg = "登录失败，用户名密码不正确" };

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

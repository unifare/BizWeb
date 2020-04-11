using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using UniNote.WebApi;
using UniOrm;
using UniOrm.Application;
using UniOrm.Startup.Web; 

namespace TestWeb.Controllers
{
    [Area("sd23nj")]
    [AdminAuthorize]
    public class AdminController : Controller
    {
        IDbFactory dbFactory;
        public AdminController(IDbFactory _dbFactory)
        {
            dbFactory = _dbFactory;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult urlmng()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpGet]
        [HttpPost]
        public async Task<RedirectResult> SignOut()
        {
            await HttpContext.SignOutAsync(AdminAuthorizeAttribute.CustomerAuthenticationScheme); 
            return Redirect("~/");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login(string userName, string password)
        {
            var user = APP.LoginAdmin(userName, password);
            // var user = _userService.Login(userName, password);
            if (user != null)
            {
                var authenticationType = AdminAuthorizeAttribute.CustomerAuthenticationScheme;
                var identity = new ClaimsIdentity(authenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                await HttpContext.SignInAsync(authenticationType, new ClaimsPrincipal(identity));
                return new { isok = true, msg = "" };

            }
            return new { isok = false, msg = "登录失败，用户名密码不正确" };

        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

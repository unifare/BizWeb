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
using SqlSugar;
using UniOrm.Model;

namespace UniNote.WebClient.Controllers
{
    [Area("sd23nj")] 
    [Route("sd23nj/[controller]/[action]")]
    [AdminAuthorize]
    public class AdminController : Controller
    {
        ISqlSugarClient dbFactory;
        public AdminController(ISqlSugarClient _dbFactory)
        {
            dbFactory = _dbFactory;
        }

        public IActionResult Index()
        {
           var sh= dbFactory.Queryable<SystemHtml>().Where(p => p.Name == "网站管理目录").Single();
            ViewBag.nav = sh.Value;
            return View();
        }
        public IActionResult console()
        {
            return View();
        }

        public IActionResult AllCon()
        {
            return View();
        }



        public IActionResult FileMng()
        {
            return View();
        }

        public IActionResult urlmng()
        {
            return View();
        }

        public IActionResult UserList()
        {
            return View();
        }

        //[HttpPost]
        //public  object  AddUser([FromBody]AConFlowStep id)
        //{
        //    return View();
        //}

        public IActionResult welcome()
        {
            return View();
        }
        public IActionResult UserMng()
        {
            return View();
        }

        public IActionResult VueGrid()
        {
            return View();
        }

        public IActionResult UserList2()
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

        public IActionResult memberlist()
        {
            return View();
        }
        public IActionResult orderlist()
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
                identity.AddClaim(new Claim(ClaimTypes.Sid, userName));
                identity.AddClaim(new Claim(ClaimTypes.Name, userName));
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin")); 
                await HttpContext.SignInAsync(authenticationType, new ClaimsPrincipal(identity));
              
                return new { isok = true, msg = "" };

            }
            return new { isok = false, msg = "登录失败，用户名密码不正确" };

        }


    }
}

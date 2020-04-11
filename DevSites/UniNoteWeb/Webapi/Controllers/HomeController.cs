using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using UniNote.WebApi;
using UniOrm;
using UniOrm.Startup.Web;

namespace TestWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IDbFactory dbFactory;
        HttpClient client;
        public HomeController(IDbFactory _dbFactory)
        {
            client = new HttpClient();
            dbFactory = _dbFactory;
        }
        [AllowAnonymous]
        public IActionResult noauth()
        {

            //User.Identity.Name
            //var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            //{ Address = "http://oauth.66wave.com/", Policy = { RequireHttps = false } });
            //if (disco.IsError)
            //    throw new Exception(disco.Error);

            //var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            //var response = await client.GetUserInfoAsync(new UserInfoRequest
            //{
            //    Address = disco.UserInfoEndpoint,
            //    Token = accessToken
            //});
            ////disco.UserInfoEndpoint
            ////var ss = dbFactory.EFCore<pigcms_adma>().CreateDefaultInstance();

            ////var qlist = ss.From<pigcms_adma>();
            ////var allist = qlist.ToList<pigcms_adma>();
            return View();
        }
        public async Task<IActionResult> Index()
        {

            //User.Identity.Name
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            { Address = "http://oauth.66wave.com/", Policy = { RequireHttps = false } });
            if (disco.IsError)
                throw new Exception(disco.Error);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = accessToken
            });
            //disco.UserInfoEndpoint
            //var ss = dbFactory.EFCore<pigcms_adma>().CreateDefaultInstance();

            //var qlist = ss.From<pigcms_adma>();
            //var allist = qlist.ToList<pigcms_adma>();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync();
            // await HttpContext.SignOutAsync(new List<string> { "Cookies", "oidc" });
            // await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("Cookies");
            //var refererUrl = Request.Headers["Referer"].ToString();
            return new SignOutResult(new List<string> { "Cookies", "oidc" },
              new AuthenticationProperties { RedirectUri = "/home/noauth" });
            //  return Redirect("/home/noauth");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

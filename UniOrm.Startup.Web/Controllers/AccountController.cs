using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;  
using UniOrm;

namespace UniOrm.Startup.Web.Controllers
{ 
    [Authorize]
    public partial class AccountController : Controller
    {
        //IDbFactory dbFactory;
        public AccountController( )
        {
            //dbFactory = _dbFactory;
        }

        [AllowAnonymous]
        public async Task<RedirectResult> SignOut()
        {
            await HttpContext.SignOutAsync( );
            await HttpContext.SignOutAsync("oidc");
            return Redirect("~/");
        }
       
    }
}

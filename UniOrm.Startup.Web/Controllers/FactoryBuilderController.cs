using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniOrm.Application;
using UniOrm.Startup.Web.DynamicController;

namespace UniOrm.Startup.Web.Controllers
{
    [AllowAnonymous]
    
    [Authorize]
    [UserAuthorize]
    [AdminAuthorize]
    public class FactoryBuilderController : Controller
    {
     
        //IGodWorker TypeMaker; 
        public FactoryBuilderController( )
        {
          
        }

        public async Task<IActionResult> Index()
        {
          
            return new EmptyResult();
        }
    }
}

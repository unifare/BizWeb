using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniOrm.Application;

namespace UniOrm.Startup.Web.Controllers
{
    [AllowAnonymous]
    [AdminAuthorize]
    [UserAuthorize]

    public class FactoryBuilderController : Controller
    {
        //IGodWorker TypeMaker;
        //public FactoryBuilderController(IGodWorker typeMaker)
        //{
        //    TypeMaker = typeMaker;
        //}

        public async Task<IActionResult> Index()
        {
            //var ss = dbFactory.EFCore<pigcms_adma>().CreateDefaultInstance();

            //var qlist = ss.From<pigcms_adma>();
            //var allist = qlist.ToList<pigcms_adma>();
           // await TypeMaker.Run(ControllerContext); 
            return new EmptyResult();
        }
    }
}

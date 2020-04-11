using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; 
using UniNote.Web.Model;
using UniNote.WebApi;
using UniOrm; 

namespace TestWeb.Controllers
{
    public class PageController : Controller
    {
        IDbFactory dbFactory;
        public PageController(IDbFactory _dbFactory)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

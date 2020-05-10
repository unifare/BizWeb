using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UniNote.WebClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public  IEnumerable<string>  Get()
        {
            return new string[] { "value1", "value2" };
            //var dss = User.Identity.Name;
            //return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    } 
}

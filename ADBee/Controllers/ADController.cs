using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ADBee.Models;
using UniOrm.Common;
using ADBee.Data;

namespace ADBee.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class ADController : ControllerBase
    {
        private readonly ADSystemDbContext _lGDbContext;
        public ADController(ADSystemDbContext lGDbContext)
        {
            _lGDbContext = lGDbContext;
        }
        [HttpGet]
        public void Visit([FromQuery]string id)
        {
            var user = _lGDbContext.Advertisements.FirstOrDefault(a => a.Ad_Uuid == id);
            if( user!=null)
            {
              
            }
        }
    }
    
}

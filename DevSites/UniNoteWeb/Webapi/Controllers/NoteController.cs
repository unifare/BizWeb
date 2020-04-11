using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniNote.Web.Model;
using UniOrm;
using UniOrm.Startup.Web;

namespace UniNote.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {  
        IDbFactory dbFactory;
        public NoteController(IDbFactory _dbFactory)
        {
            dbFactory = _dbFactory;

            var ss = dbFactory.EFCore<pigcms_adma>().CreateDefaultInstance();

            var qlist = ss.From<pigcms_adma>();
            var allist = qlist.ToList<pigcms_adma>();
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

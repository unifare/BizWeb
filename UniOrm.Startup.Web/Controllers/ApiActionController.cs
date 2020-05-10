using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniOrm;
using UniOrm.Startup.Web; 
using UniOrm.Common;
using SqlSugar;
using UniOrm.Startup.Web.DynamicController;
using System;

namespace UniOrm.Startup.Web.Controllers
{
    [Route("api/[controller]/[Action]")]
    public partial class ApiActionController : ControllerBase
    {
        ISqlSugarClient dbFactory;
        IAuthorizeHelper authorizeHelper;
        public ApiActionController(ISqlSugarClient _dbFactory, IAuthorizeHelper _authorizeHelper)
        {
            dbFactory = _dbFactory;
            authorizeHelper = _authorizeHelper;
        }


        [AdminAuthorize]
        public object AddAction(string source,[FromServices]DynamicActionProvider actionProvider,
            [FromServices] DynamicChangeTokenProvider tokenProvider)
        {
            try
            {
                actionProvider.AddControllers(source);
                tokenProvider.NotifyChanges();
                return new { isok = true };
            }
            catch (Exception ex)
            {
                return new { isok = false, err = ex.Message };
            }
        }


 

    }
}

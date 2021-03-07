using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UniOrm.Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using UniOrm.Startup.Web.DynamicController;

namespace UniOrm.Startup.Web
{
    public class GlobalActionFilter : IAsyncActionFilter
    {
        public static bool isBuild = false;
        private IHttpContextAccessor _accessor;
        IGodWorker TypeMaker;
        private readonly DynamicActionProvider _actionProvider;
        private readonly DynamicChangeTokenProvider _dynamicChangeTokenProvider;
        public GlobalActionFilter(IGodWorker typeMaker, IHttpContextAccessor _accessor, DynamicActionProvider dynamicActionProvider, DynamicChangeTokenProvider dynamicChangeToken)
        {
            TypeMaker = typeMaker;
            _actionProvider = dynamicActionProvider;
            _dynamicChangeTokenProvider = dynamicChangeToken;
        }
         
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           
            //var factory = context.HttpContext.RequestServices.GetService<ILoggerFactory>();
            // var s = context.HttpContext.Request.Path;
            //var logger = factory.CreateLogger<GlobalActionFilter>(); 
            await ExcuteFilter(context,next);
            if (!isBuild)
            {
                WebSetupExtension.BuildAllDynamicActions(_actionProvider, _dynamicChangeTokenProvider);
                isBuild = true;
            }
        }

        //public async Task OnActionExecutingAsync(ActionExecutingContext context)
        //{
        //    await ExcuteFilter(context);
        //}


        public static string regtext = @"\.(css|ico|jpg|jpeg|png|gif|bmp|js)+\?*.*$";
        public static Regex reg = new Regex(regtext);

        private async Task ExcuteFilter(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var repath = context.HttpContext.Request.Path.Value.ToLower();

            //if (repath.StartsWith("/sdfsdf/") || repath.StartsWith("/api/fact") || reg.IsMatch(repath))
            //{
            //    await next();

            //}
            //else
            //{
                await TypeMaker.Run(context);
                if (context.Result == null)
                {
                    await next();
                }

           // }
        
        }
    }
}

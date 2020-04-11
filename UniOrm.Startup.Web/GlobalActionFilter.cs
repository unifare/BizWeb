using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UniOrm.Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
namespace UniOrm.Startup.Web
{
    public class GlobalActionFilter : IAsyncActionFilter
    {
        private IHttpContextAccessor _accessor;
        IGodWorker TypeMaker;
        public GlobalActionFilter(IGodWorker typeMaker, IHttpContextAccessor _accessor)
        {
            TypeMaker = typeMaker;
        }
         
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //var factory = context.HttpContext.RequestServices.GetService<ILoggerFactory>();
            // var s = context.HttpContext.Request.Path;
            //var logger = factory.CreateLogger<GlobalActionFilter>(); 
           await ExcuteFilter(context,next); 
            // logger.LogWarning("全局ActionFilter执行之后");
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

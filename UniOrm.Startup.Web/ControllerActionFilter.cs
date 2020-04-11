using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace UniOrm.Startup.Web
{
    public class ControllerActionFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Logger.LogInfo("GlobalActionFilter", "全局ActionFilter执行之前");
            await next();
            Logger.LogInfo("GlobalActionFilter", "全局ActionFilter执行之前");
        }
    }
}

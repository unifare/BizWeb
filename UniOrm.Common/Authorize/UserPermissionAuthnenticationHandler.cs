using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers; 

namespace UniOrm.Common.Authorize
{
    public class UserPermissionAuthnenticationHandler : AuthorizationHandler<UserPermissionRequirement>
    { 
        public IAuthenticationSchemeProvider Scheme;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserPermissionAuthnenticationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserPermissionRequirement requirement)
        {
            //.net core 3就改成了endpoint了,这里算是一个不小的坑
            if (context.Resource is Endpoint endpoint)
            {
                var controllActionDesription = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                string controllerName = controllActionDesription.RouteValues["Controller"].ToLower();
                string actionName = controllActionDesription.RouteValues["Action"].ToLower();
                context.Succeed(requirement);
                List<UserPermissionItem> permissionList = UserPermissionDictionary.Get(context.User.Identity.Name);
                if (permissionList?.Where(w => w.ControllerName == controllerName && w.ActionName == actionName).Count() > 0)
                {
                    //验证通过就正常执行
                    context.Succeed(requirement);
                }
                else
                {
                    //不通过就失败
                    context.Fail();
                }
            }
            await Task.CompletedTask;
        }
    } 
}

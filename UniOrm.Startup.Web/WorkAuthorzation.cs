/*
 * ************************************
 * file:	    WorkAuthorzation.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/16 12:40:38
 * description:	
 * ************************************
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UniOrm.Startup.Web
{
    public class WorkAuthorzation : AuthorizeFilter
    { //
        // 摘要:
        //     Initializes a new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter instance.
      

        public WorkAuthorzation(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizeData> authorizeData) : base(policyProvider,authorizeData)
        {

        }
      
        ///  请求验证，当前验证部分不要抛出异常，ExceptionFilter不会处理
        /// </summary>
        /// <param name="context">请求内容信息</param>
        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
//            context.ActionDescriptor as ControllerActionDescriptor;  //获取请求进来的控制器与方法

//controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)

//.Any(a => a.GetType().Equals(typeof(NoPermissionRequiredAttribute)))      判断请求的控制器和方法有没有加上NoPermissionRequiredAttribute（不需要权限）

 

//string.IsNullOrWhiteSpace(context.HttpContext.Request.Query["LoginInfo"].ToString())     判断请求头是否有标识
            if (IsHaveAllow(context.Filters))
            {
                return;
            }


            ////解析url
            //// {/ Home / Index}
            //var url = context.HttpContext.Request.Path.Value;
            //if (string.IsNullOrWhiteSpace(url))
            //{
            //    return;
            //}

            //var list = url.Split("/");
            //if (list.Length <= 0 || url == "/")
            //{
            //    return;
            //}
            //var controllerName = list[1].ToString().Trim();
            //var actionName = list[2].ToString().Trim();


            ////验证
            //var flag = PowerIsTrue.IsHavePower(controllerName, actionName);
            //if (flag.Item1 != 0)
            //{

            //    context.Result = new RedirectResult("/Home/Index");
            //}
        }


        //判断是否不需要权限

        public static bool IsHaveAllow(IList<IFilterMetadata> filers)
        {
            for (int i = 0; i < filers.Count; i++)
            {
                if (filers[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }
            return false;

        }

    }
}

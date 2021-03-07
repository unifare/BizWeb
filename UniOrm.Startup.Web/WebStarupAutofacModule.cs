using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniOrm.Common;

namespace UniOrm.Startup.Web
{
    public class WebStarupAutofacModule : Microsoft.AspNetCore.Http.IMiddleware
    {
      
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            return  next(context);
        }
    }
}

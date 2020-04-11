using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UniOrm.Common.Middlewares
{
    public class ThemeMiddleware
    {
        private readonly RequestDelegate _next;
        public IConfiguration _configuration;
        public ThemeMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public Task Invoke(HttpContext context)
        {
            var folder = _configuration.GetSection("theme").Value;
            context.Request.HttpContext.Items[ThemeViewLocationExpander.ThemeKey] = folder ?? "";
            return _next(context);
        }
    }
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// 启用Theme中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTheme(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ThemeMiddleware>();
        }
    }

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Theme服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTheme(this IServiceCollection services)
        {
            return services.Configure<RazorViewEngineOptions>(options => {
                options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());
            });
        }
    }

}

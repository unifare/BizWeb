/*
 * ************************************
 * file:	    Class1.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/5/12 8:42:09
 * description:	
 * ************************************
 */

using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniOrm.Startup.Web.Views
{
    public class TemplateViewLocationExpander : IViewLocationExpander
    {


        public IConfiguration _configuration;


        public TemplateViewLocationExpander(  IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var template = context.Values["template"] ?? "Default";
            var folder = _configuration.GetValue<string>("APP:AppTheme");
            string[] locations = { $"/{folder}/" + template + "/{1}/{0}.cshtml", $"/{folder}/" + template + "/{0}.cshtml", $"/{folder}/" + template + "/Shared/{0}.cshtml" };
            return locations.Union(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["template"] = context.ActionContext.RouteData.Values["Template"]?.ToString() ?? "Default";
        }
    }
}

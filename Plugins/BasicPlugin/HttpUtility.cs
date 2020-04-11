 using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; 
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicPlugin
{
 
    public class HttpUtility
    {
        static ConcurrentBag<RegexEn> regexEns = new ConcurrentBag<RegexEn>();
   
        public static string GetIsTriger(List<dynamic> assrules, HttpContext context  )
        {
            //var assrules = assrulesPbj.AsDynamic();
            //var url = urlobj.AsDynamic() ;
            // dynamic assrules = new JObjectAccessor(assrulesPbj as string);

            //var assrules = (List<TrigerRuleInfo>)runtimeArgument.ResourceList.FirstOrDefault(p => p.KeyName == "allrules").RuntimeValue;
            //var url = runtimeArgument.ResourceList.FirstOrDefault(p => p.KeyName == "url").Value;
            //List<sdf> sdf = new List<sdf>();
            //sdf.Count
            //var allrows = (int)inparams1.GetPropertyValue("Count");
            var url= context.Request.Path.Value;
            var method = context.Request.Method;
            for (var i = 0; i < assrules.Count; i++)
            {
                var TrigerRow = assrules[i];
                //if(string.IsNullOrEmpty( TrigerRow.HttpMethod ?? "GET"))
                //{
                //    TrigerRow.HttpMethod = "GET";
                //}
                if (   string.Compare( TrigerRow.HttpMethod , method, true)==0 )
                {
                    string ruleText = TrigerRow.Rule;
                    if (string.IsNullOrEmpty(ruleText))
                    {
                        continue;
                    }
                    else
                    {
                        Regex reg = null;
                        var exsUrlrule = regexEns.FirstOrDefault(p => p.RuleText == TrigerRow.Rule);
                        if (exsUrlrule == null)
                        {
                            reg = new Regex(TrigerRow.Rule);
                            exsUrlrule = new RegexEn() { Regex = reg, RuleText = TrigerRow.Rule };
                            regexEns.Add(exsUrlrule);
                        }
                        else
                        {
                            reg = exsUrlrule.Regex;
                        }
                        var ismach = reg.IsMatch(url);
                        if (ismach)
                        {
                            return TrigerRow.ComposityId;
                        }
                    }
                }
            }


            return "";
        }
  
        public static object GetRequestUrl(HttpContext context)
        {
            return context.Request.Path.Value; 
        }

        public static HttpContext GetHttpContext (HttpContext context)
        {
            return context;
        }

        public static string GetRequestParam(HttpContext context,string key)
        {
            return context.Request.Query[key];
        }


        public static void ResponseText(ActionExecutingContext action,  string returnObject)
        { 
            action.Result = new ContentResult()
            {
                Content = returnObject,
                 ContentType = "text/html",
                StatusCode = 200
            };

        }
        public static void ResponseUnAuth(ActionExecutingContext action, string returnObject)
        {
            action.Result = new ContentResult()
            {
                Content = returnObject,
                ContentType = "text/html",
                StatusCode = 401
            };

        }
        public static void ResponseAjaxt(ActionExecutingContext action, object returnObject )
        { 
            action.Result = new JsonResult(returnObject)
            {  
                StatusCode = 200
            };
        }
    }
}

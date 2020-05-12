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
using Newtonsoft.Json;
using System.Text.Json;
using UniOrm;

namespace BasicPlugin
{
 
    public class HttpUtility
    {
        private static readonly string LoggerName = "HttpUtility";
        static ConcurrentBag<RegexEn> regexEns = new ConcurrentBag<RegexEn>();
   
        public static object GetIsTriger(List<dynamic> assrules, HttpContext context  )
        { 
            var url= context.Request.Path.Value;
            var method = context.Request.Method;
            for (var i = 0; i < assrules.Count; i++)
            {
                var TrigerRow = assrules[i]; 
                if ( string.IsNullOrEmpty(TrigerRow.HttpMethod)||  TrigerRow.HttpMethod.Trim().ToLower().Contains(method.ToLower()))
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
                        var maches = reg.Matches(url);
                        if (maches.Count>0)
                        { 
                            Logger.LogInfo(LoggerName, $"url: {url} ,medth: {method} ComposityId :{TrigerRow.ComposityId}");
                            return new { comid = TrigerRow.ComposityId, sections = maches };
                        }
                    }
                }
            }

            Logger.LogDebug(LoggerName, $"url: {url} ,medth: {method} not found");
            return new { comid = ""  };
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

        public static void ResponseJson(ActionExecutingContext action, object returnObject)
        {
            action.Result = new JsonResult(returnObject)
            {
                ContentType = "application/json",
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
     
    }
}

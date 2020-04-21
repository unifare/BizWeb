using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniOrm.Model;
using UniOrm.Common;
using SqlKata.Execution;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using CSScriptLib;
using System.Drawing;
using System.Threading.Tasks;

namespace UniOrm
{
    public static class RazorToolEx{
        public static object D2O (this object dynamicObj)
        {
            return  MagicExtension.BackToInst(dynamicObj);
        }
    }
    public class RazorTool
    {
        public ActionExecutingContext ActionContext { get; set; }
        public HttpContext HttpContext { get; set; } 
        public Dictionary<string, MethodDelegate> Funs { get; set; }
        public AConFlowStep Step { get; set; }
        public Dictionary<string, object> ResouceInfos { get; set; }

        public RazorTool()
        {
           
            IHttpContextAccessor factory = APPCommon.ApplicationServices.GetService<IHttpContextAccessor>();
            HttpContext = factory.HttpContext;
        }
        public string V(string key)
        {
            return APPCommon.AppConfig.GetDicstring(key);
        }

        public string L(string key)
        {
            var restr = string.Empty;
            var localcookie = GetCookies("__locallang");

            var localquery = HttpContext.Request.Query["lang"];
            if (!string.IsNullOrEmpty(localquery))
            {
                var lmodel = APPCommon.GetLoalLang(key, localquery).Result;
                restr = GetDefaultValue(key, ref lmodel);
                SetCookies("__locallang", localquery);
            }
            else
            {
                if( string.IsNullOrEmpty(localcookie))
                {
                    var lmodel = APPCommon.GetLoalLang(key, null, 0).Result;
                    restr = GetDefaultValue(key, ref lmodel);
                    restr = lmodel.Value;
                }
                else
                {
                     var lmodel = APPCommon.GetLoalLang(key, localcookie ).Result;
                    restr = GetDefaultValue(key, ref lmodel);
                    restr = lmodel.Value;
                }
               
            }
            return restr;
        }

        private static string GetDefaultValue(string key, ref LocalLangs lmodel)
        {
            string restr;
            if (lmodel == null)
            {
                lmodel = APPCommon.GetLoalLang(key, null, 0).Result;
            }
            if (lmodel == null)
            {
                restr = string.Empty;
            }
            else
            {
                restr = lmodel.Value;
            }

            return restr;
        }

        public string L(string key, int lang)
        {
            var s = APPCommon.GetLoalLang(key, null, lang).Result;
            return s.Value;
        }

        public string L(string key, string langName)
        {
            var s = APPCommon.GetLoalLang(key, langName).Result;
            return s.Value;
        }

        public string Include(string RelativefilePath)
        {
            RelativefilePath = Path.Join(  APPCommon.UserUploadBaseDir,  RelativefilePath.UrlDecode());
            var fullpath = RelativefilePath.ToServerFullPath();
            var content = fullpath.ReadAsTextFile();
            return content;
        }

        public List<dynamic> GetData(string sql, object args)
        {
            return DB.UniClient.Ado.SqlQuery<dynamic>(sql, args);
        }

        private QueryFactory kata;
        public QueryFactory Kata
        {
            get
            {
                if (kata == null)
                {
                    return DB.Kata;
                }
                else
                {
                    return kata ;
                }
            }
        }

        public IFormCollection Form
        {
            get
            {
                return HttpContext.Request.Form;
            } 
        }

        public int Insert(string tablenmae,object inserObject)
        {
          return Kata.Query(tablenmae).Insert(inserObject);
        }

        public IQueryCollection Query
        {
            get
            {
                return HttpContext.Request.Query;
            }
        }
        public object F(string funcName,object arags)
        {
            if (this.Funs == null)
            {
                return null;
            }
            else
            {
                if (this.Funs.ContainsKey(funcName))
                {
                   return Funs[funcName]( arags);
                }
            }
            return null;
        }
        public object R(string key)
        {
            if (this.ResouceInfos == null)
            {
                return null;
            }
            else
            {
                if (this.ResouceInfos.ContainsKey(key))
                {
                    return ResouceInfos[key].AsDynamic();
                }
            }
            return null;
        }
        public int Rand(int min, int max)
        {
            return new Random(DateTime.Now.Millisecond).Next(min, max);
        }
    

        public Image ImageFromFile(string relativePath)
        {
          return  Image.FromFile(relativePath.ToServerFullPathEnEnsure());
        }

        public Image ImageSetText(string relativePath)
        {
            return Image.FromFile(relativePath.ToServerFullPathEnEnsure());
        }

        public object R2I(object DynaObject)
        {
            return MagicExtension.BackToInst(DynaObject); 
        }

        public dynamic FormToOjbect(  ) 
        { 
            var tablename = Form["_tablename"];
             var cols=   DB.UniClient.DbMaintenance.GetColumnInfosByTableName(tablename); 
            dynamic result = new System.Dynamic.ExpandoObject();
            foreach(var cinfo in cols)
            {
                if(Form.ContainsKey(cinfo.DbColumnName))
                { 
                (result as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>(cinfo.DbColumnName,
                  Form[cinfo.DbColumnName])); 
                }
            }
            return result;
        }
        public Dictionary<string,object> FormToDic( string tablename)
        { 
            var cols = DB.UniClient.DbMaintenance.GetColumnInfosByTableName(APPCommon.GetWTableName(tablename));
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var cinfo in cols)
            {
                if (Form.ContainsKey(cinfo.DbColumnName))
                {
                    dic.Add(cinfo.DbColumnName, Form[cinfo.DbColumnName]);
                }
            }
            return dic;
        }
        public Dictionary<string, object> QueryToDic(string tableKey)
        {
            if(!Query.ContainsKey(tableKey))
            {
                return new Dictionary<string, object>();
            }
            var tablename = Query[tableKey];
            var cols = DB.UniClient.DbMaintenance.GetColumnInfosByTableName(tablename);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var cinfo in cols)
            {
                if (Query.ContainsKey(cinfo.DbColumnName))
                {
                    dic.Add(cinfo.DbColumnName, Query[cinfo.DbColumnName]);
                }
            }
            return dic;
        }

        public int InsertForm()
        {
            var tablename = Form["_tablename"];
            var obj = FormToDic(tablename);
            return DB.Kata.Query(APPCommon.GetWTableName(tablename)).Insert(obj);
        }

        public void  Auth()
        {
            if ( !HttpContext.User.Identity.IsAuthenticated)
            {
                APPCommon.ResponseUnAuth(ActionContext, null); 
            } 
        }

        public bool IsAuth()
        {
            if ( HttpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }


        public object ReTrueJson(object obj)
        { 
            return new { isok = true, data = obj};
        }

        public object ReFalseJson(object obj)
        {
            return new { isok = false, data = obj };
        }

        public int InsertQuery(string tableKey)
        { 
            var obj = QueryToDic(tableKey);
            return DB.Kata.Query(tableKey).Insert(obj);
        }

        //public dynamic GetData(string sql, object[] args)
        //{
        //    var aConFlowSteps = DB.Inst.Queryable<AConFlowStep>().ToList();
        //}

        public string Session(string key)
        {
            return HttpContext.Session.GetString(key);
        }

        public void Session(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }

        public string GetIp( )
        {
            var ip =  Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }


        public string GetLocalIp()
        {
            return Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
                //":"
                //+ Request.HttpContext.Connection.LocalPort);
            //return str;
        }


        public string Url(string key)
        {
            return HttpContext.Request.Query[key];
        }


        public string UrlQuery(string key)
        {
            return HttpContext.Request.Query[key];
        }

        public string GetForm(string key)
        {
            return HttpContext.Request.Form[key];
        }

        public HttpRequest Request
        {
            get
            {
                return HttpContext.Request;
            }
        }

        public HttpResponse Respone
        {
            get
            {
                return HttpContext.Response;
            }
        }
        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="minutes">过期时长，单位：分钟</param>      
        public void SetCookies(string key, string value, int minutes = 30)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }
        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        public void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        public string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
    }
}

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
using PetaPoco.NetCore;
using System.Dynamic;
using System.Collections;
using SqlKata;
using System.Text.RegularExpressions;
using UniOrm.Common.Core;

namespace UniOrm
{
    public static class RazorToolEx{
        public static object D2O (this object dynamicObj)
        {
            return  MagicExtension.BackToInst(dynamicObj);
        }

        public static IEnumerable<dynamic> List(this Sql sql )
        {
            var result = DB.Peta.Query<dynamic>( sql);//第一个参数是页码，第二个参数是页容量，第三个参数是SQL语句  
            return result;
        }

        public static DataPage<dynamic> Page(this Query query, int pageindex = 1, int pagesize = 30)
        {
            //var result = query.ForPage(pageindex,pagesize).Get();//第一个参数是页码，第二个参数是页容量，第三个参数是SQL语句  
            var presult = query.Paginate(pageindex, pagesize);
            var page = new DataPage<dynamic>();
            page.CurrentPage = presult.Page;
            page.Items = presult.List.ToList();
            page.ItemsPerPage = presult.PerPage;
            page.TotalPages = presult.TotalPages;
            page.TotalItems = presult.Count;


            return page;
        }

        public static Page<dynamic> Page( this Sql sql, int pageindex=1,int pagesize=30)
        { 
            var result = DB.Peta.Page<dynamic>(pageindex, pagesize, sql);//第一个参数是页码，第二个参数是页容量，第三个参数是SQL语句  
            return result;
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

        public static bool IsPropertyExist(dynamic data, string propertyname)
        {
            if (data is ExpandoObject)
                return ((IDictionary<string, object>)data).ContainsKey(propertyname);
            return data.GetType().GetProperty(propertyname) != null;
        }

        public bool HasPro(dynamic data, string propertyname)
        {
            if (data is ExpandoObject)
                return ((IDictionary<string, object>)data).ContainsKey(propertyname);
            return data.GetType().GetProperty(propertyname) != null;
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
                    restr = lmodel?.Value;
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
        
        public  string TmplHtml(string templateKey, object model = null, ExpandoObject viewBag = null)
        {
            var content =  APPCommon.RenderRazorKey(templateKey, model, viewBag)?.GetAwaiter().GetResult();
            return content;
        }
        //public string DBHtml(string key)
        //{
        //    return DB.UniClient.Queryable<i(APPCommon.GetWTableName(tablename)).Insert(obj);
        //}
        public dynamic NewObj()
        {
            return new ExpandoObject();
        }

        Uni _url2 = null;

        public Uni Url2 {
            get
            {
                if(_url2 == null)
                {
                    _url2 = new Uni();
                    foreach(var i in Request.Query)
                    {
                        if(!_url2.Dictionary.ContainsKey(i.Key))
                        {
                            _url2.Dictionary.Add(i.Key, i.Value);
                        }
                    }
                }
                return _url2;
            }
        }
        Uni _form2 = null;

        public Uni Form2
        {
            get
            {
                if (_form2 == null)
                {
                    _form2 = new Uni();
                    foreach (var i in Request.Form)
                    {
                        if (!_form2.Dictionary.ContainsKey(i.Key))
                        {
                            _form2.Dictionary.Add(i.Key, i.Value);
                        }
                    }
                }
                return _form2;
            }
        }

        public Stream Stream
        {
            get
            { 
                return Request.Body;
            }
        }
        public string StreamToText()
        { 
            using (StreamReader sr = new StreamReader(Request.Body))
            {
              return   sr.ReadToEnd(); 
            } 
        }

        public bool SaveFile(int index, string dir,string newfileName=null)
        {
            if(Request.Form.Files.Count<=index)
            {
                return false;
            }
            else
            {
                Request.Form.Files[index].UploadSaveSingleFile(dir, newfileName);
                return true;
            }
        }

        public List<dynamic> GetData(string sql, object args)
        {
            return Db.Ado.SqlQuery<dynamic>(sql, args) ;
        }

        //public Page<dynamic> DataSql(int pageIndex = 1, int pageSize = 30, string sql =null, params object[] args )
        //{
        //    if( sql==null)
        //    {
        //        return null;
        //    }
        //    Sql Sql = new Sql(sql);
        //    if(args!=null)
        //    { 
        //        Sql = new Sql(sql, args); 
        //    }  
        //    var result = DB.Peta.Page<dynamic>(pageIndex, pageSize, Sql);//第一个参数是页码，第二个参数是页容量，第三个参数是SQL语句  
        //    return result;
        //}

        public Sql Sql( string sql , params object[] args)
        {
            if (sql == null)
            {
                return null;
            }
            Sql Sql = new Sql(sql);
            if (args != null)
            {
                Sql = new Sql(sql, args);
            }
            var result = DB.Peta.Query<dynamic>(  Sql);//第一个参数是页码，第二个参数是页容量，第三个参数是SQL语句  
            return Sql;
        }

     

        public Query DataObj( string tableName ,
          string orderby = null,
          List<dynamic> oargs = null,
        object where = null )
        {

            var result = DB.Kata.Query(tableName);
            if (where != null)
            {
                result = result.Where(where);
            }

            if (orderby != null)
            {
                if (oargs != null && oargs.Any())
                {
                    result = result.OrderByRaw(orderby, oargs);
                }
                else
                {
                    result = result.OrderByRaw(orderby);
                }

            }
          
            return result;
        }

        public Query Data(string tableName, 
            string orderby = null,
            List<dynamic> oargs = null, 
            params object[] where
            )
        {

            var result = DB.Kata.Query(tableName);
            if (where != null && where.Any())
            {
                foreach (object item in where)
                {
                    if (string.Compare(item.GetProp("c").ToSafeString() , "or", true) == 0)
                    {
                        result = result.OrWhere(item.GetProp("name").ToString(), item.GetProp("op").ToString(), item.GetProp("value"));
                    }
                    else
                    {
                        result = result.Where(item.GetProp("name").ToString(), item.GetProp("op").ToString(), item.GetProp("value"));
                    }
                }
            }

            if (orderby != null)
            {
                if (oargs != null && oargs.Any())
                {
                    result = result.OrderByRaw(orderby, oargs);
                }
                else
                {
                    result = result.OrderByRaw(orderby);
                }

            }
             
            return result;
        }

        public Query DataStr(string tableName, IEnumerable<object[]> where,
            string orderby = null,
            List<dynamic> oargs = null
            )
        {

            var result = DB.Kata.Query(tableName);
            if (where != null && where.Any())
            {
                foreach (var item in where)
                {
                    if (string.Compare(item[0].ToString(),"or",true) ==0)
                    {
                        result = result.OrWhere(item[1].ToString(), item[2].ToString(), item[3]);
                    } 
                    else
                    {
                        result = result.Where( item[1].ToString(), item[2].ToString(), item[3]);
                    }
                }
            }

            if (orderby != null)
            {
                if (oargs != null && oargs.Any())
                {
                    result = result.OrderByRaw(orderby, oargs);
                }
                else
                {
                    result = result.OrderByRaw(orderby);
                }

            } 
            return result;
        }

        private ISqlSugarClient _Db;
        public ISqlSugarClient Db
        {
            get
            {
                if (kata == null)
                {
                    _Db= DB.UniClient;
                }
               
              return _Db;
                 
            }
        }
        private QueryFactory kata;
        public QueryFactory Kata
        {
            get
            {
                if (kata == null)
                {
                    kata = DB.Kata;
                } 
                return kata; 
            }
        }

        public IFormCollection FormCollection
        {
            get
            {
                return HttpContext.Request.Form;
            } 
        }
        public string Form(string key)
        { 
            return FormCollection[key].ToString();
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
        public dynamic R(string key)
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
            var tablename = FormCollection["_tablename"];
             var cols=   DB.UniClient.DbMaintenance.GetColumnInfosByTableName(tablename); 
            dynamic result = new System.Dynamic.ExpandoObject();
            foreach(var cinfo in cols)
            {
                if(FormCollection.ContainsKey(cinfo.DbColumnName))
                { 
                (result as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>(cinfo.DbColumnName,
                  FormCollection[cinfo.DbColumnName])); 
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
                if (FormCollection.ContainsKey(cinfo.DbColumnName))
                {
                    dic.Add(cinfo.DbColumnName, FormCollection[cinfo.DbColumnName]);
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
            var tablename = FormCollection["_tablename"];
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

        public string Router(int order)
        {
            return R("_sections")[order].Value;
        }


        public string UrlQuery(string key)
        {
            return HttpContext.Request.Query[key];
        }

        public string GetForm(string key)
        {
            return HttpContext.Request.Form[key];
        }

        public ResultInfoBase SaveFiles(  string dirName)
        {
            return APPCommon.UploadFile(Request, dirName);
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

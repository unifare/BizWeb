using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using UniOrm;
using UniOrm.Common;
using UniOrm.Model;
using UniOrm.Model.DataService;
using UniOrm.Startup.Web;
using SqlSugar;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using CSScriptLib;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using SqlKata.Compilers;
using SqlKata;
using SqlKata.Execution;

namespace UniNote.WebClient.Controllers
{
    [Area("sd23nj")]
    [AdminAuthorize]
    public class DbFormController : Controller
    {
        readonly string LoggerName = nameof(DbFormController);
        IHostingEnvironment _hostingEnvironment;
        ISysDatabaseService m_codeService;
        public DbFormController(ISysDatabaseService codeService, IHostingEnvironment hostingEnvironment)
        {

            m_codeService = codeService;
            _hostingEnvironment = hostingEnvironment;

            //var ss = HttpContext.Session["admin"] ?? "";
            //if( )
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DataMng()
        {
            ViewBag.Connections = APPCommon.AppConfig.Connectionstrings;

            return View();
        }

        public IActionResult DelTable(string sqlconnestring, string tname)
        { 
            var sqlcontypestr = sqlconnestring.Substring(0, sqlconnestring.IndexOf('-'));
            var sqlconstring = sqlconnestring.Substring(sqlconnestring.IndexOf('-') + 1);
            var istest = false;

            var db = GetSqlSugarClient(sqlcontypestr, sqlconstring, out istest);
            var dbtablist = db.DbMaintenance.DropTable(tname);
            return new JsonResult(new { isok = true, data = dbtablist });

        }
        public IActionResult GetColumnList(  string sqlconnestring,string tname)
        { 
            var sqlcontypestr = sqlconnestring.Substring(0, sqlconnestring.IndexOf('-'));
            var sqlconstring = sqlconnestring.Substring(sqlconnestring.IndexOf('-') + 1);
            var istest = false;

            var db = GetSqlSugarClient(sqlcontypestr, sqlconstring, out istest);
            var dbtablist = db.DbMaintenance.GetColumnInfosByTableName(tname, false);
            return new JsonResult(new { isok = true, data = dbtablist });

        }

        public IActionResult GetDbTableList(string cmd, string sqlconnestring)
        { 
            var sqlcontypestr = sqlconnestring.Substring(0, sqlconnestring.IndexOf('-'));
            var sqlconstring = sqlconnestring.Substring(sqlconnestring.IndexOf('-') + 1);
            var istest = false;

            var db = GetSqlSugarClient(sqlcontypestr, sqlconstring, out istest);
            var dbtablist=  db.DbMaintenance.GetTableInfoList(false) ;
            return new JsonResult(new { isok = true,   data = dbtablist });
           
        }
        //public IActionResult CreateNewTable(string cmd, string sqlconnestring)
        //{
        //    var isquery = false;
        //    var sqlcontypestr = sqlconnestring.Substring(0, sqlconnestring.IndexOf('-'));
        //    var sqlconstring = sqlconnestring.Substring(sqlconnestring.IndexOf('-') + 1);
        //    var istest = false;

        //    var db = GetSqlSugarClient(sqlcontypestr, sqlconstring, out istest);
        //    var dbtablist = db.DbMaintenance.GetTableInfoList();
        //    return new JsonResult(new { isok = true, data = dbtablist });

        //}

        private SqlSugarClient GetSqlSugarClient(string sqlcontypestr, string sqlconstring,out bool istest)
        {

            istest = false;
            var dttypesqlsugar = DbType.MySql;
            switch (sqlcontypestr)
            {
                case "sqlite":
                    dttypesqlsugar = DbType.Sqlite;
                    break;
                case "mysql":
                    dttypesqlsugar = DbType.MySql;
                    break;
                case "sqlserver":
                    dttypesqlsugar = DbType.MySql;
                    break;
                case "测试数据":
                    istest = true;
                    break;

            }
         

           return new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = sqlconstring,
                    DbType = dttypesqlsugar,//设置数据库类型
                    IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                    InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
                });
        }

        public    IActionResult  UpdateDBRow(string tablename,string key,int id, string keyvalue, string sqlconnestring)
        {
            object dbobject = StrngToObject(tablename, keyvalue, sqlconnestring);

            var db2 = sqlconnestring.GetSqlKataContext();
            int reint = db2.Query(tablename).Where(key, id).Update(ToMap(dbobject)); 
            return new JsonResult(new { isok = true, istest = false, leng = reint, data = new List<int>() });
        }
        private static dynamic ToDynamic(IDictionary<string, object> dict)
        {
            dynamic result = new System.Dynamic.ExpandoObject();

            foreach (var entry in dict)
            {
                (result as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>(entry.Key, entry.Value));
            }

            return result;
        } 
        public    IActionResult  AddDBRow(string tablename, string keyvalue, IDictionary<string,object> keyValues, string sqlconnestring)
        {  
            try
            {
                object dbobject = StrngToObject(tablename, keyvalue, sqlconnestring);

                var db2 = sqlconnestring.GetSqlKataContext();
                int reint = db2.Query(tablename).Insert(ToMap(dbobject));
 

                return new JsonResult(new { isok = true, istest = false, leng = reint, data = new List<int>() });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { isok = true, istest = false, leng = 0, data = new List<int>() });
            } 
        }

        private static object StrngToObject(string tablename, string keyvalue, string sqlconnestring)
        {
            var db = sqlconnestring.GetSqlSugarContext();
            var jobj = JObject.Parse(keyvalue);
            var colsinfo = db.DbMaintenance.GetColumnInfosByTableName(tablename, false);
            var newtype = TypeCreator.NewClassBulder(tablename);
            foreach (var col in colsinfo)
            {
                col.PropertyType = db.DBTypeStringToCShapType(col);
                newtype.AddProperityName(col.DbColumnName, col.PropertyType, col.IsNullable, null);
            }
            var dbobjecttype = CSScript.Evaluator.CompileCode(newtype.ToString()).GetType("css_root+" + tablename);
            var setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            setting.DefaultValueHandling = DefaultValueHandling.Ignore;
            var dbobject = JsonConvert.DeserializeObject(keyvalue, dbobjecttype, setting);
            return dbobject;
        }

        /// <summary>
        /// 
        /// 将对象属性转换为key-value对
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> ToMap(Object o)
        {
            Dictionary<String, Object> map = new Dictionary<string, object>();

            Type t = o.GetType();

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();

                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke(o, new Object[] { }));
                }
            }

            return map;

        }

        public IActionResult EditTableRowDef(string tablename, List<DbColumnInfo> keyvalue, string sqlconnestring)
        { 
            var sqlcontypestr = sqlconnestring.Substring(0, sqlconnestring.IndexOf('-'));
            var sqlconstring = sqlconnestring.Substring(sqlconnestring.IndexOf('-') + 1);
            var istest = false;

            var db = GetSqlSugarClient(sqlcontypestr, sqlconstring, out istest);
            if (istest)
            {
                return new JsonResult(new { isok = true, istest = true, leng = 0, data = new List<int>() });
            }
            var colsinfo = db.DbMaintenance.GetColumnInfosByTableName(tablename,false);
            var lisupdate = new List<  DbColumnInfo>();
            var lisadd= new List<DbColumnInfo>();
            foreach (var col in keyvalue)
            {
                bool isadd = true;
                foreach (var colold in colsinfo) 
                {
                    if(string.Compare( col.DbColumnName,colold.DbColumnName ,true)==0)
                    {
                        isadd = false;
                        var isupdate = false;
                        if ( col.DataType!= colold.DataType)
                        {
                            isupdate = true;
                        }
                        if (col.IsNullable != colold.IsNullable)
                        {
                            isupdate = true;
                        }
                        if (col.Length != colold.Length)
                        {
                            isupdate = true;
                        }
                        if (col.DecimalDigits != colold.DecimalDigits)
                        {
                            isupdate = true;
                        }
                        if (col.ColumnDescription != colold.ColumnDescription)
                        {
                            isupdate = true;
                        }
                        if (col.DefaultValue != colold.DefaultValue)
                        {
                            isupdate = true;
                        }
                        if (isupdate==true)
                        {
                            if(  db.Ado.SqlQuery<dynamic>("select * from "+tablename).Any() )
                            {
                                isupdate = false;
                                Logger.LogDebug(LoggerName, " update table {tablename} failed because of data exsit");
                            }
                        }
                        if (isupdate == true)
                        {
                            lisupdate.Add(col);
                        }
                        
                        break;
                    }
                }
                if( isadd)
                {
                    lisadd.Add(col);
                } 
            }
            var reint = true;
            foreach (var col in lisupdate)
            {
                reint = db.DbMaintenance.UpdateColumn(tablename, col);
            }
            foreach (var col in lisadd)
            {
                reint  = db.DbMaintenance.AddColumn(tablename, col);
            }
            var listdel = new List<DbColumnInfo>();
            foreach (var colold in colsinfo)
            {
                bool isfound = false;
                foreach (var col in keyvalue)
                {
                    if (col.DbColumnName == colold.DbColumnName)
                    {
                        isfound = true; 

                        break;
                    }

                }
                if (!isfound)
                {
                    listdel.Add(colold);
                }
            }
            foreach (var colold in listdel)
            {
                reint = db.DbMaintenance.DropColumn(tablename, colold.DbColumnName);
            } 

            return new JsonResult(new { isok = reint, istest = false, leng = 0, data = new List<int>() });
        }

        //craete table with columns
        public IActionResult AddTableRowDef(string tablename, List<DbColumnInfo> keyvalue, string sqlconnestring)
        { 
            var sqlcontypestr = sqlconnestring.Substring(0, sqlconnestring.IndexOf('-'));
            var sqlconstring = sqlconnestring.Substring(sqlconnestring.IndexOf('-') + 1);
            var istest = false;

            var db = GetSqlSugarClient(sqlcontypestr, sqlconstring, out istest);
            if (istest)
            {
                return new JsonResult(new { isok = true, istest = true, leng = 0, data = new List<int>() });
            }
            var reint = db.DbMaintenance.CreateTable(tablename, keyvalue);
            return new JsonResult(new { isok = true, istest = false, leng = reint, data = new List<int>() });
        }

        public async Task<IActionResult> DelDBRow(string tablename, string id, string sqlconnestring)
        {
            var isquery = false;
            var sqlcontypestr = sqlconnestring.Substring(0, sqlconnestring.IndexOf('-'));
            var sqlconstring = sqlconnestring.Substring(sqlconnestring.IndexOf('-') + 1);
            var istest = false;

            var db = GetSqlSugarClient(sqlcontypestr, sqlconstring, out istest);
            if (istest)
            {
                return new JsonResult(new { isok = true, istest = true, leng = 0, data = new List<int>() });
            }
            var reint =  await  db.Ado.ExecuteCommandAsync("delete from "+ tablename + " where id= "+id );
            return new JsonResult(new { isok = true, istest = false, leng = reint, data = new List<int>() });
        }

        public async Task<IActionResult> ExcuteCmd(string cmd ,string sqlconnestring)
        {
            var isquery = false;
            var sqlcontypestr = sqlconnestring.Substring(0, sqlconnestring.IndexOf('-'));
            var sqlconstring=sqlconnestring.Substring(  sqlconnestring.IndexOf('-')+1);
            var istest = false;

            var db = GetSqlSugarClient(sqlcontypestr, sqlconstring, out istest);
            if( istest)
            {
                return new JsonResult(new { isok = true, istest = true, leng = 0, data = new List<int>() });
            }

            if (cmd.Trim().ToLower().StartsWith("select", StringComparison.OrdinalIgnoreCase))
            {
                isquery = true;
            }
            if (  isquery )
            {
                var relist = await db.Ado.SqlQueryAsync<dynamic>(cmd);
                var maxleng = 1000;
                if(relist.Count<=1000)
                {
                    maxleng = relist.Count;
                }
                else
                {
                    return new JsonResult(new { isok = true, istest = false, leng = maxleng, data = relist.Take(1000) }); 
                }
                return new JsonResult(new { isok = true, istest = false, leng= maxleng,  data = relist });
            }
            else
            {
                var reint = await db.Ado.ExecuteCommandAsync(cmd);
                return new JsonResult(new { isok = true, istest = false, leng = reint, data = new List<int>() });

            }
             
        }
    }
}

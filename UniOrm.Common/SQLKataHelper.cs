/*
 * ************************************
 * file:	    SQLKataHelper.cs
 * creator:	    Harry Liang(215607739@qq.com)
 * date:	    2020/4/5 10:05:31
 * description:	
 * ************************************
 */
extern alias MySqlConnectorAlias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Sqlite;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text; 
using PetaPoco.NetCore;
using MySqlConnectorAlias::MySql.Data.MySqlClient;

namespace UniOrm
{
    public class SQLKataHelper
    {
        public static MySqlCompiler MySqlCompiler = new MySqlCompiler();
        public static SqlServerCompiler SqlServerCompiler = new SqlServerCompiler();
        public static SqliteCompiler SqliteCompiler = new SqliteCompiler();
        public static PostgresCompiler PostgresCompiler = new PostgresCompiler();

        public static QueryFactory CreateSqlKata(string sqlcontypestr, string sqlconstring)
        {
            IDbConnection connection = new MySqlConnectorAlias::MySql.Data.MySqlClient.MySqlConnection(sqlconstring);
            Compiler compiler = SQLKataHelper.MySqlCompiler;  //2
                                                              // var dttypesqlsugar = DbType.MySql;
            switch (sqlcontypestr)
            {
                case "sqlite": //0
                    connection = new SqliteConnection(sqlconstring);
                    compiler = SQLKataHelper.SqliteCompiler;
                    break;
                case "sqlserver"://1
                    connection = new SqlConnection(sqlconstring);
                    compiler = SQLKataHelper.SqlServerCompiler;
                    break;
                case "postgre": //3
                    connection = new NpgsqlConnection(sqlconstring);
                    compiler = SQLKataHelper.PostgresCompiler;
                    break;
            }
            return new QueryFactory(connection, compiler);
        }
        public static QueryFactory CreateSqlKata(int dbtyp, string sqlconstring)
        {
            IDbConnection connection = null; 
            Compiler compiler = null;   //2
                                                              // var dttypesqlsugar = DbType.MySql;
            switch (dbtyp)
            {
                case 1:
                    connection = new SqlConnection(sqlconstring);
                    compiler = SQLKataHelper.SqlServerCompiler;
                    break;
                case 2:
                    connection = new SqliteConnection(sqlconstring);
                    compiler = SQLKataHelper.SqliteCompiler;
                    break;
                case 0:
                     connection = new MySqlConnection(sqlconstring);
                     compiler = SQLKataHelper.MySqlCompiler;  //2
                    break;
                case 4:
                    connection = new NpgsqlConnection(sqlconstring);
                    compiler = SQLKataHelper.PostgresCompiler;
                    break;
               
            }
            return new QueryFactory(connection, compiler);
        }

    }
    public class SQLPetaPocoHelper
    { 
        public static Database CreatePeta(string sqlcontypestr, string sqlconstring)
        {
            IDbConnection connection = new MySqlConnectorAlias::MySql.Data.MySqlClient.MySqlConnection(sqlconstring);
     
                                                              // var dttypesqlsugar = DbType.MySql;
            switch (sqlcontypestr)
            {
                case "sqlite": //0
                    connection = new SqliteConnection(sqlconstring);
                
                    break;
                case "sqlserver"://1
                    connection = new SqlConnection(sqlconstring);
               
                    break;
                case "postgre": //3
                    connection = new NpgsqlConnection(sqlconstring);
                   
                    break;
            }
            return new Database(connection );
        }
        public static Database CreatePeta(int dbtyp, string sqlconstring)
        {
            IDbConnection connection = null; 
                                        // var dttypesqlsugar = DbType.MySql;
            switch (dbtyp)
            {
                case 1:
                    connection = new SqlConnection(sqlconstring); 
                    break;
                case 2:
                    connection = new SqliteConnection(sqlconstring); 
                    break;
                case 0:
                    connection = new MySqlConnectorAlias::MySql.Data.MySqlClient.MySqlConnection(sqlconstring); 
                    break;
                case 4:
                    connection = new NpgsqlConnection(sqlconstring); 
                    break;

            }
            return new Database(connection);
        }

    }
    public static class SQLKataHelperEx
    {
        public static Type DBTypeStringToCShapType(this SqlSugarClient db, DbColumnInfo colinfo)
        {
            Type rettype = null;
            var dbstring = colinfo.DataType;
            switch (dbstring.ToLower())
            {
                case "guid":
                    rettype = typeof(Guid);
                    break;
                case "datetime":
                case "datetime2":
                    rettype = typeof(DateTime);
                    break;
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "memo":
                    rettype = typeof(string);
                    break;
                case "bigint":
                case "long":
                    rettype = typeof(long);
                    break;
                case "int":
                case "integer":
                    if (colinfo.Length == 1)
                    {
                        rettype = typeof(bool);
                    }
                    else
                    {
                        rettype = typeof(int);
                    }
                    break;
                case "smallint":
                    rettype = typeof(char);
                    break;
                case "bool":
                case "boolean":
                case "bit":
                    rettype = typeof(bool);
                    break;
                case "tinyint":
                    if (db.CurrentConnectionConfig.DbType == SqlSugar.DbType.MySql)
                    {
                        rettype = typeof(bool);

                    }
                    else
                    {
                        rettype = typeof(int);
                    }

                    break;
                case "decimal":
                    rettype = typeof(decimal);
                    break;
                case "float":
                case "real":
                    rettype = typeof(float);
                    break;
                case "double":
                    rettype = typeof(double);
                    break;
            }
            return rettype;
        }

        public static SqlSugarClient GetSqlSugarContext(this string allconnectionstring)
        {
            var index = allconnectionstring.IndexOf('-');
            var sqlcontypestr = allconnectionstring.Substring(0, index);
            var sqlconstring = allconnectionstring.Substring(index + 1);

            var dttypesqlsugar = SqlSugar.DbType.MySql;
            switch (sqlcontypestr)
            {
                case "sqlite":
                    dttypesqlsugar = SqlSugar.DbType.Sqlite;
                    break;
                case "mysql":
                    dttypesqlsugar = SqlSugar.DbType.MySql;
                    break;
                case "sqlserver":
                    dttypesqlsugar = SqlSugar.DbType.MySql;
                    break;
                case "postgre":
                    dttypesqlsugar = SqlSugar.DbType.PostgreSQL;
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

      
        public static QueryFactory GetSqlKataContext(this string allconnectionstring)
        {
            var index = allconnectionstring.IndexOf('-');
            var sqlcontypestr = allconnectionstring.Substring(0, index);
            var sqlconstring = allconnectionstring.Substring(index + 1);
            //SqlKataExtensions.DefaultCompiler = CompilerType.Postgres;
            return SQLKataHelper.CreateSqlKata(sqlcontypestr, sqlconstring);
        }

     
    }
}

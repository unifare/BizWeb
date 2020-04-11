using SqlKata.Execution;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace UniOrm.Common
{
    public class DB
    {
        public DbConnectionConfig DcConnectionConfig { get; set; }
        public DbType DbType { get; set; }

        public SqlSugarClient Client 
        {
            get
            {
                return Open();
            }
        }

        public DB(string connstring, int dbType, string tableprefix)
        {
            DcConnectionConfig.Connectionstring   = connstring;
            DcConnectionConfig.DefaultDbPrefixName = tableprefix;
            DbType= DcConnectionConfig.DBType = (DbType)dbType; 
        }

        public DB(DbConnectionConfig  dcConnectionConfig)
        {
            DcConnectionConfig = dcConnectionConfig ;
          
        }

        public static SqlSugarClient UniClient
        {
            get
            {
                var configapp = APPCommon.AppConfig.UsingDBConfig;
                var config = GetConfig(configapp);

                config.DbType = configapp.DBType;
                return new SqlSugarClient(config);
            }
        }
        public static QueryFactory Kata
        {
            get
            {
                var configapp = APPCommon.AppConfig.UsingDBConfig;
                //var config = GetConfig(configapp);

                return SQLKataHelper.CreateSqlKata((int)configapp.DBType, configapp.Connectionstring);
            }
        }
        private static ConnectionConfig GetConfig(DbConnectionConfig configapp)
        {
            var config = new ConnectionConfig()
            {
                ConnectionString = configapp.Connectionstring,

                IsAutoCloseConnection = true,
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    EntityService = (property, column) =>
                    {

                        var attributes = property.GetCustomAttributes(true);//get all attributes     
                        if (attributes.Any(it => it is KeyAttribute))//根据自定义属性    
                        {
                            column.IsPrimarykey = true;
                        }
                    },
                    EntityNameService = (type, entity) =>
                    {
                        var attributes = type.GetCustomAttributes(true);
                        if (attributes.Any(it => it is TableAttribute))
                        {
                            entity.DbTableName = (attributes.First(it => it is TableAttribute) as TableAttribute).Name;
                        }
                        else
                        {
                            entity.DbTableName = configapp.DefaultDbPrefixName + entity.DbTableName;
                        }

                    }
                }
            };

            config.DbType = configapp.DBType;

            return config;
        }

        public static SqlSugarClient New(string connectionstring, int dbtype, string tableprefix)
        {

            var config = new ConnectionConfig()
            {
                ConnectionString = connectionstring,
                //DbType= DbType.
            };

            switch (dbtype)
            {
                case 1:
                    config.DbType = DbType.SqlServer;
                    break;
                case 0:
                    config.DbType = DbType.Sqlite;
                    break;
                case 2:
                    config.DbType = DbType.MySql;
                    break;
                case 3:
                    config.DbType = DbType.PostgreSQL;
                    break;
            }

            return new SqlSugarClient(config);

        }

        public   SqlSugarClient Open( )
        {

            var config = GetConfig(DcConnectionConfig);

            return new SqlSugarClient(config);

        }
    }
}


using Microsoft.Extensions.DependencyInjection; 
using System;
using System.Collections.Generic;
using System.Text;
using FlunentMigratorFactory;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using UniOrm.Core;
using UniOrm;
using UniOrm.Common; 
using UniOrm.DataMigrationiHistrory;
using SqlSugar;

namespace UniOrm.Application
{
    public class DbMigrationHelper
    {
        public static void EnsureDaContext(DbConnectionConfig SystemConConfig)
        {
            var dbtype = (DbType)SystemConConfig.DBType;
         
                var fuType = FlunentDBType.Sqlite;

                switch (dbtype)
                {
                    case DbType.Sqlite:
                        fuType = FlunentDBType.Sqlite;
                        break;
                    case DbType.SqlServer:
                        fuType = FlunentDBType.MsSql;
                        break;
                    case DbType.MySql:
                        fuType = FlunentDBType.MySql4;
                        break;
                    case DbType.PostgreSQL:
                        fuType = FlunentDBType.Postgre;
                        break;
                }
                MigratorFactory.CreateServices(fuType, SystemConConfig.Connectionstring, MigrationOperation.MigrateUp, 0, typeof(Init).Assembly);
          
        }
        public static void EnsureDaContext(DbConnectionConfig SystemConConfig, params Assembly[] assemblies)
        {
            var dbtype = (DbType)SystemConConfig.DBType;
           
                var fuType = FlunentDBType.Sqlite;

                switch (dbtype)
                {
                    case DbType.Sqlite:
                        fuType = FlunentDBType.Sqlite;
                        break;
                    case DbType.SqlServer:
                        fuType = FlunentDBType.MsSql;
                        break;
                    case DbType.MySql:
                        fuType = FlunentDBType.MySql4;
                        break;
                    case DbType.PostgreSQL:
                        fuType = FlunentDBType.Postgre;
                        break;
                }
                MigratorFactory.CreateServices(fuType, SystemConConfig.Connectionstring, MigrationOperation.MigrateUp, 0, assemblies);
           
        }

    }
}

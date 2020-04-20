using FluentMigrator;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniOrm.Common;

namespace UniOrm.DataMigrationiHistrory
{
    [Migration(112)]
    public class DBMIgrate_112 : DBMIgrateBase
    {

        public override void Up()
        {

            IfDatabase("SqlServer", "Postgres", "sqlite").Create.Table(WholeTableName("LocalLangs"))
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("Guid").AsString(300)
                  .WithColumn("Name").AsString(300)
                  .WithColumn("IsSytem").AsBoolean()
                  .WithColumn("Value").AsCustom("ntext").Nullable()
                  .WithColumn("Lang").AsInt32()
                  .WithColumn("AddTime").AsDateTime().Nullable()
                ;
            IfDatabase("mysql").Create.Table(WholeTableName("LocalLangs"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Guid").AsString(300)
                .WithColumn("Name").AsString(300)
                .WithColumn("IsSytem").AsBoolean()
                .WithColumn("Value").AsCustom("text").Nullable()
                .WithColumn("Lang").AsInt32()
                .WithColumn("AddTime").AsDateTime().Nullable()
                ;
            var SystemDictionary1 = new
            { 
                Name = "Hello", 
                Guid = Guid.NewGuid().ToString(),
                IsSytem = true,
                Value = @"你好",
                Lang=0
            };
            var SystemDictionary2 = new
            {
                Name = "Hello",
                Guid = Guid.NewGuid().ToString(),
                IsSytem = true,
                Value = @"Hello",
                Lang = 2
            };
            Insert.IntoTable(WholeTableName("LocalLangs")).Row(SystemDictionary1);
            Insert.IntoTable(WholeTableName("LocalLangs")).Row(SystemDictionary2);
        }

        public override void Down()
        {
         


        }
    }
}

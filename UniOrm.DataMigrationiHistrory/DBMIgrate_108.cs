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
    [Migration(108)]
    public class DBMIgrate_108 : DBMIgrateBase
    {

        public override void Up()
        {

            IfDatabase("SqlServer", "Postgres", "sqlite").Create.Table(WholeTableName("AconFunction"))
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Guid").AsString(200)
                .WithColumn("FunctionName").AsString(300)
                .WithColumn("FunctionMemo").AsString(1000).Nullable()
                .WithColumn("FunctionCode").AsCustom ("ntext").Nullable()
                .WithColumn("FunctionNameSpace").AsString(1000).Nullable()
                .WithColumn("ReferanceList").AsString(1000).Nullable() 
                 .WithColumn("AddTime").AsDateTime().Nullable()
            ;
            IfDatabase("mysql").Create.Table(WholeTableName("AconFunction"))
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Guid").AsString(200)
            .WithColumn("FunctionName").AsString(300)
            .WithColumn("FunctionMemo").AsString(1000).Nullable()
            .WithColumn("FunctionCode").AsCustom("text").Nullable()
            .WithColumn("FunctionNameSpace").AsString(1000).Nullable()
            .WithColumn("ReferanceList").AsString(1000).Nullable()
             .WithColumn("AddTime").AsDateTime().Nullable()
        ;
        }

        public override void Down()
        {
           Delete.Table(WholeTableName("AconFunction"));
            
        }
    }
}

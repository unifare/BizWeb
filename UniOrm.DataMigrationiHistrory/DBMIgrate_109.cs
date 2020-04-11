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
    [Migration(109)]
    public class DBMIgrate_109 : DBMIgrateBase
    {

        public override void Up()
        {

            Alter.Table(WholeTableName("AConFlowStep"))
                .AddColumn("IsUsingAuth").AsBoolean().WithDefaultValue(false).Nullable()
                 .AddColumn("UserName").AsString(100).Nullable()
                 .AddColumn("UserRole").AsString(100).Nullable()
                ; 
        ;
        }

        public override void Down()
        {
         


        }
    }
}

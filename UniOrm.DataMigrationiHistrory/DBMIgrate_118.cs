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
    [Migration(118)]
    public class DBMIgrate_118 : DBMIgrateBase
    {

        public override void Up()
        {

            Alter.Table(WholeTableName("AConMvcClass")) 
                 .AddColumn("IsController").AsBoolean().Nullable().WithDefaultValue(true)
                  .AddColumn("IsSelfDefine").AsBoolean().Nullable().WithDefaultValue(false)
                     .AddColumn("InhiredClass").AsString(300).Nullable()
                ; 
        ;
        }

        public override void Down()
        {
         


        }
    }
}

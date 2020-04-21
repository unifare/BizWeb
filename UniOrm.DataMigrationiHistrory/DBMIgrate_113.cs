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
    [Migration(113)]
    public class DBMIgrate_113 : DBMIgrateBase
    {

        public override void Up()
        {
            var SystemDictionary2 = new
            {
                Name = "网站名称",
                Guid = Guid.NewGuid().ToString(),
                IsSytem = true,
                Value = @"无敌网址导航",
                LangName = "zh",
                Lang = 2
            }; 
            Insert.IntoTable(WholeTableName("LocalLangs")).Row(SystemDictionary2);
            //Alter.Table(WholeTableName("LocalLangs"))
            //           .AddColumn("LangName").AsString(50).Nullable()
            //          ;
        }

        public override void Down()
        {
         


        }
    }
}

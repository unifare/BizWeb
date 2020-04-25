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
    [Migration(115)]
    public class DBMIgrate_115 : DBMIgrateBase
    {

        public override void Up()
        {
            var SystemDictionary = new
            {
                KeyName = "后台dir",
                Value = "sd23nj",
                IsSystem = true,
                SystemDictionarytype = 0,
                AddTime = DateTime.Now, 
            };
            Insert.IntoTable(WholeTableName("SystemDictionary")).Row(SystemDictionary);
            var SystemDictionary2 = new
            {
                KeyName = "db_pre",
                Value = "uniorm_",
                IsSystem = true,
                SystemDictionarytype = 0,
                AddTime = DateTime.Now,
            };
            Insert.IntoTable(WholeTableName("SystemDictionary")).Row(SystemDictionary2); 
        }

        public override void Down()
        {
         


        }
    }
}

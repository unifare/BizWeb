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
    [Migration(117)]
    public class DBMIgrate_117 : DBMIgrateBase
    {

        public override void Up()
        {

            Alter.Table(WholeTableName("AConMvcClass")) 
                 .AddColumn("Guid").AsString(100).Nullable()
                ; 
        ;
        }

        public override void Down()
        {
         


        }
    }
}

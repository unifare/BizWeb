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
    [Migration(110)]
    public class DBMIgrate_110 : DBMIgrateBase
    {

        public override void Up()
        {

            Alter.Table(WholeTableName("AConFlowStep")) 
                 .AddColumn("LoginUrl").AsString(1500).Nullable() 
                ; 
        ;
        }

        public override void Down()
        {
         


        }
    }
}

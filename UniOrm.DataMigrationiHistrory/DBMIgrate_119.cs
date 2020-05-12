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
    [Migration(119)]
    public class DBMIgrate_119 : DBMIgrateBase
    {

        public override void Up()
        { 
            Update.Table(WholeTableName("AConFlowStep")).Set(new { StorePoolKey = "_NextRunTimeKey:comid,_sections:sections" })
                .Where(new { Guid = "BBF6B4BC-AEDA-4607-83ED-406E8BB67351" });
        }

        public override void Down()
        {
         


        }
    }
}

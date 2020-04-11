using FluentMigrator;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UniOrm.Common
{
    public class UniMigrationAttribute : MigrationAttribute
    {
        public UniMigrationAttribute(string filename ) :
             base(Convert.ToInt64(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename))))
        {
        }
        public UniMigrationAttribute(string filename, string description):
            base(Convert.ToInt64(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  filename))),description)
        { 
        }

    }


    public class DBMIgrateBase: Migration
    {
        private static string _Prefixname;
        public static string Prefixname
        {
            get
            {
                if (_Prefixname == null)
                {
                    var configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config" + Path.DirectorySeparatorChar + "System.json");
                    var configroot = JToken.Parse(File.ReadAllText(configpath));
                    _Prefixname = configroot["App"]["UsingDBConfig"]["DefaultDbPrefixName"].ToString();

                }
                return _Prefixname;
            }
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            throw new NotImplementedException();
        }

        protected string WholeTableName(string tablename)
        {
            return Prefixname + tablename;
        }
    }

    //public static class DBMIgrateBaseExtense { 
    //    public static 
    
    //}


}

using UniOrm.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SqlSugar;
using UniOrm.Model;

namespace UniOrm
{
    public class AppConfig
    {
        public AppConfig()
        {
            SystemDictionaries = new List<SystemDictionary>();

            ResultDictionary = new Dictionary<string, object>();
        }

        public string AppType { get; set; }
        public string ModuleConfigDir { get; set; } = "./";
        public string AppName { get; set; } 
        public string AdminAreas { get; set; } = "sd23nj";
        public string UserSpaceDir { get; set; } = "config/UploadPage";
        public string AppTheme { get; set; } = "Aro";
        public string AppTmpl { get; set; } = "default";
        public string DefaultDbPrefixName { get; set; }
        public bool IsUseGloableCahe { get; set; } = false;
        public DbConnectionConfig UsingDBConfig { get; set; }
        public List<DbConnectionConfig> Connectionstrings { get; set; }
        public List<RegestedModel> EFRegestedModels { get; set; }
        public List<string> OrmTypes { get; set; }
        public string TrigerType { get; set; }
        public string StartUpCompoistyID { get; set; }
        public List<SystemDictionary> SystemDictionaries { get; set; }
        public Dictionary<string, object> ResultDictionary { get; set; }
        public List<string> RazorNamesapace { get; set; } = new List<string>();
        public SqlSugarClient Db
        {
            get
            {
                return DB.UniClient;
            }

        } 

        public void LoadDBDictionary()
        { 
            SystemDictionaries.AddRange(Db.Queryable<SystemDictionary>().ToList());
        }

       
        public string GetDicstring(string key)
        {
            var item = SystemDictionaries.FirstOrDefault(p => p.KeyName == key);
            if (item != null)
            {
                return item.Value;
            }
            return string.Empty;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class SystemACon
    {
        public long Id { get; set; }
        public string AppName { get; set; }
        public string AppDiscription { get; set; }
        public string AppConfigs { get; set; }
        public string Author { get; set; }

        public string UpdateUrl { get; set; }
        public string UpdateParamter { get; set; }
        public DateTime LastRunTime { get; set; }
        public string VersionNum { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}

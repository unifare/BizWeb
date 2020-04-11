using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common
{
    public class RegestedModel
    {
        public string Dll { get; set; }
        public string[] TypeNames { get; set; }
    }
    public class RegestedModelConfig
    {
        public List<RegestedModel> RegestedModels { get; set; }
    }
    public class DbConnectionConfig
    {
        public string DefaultDbPrefixName { get; set; }
        
        public bool IsNeedRegModel { get; set; }
        public string OrmName { get; set; }
        public string Name { get; set; }
        public string Connectionstring { get; set; }
        public DbType DBType { get; set; }
    }
}

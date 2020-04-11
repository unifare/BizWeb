using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public enum RunMode
    {
        Coding,
        Debug,
        TroubleSome,
        release,
    }
    public class ComposeEntity
    {
        public ComposeEntity()
        {
            AddTime = DateTime.Now;
            AppType = "aspnetcore";
            Theme = "default";
            //resouceInfos = new List<ResouceInfo>();
            IsUsingParentConnstring = true; 
        }
        private static readonly object lockobject = new object();
        public int GetHash()
        {
            return this.GetHashCode();
        }
        public long Id { get; set; }
        public string Guid { get; set; }
        public string Templateid { get; set; }
        public string AppName { get; set; } = "default";
        public string Theme { get; set; } = "default";
        public string VersionNum { get; set; }
        public string Connectionstring { get; set; }
        public bool IsBuildIn { get; set; }
        public bool? IsUsingParentConnstring { get; set; } 
        public string AppType { get; set; }
        public string TrigeMethod { get; set; }

        public string TrigeType { get; set; } 
        public RunMode RunMode { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; } 
        public string CalStackMD5 { get; set; }
        public string TransferParamterLib { get; set; }
        public string TransferParamterPlantform { get; set; }
        public bool IsStatic { get; set; }
        public string TransferParamterType { get; set; }
        public string TransferParamterMethod { get; set; }
        public string TransferParamterInArguments { get; set; } 
        public int? RequsetEnviromentId { get; set; }
        public DateTime? AddTime { get; set; }
    }
}

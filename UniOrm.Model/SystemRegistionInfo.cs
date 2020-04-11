using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class SystemRegistionInfo
    {
        public long Id { get; set; }
        public string AuthorizeWay { get; set; }
        public string MachineCode { get; set; }
        public string RigesterCode { get; set; } 

        public string RigesterUrl { get; set; }
        public string RigesterFilePath { get; set; }
        public string ClientName { get; set; }
        public int? TryNumber { get; set; }

        public DateTime? TryEndTime { get; set; }
        public DateTime? RegisterOkTime { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}

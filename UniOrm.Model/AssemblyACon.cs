using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class AssemblyACon
    {
        public long Id { get; set; }
        public string VersionNum { get; set; }

        public string NameSpace { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Description { get; set; }
        public string DllPath { get; set; }
        public bool? IsVirtual { get; set; }
        public DateTime? AddTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class ClassACon
    {
        public string VersionNum { get; set; }
        public long Id { get; set; }
        public bool? IsVirtual { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Description { get; set; }  

        public DateTime AddTime { get; set; }
    }
}

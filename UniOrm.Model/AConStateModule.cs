using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public   class AConStateModule
    {
        public long Id { get; set; }
        
        public string VersionNum { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Addtime { get; set; }
    }
}

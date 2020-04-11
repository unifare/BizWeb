using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class AuthorizeInfo
    {
        public string VersionNum { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public int? ModuleID { get; set; }
        public int? FlowID { get; set; }
        public string  AocInfo { get; set; }
        public DateTime? Addtime { get; set; }
    }
}

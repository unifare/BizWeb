using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class ResouceInfo
    {
        public long Id { get; set; }
        public string VersionNum { get; set; }

        public int? ComposeEntityId { get; set; }
        public int? FlowId { get; set; }
        public int? MoudelId { get; set; }
        public string KeyName { get; set; }
        public string Value { get; set; }
        public object RuntimeValue=null;
        public string SerialObject { get; set; }
        public DateTime? Addtime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.DepenceCore
{
    public class WorkFlowBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class StepParamter
    {
        public string VersionNum { get; set; }
        public long Id { get; set; }
        public int AConFlowStepId { get; set; }
        public string Name { get; set; }
        public string GetValueWayID { get; set; }
        public DateTime Addtime { get; set; }
    }
}

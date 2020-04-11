using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.DepenceCore
{
    public enum OperationType
    {
        Plus,
        Abstract,
        Multi,
        Divie,
        SetTo,

    }

    public class WorkFlowStep : WorkFlowBase
    {
        public int StepID   { get;set; }

        public MaterialSet RequirePool { get; set; }

        public OperationType OperationType { get; set; }

        public List<object> InputObjets { get; set; }
        public List<object> OutputObjets { get; set; }
    }
}

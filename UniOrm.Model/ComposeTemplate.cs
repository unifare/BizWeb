using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class ComposeTemplate
    {
        public long Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string StepIds { get; set; }
        public StepType StepType { get; set; }
        public bool Isenable {get;set;}
        public DateTime? AddTime { get; set; }
    }
    //public class ComposeTemplate
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string StepId { get; set; }
    //    public int StepOrder { get; set; }
    //    public StepType StepType { get; set; }
    //    public DateTime? AddTime { get; set; }
    //}
    public enum StepType
    {
        Common,
        StepPlaceMaster,
        StepPlaceholder
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class FormViewDef
    {
        public int Id { get; set; }
        public int ParentFormId { get; set; }
        public int DbColumnDefId { get; set; }

        public string ShowText { get; set; }
        public string TipText { get; set; }
        public int? Length { get; set; }
        public string ShowDbOrder { get; set; }
        public bool IsCanEdit { get; set; }
        public bool IsShow { get; set; }
        public string EditorType { get; set; }
        public string EditorSourceType{ get; set; }
        public string EditorSourceList { get; set; }

        public string EditorSourceSql { get; set; }
        public string EditorSourceTable { get; set; }
        public bool IsReadOnly { get; set; }
        public bool isValidateNeeded { get; set; }
        public bool  ValidateRule { get; set; }
        public DateTime AddTime { get; set; }
    }
}

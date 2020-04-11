using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class DbColumnDef
    {
        public int Id { get; set; }
        public string ColumnGuid { get; set; }
        public int TableId { get; set; }
        public int IsPrimayKey { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public int? Length { get; set; }
        public string ColumnDbOrder { get; set; }
         
        public DateTime AddTime { get; set; }
    }

}

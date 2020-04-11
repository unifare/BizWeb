using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class DbTableDef
    {
        public DbTableDef()
        {

        }
        public int Id { get; set; }
        public string TableName { get; set; }
        public string TablePrefix { get; set; }
        public bool IsSourceTable { get; set; }
        public DateTime AddTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Model
{
    public class AdminUser
    {
        public long Id { get; set; }
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDisable { get; set; }
        public DateTime AddTime { get; set; }
    }
}

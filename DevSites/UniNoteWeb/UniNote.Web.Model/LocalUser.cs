using System;
using System.Collections.Generic;
using System.Text;

namespace UniNote.Web.Model
{
    public class LocalUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string LatestToken { get; set; }

        public string LatestRefreshToken { get; set; }

        public DateTime AddTime { get; set; }
    }
}

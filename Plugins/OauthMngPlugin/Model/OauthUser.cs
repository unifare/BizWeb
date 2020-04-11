using System;
using System.Collections.Generic;
using System.Text;

namespace OauthMngPlugin.Model
{
    public class OauthUser
    {
        public long Id { get; set; }
        public string Guid { get; set; }

        public string  Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public string LastIP { get; set; }

        public bool IsEnable { get; set; }

        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public string ReferorId { get; set; }
        public DateTime AddTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common
{
    public class OauthClientModel
    {
        public string IdentityUrl { get; set; }
        public string Endpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string GrantType { get; set; }
        public string ReponseType { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string AccessToken { get; set; }

        public DateTime ExpiresTime { get; set; }

    }
}

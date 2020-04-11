using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public const string CustomerAuthenticationScheme = "UserAuthorizeAttribute";
        public UserAuthorizeAttribute()
        {
            this.AuthenticationSchemes = CustomerAuthenticationScheme;
           
        }
    }
}

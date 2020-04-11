using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public const string CustomerAuthenticationScheme = "CustomerAuthenticationScheme";
        public AdminAuthorizeAttribute()
        {
            this.AuthenticationSchemes = CustomerAuthenticationScheme;
        }
    }
}

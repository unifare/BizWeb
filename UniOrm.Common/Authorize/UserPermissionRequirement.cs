using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniOrm.Common.Authorize
{
    public class UserPermissionRequirement: IAuthorizationRequirement
    {
        public string DeniedAction { get; set; }
        public string LoginPath { get; set; }
        public string ClaimType { get; set; }
        public TimeSpan Expiration { get; set; }
        public string RoleName { get; set; }
        public UserPermissionRequirement(string RoleName,string deniedAction, string loginPath, string claimType, TimeSpan expiration)
        {
            DeniedAction = deniedAction;
            LoginPath = loginPath;
            ClaimType = claimType;
            Expiration = expiration;
        } 
    }
}

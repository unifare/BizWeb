using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UniOrm.Common
{
    public interface IAuthorizeHelper
    {
        HttpClient Client { get;   }
        Task<TokenResponse> LoginToIds4Async(HttpContext httpContext, string usename, string password, string refreshToken = null);

        Task<TokenResponse> LoginToIds4Async(HttpContext httpContext, OauthClientModel clientmodel );

        Task<ProtocolResponse> Logout(HttpContext httpContext, string token);


    }
}

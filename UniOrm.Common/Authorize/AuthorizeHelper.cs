using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using UniOrm.Common; 

namespace UniOrm.Common
{
    public class AuthorizeHelper : IAuthorizeHelper
    {
        public HttpClient Client
        {
            get
            {
                return APPCommon.Client;
            }
        }
        public AuthorizeHelper()
        {
            //Client = new HttpClient();
        }
        AppConfig AppConfig { get; set; }
        public AuthorizeHelper(IConfiguration   config)
        {
            AppConfig = config.GetSection("App").Get<AppConfig>();   //config.GetValue<AppConfig>("App");
        }
        public async Task<TokenResponse> LoginToIds4Async(HttpContext httpContext, string usename, string password, string refreshToken = null)
        {
            var identityserver4url = AppConfig.GetDicstring("Identityserver4.url");
            var ClientId = AppConfig.GetDicstring("idsr4_ClientId");
            var ClientSecret = AppConfig.GetDicstring("idsr4_ClientSecret");
            var clientmodel = new OauthClientModel()
            {
                IdentityUrl = identityserver4url,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                UserName = usename,
                Password = password,
                RefreshToken = refreshToken
            };
            return await LoginToIds4Async(httpContext, clientmodel);

        }

        public async Task<TokenResponse> LoginToIds4Async(HttpContext httpContext, OauthClientModel clientmodel)
        {
            if (!string.IsNullOrEmpty(clientmodel.AccessToken) && clientmodel.ExpiresTime > DateTime.Now)
            {
                return null;
            }


            var disco = await Client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            { Address = clientmodel.IdentityUrl, Policy = { RequireHttps = false } });
            var refreshToken = clientmodel.RefreshToken;

            //获取用户角色
            //var userres = RequestUserInfo(disco.UserInfoEndpoint, Client, clientmodel.AccessToken);

            //Token作废
            TokenResponse RequesttokenResponse = null;
            if (refreshToken != null && refreshToken.Length > 20)
            {
                RequesttokenResponse = await Client.RequestRefreshTokenAsync(new RefreshTokenRequest()
                {
                    Address = disco.TokenEndpoint,
                    ClientId = clientmodel.ClientSecret,
                    ClientSecret = clientmodel.ClientSecret,
                    Scope = clientmodel.Scope,
                    GrantType = OpenIdConnectGrantTypes.RefreshToken,
                    RefreshToken = refreshToken,
                });

                if (RequesttokenResponse.IsError)
                {
                    RequesttokenResponse = await LoginGetAccesstoken(Client, clientmodel.ClientId,
                        clientmodel.ClientSecret, clientmodel.UserName, clientmodel.Password,
                        disco.TokenEndpoint);

                }

            }
            else
            {
                RequesttokenResponse = await LoginGetAccesstoken(Client, clientmodel.ClientId,
                    clientmodel.ClientSecret, clientmodel.UserName, clientmodel.Password,
                    disco.TokenEndpoint);



            }

            var expiresAt = DateTime.Now + TimeSpan.FromSeconds(RequesttokenResponse.ExpiresIn);

            var tokens = new[] {
                        new AuthenticationToken
                        {
                            Name = OpenIdConnectParameterNames.IdToken,
                            Value = RequesttokenResponse.IdentityToken
                        },
                        new AuthenticationToken
                        {
                            Name = OpenIdConnectParameterNames.AccessToken,
                            Value = RequesttokenResponse.AccessToken
                        },
                        new AuthenticationToken
                        {
                            Name = OpenIdConnectParameterNames.RefreshToken,
                            Value = RequesttokenResponse.RefreshToken
                            },
                            new AuthenticationToken
                            {
                                Name = "expires_at",
                                Value = expiresAt.ToString("o",CultureInfo.InvariantCulture)
                            }
                        };
            // 获取身份认证的结果，包含当前的pricipal和 properties
            var currentAuthenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 把新的tokens存起来
            currentAuthenticateResult.Properties.StoreTokens(tokens);

            // 登陆
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                currentAuthenticateResult.Principal, currentAuthenticateResult.Properties);
            clientmodel.AccessToken = RequesttokenResponse.AccessToken;
            clientmodel.RefreshToken = RequesttokenResponse.RefreshToken;
            clientmodel.ExpiresTime = expiresAt;
            return RequesttokenResponse;

        }


        public async Task<ProtocolResponse> Logout(HttpContext httpContext, string token)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(token);
            var identityserver4url = AppConfig.GetDicstring("Identityserver4.url");
            var ClientId = AppConfig.GetDicstring("idsr4_ClientId");
            var ClientSecret = AppConfig.GetDicstring("idsr4_ClientSecret");
            await httpContext.SignOutAsync();
            await httpContext.SignOutAsync("oidc");

            var disco = await Client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            { Address = identityserver4url, Policy = { RequireHttps = false } });



            var response = await Client.IntrospectTokenAsync(new TokenIntrospectionRequest
            {
                Address = "http://oauth.66wave.com/connect/introspect",
                ClientId = ClientId,
                ClientSecret = ClientSecret,

                Token = token
            });
            //client.vil
            var RequesttokenResponse = await Client.RevokeTokenAsync(new TokenRevocationRequest()
            {
                Address = disco.RevocationEndpoint,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Token = token
            });
            return RequesttokenResponse;
        }


        private async Task<ProtocolResponse> RequestUserInfo(string endpoint_url, HttpClient client, string accessToken)
        {
            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = endpoint_url,
                Token = accessToken
            });
            return response;


        }
        private async Task<TokenResponse> LoginGetAccesstoken(HttpClient client, string clientid,
            string ClientSecret, string UserName, string Password,
            string TokenEndpoint)
        {
            var ptr = new PasswordTokenRequest();
            ptr.Address = TokenEndpoint;
            ptr.ClientId = clientid;
            ptr.ClientSecret = ClientSecret;
            ptr.UserName = UserName;
            ptr.Password = Password;

            var tokenResponse = await client.RequestPasswordTokenAsync(ptr);

            return tokenResponse;
        }

    }
}

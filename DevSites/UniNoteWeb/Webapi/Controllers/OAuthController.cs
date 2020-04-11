using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UniNote.Web.Model;
using UniOrm; 
using UniOrm.Startup.Web; 

namespace TestWeb.Controllers
{
    public class OAuthController : Controller
    {
        public const string LoggerName = "OAuthController";
        /// <summary>
        /// <![CDATA[获取访问令牌]]>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RestfulData<AccessTokenObj>> GetToken(string userName, string password)
        {
            var result = new RestfulData<AccessTokenObj>();
            try
            {
                if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("user", "用户名不能为空！");
                if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password", "密码不能为空！");

                //验证用户名和密码
                var user = APP.LoginDefaultUser(userName, password);
                var claims = new Claim[]
                {
                       new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var signingkey = APP.GetDicstring("JWT.IssuerSigningKey");
                var isuser = APP.GetDicstring("JWT.Issuer");
                var audience = APP.GetDicstring("JWT.Audience");
                //配置授权
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingkey));
                var expires = DateTime.Now.AddDays(28);//
                var token = new JwtSecurityToken(
                            issuer: isuser,
                            audience: audience,
                            claims: claims,
                            notBefore: DateTime.Now,
                            expires: expires,
                            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
                var handler = new JwtSecurityTokenHandler(); 

                //生成Token
                string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                result.code = 1;
                result.data = new AccessTokenObj() { AccessToken = jwtToken, Expires = expires, Isuser = audience, Audience = audience };
                result.message = "授权成功！";
                return result;
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.code = 0;
                Logger.LogError(LoggerName, "获取访问令牌时发生错误！", ex);
                return result;
            }
        }



    }
}

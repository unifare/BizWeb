using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OauthMngPlugin.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniOrm;
using UniOrm.Common;

using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using OauthMngPlugin.Model.DTO;
using IdentityModel.Client;

namespace OauthMngPlugin.Contorllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserMngController : ControllerBase
    {
        readonly OauthMngModule OauthMngModule;
        readonly IAuthorizeHelper authorizeHelper;
        public UserMngController(OauthMngModule oauthMngModule, IAuthorizeHelper _authorizeHelper)
        {
            authorizeHelper = _authorizeHelper;
            // dd = OauthMngModule.ServiceProvider.GetService<Itest>();
            OauthMngModule = oauthMngModule;
        }

        [AdminAuthorize]

        [HttpGet]
        public async Task<string> GetAllUserFromServer()
        {
            var response = await authorizeHelper.LoginToIds4Async(HttpContext, OauthMngModule.OauthClient);


            var apiurl = OauthMngModule.Configuration["OauthMngModule:ids4_api_url"];
            authorizeHelper.Client.SetBearerToken(OauthMngModule.OauthClient.AccessToken);//add bearer with access_token


            var resultjson = await authorizeHelper.Client.GetAsync(apiurl + "/api/Users");
            var allresoult = await resultjson.Content.ReadAsStringAsync();
            //string wherestring = ""; int pindex = 0; int pagesize = 10;
            //var query = OauthMngModule.DB.Client.Queryable<OauthUser>();
            //query.WhereIF(!string.IsNullOrEmpty(wherestring),
            //    it => it.Name.Contains(wherestring) || it.UserName.Contains(wherestring)); 
            var usertoken = allresoult.ToJsonToken()["users"];

            return usertoken.ToString();
        }


        [AdminAuthorize]
        [HttpPost]
        public async Task<object> DelUserFromServer(string id)
        {
            var response = await authorizeHelper.LoginToIds4Async(HttpContext, OauthMngModule.OauthClient);
            var apiurl = OauthMngModule.Configuration["OauthMngModule:ids4_api_url"];
            authorizeHelper.Client.SetBearerToken(OauthMngModule.OauthClient.AccessToken);//add bearer with access_token
            var resultjson = await authorizeHelper.Client.DeleteAsync(apiurl + "/api/Users/" + id);

            return new { isok = resultjson.IsSuccessStatusCode };
        }

        [AdminAuthorize]
        [HttpPost]
        public async Task<object> AddUserFromServer([FromBody]UserDTO user)
        {
            var response = await authorizeHelper.LoginToIds4Async(HttpContext, OauthMngModule.OauthClient);
            var apiurl = OauthMngModule.Configuration["OauthMngModule:ids4_api_url"];
            authorizeHelper.Client.SetBearerToken(OauthMngModule.OauthClient.AccessToken);//add bearer with access_token

            var httpupdt = new StringContent(user.ToJson());
            var resultjson = await authorizeHelper.Client.PostAsync(apiurl + "/api/Users/" , httpupdt);

            return new { isok = resultjson.IsSuccessStatusCode };
        }

        [AdminAuthorize]
        [HttpPost]
        public async Task<object> UpdateUserFromServer([FromBody]UserDTO user)
        {
            var response = await authorizeHelper.LoginToIds4Async(HttpContext, OauthMngModule.OauthClient);
            var apiurl = OauthMngModule.Configuration["OauthMngModule:ids4_api_url"];
            authorizeHelper.Client.SetBearerToken(OauthMngModule.OauthClient.AccessToken);//add bearer with access_token

            var httpupdt = new StringContent(user.ToJson());
            var resultjson = await authorizeHelper.Client.PutAsync(apiurl + "/api/Users/", httpupdt);

            return new { isok = resultjson.IsSuccessStatusCode };
        }

        //[AdminAuthorize]
        //[HttpGet]
        //public async Task<IEnumerable<OauthUser>> GetAllUser()
        //{
        //    string wherestring = ""; int pindex = 0; int pagesize = 10;
        //    var query = OauthMngModule.DB.Client.Queryable<OauthUser>();
        //    query.WhereIF(!string.IsNullOrEmpty(wherestring),
        //        it => it.Name.Contains(wherestring) || it.UserName.Contains(wherestring));

        //    return await query.ToPageListAsync(pindex, pagesize);
        //}
    }
}

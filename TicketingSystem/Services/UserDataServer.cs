using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TicketingSystem.Models;
using System.Web;
using Newtonsoft.Json;

namespace TicketingSystem.Services
{
    public class UserDataServer
    {
        public UserData GetUserData(string UserId)
        {
            TokenData td = GetAPIToken();

            var client = new RestClient("https://robertswarehousing.auth0.com/api/v2/users/" + UserId);
            var req = new RestRequest(Method.GET);
            req.AddHeader("content-type", "application/json");
            req.AddHeader("authorization", "Bearer " + td.access_token);
            var response = client.Execute(req);
            var content = response.Content;

            UserData ud = JsonConvert.DeserializeObject<UserData>(content);
            return ud;
        }

        public TokenData GetAPIToken()
        {
            var client = new RestClient("https://robertswarehousing.auth0.com/oauth/token");
            var req = new RestRequest(Method.POST);
            string tokenUrl = "grant_type=client_credentials&client_id=ZLjHZNvuAQ4Vjt59sdwkKBAya8GQejQx&client_secret=Gu6SJweNziCtnVo04e2nsVH6PkCB-vUCMBcYi5Ld-f_a-q04mGuzyNil4roJbTtP&audience=https://robertswarehousing.auth0.com/api/v2/";
            req.AddHeader("content-type", "application/x-www-form-urlencoded");
            req.AddParameter("application/x-www-form-urlencoded", tokenUrl, ParameterType.RequestBody);

            string content = client.Execute(req).Content;

            TokenData td = JsonConvert.DeserializeObject<TokenData>(content);
            return td;
        }
    }
}

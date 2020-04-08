using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TicketingSystem.Models;
using System.Web;
using Newtonsoft.Json;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using TicketingSystem.ExceptionReport;

namespace TicketingSystem.Services
{
    public static class Auth0APIClient
    {
        static readonly string baseUrl = "https://robertswarehousing.auth0.com/api/v2/";
        static TokenData tokenData;
        static DateTime tokenGrantedAt;

        public static UserData GetUserData(string Auth0ID)
        {
            try
            {
                if (!ValidateToken())
                {
                    InitAPIToken();
                }

                var client = new RestClient(baseUrl + "users/" + Auth0ID);
                var req = new RestRequest(Method.GET);
                req.AddHeader("content-type", "application/json");
                req.AddHeader("authorization", "Bearer " + tokenData.access_token);
                var response = client.Execute(req);
                var content = response.Content;

                UserData ud = JsonConvert.DeserializeObject<UserData>(content);
                return ud;
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return new UserData();
            }

        }

        public static bool InitAPIToken()
        {
            try
            {
                var client = new RestClient("https://robertswarehousing.auth0.com/oauth/token");
                var req = new RestRequest(Method.POST);
                string tokenUrl = "grant_type=client_credentials&client_id=ZLjHZNvuAQ4Vjt59sdwkKBAya8GQejQx&client_secret=Gu6SJweNziCtnVo04e2nsVH6PkCB-vUCMBcYi5Ld-f_a-q04mGuzyNil4roJbTtP&audience=https://robertswarehousing.auth0.com/api/v2/";
                req.AddHeader("content-type", "application/x-www-form-urlencoded");
                req.AddParameter("application/x-www-form-urlencoded", tokenUrl, ParameterType.RequestBody);

                string content = client.Execute(req).Content;
                tokenGrantedAt = DateTime.Now;
                tokenData = JsonConvert.DeserializeObject<TokenData>(content);
                return true;
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return false;
            }
        }

        public static bool UpdateUsers(string Auth0ID)
        {
            try
            {
                if (!ValidateToken())
                {
                    InitAPIToken();
                }

                using (var db = new TicketingSystemDBContext())
                {
                    UserData ud = GetUserData(Auth0ID);

                    var u = db.Users.Where(uid => uid.Email == ud.email);
                    if (u.Count() == 0)
                    {
                        Users user = new Users();
                        user.Email = ud.email;
                        user.Auth0Uid = ud.user_id;

                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return false;
            }
        }

        public static List<UserData> GetAllUsers()
        {
            try
            {
                if (!ValidateToken())
                {
                    InitAPIToken();
                }

                List<UserData> users = new List<UserData>();
                var client = new RestClient(baseUrl + "users");
                var req = new RestRequest(Method.GET);
                req.AddHeader("content-type", "application/json");
                req.AddHeader("authorization", "Bearer " + tokenData.access_token);
                var response = client.Execute(req);
                var content = response.Content;

                users = JsonConvert.DeserializeObject<List<UserData>>(content);

                return users;
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return new List<UserData>();
            }
        }

        public static List<UserPermission> GetPermissions(string userId)
        {
            try
            {
                if (!ValidateToken())
                {
                    InitAPIToken();
                }

                List<UserPermission> permissions = new List<UserPermission>();
                var client = new RestClient(baseUrl + "users/" + userId + "/permissions");
                var req = new RestRequest(Method.GET);
                req.AddHeader("content-type", "application/json");
                req.AddHeader("authorization", "Bearer " + tokenData.access_token);

                var response = client.Execute(req);
                var content = response.Content;

                return JsonConvert.DeserializeObject<List<UserPermission>>(content);
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return new List<UserPermission>();
            }
        }

        public static bool ValidateToken()
        {
            try
            {
                if (tokenData == null)
                    InitAPIToken();

                double expiresIn = double.Parse(tokenData.expires_in);
                TimeSpan tokenActive = DateTime.Now - tokenGrantedAt;
                return tokenActive.TotalSeconds < expiresIn;
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return false;
            }

        }
    }
}

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
using System.Web.Http;
using System.Net;

namespace TicketingSystem.Services
{
    public static class Auth0APIClient
    {
        //Base URL for management API
        static readonly string baseUrl = "https://robertswarehousing.auth0.com/api/v2/";

        //Track token and time it was granted
        static TokenData tokenData;
        static DateTime tokenGrantedAt;

        /// <summary>
        /// Get user data from Auth0 API using the unique Auth0 ID
        /// </summary>
        /// <param name="Auth0ID">The Auth0ID of the desired user</param>
        /// <returns>A UserData instance of the found user</returns>
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
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }

        }

        /// <summary>
        /// Initializes a new Token to allow access to the management API
        /// </summary>
        /// <returns>Boolean indicating success</returns>
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
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        /// <summary>
        /// Updates the user in the database after creation in Auth0
        /// </summary>
        /// <param name="Auth0ID"></param>
        /// <returns>Boolean indicating success</returns>
        public static bool UpdateUser(string Auth0ID)
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

                    var u = db.Users.Where(uid => uid.Email == ud.email).FirstOrDefault();
                    u.Auth0Uid = ud.user_id;
                    db.SaveChanges();
                    
                }
                return true;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        /// <summary>
        /// Creates the newly created database user in Auth0
        /// </summary>
        /// <param name="newUser">The Users object to be added</param>
        /// <returns></returns>
        public static string AddUser(Users newUser)
        {
            try
            {
                if (!ValidateToken())
                {
                    InitAPIToken();
                }

                var client = new RestClient(baseUrl + "users");
                var req = new RestRequest(Method.POST);

                Auth0UserPayload usr = new Auth0UserPayload();
                usr.email = newUser.Email;
                usr.name = newUser.FullName;
                usr.password = Guid.NewGuid().ToString().Substring(0,12);
                usr.connection = "Username-Password-Authentication";

                req.AddJsonBody(usr);

                req.AddHeader("content-type", "application/json");
                req.AddHeader("authorization", "Bearer " + tokenData.access_token);
                var response = client.Execute(req);
                var content = response.Content;

                UserPostResponse upp = JsonConvert.DeserializeObject<UserPostResponse>(content);
                UpdateUser(upp.user_id);

                return upp.user_id;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        /// <summary>
        /// Gets all users contained in Auth0
        /// </summary>
        /// <returns>A list of UserData containing all users</returns>
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
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        /// <summary>
        /// Gets all of the permissions associated with a given user
        /// </summary>
        /// <param name="userId">The Auth0 ID</param>
        /// <returns>A list of permissions for the user</returns>
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
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        /// <summary>
        /// Validates the current token and reinitializes if it has expired 
        /// </summary>
        /// <returns></returns>
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
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        /// <summary>
        /// Set the role of the chosen user in Auth0
        /// </summary>
        /// <param name="auth0ID"></param>
        /// <param name="shiftType"></param>
        /// <returns></returns>
        public static bool SetRole(string auth0ID, string shiftType)
        {
            try
            {
                if (!ValidateToken())
                {
                    InitAPIToken();
                }

                var client = new RestClient(baseUrl + "users/" + auth0ID + "/roles");
                var req = new RestRequest(Method.POST);

                Auth0RolesPayload payload = new Auth0RolesPayload();
                Auth0Role role = FetchRole(shiftType);
                string[] roles = new string[] { role.id };
                payload.roles = roles;
                req.AddJsonBody(payload);

                req.AddHeader("content-type", "application/json");
                req.AddHeader("authorization", "Bearer " + tokenData.access_token);
                var response = client.Execute(req);
                var content = response.Content;

                UserPostResponse upp = JsonConvert.DeserializeObject<UserPostResponse>(content);
                return true;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        /// <summary>
        /// Get the role object associated with the given shiftType
        /// </summary>
        /// <param name="shiftType"></param>
        /// <returns>The Auth0Role object</returns>
        public static Auth0Role FetchRole(string shiftType)
        {
            try
            {
                if (!ValidateToken())
                {
                    InitAPIToken();
                }

                var client = new RestClient(baseUrl + "roles");
                var req = new RestRequest(Method.GET);
                req.AddHeader("content-type", "application/json");
                req.AddHeader("authorization", "Bearer " + tokenData.access_token);
                var response = client.Execute(req);
                var content = response.Content;

                List<Auth0Role> roles = JsonConvert.DeserializeObject<List<Auth0Role>>(content);
                foreach (var r in roles)
                {
                    if (r.name == shiftType)
                    {
                        return r;
                    }
                }
                return new Auth0Role();
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }
    }
}

﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using TicketingSystem.ExceptionReport;
using TicketingSystem.Models;
using TicketingSystem.Services;

namespace SampleMvcApp.Controllers
{
    public class AccountController : Controller
    {

        private static UserData loggedInUser;

        public async Task Login()
        {
            try
            {
                await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = "/" });
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task Logout()
        {
            try
            {
                await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
                {
                    // Indicate here where Auth0 should redirect the user after a logout.
                    // Note that the resulting absolute Uri must be whitelisted in the 
                    // **Allowed Logout URLs** settings for the client.
                    RedirectUri = Url.Action("Landing", "Home")
                });
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
            }
        }

        /// <summary>
        /// Endpoint to return all the permissions associated with a given user 
        /// </summary>
        /// <returns>JSON containing permissions</returns>
        public async Task<JsonResult> Permissions()
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateHttpErrorView(e, "401 Unauthorized"))
                });
            }
            try
            {
                var userId = User.Claims.First().Value;
                UserData ud = Auth0APIClient.GetUserData(userId);
                List<UserPermission> permissions = Auth0APIClient.GetPermissions(ud.user_id);

                return Json(new
                {
                    permissions = permissions
                });
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return Json(new
                {
                    newUrl = Url.Action("ServerError", error)
                });
            }
            catch (Exception e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateBasicExceptionView(e, guid))
                });
            }
        }

        /// <summary>
        /// Endpoint to return the UsersHome view with all users in the database
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResult> UsersHome()
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }
            UserManager um = new UserManager();
            try
            {
                return View("UsersHome", um.GetUsers());
            }
            catch (HttpResponseException e)
            {
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return View("ServerError", error);
            }
            catch (Exception e)
            {
                var guid = ExceptionReporter.DumpException(e);
                ErrorViewModel error = Utility.CreateBasicExceptionView(e, guid);
                return View("Error", error);
            }
        }

        /// <summary>
        /// Endpoint to create a new user in the database and Auth0
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public async Task<ViewResult> CreateUser(Users newUser)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }
            UserManager um = new UserManager();
            try
            {
                um.CreateUser(newUser, Auth0APIClient.GetUserData(User.Claims.First().Value));
                return View("UsersHome", um.GetUsers());
            }
            catch (HttpResponseException e)
            {
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return View("ServerError", error);
            }
            catch (Exception e)
            {
                var guid = ExceptionReporter.DumpException(e);
                ErrorViewModel error = Utility.CreateBasicExceptionView(e, guid);
                return View("Error", error);
            }

        }

        //Not supported due to management API restrictions

        //public JsonResult EditUser(string userId)
        //{
        //    try
        //    {
        //        Authorize();
        //    }
        //    catch (HttpResponseException e)
        //    {
        //        return Json(new
        //        {
        //            newUrl = Url.Action("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"))
        //        });
        //    }
        //    try
        //    {
        //        UserManager um = new UserManager();
        //        Users user = um.GetUserByID(int.Parse(userId));
        //        //return View("UserEdit", um.GetUserByID(int.Parse(userId)));
        //        return Json(new
        //        {
        //            newUrl = Url.Action("UserEdit", "Account", user)
        //        });
        //    }
        //    catch (HttpResponseException e)
        //    {
        //        return Json(new
        //        {
        //            newUrl = Url.Action("ServerError", Utility.CreateServerErrorView(e))
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        var guid = ExceptionReporter.DumpException(e);
        //        return Json(new
        //        {
        //            newUrl = Url.Action("Error", Utility.CreateBasicExceptionView(e, guid))
        //        });
        //    }
        //}

        //public async Task<ViewResult> UserEdit(Users user)
        //{
        //    try
        //    {
        //        Authorize();
        //    }
        //    catch (HttpResponseException e)
        //    {
        //        return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
        //    }

        //    try
        //    {
        //        UserManager um = new UserManager();
        //        return View("UserEdit", user);
        //    }
        //    catch (HttpResponseException e)
        //    {
        //        ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
        //        return View("ServerError", error);
        //    }
        //    catch (Exception e)
        //    {
        //        var guid = ExceptionReporter.DumpException(e);
        //        ErrorViewModel error = Utility.CreateBasicExceptionView(e, guid);
        //        return View("Error", error);
        //    }
        //}

        //public async Task<JsonResult> PostEdit(Users user)
        //{
        //    try
        //    {
        //        Authorize();
        //    }
        //    catch (HttpResponseException e)
        //    {
        //        string guid = ExceptionReporter.DumpException(e);
        //        return Json(new
        //        {
        //            newUrl = Url.Action("UsersHome"),
        //            message = "401 Not Authorized",
        //            guid = guid
        //        });
        //    }
        //    try
        //    {
        //        UserManager um = new UserManager();
        //        um.UpdateUser(user);

        //        return Json(new
        //        {
        //            newUrl = Url.Action("UsersHome", "Account")
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        string guid = ExceptionReporter.DumpException(e);
        //        return Json(new
        //        {
        //            newUrl = Url.Action("UsersHome"),
        //            message = e.Message,
        //            guid = guid
        //        });
        //    }
        //}


        /// <summary>
        /// Endpoint to trigger an update of database users from Auth0
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> UpdateUsers()
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateHttpErrorView(e, "401 Unauthorized"))
                });
            }
            try
            {
                UserManager um = new UserManager();
                um.UpdateUsersFromAuth0();
                return Json(new
                {
                    message = "successful update",
                    newUrl = Url.Action("UsersHome", um.GetUsers())
                });
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return Json(new
                {
                    newUrl = Url.Action("ServerError", error)
                });
            }
            catch (Exception e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateBasicExceptionView(e, guid))
                });
            }
        }

        /// <summary>
        /// Endpoint to ImportUsers from Auth0
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> ImportUsers()
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateHttpErrorView(e, "401 Unauthorized"))
                });
            }
            try
            {
                UserManager um = new UserManager();
                um.ImportUsersFromAuth0();
                return Json(new
                {
                    message = "successful import",
                    newUrl = Url.Action("UsersHome", um.GetUsers())
                }); 
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return Json(new
                {
                    newUrl = Url.Action("ServerError", error)
                });
            }
            catch (Exception e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateBasicExceptionView(e, guid))
                });
            }

        }

        public ViewResult ServerError(ServerErrorViewModel error)
        {
            return View("Error", error);
        }

        public ViewResult Error(ErrorViewModel error)
        {
            return View("Error", error);
        }

        /// <summary>
        /// Endpoint to delete a user from the database and Auth0
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<JsonResult> ToggleActivation(string userId)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateHttpErrorView(e, "401 Unauthorized"))
                });
            }
            UserManager um = new UserManager();
            try
            {
                um.ToggleActivation(int.Parse(userId));
                return Json(new
                {
                    newUrl = Url.Action("UsersHome", um.GetUsers()),
                    message = "Changed User Status",
                    id = userId
                }) ;
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return Json(new
                {
                    newUrl = Url.Action("ServerError", error)
                });
            }
            catch (Exception e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateBasicExceptionView(e, guid))
                });
            }
        }

        /// <summary>
        /// Endpoint that returns a list of valid emails in the system
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetEmails()
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateHttpErrorView(e, "401 Unauthorized"))
                });
            }
            try
            {
                UserManager um = new UserManager();
                List<string> emails = new List<string>();
                foreach (Users u in um.GetUsers())
                {
                    emails.Add(u.Email);
                }

                return Json(new
                {
                    emails = emails
                });
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return Json(new
                {
                    newUrl = Url.Action("ServerError", error)
                });
            }
            catch (Exception e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateBasicExceptionView(e, guid))
                });
            }
        }

        /// <summary>
        /// Function to authorize endpoints based on the logged in user
        /// </summary>
        /// <returns></returns>
        public bool Authorize()
        {
            try
            {
                var userId = User.Claims.First().Value;

                UserData ud = Auth0APIClient.GetUserData(userId);
                loggedInUser = ud;
                List<UserPermission> permissions = Auth0APIClient.GetPermissions(ud.user_id);
                bool authorized = false;

                foreach (UserPermission perm in permissions)
                {
                    if (perm.permission_name == ModelUtility.AccessLevel1 ||
                    perm.permission_name == ModelUtility.AccessLevel2 ||
                    perm.permission_name == ModelUtility.AccessLevel4)
                    {
                        authorized = true;
                        break;
                    }
                }

                if (authorized == false)
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);

                return authorized;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }

        }

    }
}

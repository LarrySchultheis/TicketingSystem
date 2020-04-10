using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
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


        //[Authorize]
        //public ViewResult Claims()
        //{
        //    return View();
        //}

        //public ViewResult AccessDenied()
        //{
        //    return View();
        //}

        /// <summary>
        /// Endpoint to return all the permissions associated with a given user 
        /// </summary>
        /// <returns>JSON containing permissions</returns>
        public JsonResult Permissions()
        {
            var userId = User.Claims.First().Value;
            UserData ud = Auth0APIClient.GetUserData(userId);
            List<UserPermission> permissions = Auth0APIClient.GetPermissions(ud.user_id);

            return Json(new
            {
                permissions = permissions
            });
        }

        /// <summary>
        /// Endpoint to return the UsersHome view with all users in the database
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResult> UsersHome()
        {
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

        public async Task<ViewResult> UserEdit(string userId)
        {
            UserManager um = new UserManager();
            try
            {
                return View("UserEdit", um.GetUserByID(int.Parse(userId)));
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
    }
}

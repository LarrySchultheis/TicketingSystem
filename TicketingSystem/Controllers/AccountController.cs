using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using TicketingSystem.Models;
using TicketingSystem.Services;

namespace SampleMvcApp.Controllers
{
    public class AccountController : Controller
    {


        public async Task Login()
        {
            var x = 0;
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = "/" });

        }

        [Authorize]
        public async Task Logout()
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

        /// <summary>
        /// This is just a helper action to enable you to easily see all claims related to a user. It helps when debugging your
        /// application to see the in claims populated from the Auth0 ID Token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

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
    }
}

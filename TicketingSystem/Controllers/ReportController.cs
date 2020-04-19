using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using System.Net.Http;
using Microsoft.Reporting.WebForms;
using System.Web.Http;
using System.Net;

namespace TicketingSystem.Controllers
{
    public class ReportController : Controller
    {
        public ViewResult Index()
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }
            return View();
        }

        /// <summary>
        /// Endpoint to run the selected report
        /// </summary>
        /// <param name="reportData"></param>
        /// <returns></returns>
        public async Task<JsonResult> RunReport(ReportInput reportData)
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
            HttpResponseMessage resp = null;
            try
            {
                ReportGenerator rg = new ReportGenerator();
                resp = await rg.GenerateReport(reportData);
                var content = resp.Content;
                var bytes = await resp.Content.ReadAsByteArrayAsync();

                return Json(new
                {
                    content = content,
                    data = bytes
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
        /// Function to authorize the currently logged in user
        /// </summary>
        /// <returns></returns>
        public bool Authorize()
        {
            try
            {
                var userId = User.Claims.First().Value;
                UserData ud = Auth0APIClient.GetUserData(userId);
                List<UserPermission> permissions = Auth0APIClient.GetPermissions(ud.user_id);
                bool authorized = false;

                foreach (UserPermission perm in permissions)
                {
                    if (perm.permission_name == ModelUtility.AccessLevel1 ||
                    perm.permission_name == ModelUtility.AccessLevel2 ||
                    perm.permission_name == ModelUtility.AccessLevel3)
                    {
                        authorized = true;
                        break;
                    }
                }

                if (authorized == false)
                    throw new System.Web.Http.HttpResponseException(HttpStatusCode.Unauthorized);

                return authorized;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }

        }
    }
}
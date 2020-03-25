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
        public IActionResult Index()
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateErrorView(e));
            }
            return View();
        }

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
                    code = (int)e.Response.StatusCode,
                    error = e.Response.StatusCode.ToString()
                });
            }
            HttpResponseMessage resp = null;
            try
            {
                ReportGenerator rg = new ReportGenerator();
                resp = await rg.GenerateReport(reportData);

            }
            catch (Exception e)
            {
                ExceptionReporter er = new ExceptionReporter();
                er.DumpException(e);
            }

            var content = resp.Content;
            var bytes = await resp.Content.ReadAsByteArrayAsync();


            return Json(new
            {
                content = content,
                data = bytes
            });

        }
        private bool Authorize()
        {
            var userId = User.Claims.First().Value;
            UserData ud = Auth0APIClient.GetUserData(userId);
            List<UserPermission> permissions = Auth0APIClient.GetPermissions(ud.user_id);
            bool authorized = false;

            foreach (UserPermission perm in permissions)
            {
                if (perm.permission_name == "access:lvl1" || perm.permission_name == "access:lvl2" || perm.permission_name == "access:lvl3")
                {
                    authorized = true;
                    break;
                }
            }

            if (authorized == false)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return authorized;
        }
    }
}
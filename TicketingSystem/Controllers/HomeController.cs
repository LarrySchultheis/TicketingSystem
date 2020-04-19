using System;
using TicketingSystem.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using System.Net;
using System.Web.Http;
using System.Web.Helpers;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting.Internal;
using System.Threading.Tasks;
using System.Reflection;

namespace TicketingSystem.Controllers
{
    public class HomeController : Controller
    {
        private static UserData loggedInUser;
        private static readonly int numberOfRecords = 1000;

        public ViewResult Landing()
        {
            return View("Landing");
        }

        /// <summary>
        /// Gets the Data Entry page
        /// </summary>
        /// <returns>DataEntry View</returns>
        public ViewResult DataEntry()
        {   
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }
            return View("DataEntry");
        }

        /// <summary>
        /// Get the EntryClose view with the specified TicketData entry
        /// </summary>
        /// <param name="td">TicketData entry from HomePage table</param>
        /// <returns>EntryClose View and TicketData entry</returns>
        public async Task<ViewResult> EntryClose(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }
            try
            {
                RecordRetriever rr = new RecordRetriever();
                var tdRes = rr.GetRecordByID(td.EntryId);
                return View("EntryClose", tdRes);
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
        /// Posts the ticket entry to be closed to the DataEntry.CloseTicket service 
        /// </summary>
        /// <param name="td">TicketData entry to close</param>
        /// <returns>HomePage view with Open records</returns>
        public async Task<ViewResult> PostEntryClose(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }
            try
            {
                DataEntry de = new DataEntry();
                RecordRetriever rr = new RecordRetriever();
                de.CloseTicket(td);
                var tdRes = rr.RetrieveRecords(numberOfRecords);
                return View("HomePage", tdRes);
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
        ///  Gets the HomePage view with current open records
        /// </summary>
        /// <returns>HomePage view with open records</returns>
        public async Task<ViewResult> HomePage()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    RecordRetriever rr = new RecordRetriever();
                    var records = rr.RetrieveRecords(numberOfRecords);
                    return View("HomePage", records);
                }
                else
                {
                    return View("Landing");
                }
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
        /// Gets both open and closed tickets 
        /// </summary>
        /// <returns>HomePage view with current records</returns>
        public async Task<ViewResult> AllTickets()
        {
            try
            {
                RecordRetriever rr = new RecordRetriever();
                var records = rr.RetrieveRecords(numberOfRecords);
                return View("HomePage", records);
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
        /// Endpoint to get only open records
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResult> OpenTickets()
        {
            try
            {
                RecordRetriever rr = new RecordRetriever();
                var records = rr.GetOpenRecords(numberOfRecords);
                return View("HomePage", records);
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
        /// Posts new TicketData entry to DataEntry.PostEntry service
        /// </summary>
        /// <param name="td">TicketData entry from DataEntry form</param>
        /// <returns>DataEntry view with TicketData entry</returns>
        public async Task<ViewResult> PostEntry(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }
            try
            {
                DataEntry de = new DataEntry();
                bool success = de.PostEntry(td, loggedInUser);
                RecordRetriever rr = new RecordRetriever();
                return View("HomePage", rr.RetrieveRecords(numberOfRecords));
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
        /// Gets the TicketData entry based on Entry ID of ticket clicked on in HomePage table
        /// </summary>
        /// <param name="entryID">Entry ID specified in HomePage table</param>
        /// <returns>JSON containing redirect URL</returns>
        public async Task<JsonResult> OpenEntry(string entryID)
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
                using (var context = new TicketingSystemDBContext())
                {
                    int id = int.Parse(entryID);
                    RecordRetriever rr = new RecordRetriever();
                    TicketData td = rr.GetRecordByID(id);

               
                    return Json(new
                    {
                        newUrl = Url.Action("EntryClose", "Home", td)
                    });
                }
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
        /// Returns a list of valid worker names in the system
        /// </summary>
        /// <returns></returns>
        public JsonResult ValidNames()
        {
            return Json(new
            {
                names = Utility.GetValidNames()
            });
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

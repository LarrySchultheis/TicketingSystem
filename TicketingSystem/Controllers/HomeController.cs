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

        public IActionResult Landing()
        {
            return View("Landing");
        }

        public IActionResult Privacy()
        {
            return View("Index");
        }

        /// <summary>
        /// Gets the Data Entry page
        /// </summary>
        /// <returns>DataEntry View</returns>
        public IActionResult DataEntry()
        {   
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateErrorView(e, "You do not have the permissions to view this page"));
            }
            return View("DataEntry");
        }

        /// <summary>
        /// Get the EntryClose view with the specified TicketData entry
        /// </summary>
        /// <param name="td">TicketData entry from HomePage table</param>
        /// <returns>EntryClose View and TicketData entry</returns>
        public IActionResult EntryClose(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateErrorView(e, "You do not have the permissions to view this page"));
            }
            RecordRetriever rr = new RecordRetriever();
            var tdRes = rr.GetRecordByID(td.EntryId);
            return View("EntryClose", tdRes);
        }

        /// <summary>
        /// Posts the ticket entry to be closed to the DataEntry.CloseTicket service 
        /// </summary>
        /// <param name="td">TicketData entry to close</param>
        /// <returns>HomePage view with Open records</returns>
        public IActionResult PostEntryClose(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateErrorView(e, "You do not have the permissions to view this page"));
            }
            DataEntry de = new DataEntry();
            RecordRetriever rr = new RecordRetriever();
            de.CloseTicket(td);
            var tdRes = rr.RetrieveRecords(numberOfRecords);
            return View("HomePage", tdRes);
        }

        /// <summary>
        ///  Gets the HomePage view with current open records
        /// </summary>
        /// <returns>HomePage view with open records</returns>
        public async Task<IActionResult> HomePage()
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

                //string message = await Utility.RenderErrorResponse(e.Response);
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return View("ServerError", error);
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return View("Landing");
            }

        }

        /// <summary>
        /// Gets both open and closed tickets 
        /// </summary>
        /// <returns>HomePage view with current records</returns>
        public IActionResult AllTickets()
        {
            RecordRetriever rr = new RecordRetriever();
            var records = rr.RetrieveRecords(numberOfRecords);
            return View("HomePage", records);
        }

        public IActionResult OpenTickets()
        {
            RecordRetriever rr = new RecordRetriever();
            var records = rr.GetOpenRecords(numberOfRecords);
            return View("HomePage", records);
        }

        /// <summary>
        /// Posts new TicketData entry to DataEntry.PostEntry service
        /// </summary>
        /// <param name="td">TicketData entry from DataEntry form</param>
        /// <returns>DataEntry view with TicketData entry</returns>
        public IActionResult PostEntry(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateErrorView(e, "You do not have the permissions to view this page"));
            }
            try
            {
                DataEntry de = new DataEntry();
               
               // UserData loggedInUser = Auth0APIClient.GetUserData(User.Claims.First().Value);
                bool success = de.PostEntry(td, loggedInUser);
            }

            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
            }
            RecordRetriever rr = new RecordRetriever();
            return View("HomePage", rr.RetrieveRecords(numberOfRecords));
        }

        /// <summary>
        /// Gets the TicketData entry based on Entry ID of ticket clicked on in HomePage table
        /// </summary>
        /// <param name="entryID">Entry ID specified in HomePage table</param>
        /// <returns>JSON containing redirect URL</returns>
        public JsonResult OpenEntry(string entryID)
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
            catch(Exception e)
            {
                ExceptionReporter.DumpException(e);
                RecordRetriever rr = new RecordRetriever();

                //If exception occurred return the home page
                return Json(new
                {
                    newUrl = Url.Action("HomePage", "Home", rr.RetrieveRecords(numberOfRecords))
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { ErrorCode = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Function to authorize endpoints based on the logged in user
        /// </summary>
        /// <returns></returns>
        public bool Authorize()
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
    }
}

using System;
using TicketingSystem.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;

namespace TicketingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IEnumerable<TicketData> latestData;

        [HttpGet]
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
        [HttpGet]
        public IActionResult DataEntry()
        {            
            return View("DataEntry");
        }

        /// <summary>
        /// Get the EntryClose view with the specified TicketData entry
        /// </summary>
        /// <param name="td">TicketData entry from HomePage table</param>
        /// <returns>EntryClose View and TicketData entry</returns>
        public IActionResult EntryClose(TicketData td)
        {
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
            DataEntry de = new DataEntry();
            RecordRetriever rr = new RecordRetriever();
            de.CloseTicket(td);
            var tdRes = rr.RetrieveRecords();
            return View("HomePage", tdRes);
        }

        /// <summary>
        ///  Gets the HomePage view with current open records
        /// </summary>
        /// <returns>HomePage view with open records</returns>
        [HttpGet]
        public IActionResult HomePage()
        {
            if (User.Identity.IsAuthenticated)
            {
                //Code snippet to grab user info

                var userId = User.Claims.First().Value;
                Auth0APIClient a0client = new Auth0APIClient();
                a0client.UpdateUsers(userId);
                var users = a0client.GetAllUsers();
                //UserDataServer uds = new UserDataServer();
                //UserData ud = uds.GetUserData(userId);


                RecordRetriever rr = new RecordRetriever();
                var records = rr.RetrieveRecords();
                latestData = records;
                return View("HomePage", records);
            }
            else
            {
                return View("Landing");
            }
        }



        /// <summary>
        /// Gets both open and closed tickets 
        /// </summary>
        /// <returns>HomePage view with current records</returns>
        [HttpGet]
        public IActionResult AllTickets()
        {
            RecordRetriever rr = new RecordRetriever();
            var records = rr.RetrieveRecords();
            return View("HomePage", records);
        }

        [HttpGet]
        public IActionResult OpenTickets()
        {
            RecordRetriever rr = new RecordRetriever();
            var records = rr.GetOpenRecords();
            return View("HomePage", records);
        }

        [HttpPost]
        public IActionResult VerifyLogin(Users user)
        {
            return View("HomePage", user);
        }

        /// <summary>
        /// Posts new TicketData entry to DataEntry.PostEntry service
        /// </summary>
        /// <param name="td">TicketData entry from DataEntry form</param>
        /// <returns>DataEntry view with TicketData entry</returns>
        [HttpPost]
        public IActionResult PostEntry(TicketData td)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    DataEntry de = new DataEntry();
                    bool success = de.PostEntry(td);
                }
            }
            catch(Exception e)
            {
                ExceptionReporter er = new ExceptionReporter();
                er.DumpException(e);
            }
            RecordRetriever rr = new RecordRetriever();
            return View("HomePage", rr.RetrieveRecords());
        }

        /// <summary>
        /// Gets the TicketData entry based on Entry ID of ticket clicked on in HomePage table
        /// </summary>
        /// <param name="entryID">Entry ID specified in HomePage table</param>
        /// <returns>JSON containing redirect URL</returns>
        [HttpPost]
        public JsonResult OpenEntry(string entryID)
        {
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
                ExceptionReporter er = new ExceptionReporter();
                er.DumpException(e);
                RecordRetriever rr = new RecordRetriever();

                //If exception occurred return the home page
                return Json(new
                {
                    newUrl = Url.Action("HomePage", "Home", rr.RetrieveRecords())
                });
            }
        }

        //public IActionResult CloseTicket()
        //{
        //    using (var context = new TicketingSystemDBContext())
        //    {
        //        var td = context.TicketData.Where(t => t.EntryAuthor.Email == "admin123@gmail.com").FirstOrDefault();
        //        DataEntry de = new DataEntry();
        //        bool success = de.CloseTicket(td);
        //    }
        //    RecordRetriever rr = new RecordRetriever();
        //    return View("HomePage", rr.RetrieveRecords());
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { ErrorCode = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using TicketingSystem.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketingSystem.Models;
namespace TicketingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IEnumerable<TicketData> latestData;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View("Index");
        }

        [HttpGet]
        public IActionResult DataEntry()
        {            
            return View("DataEntry");
        }

        [HttpGet]
        public IActionResult ReportsHome()
        {
            return View("ReportsHome");
        }

        public IActionResult EntryClose(TicketData td)
        {
            RecordRetriever rr = new RecordRetriever();
            var tdRes = rr.GetRecordByID(td.EntryId);
            return View("EntryClose", tdRes);
        }

        [HttpPost]
        public IActionResult PostEntryClose(TicketData td)
        {
            RecordRetriever rr = new RecordRetriever();
            DataEntry de = new DataEntry();
            de.CloseTicket(td);
            return View("HomePage", rr.GetOpenRecords());
        }

        [HttpGet]
        public IActionResult HomePage()
        {
            RecordRetriever rr = new RecordRetriever();
            var records = rr.GetOpenRecords();
            latestData = records;
            return View("HomePage", records);
        }

        [HttpPost]
        public IActionResult VerifyLogin(Users user)
        {

            return View("HomePage", user);
        }

        [HttpPost]
        public IActionResult PostEntry(TicketData td)
        {
            DataEntry de = new DataEntry();
            bool success = de.PostEntry(td);

            return View("DataEntry", td);
        }

        [HttpPost]
        public JsonResult OpenEntry(string entryID)
        {
            int id = int.Parse(entryID);
            RecordRetriever rr = new RecordRetriever();
            TicketData td = rr.GetRecordByID(id);
            JsonResult tdJson = new JsonResult(td);
            //return View("EntryClose", td);
            return Json(new
            {
                newUrl = Url.Action("EntryClose", "Home", td)
            });
        }

        public IActionResult CloseTicket()
        {
            using (var context = new TicketingSystemDBContext())
            {
                var td = context.TicketData.Where(t => t.EntryAuthor.Email == "admin123@gmail.com").FirstOrDefault();
                DataEntry de = new DataEntry();
                bool success = de.CloseTicket(td);
            }
            RecordRetriever rr = new RecordRetriever();
            return View("HomePage", rr.GetOpenRecords());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

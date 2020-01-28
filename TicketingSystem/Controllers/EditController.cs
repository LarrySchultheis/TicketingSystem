using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Models;
using TicketingSystem.Services;
using System.Diagnostics;
using TicketingSystem.ExceptionReport;

namespace TicketingSystem.Controllers
{
    public class EditController : Controller
    {
        /// <summary>
        /// Gets Index page when HomePage/Index is hit
        /// </summary>
        /// <returns>
        /// Index View
        /// </returns>
        public IActionResult Index()
        {
            RecordRetriever rr = new RecordRetriever();
            var res = rr.RetrieveRecords();
            return View("Index", res);
        }

        /// <summary>
        /// Displays EditForm view with TicketData instance as the model
        /// </summary>
        /// <param name="td">TicketData instance</param>
        /// <returns>EditForm View with specified TicketData entry</returns>
        public IActionResult EditForm(TicketData td)
        {
            RecordRetriever rr = new RecordRetriever();
            var tdRes = rr.GetRecordByID(td.EntryId);
            return View("EditForm", tdRes);
        }

        /// <summary>
        /// Gets the TicketData entry based on the entryID input in Index page
        /// </summary>
        /// <param name="entryId">Entry ID input from index</param>
        /// <returns>JSON holding redirect URL</returns>
        public JsonResult GetRecord(string entryId)
        {
            try
            {
                using (var context = TicketingSystemDBContext())
                {
                    RecordRetriever rr = new RecordRetriever();
                    var result = rr.GetRecordByID(int.Parse(entryId));

                    return Json(new
                    {
                        newUrl = Url.Action("EditForm", "Edit", result)
                    });
                }
            }
            catch(Exception e)
            {
                ExceptionReporter er = new ExceptionReporter();
                er.DumpException(e);
                return false;
            }
        }


        /// <summary>
        /// Posts edited ticket data entry to DataEditor service
        /// </summary>
        /// <param name="td">TicketData instance from edit form</param>
        /// <returns>Index View</returns>
        public IActionResult PostEdit(TicketData td)
        {
            DataEditor de = new DataEditor();
            de.PostEditor(td);
            RecordRetriever rr = new RecordRetriever();
            var res = rr.RetrieveRecords();
            return View("Index", res);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Models;
using TicketingSystem.Services;
using System.Diagnostics;

namespace TicketingSystem.Controllers
{
    public class EditController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditForm(TicketData td)
        {
            RecordRetriever rr = new RecordRetriever();
            var tdRes = rr.GetRecordByID(td.EntryId);
            return View("EditForm", tdRes);
        }

        public JsonResult GetRecord(string entryId)
        {
            RecordRetriever rr = new RecordRetriever();
            var result = rr.GetRecordByID(int.Parse(entryId));

            return Json(new
            {
                newUrl = Url.Action("EditForm", "Edit", result)
            });
        }

        public IActionResult PostEdit(TicketData td)
        {
            DataEditor de = new DataEditor();
            de.PostEditor(td);
            return View("Index");
        }
    }
}
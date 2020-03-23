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

namespace TicketingSystem.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> RunReport(ReportInput reportData)
        {
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
    }
}
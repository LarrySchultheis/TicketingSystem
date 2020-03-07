using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;

namespace TicketingSystem.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RunReport(ReportInput reportData)
        {
            try
            {
                ReportGenerator rg = new ReportGenerator();
                rg.GenerateReport(reportData);

            }
            catch (Exception e)
            {
                ExceptionReporter er = new ExceptionReporter();
                er.DumpException(e);
            }

            return View("Index");
        }
    }
}
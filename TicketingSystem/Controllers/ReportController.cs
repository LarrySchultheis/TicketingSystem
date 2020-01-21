using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Services;

namespace TicketingSystem.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RunReport(string reportType)
        {
            ReportGenerator rg = new ReportGenerator();


            if (reportType == "0")
                rg.GenerateIncentveReport();

            else if (reportType == "1")
                rg.GenerateLaborHoursByJob();

            else
                rg.GenerateLaborHoursByJobAndEmployee();
                
            return View("Index");
        }
    }
}
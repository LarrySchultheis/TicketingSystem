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

                if (reportData.ReportType == 0)
                    rg.GenerateIncentveReport(reportData.StartDate, reportData.EndDate);

                else if (reportData.ReportType == 1)
                    rg.GenerateLaborHoursByJob();//reportData.StartDate, reportData.EndDate);

                else if (reportData.ReportType == 2)
                    rg.GenerateLaborHoursByJobAndEmployee(reportData.StartDate, reportData.EndDate);

                else
                {
                    Exception e = new Exception("Error, Report Type is not valid");
                    throw e;
                }
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
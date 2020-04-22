using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Services;
using TicketingSystem.Models;

namespace Tests.ServiceTests
{
    [TestFixture]
    class ReportGeneratorTests
    {
        ReportGenerator rg;

        public ReportGeneratorTests()
        {
            rg = new ReportGenerator();
        }

        [Test]
        public async Task GenerateReportTest()
        {
            ReportInput reportInput = new ReportInput();
            reportInput.ReportName = ReportName.LaborHoursByJob;
            reportInput.ReportFormat = ReportFormat.PDF;
            reportInput.StartDate = DateTime.Today;
            reportInput.EndDate = DateTime.Today;

            var result = rg.GenerateReport(reportInput);
            Assert.IsNotNull(result);

            reportInput.ReportName = ReportName.LaborHoursByJob;
            reportInput.ReportFormat = ReportFormat.PDF;
            reportInput.StartDate = DateTime.Today;
            reportInput.EndDate = DateTime.Today;

            result = rg.GenerateReport(reportInput);
            Assert.IsNotNull(result);

            reportInput.ReportName = ReportName.LaborHoursByJob;
            reportInput.ReportFormat = ReportFormat.PDF;
            reportInput.StartDate = DateTime.Today;
            reportInput.EndDate = DateTime.Today;

            result = rg.GenerateReport(reportInput);
            Assert.IsNotNull(result);

            reportInput.ReportName = ReportName.LaborHoursByJob;
            reportInput.ReportFormat = ReportFormat.PDF;
            reportInput.StartDate = DateTime.Today;
            reportInput.EndDate = DateTime.Today;

            result = rg.GenerateReport(reportInput);
            Assert.IsNotNull(result);
        }
    }
}

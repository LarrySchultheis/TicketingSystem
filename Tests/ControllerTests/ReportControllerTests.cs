using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Controllers;
using TicketingSystem.Models;

namespace Tests.ControllerTests
{
	[TestFixture]
	class ReportControllerTests
	{
		ReportController rc;
		ReportInput reportInput;

		public ReportControllerTests()
		{
			Users dbUser;
			using (var db = new TicketingSystemDBContext())
			{
				dbUser = db.Users.Where(u => u.FullName == "Test User").First();
			}
			var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
			new Claim(ClaimTypes.Name, dbUser.Auth0Uid)

			}, "mock"));

			rc = new ReportController();
			rc.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext
				{
					User = user
				}
			};

			InitReport();
		}

		private void InitReport()
		{
			reportInput = new ReportInput();
			DateTime now = DateTime.Now;
			DateTime lastWeek = now.AddDays(-(int)now.DayOfWeek - 7);

			reportInput.StartDate = lastWeek;
			reportInput.EndDate = DateTime.Now;
		}

		[Test]
		public void IndexTest()
		{
			Assert.IsNotNull(rc.Index());
		}

		[Test]
		public async Task RunLaborHoursByJobReportPDFTest()
		{
			reportInput.ReportFormat = ReportFormat.PDF;
			reportInput.ReportName = ReportName.LaborHoursByJob;
			var result = await rc.RunReport(reportInput);
			Assert.IsNotNull(result);
		}

		[Test]
		public async Task RunLaborHoursByJobReportCSVTest()
		{
			reportInput.ReportFormat = ReportFormat.CSV;
			reportInput.ReportName = ReportName.LaborHoursByJob;
			var result = await rc.RunReport(reportInput);
			Assert.IsNotNull(result);
		}

		[Test]
		public async Task RunLaborHoursByJobAndEmployeReportPDFTest()
		{
			reportInput.ReportFormat = ReportFormat.PDF;
			reportInput.ReportName = ReportName.LaborHoursByJobAndEmployee;
			var result = await rc.RunReport(reportInput);
			Assert.IsNotNull(result);
		}

		[Test]
		public async Task RunLaborHoursByJobAndEmployeReportCSVTest()
		{
			reportInput.ReportFormat = ReportFormat.CSV;
			reportInput.ReportName = ReportName.LaborHoursByJobAndEmployee;
			var result = await rc.RunReport(reportInput);
			Assert.IsNotNull(result);
		}

		[Test]
		public void AuthorizeTest()
		{
			Assert.IsTrue(rc.Authorize());
		}

		[TearDown]
		public void Cleanup()
		{
			TestUtility.Cleanup();
		}
	}
}

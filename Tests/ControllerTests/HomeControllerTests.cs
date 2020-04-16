using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TicketingSystem.Controllers;
using TicketingSystem.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TicketingSystem.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Threading.Tasks;

namespace Tests.ControllerTests
{
    [TestFixture]
    class HomeControllerTests : Controller
    {
        HomeController hc;
        public HomeControllerTests()
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

            hc = new HomeController();
            hc.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
        }
        [Test]
        public async Task HomePageTest()
        {
            ViewResult result = await hc.HomePage();
            ViewResult testView = View("HomePage", "Home");

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }

        [Test]
        public void LandingTest()
        {
            ViewResult result = hc.Landing();
            ViewResult testView = View("Landing", "Home");

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }

        [Test]
        public void DataEntryTest()
        {
            ViewResult result = hc.DataEntry();
            ViewResult testView = View("DataEntry", "Home");

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }

        public async Task EntryCloseTest(TicketData td)
        {
            ViewResult result = await hc.EntryClose(td);
            ViewResult testView = View("HomePage", "Home");

            testView = View("EntryClose", "Home");

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewData, result.ViewData);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }


        [Test]
        public async Task PostEntryTest()
        {
            TicketData td = CreateTestData();

            ViewResult result = await hc.PostEntry(td);
            ViewResult testView = View("HomePage", "Home");

            await EntryCloseTest(td);
            await PostEntryCloseTest(td);

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewData, result.ViewData);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }

        public async Task PostEntryCloseTest(TicketData td)
        {

            ViewResult result = await hc.PostEntryClose(td);
            ViewResult testView = View("HomePage", "Home");

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewData, result.ViewData);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }

        [Test]
        public async Task AllTicketsTest()
        {
            ViewResult result = await hc.AllTickets();
            ViewResult testView = View("HomePage", "Home");

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewData, result.ViewData);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }

        [Test]
        public async Task OpenTicketsTest()
        {
            ViewResult result = await hc.OpenTickets();
            ViewResult testView = View("HomePage", "Home");

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewData, result.ViewData);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }

        //[Test]
        //public void OpenEntryTest()
        //{
        //    using (var db = new TicketingSystemDBContext())
        //    {
        //        Users user = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
        //        TicketData td = db.TicketData.Where(t => t.EntryAuthorId == user.UserId).FirstOrDefault();
        //        td.TicketWorker = db.Users.Where(u => u.UserId == td.TicketWorkerId).FirstOrDefault();
        //        td.JobType = db.JobType.Where(jt => jt.JobTypeId == td.JobTypeId).FirstOrDefault();
        //        string x = td.EntryId.ToString();
        //        var result = hc.OpenEntry(x);

        //        Assert.IsNotNull(result);
        //    }

        //}

        [Test]
        public void ValidNamesTest()
        {
            Assert.IsNotNull(hc.ValidNames());
        }

        [Test]
        public void AuthorizeTest()
        {
            Assert.IsTrue(hc.Authorize());
        }

        public TicketData CreateTestData()
        {
            using (var context = new TicketingSystemDBContext())
            {
                JobType j = new JobType();
                j.JobName = "Miscellaneous";

                RecordRetriever rr = new RecordRetriever();
                var records = rr.RetrieveRecords(10);

                Users ticketWorker = context.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
                Users entryAuthor = context.Users.Where(w => w.FullName == "Test User").FirstOrDefault();

                TicketData td = new TicketData()
                {
                    TripNum = -1,
                    StageNum = "S01",
                    JobType = j,
                    EntryAuthor = entryAuthor,
                    EntryAuthorId = entryAuthor.UserId,
                    TicketWorker = ticketWorker,
                    TicketWorkerId = ticketWorker.UserId,
                    WorkerName = "Test User",
                    StartTime = "12:00",
                    EndTime = "12:00",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today,
                    Comments = "Entry generated by HomeController.PostEntry unit test"
                };
                return td;
            }
        }

        [TearDown]
        public void Cleanup()
        {
            TestUtility.Cleanup();
        }
    }

}

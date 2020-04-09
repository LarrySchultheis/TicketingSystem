using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TicketingSystem.Controllers;
using TicketingSystem.Models;
using TicketingSystem.Services;

namespace Tests.ControllerTests
{
    [TestFixture]
    class EditControllerTests : Controller
    {
        EditController ec;
        public EditControllerTests()
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

            ec = new EditController();
            ec.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
        } 

        [Test]
        public void IndexTest()
        {
            ViewResult result = (ViewResult) ec.Index();
            ViewResult testView = View("Index", "Edit");

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }

        [Test]
        public void EditFormTest()
        {

            using (var db = new TicketingSystemDBContext())
            {
                Users user = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();

                DataEntry de = new DataEntry();
                de.PostEntry(TestUtility.CreateTestData(), Auth0APIClient.GetUserData(user.Auth0Uid));

                TicketData td = db.TicketData.Where(t => t.EntryAuthorId == user.UserId).FirstOrDefault();
                
                ViewResult result = (ViewResult)ec.EditForm(td);
                ViewResult testView = View("EditForm", td);

                Assert.IsNotNull(result);
                Assert.AreEqual(testView.ViewName, result.ViewName);
            }
        }

        [Test]
        public void ErrorTest()
        {
            ErrorViewModel error = new ErrorViewModel();
            error.ErrorCode = "401";
            ViewResult result = (ViewResult)ec.Error();
            ViewResult testView = View("Error", error);

            Assert.IsNotNull(result);
            Assert.AreEqual(testView.ViewName, result.ViewName);
        }

        [Test]
        public void PostEditorTest()
        {
            using (var db = new TicketingSystemDBContext())
            {
                Users user = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();

                DataEntry de = new DataEntry();
                de.PostEntry(TestUtility.CreateTestData(), Auth0APIClient.GetUserData(user.Auth0Uid));

                TicketData td = db.TicketData.Where(t => t.EntryAuthorId == user.UserId).FirstOrDefault();
                td.Comments = "Changed entry in PostEditTest";

                ViewResult result = (ViewResult)ec.PostEdit(td);
                RecordRetriever rr = new RecordRetriever();
                ViewResult testView = View("Index", rr.RetrieveRecords(10));

                Assert.IsNotNull(result);
                Assert.AreEqual(testView.ViewName, result.ViewName);
            }
        }

        [Test]
        public void AuthorizeTest()
        {
            Assert.IsTrue(ec.Authorize());
        }

        [TearDown]
        public void Cleanup()
        {
            TestUtility.Cleanup();
        }
    }
}

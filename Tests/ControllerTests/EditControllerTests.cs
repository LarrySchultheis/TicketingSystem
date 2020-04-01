using System;
using System.Collections.Generic;
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
    }
}

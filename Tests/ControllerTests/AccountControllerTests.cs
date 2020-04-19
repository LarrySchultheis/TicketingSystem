using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using NUnit.Framework;
using SampleMvcApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using TicketingSystem.ExceptionReport;
using TicketingSystem.Models;
using TicketingSystem.Services;

namespace Tests.ControllerTests
{
    [TestFixture]
    class AccountControllerTests : Controller
    {
        AccountController ac;
        public AccountControllerTests()
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

            ac = new AccountController();
            ac.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
        }

        [Test]
        public async Task PermissionsTest()
        {
            JsonResult json = await ac.Permissions();
            string val = json.Value.ToString();

            Assert.IsNotNull(json);
            Assert.IsTrue(val.Contains("permissions"));

        }

        [Test]
        public async Task UsersHomeTest()
        {
            UserManager um = new UserManager();
            ViewResult actual = await ac.UsersHome();
            ViewResult expected = View("UsersHome");

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.ViewName, expected.ViewName);
        }

        //[Test]
        //public async Task UserEditTest()
        //{
        //    using (var db = new TicketingSystemDBContext())
        //    {
        //        Users user = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
        //        ViewResult actual = await ac.EditUser(user.UserId.ToString());
        //        ViewResult expected = View("UserEdit");

        //        Assert.IsNotNull(actual);
        //        Assert.AreEqual(actual.ViewName, expected.ViewName);
        //    }
        //}

        [Test]
        public async Task CreateUserTest()
        {
            Users newUser = TestUtility.CreateTestUser();

            ViewResult actual = await ac.CreateUser(newUser);
            ViewResult expected = View("UsersHome");

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.ViewName, expected.ViewName);
        }


        [Test]
        public void AuthorizeTest()
        {
            var result = ac.Authorize();
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetEmailsTest()
        {
            JsonResult res = await ac.GetEmails();
            string val = res.Value.ToString();
            Assert.IsNotNull(res);
            Assert.IsTrue(val.Contains("emails"));
        }

        [TearDown]
        public void Cleanup()
        {
            TestUtility.UserCleanup();
        }
    }
}

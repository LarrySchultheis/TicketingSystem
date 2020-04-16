using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using NUnit.Framework;
using SampleMvcApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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

            DeleteUserTest(newUser);

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.ViewName, expected.ViewName);
        }

        public void DeleteUserTest(Users user)
        {
            //Users newUser = TestUtility.CreateTestUser();
            //UserManager um = new UserManager();
            //um.CreateUser(newUser, Auth0APIClient.GetUserData(User.Claims.First().Value));
            //using (var db = new TicketingSystemDBContext())
            //{
            //    JsonResult result = ac.DeleteUser(db.Users.Where(u => u.FullName == "Unit Test User").First().UserId.ToString());
            //    string val = result.Value.ToString();

            //    Assert.IsNotNull(result);
            //}

            using (var db = new TicketingSystemDBContext())
            {
                string auth0Id = db.Users.Find(user.UserId).Auth0Uid;
                JsonResult res = ac.ToggleActivation(auth0Id);

                Assert.IsNotNull(res);
            }


        }

        [Test]
        public void AuthorizeTest()
        {
            var result = ac.Authorize();
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TearDown]
        public void Cleanup()
        {
            UserManager um = new UserManager();
            using (var db = new TicketingSystemDBContext())
            {
                var users = db.Users.Where(u => u.FullName == "Unit Test User").ToList();
                foreach (Users u in users)
                {
                    um.ToggleActivation(u.UserId);
                }
            }
        }
    }
}

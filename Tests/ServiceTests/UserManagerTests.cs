﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.Services;
using TicketingSystem.Models;
using System.Linq;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

namespace Tests.ServiceTests
{
    [TestFixture]
    class UserManagerTests
    {
        UserManager um;
        public UserManagerTests()
        {
            um = new UserManager();
        }

        [Test]
        public void GetUsersTest()
        { 
            var actual = um.GetUsers();
            List<Users> expected;

            using (var db = new TicketingSystemDBContext())
            {
                expected = db.Users.ToList();
            }
            Assert.IsNotNull(actual);

        }

        [Test]
        public void GetUserByIDTest()
        {
            using (var db = new TicketingSystemDBContext())
            {
                int userId = db.Users.FirstOrDefault().UserId;
                var actual = um.GetUserByID(userId);

                Assert.IsNotNull(actual);
            }
        }

        [Test]
        public void CreateUserTest()
        {


            using (var db = new TicketingSystemDBContext())
            {
                Users newUser = TestUtility.CreateTestUser();

                Users loggedUser = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
                UserData userData = Auth0APIClient.GetUserData(loggedUser.Auth0Uid);

                bool actual = um.CreateUser(newUser, userData);

                DeleteUserTest(newUser);

                Assert.IsTrue(actual);
            }
        }

        public void DeleteUserTest(Users user)
        {

                var actual = um.DeleteUser(user.UserId);

                Assert.IsTrue(actual);
            
        }

        //[TearDown]
        //public void Cleanup()
        //{
        //    TestUtility.UserCleanup();
        //}
    }
}

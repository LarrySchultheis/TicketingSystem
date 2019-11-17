﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TicketingSystem.Controllers;
using TicketingSystem.Models;
using System.Linq;

namespace Tests.ControllerTests
{
    [TestFixture]
    class HomeControllerTests
    {
        HomeController hc;
        public HomeControllerTests()
        {
            hc = new HomeController();
        }
        [Test]
        public void HomePageTest()
        {
            var result = hc.HomePage();
            Assert.IsNotNull(result);
        }

        [Test]
        public void VerifyLoginTest()
        {
            var result = hc.VerifyLogin(new Users());
            Assert.IsNotNull(result);
        }

        [Test]
        public void DataEntryTest()
        {
            var result = hc.DataEntry();
            Assert.IsNotNull(result);
        }

        [Test]
        public void PostEntryTest()
        {
            using (var context = new TicketingSystemDBContext())
            {
                TicketData td = new TicketData()
                {
                    TripNum = -1,
                    StageNum = "-1",
                    JobTypeId = 13,
                    EntryAuthor = context.Users.Where(e => e.Email == "admin123@gmail.com").FirstOrDefault(),
                    StartTime = DateTime.Now,
                    Comments = "Entry generated by automated unit test"
                };
                var result = hc.PostEntry(td);
                Assert.IsNotNull(result);
            }

        }
    }
}

﻿//using System;
//using System.Collections.Generic;
//using System.Text;
//using NUnit.Framework;

//namespace Prototype.Tests.ControllerTests
//{
//    [TestFixture]
//    class HomeControllerTests
//    {
//        HomeController hc;
//        public HomeControllerTests()
//        {
//            hc = new HomeController();
//        }
//        [Test]
//        public void HomePageTest()
//        {
//            var result = hc.HomePage();
//            Assert.IsNotNull(result);
//        }

//        [Test]
//        public void VerifyLoginTest()
//        {
//            var result = hc.VerifyLogin(new User());
//            Assert.IsNotNull(result);
//        }

//        [Test]
//        public void DataEntryTest()
//        {
//            var result = hc.DataEntry();
//            Assert.IsNotNull(result);
//        }

//        [Test]
//        public void PostEntryTest()
//        {
//            TicketData td = new TicketData()
//            {
//                TripNumber = -1,
//                StageNumber = "-1",
//                JobTypeId = 13,
//                EmployeeName = "ControllerPostEntryTest",
//                StartTime = DateTime.Now,
//                Comments = "Entry generated by automated unit test"
//            };
//            var result = hc.PostEntry(td);
//            Assert.IsNotNull(result);
//        }
//    }
//}

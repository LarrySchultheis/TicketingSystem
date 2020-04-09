using NUnit.Framework;
using System.Collections.Generic;
using TicketingSystem.Services;
using TicketingSystem.Models;
using System;
using System.Linq;

namespace Tests.ServiceTests
{
    [TestFixture]
    public class DataEntryTests
    {
        [Test]
        public void PostEntryTest()
        {
            using (var context = new TicketingSystemDBContext())
            {
                TicketData td = TestUtility.CreateTestData();

                DataEntry de = new DataEntry();
                Users dbUser = context.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
                Assert.IsTrue(de.PostEntry(td, Auth0APIClient.GetUserData(dbUser.Auth0Uid)));
            }
        }

        [Test]
        public void CloseTicketTest()
        {
            using (var context = new TicketingSystemDBContext())
            {
                var td = context.TicketData.FirstOrDefault();
                DataEntry de = new DataEntry();
                Assert.IsTrue(de.CloseTicket(td));
            }

        }

        [TearDown]
        public void Cleanup()
        {
            TestUtility.Cleanup();
        }
    }
}

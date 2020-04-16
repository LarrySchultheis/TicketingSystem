using NUnit.Framework;
using System.Collections.Generic;
using TicketingSystem.Services;
using TicketingSystem.Models;
using System;
using System.Linq;
using NUnit.Framework.Internal;

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

                UserData ud = Auth0APIClient.GetUserData(dbUser.Auth0Uid);

                var result = de.PostEntry(td, ud);


                Assert.IsTrue(result);
            }
        }

        public void CloseTicketTest(TicketData td)
        {

            DataEntry de = new DataEntry();
            Assert.IsTrue(de.CloseTicket(td));
            
        }

        [TearDown]
        public void Cleanup()
        {
            TestUtility.Cleanup();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TicketingSystem.Services;
using TicketingSystem.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Internal;

namespace Tests.ServiceTests
{
    [TestFixture]
    class DataEditorTest
    {
        [Test]
        public void PostEditTest()
        {
            using (var context = new TicketingSystemDBContext())
            {
                TicketData td = TestUtility.CreateTestData();

                DataEditor DaEd = new DataEditor();
                Users dbUser = context.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
                UserData ud = Auth0APIClient.GetUserData(dbUser.Auth0Uid);

                var result = DaEd.PostEditor(td, ud);

                DeleteEntryTest(td, ud);

                Assert.IsTrue(result);
            }
        }

        public void DeleteEntryTest(TicketData td, UserData ud)
        {
            DataEditor dataEditor = new DataEditor();
            var result = dataEditor.DeleteEntry(td.EntryId.ToString(), ud);
            Assert.IsTrue(result);
            
        }

        [TearDown]
        public void Cleanup()
        {
            TestUtility.Cleanup();
        }
    }
}

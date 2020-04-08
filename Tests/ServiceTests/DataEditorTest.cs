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
                Assert.IsTrue(DaEd.PostEditor(td, Auth0APIClient.GetUserData(dbUser.Auth0Uid)));
            }
        }

        [Test]
        public void DeleteEntryTest()
        {
            using (var db = new TicketingSystemDBContext())
            {
                Users user = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();

                DataEntry dataEntry = new DataEntry();
                dataEntry.PostEntry(TestUtility.CreateTestData(), Auth0APIClient.GetUserData(user.Auth0Uid));

                TicketData td = TestUtility.GetTestData();

                UserData ud = Auth0APIClient.GetUserData(td.EntryAuthor.Auth0Uid);

                DataEditor dataEditor = new DataEditor();
                Assert.IsTrue(dataEditor.DeleteEntry(td.EntryId.ToString(), ud));


            }
        }

        [TearDown]
        public void Cleanup()
        {
            TestUtility.Cleanup();
        }
    }
}

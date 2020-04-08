using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Http;
using TicketingSystem.Services;

namespace Tests.ServiceTests
{
    [TestFixture]
    class UtilityTests
    {
        [Test]
        public void CreateErrorViewTest()
        {
            HttpResponseException e = new HttpResponseException(HttpStatusCode.OK);
            var result = Utility.CreateErrorView(e, "test");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetValidNamesTest()
        {
            Assert.IsNotNull(Utility.GetValidNames());
        }

        [Test]
        public void GetUserByNameTest()
        {
            Assert.IsNotNull(Utility.GetUserByName("Test User"));
        }
    }
}

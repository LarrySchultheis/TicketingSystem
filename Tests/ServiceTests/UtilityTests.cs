using NUnit.Framework;
using NUnit.Framework.Interfaces;
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
        string encrypted;
        string message = "I hope this works";

        [Test]
        public void CreateErrorViewTest()
        {
            HttpResponseException e = new HttpResponseException(HttpStatusCode.OK);
            var result = Utility.CreateHttpErrorView(e, "test");
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

        [Test]
        public void EncryptDecryptTest()
        {
            string message = "hope this works";
            encrypted = Utility.Encrypt(message);
            Assert.IsNotNull(encrypted);

            string decrypted = Utility.Decrypt(encrypted);
            Assert.AreEqual(decrypted, message);
        }

    }
}

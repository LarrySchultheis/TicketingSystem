using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TicketingSystem.ExceptionReport;
using TicketingSystem.Models;
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

        [Test]
        public async Task CreateResponseMessageTest()
        {
            Exception e = new Exception("Testing exception methods");
            HttpResponseMessage response = Utility.CreateResponseMessage(e);
            string message = await response.Content.ReadAsStringAsync();

            CreateServerErrorViewTest(new HttpResponseException(response));
            CreateHttpErrorViewTest(new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)));

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [Test]
        public void CreateBasicExceptionViewTest()
        {
            Exception e = new Exception("Testing exception methods");
            ErrorViewModel error = Utility.CreateBasicExceptionView(e, ExceptionReporter.DumpException(e));
        }

        public async void CreateServerErrorViewTest(HttpResponseException exception)
        {
            ServerErrorViewModel result = await Utility.CreateServerErrorView(exception);
            Assert.IsNotNull(result);
        }

        public void CreateHttpErrorViewTest(HttpResponseException exception)
        {
            ErrorViewModel result = Utility.CreateHttpErrorView(exception, "401 Unauthorized");
            Assert.IsNotNull(result);
        }
    }
}

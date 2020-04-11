using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TicketingSystem.ExceptionReport;

namespace Tests.ServiceTests
{
    [TestFixture]
    class ExceptionReporterTest
    {
        [Test]
        public void DumpExceptionTest()
        {
            string guid = ExceptionReporter.DumpException(new Exception());
            var actual = Guid.Parse(guid);
            var expected = Guid.NewGuid();


            Assert.IsNotNull(guid);
            Assert.AreEqual(actual.GetType(), expected.GetType());
        }
    }
}

using NUnit.Framework;
using TicketingSystem.Services;
using TicketingSystem.Models;
using System.Collections.Generic;

namespace Prototype.Tests.ServiceTests
{
    [TestFixture]
    public class RecordRetrieverTests
    {
        [Test]
        public void RetrieveRecordsTest()
        {
            RecordRetriever rr = new RecordRetriever();
            var res = rr.RetrieveRecords();
            IEnumerable<TicketData> testModel = new List<TicketData>();
            Assert.IsNotNull(res);
            Assert.IsTrue(res.GetType() == testModel.GetType());
        }
    }
}

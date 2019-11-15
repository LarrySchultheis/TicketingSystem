using NUnit.Framework;
using TicketingSystem.Services;
using TicketingSystem.Models;
using System.Collections.Generic;
using System.Linq;

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

        [Test]
        public void GetRecordByIDTest()
        {
            using (var context = new TicketingSystemDBContext())
            {
                int id = context.TicketData.First().EntryId;
                RecordRetriever rr = new RecordRetriever();
                var res = rr.GetRecordByID(id);
                Assert.IsNotNull(res);
                Assert.IsTrue(res.GetType() == typeof(TicketData));
            }
        }

        [Test]
        public void GetOpenRecords()
        {
            RecordRetriever rr = new RecordRetriever();
            var res = rr.GetOpenRecords();
            IEnumerable<TicketData> testModel = new List<TicketData>();
            Assert.IsNotNull(res);
            Assert.IsTrue(res.GetType() == testModel.GetType());
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TicketingSystem.Services;
using TicketingSystem.Models;
using System;
using System.Linq;

namespace Tests.ServiceTests
{
    [TestFixture]
    class DataEditorTest
    {
        [Test]
        public void PostEditorTest()
        {
            using (var context = new TicketingSystemDBContext())
            {
                /*JobType j = new JobType();
                j.JobName = "Miscellaneous";

                TicketData td = new TicketData()
                {
                    TripNum = -1,
                    StageNum = "-1",
                    JobType = j,
                    EntryAuthor = context.Users.Where(u => u.FullName == "System Admin").FirstOrDefault(),
                    TicketWorker = context.Users.Where(w => w.FullName == "System Admin").FirstOrDefault(),
                    WorkerName = "System Admin",
                    StartTime = DateTime.Now,
                    Comments = "Entry generated by automated unit test"
                };

                DataEntry de = new DataEntry();
                Assert.IsTrue(de.PostEntry(td));*/
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingSystem.Models;
using TicketingSystem.Services;

namespace Tests
{
    class TestUtility
    {
        public static TicketData CreateTestData()
        {
            using (var context = new TicketingSystemDBContext())
            {
                JobType j = context.JobType.Where(jt => jt.JobName == "Miscellaneous").FirstOrDefault();

                RecordRetriever rr = new RecordRetriever();
                var records = rr.RetrieveRecords(10);

                Users ticketWorker = context.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
                Users entryAuthor = context.Users.Where(w => w.FullName == "Test User").FirstOrDefault();

                TicketData td = new TicketData()
                {
                    TripNum = -1,
                    StageNum = "S01",
                    JobType = j,
                    EntryAuthor = entryAuthor,
                    EntryAuthorId = entryAuthor.UserId,
                    TicketWorker = ticketWorker,
                    TicketWorkerId = ticketWorker.UserId,
                    WorkerName = "Test User",
                    StartTime = "12:00",
                    EndTime = "12:00",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today,
                    Comments = "Entry generated by HomeController.PostEntry unit test"
                };
                return td;
            }
        }

        public static TicketData GetTestData()
        {
            using (var db = new TicketingSystemDBContext())
            {
                Users user = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
                TicketData td = db.TicketData.Where(t => t.EntryAuthorId == user.UserId).FirstOrDefault();
                td.EntryAuthor = user;
                td.TicketWorker = db.Users.Where(u => u.UserId == td.TicketWorkerId).FirstOrDefault();
                td.JobType = db.JobType.Where(jt => jt.JobTypeId == td.JobTypeId).FirstOrDefault();
                return td;
            }
        }

        public static void Cleanup()
        {
            using (var db = new TicketingSystemDBContext())
            {
                Users user = db.Users.Where(u => u.FullName == "Test User").FirstOrDefault();
                var data = db.TicketData.Where(t => t.EntryAuthorId == user.UserId);

                db.TicketData.RemoveRange(data);
                db.SaveChanges();
            }
        }

        public static Users CreateTestUser()
        {
            Users newUser = new Users();
            newUser.Email = "larrytixsysuser@gmail.com";
            newUser.FullName = "Unit Test User";
            newUser.ShiftType = "Warehouse";
            return newUser;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;

namespace TicketingSystem.Services
{
    public class RecordRetriever
    {
        public IEnumerable<TicketData> RetrieveRecords()
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {

                    var tdata = context.TicketData.ToList();
                    foreach (TicketData td in tdata)
                    {
                        td.JobType = context.JobType.Find(td.JobTypeId);
                        td.TicketWorker = context.Users.Find(td.TicketWorkerId);
                        td.EntryAuthor = context.Users.Find(td.EntryAuthorId);
                    }
                    return tdata;
                }
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return new List<TicketData>();

            }
        }

        public TicketData GetRecordByID (int entryID)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    TicketData td = context.TicketData.Find(entryID);
                    td.JobType = context.JobType.Find(td.JobTypeId);
                    td.EntryAuthor = context.Users.Find(td.EntryAuthorId);
                    td.TicketWorker = context.Users.Find(td.TicketWorkerId);
                    return td;
                }
            }
            catch(Exception e)
            {
                ExceptionReporter.DumpException(e);
                return new TicketData();
            }

        }

        public IEnumerable<TicketData> GetOpenRecords()
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    var tdata = context.TicketData.Where(t => t.TicketClosed == false).ToList();
                    foreach (TicketData td in tdata)
                    {
                        td.JobType = context.JobType.Find(td.JobTypeId);
                        td.TicketWorker = context.Users.Find(td.TicketWorkerId);
                        td.EntryAuthor = context.Users.Find(td.EntryAuthorId);
                    }
                    return tdata;
                }
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return new List<TicketData>();
            }

        }
    }
}

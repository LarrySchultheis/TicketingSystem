using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using System.ComponentModel;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace TicketingSystem.Services
{
    public class RecordRetriever
    {
        /// <summary>
        /// Gets the top N records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TicketData> RetrieveRecords(int numberOfRecords)
        {
            List<TicketData> data = new List<TicketData>();
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    data = context.TicketData.Take(numberOfRecords).ToList();
                    foreach (TicketData td in data)
                    {
                        td.JobType = context.JobType.Find(td.JobTypeId);
                        td.TicketWorker = context.Users.Find(td.TicketWorkerId);
                        td.EntryAuthor = context.Users.Find(td.EntryAuthorId);
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        /// <summary>
        /// Get the record with the given entryID
        /// </summary>
        /// <param name="entryID"></param>
        /// <returns></returns>
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
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }

        }

        /// <summary>
        /// Get only open records
        /// </summary>
        /// <param name="numberOfRecords"></param>
        /// <returns></returns>
        public IEnumerable<TicketData> GetOpenRecords(int numberOfRecords)
        {
            List<TicketData> data = new List<TicketData>();
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    data = context.TicketData.Where(t => t.TicketClosed == false).Take(numberOfRecords).ToList();
                    foreach (TicketData td in data)
                    {
                        td.JobType = context.JobType.Find(td.JobTypeId);
                        td.TicketWorker = context.Users.Find(td.TicketWorkerId);
                        td.EntryAuthor = context.Users.Find(td.EntryAuthorId);
                    }
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
            return data;
        }

    }
}

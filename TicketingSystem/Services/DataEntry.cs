using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using System.Web.Http;
using System.Net;

namespace TicketingSystem.Services
{
    public class DataEntry
    {
        /// <summary>
        /// Posts the new ticket entry to the database
        /// </summary>
        /// <param name="td"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        public bool PostEntry(TicketData td, UserData loggedInUser)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    IEnumerable<JobType> jobs = context.JobType;
                    int jtypeID = 1;

                    jtypeID = context.JobType.Where(j => j.JobName == td.JobType.JobName).FirstOrDefault().JobTypeId;
                    int authorID = context.Users.Where(a => a.Auth0Uid == loggedInUser.user_id).FirstOrDefault().UserId;

                    var worker = context.Users.Where(w => w.FullName == td.TicketWorker.FullName).FirstOrDefault();
                    int workerID;

                    if (worker != null)
                    {
                        workerID = worker.UserId;
                        td.TicketWorkerId = workerID;
                    }
                    else
                    {
                        throw new Exception("Error, Employee with name: " + worker.FullName + " not found in System");
                    }

                    td.JobTypeId = jtypeID;
                    td.EntryDate = DateTime.Today;
                    td.TicketClosed = false;
                    td.EntryAuthorId = authorID;
                    td.WorkerName = td.TicketWorker.FullName;

                    //very important null assignment
                    td.JobType = null;
                    td.TicketWorker = null;
                    td.EntryAuthor = null;

                    context.TicketData.Add(td);
                    context.SaveChanges();

                    int entryID = td.EntryId;

                    TicketDataLogger tdl = new TicketDataLogger();
                    tdl.LogChange("new entry", "created new entry", entryID, authorID);

                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
            return true;
        }

        /// <summary>
        /// Closes the specified ticket data entry
        /// </summary>
        /// <param name="td"></param>
        /// <returns></returns>
        public bool CloseTicket(TicketData td)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    context.TicketData.Find(td.EntryId).TicketClosed = true;
                    context.TicketData.Find(td.EntryId).EndTime = td.EndTime;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
            return true;
        }

    }

}

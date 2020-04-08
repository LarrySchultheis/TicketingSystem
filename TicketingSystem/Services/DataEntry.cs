using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;

namespace TicketingSystem.Services
{
    public class DataEntry
    {

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
                ExceptionReporter.DumpException(e);
                return false;
            }
            return true;
        }

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
                ExceptionReporter.DumpException(e);
                return false;
            }
            return true;
        }

    }

}

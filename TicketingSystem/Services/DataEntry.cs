using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class DataEntry
    {

        public bool PostEntry(TicketData td)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    IEnumerable<JobType> jobs = context.JobType;
                    int jtypeID = 1;
                    //foreach (JobType j in jobs)
                    //{
                    //    if (td.JobType.JobName == j.JobName)
                    //    {
                    //        jtypeID = j.JobTypeId;
                    //        break;
                    //    }
                    //}
                    //int workerID = context.Users.Where(w => w.)
                    jtypeID = context.JobType.Where(j => j.JobName == td.JobType.JobName).FirstOrDefault().JobTypeId;
                    int authorID = context.Users.Where(a => a.FullName == "System Admin").FirstOrDefault().UserId;
                    int workerID = context.Users.Where(w => w.FullName == td.TicketWorker.FullName).FirstOrDefault().UserId;

                    td.JobTypeId = jtypeID;
                    td.EntryDate = DateTime.Today;
                    td.TicketClosed = false;
                    td.EntryAuthorId = authorID;
                    td.TicketWorkerId = workerID;
                    td.WorkerName = td.TicketWorker.FullName;

                    //very important null assignment
                    td.JobType = null;
                    td.TicketWorker = null;
                    td.EntryAuthor = null;

                    context.TicketData.Add(td);
                    context.SaveChanges();

                    int entryID = td.EntryId;

                    TicketDataLogger tdl = new TicketDataLogger();
                    tdl.LogChange("new entry", "created new entry", entryID);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
    }

}

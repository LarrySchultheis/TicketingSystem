using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class DataEditor
    {
        public bool PostEditor(TicketData td)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    IEnumerable<JobType> jobs = context.JobType;
                    int jtypeID = 1;

                    jtypeID = context.JobType.Where(j => j.JobName == td.JobType.JobName).FirstOrDefault().JobTypeId;
                    int authorID = context.Users.Where(a => a.FullName == "System Admin").FirstOrDefault().UserId;
                    int workerID = context.Users.Where(w => w.FullName == td.TicketWorker.FullName).FirstOrDefault().UserId;

                    td.JobTypeId = jtypeID;
                    td.EntryDate = DateTime.Now;
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
                    tdl.LogChange("new edits", "edited entry", entryID);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
    }
}

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
                    TicketData ticketToEdit = context.TicketData.Find(td.EntryId);
                    IEnumerable<JobType> jobs = context.JobType;
                    int jtypeID;
                    jtypeID = context.JobType.Where(j => j.JobName == td.JobType.JobName).FirstOrDefault().JobTypeId;
                    int authorID = context.Users.Where(a => a.FullName == "System Admin").FirstOrDefault().UserId;
                    int workerID = context.Users.Where(w => w.FullName == td.TicketWorker.FullName).FirstOrDefault().UserId;

                    td.JobTypeId = jtypeID;
                    td.EntryAuthorId = authorID;
                    td.TicketWorkerId = workerID;
                    td.WorkerName = td.TicketWorker.FullName;

                    //very important null assignment
                    td.JobType = null;
                    td.TicketWorker = null;
                    td.EntryAuthor = null;

                    ticketToEdit = td;

                    context.SaveChanges();

                    TicketDataLogger tdl = new TicketDataLogger();
                    tdl.LogChange("new edits", "edited entry", td.EntryId);
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

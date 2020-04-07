using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using System.IO;
using System.Diagnostics;

namespace TicketingSystem.Services
{
    public class DataEditor
    {
        public bool PostEditor(TicketData td, UserData loggedInUser)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    IEnumerable<JobType> jobs = context.JobType;
                    int jtypeID;
                    jtypeID = context.JobType.Where(j => j.JobName == td.JobType.JobName).FirstOrDefault().JobTypeId;

                    int authorID = -1;
                    int workerID;

                    var author = context.Users.Where(a => a.Auth0Uid == loggedInUser.user_id).FirstOrDefault();
                    var worker = context.Users.Where(w => w.FullName == td.TicketWorker.FullName).FirstOrDefault();

                    if (author != null)
                    {
                        authorID = author.UserId;
                        td.EntryAuthorId = authorID;
                    }

                    if (worker != null)
                    {
                        workerID = worker.UserId;
                        td.TicketWorkerId = workerID;
                    }

                    td.JobTypeId = jtypeID;
                    td.WorkerName = td.TicketWorker.FullName;

                    //very important null assignment
                    td.JobType = null;
                    td.TicketWorker = null;
                    td.EntryAuthor = null;

                    context.TicketData.Update(td);
                    context.SaveChanges();

                    TicketDataLogger tdl = new TicketDataLogger();
                    tdl.LogChange("new edits", "edited entry", td.EntryId, authorID);
                }
            }

            catch (Exception e)
            {
                ExceptionReporter er = new ExceptionReporter();
                er.DumpException(e);
                return false;
            }

            return true;
        }

        public bool DeleteEntry(string entryId, UserData loggedInUser)
        {
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    TicketData td = db.TicketData.Find(int.Parse(entryId));
                    db.TicketData.Remove(td);
                    db.SaveChanges();

                    var author = db.Users.Where(auth => auth.Auth0Uid == loggedInUser.user_id).FirstOrDefault();

                    TicketDataLogger tdl = new TicketDataLogger();
                    tdl.LogChange("delete", "deleted entry with ID: " + td.EntryId, td.EntryId, author.UserId);
                }
            }
            catch (Exception e)
            {
                ExceptionReporter er = new ExceptionReporter();
                er.DumpException(e);
            }

            return true;
        }
    }
}

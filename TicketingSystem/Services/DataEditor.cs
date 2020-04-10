using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using System.IO;
using System.Diagnostics;
using System.Web.Http;

namespace TicketingSystem.Services
{
    public class DataEditor
    {
        /// <summary>
        /// Posts the edited TicketData object to the database and logs the user who triggered it
        /// </summary>
        /// <param name="td">The ticket data instance to update</param>
        /// <param name="loggedInUser">The currently logged user</param>
        /// <returns></returns>
        public bool PostEditor(TicketData td, UserData loggedInUser)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    IEnumerable<JobType> jobs = context.JobType;
                    TicketData ticketToEdit = context.TicketData.Find(td.EntryId);


                    int jtypeID;
                    jtypeID = context.JobType.Where(j => j.JobName == td.JobType.JobName).FirstOrDefault().JobTypeId;

                    int authorID = -1;
                    int workerID;

                    var author = context.Users.Where(a => a.Auth0Uid == loggedInUser.user_id).FirstOrDefault();
                    var worker = context.Users.Where(w => w.FullName == td.TicketWorker.FullName).FirstOrDefault();

                    if (author != null)
                    {
                        authorID = author.UserId;
                        ticketToEdit.EntryAuthorId = authorID;
                    }

                    if (worker != null)
                    {
                        workerID = worker.UserId;
                        ticketToEdit.TicketWorkerId = workerID;
                    }

                    ticketToEdit.JobTypeId = jtypeID;
                    ticketToEdit.WorkerName = td.TicketWorker.FullName;

                    //very important null assignment
                    ticketToEdit.JobType = null;
                    ticketToEdit.TicketWorker = null;
                    ticketToEdit.EntryAuthor = null;

                    context.TicketData.Update(ticketToEdit);
                    context.SaveChanges();

                    TicketDataLogger tdl = new TicketDataLogger();
                    tdl.LogChange("new edits", "edited entry", td.EntryId, authorID);
                }
            }

            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
            return true;
        }

        /// <summary>
        /// Deletes the specified ticket data entry
        /// </summary>
        /// <param name="entryId">The unique id of the ticket to be deleted</param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
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
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }

            return true;
        }
    }
}

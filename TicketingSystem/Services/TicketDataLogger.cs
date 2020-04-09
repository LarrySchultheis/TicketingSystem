using System;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using System.Web.Http;

namespace TicketingSystem.Services
{
    public class TicketDataLogger
    {
        /// <summary>
        /// Function to log changes in entries to the database
        /// </summary>
        /// <param name="action"></param>
        /// <param name="details"></param>
        /// <param name="entryID"></param>
        /// <param name="changedByUserID"></param>
        /// <returns></returns>
        public bool LogChange(string action, string details, int entryID, int changedByUserID)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    TicketDataLog tdLog = new TicketDataLog()
                    {
                        ChangedByUserId = changedByUserID,
                        ChangeTime = DateTime.Now,
                        DataAction = action,
                        Details = details,
                        EntryId = entryID
                    };

                    context.TicketDataLog.Add(tdLog);
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

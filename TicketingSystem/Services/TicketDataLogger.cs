using System;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;

namespace TicketingSystem.Services
{
    public class TicketDataLogger
    {
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
                ExceptionReporter.DumpException(e);
                return false;
            }
            return true;
        }
    }
}

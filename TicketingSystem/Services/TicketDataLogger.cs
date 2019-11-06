using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class TicketDataLogger
    {
        public bool LogChange(string action, string details, int entryID)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    TicketDataLog tdLog = new TicketDataLog()
                    {
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
                return false;
            }
            return true;
        }
    }
}

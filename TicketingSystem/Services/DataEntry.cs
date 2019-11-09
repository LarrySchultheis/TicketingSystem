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
                    foreach (JobType j in jobs)
                    {
                        if (td.JobType.JobType1 == j.JobType1)
                        {
                            jtypeID = j.JobTypeId;
                            break;
                        }
                    }

                    td.JobTypeId = jtypeID;
                    td.EntryDate = DateTime.Now;
                    td.IsClosed = false;

                    //very important null assignment
                    td.JobType = null;
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
                    context.TicketData.Find(td.EntryId).IsClosed = true;
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

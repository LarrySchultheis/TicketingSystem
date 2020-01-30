using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class ReportGenerator
    {
        public List<TicketData> GenerateIncentveReport(DateTime startDate, DateTime endDate)
        {

            return null;
        }

        public void GenerateLaborHoursByJob(DateTime startDate, DateTime endDate)
        {
            List<LaborHoursByJob> data = new List<LaborHoursByJob>();

            using (var db = new TicketingSystemDBContext())
            {
                var td = db.TicketData.Where(t => t.EntryDate >= startDate && t.EntryDate <= endDate).ToList();
                foreach (TicketData t in td)
                {
                    LaborHoursByJob lh = new LaborHoursByJob();
                    lh.Cases = t.CasesNum;
                    lh.Pallets = t.PalletNum;
                    lh.Job = db.JobType.Where(j => j.JobTypeId == t.JobTypeId).FirstOrDefault();
                    if (t.EndTime == null)
                    {
                        lh.TotalHours = DateTime.Now - t.StartTime;
                    }
                    else
                    {
                        lh.TotalHours = t.EndTime - t.StartTime;
                    }
                    data.Add(lh);
                }
            }
        }

        public void GenerateLaborHoursByJobAndEmployee(DateTime startDate, DateTime endDate)
        {
        }

        public LaborHoursByJob GetLaborHoursByJob(DateTime startDate, DateTime endDate)
        {
            LaborHoursByJob lh = new LaborHoursByJob();



            return lh;
        }
    }
}

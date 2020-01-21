using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class ReportGenerator
    {
        public List<TicketData> GenerateIncentveReport()
        {
            using (var context = new TicketingSystemDBContext())
            {
                var data = context.TicketData.Where(u => u.TicketWorker.FullName == "Basic User").ToList();

                return data;
            }
        }

        public void GenerateLaborHoursByJob()
        {
        }

        public void GenerateLaborHoursByJobAndEmployee()
        {
        }
    }
}

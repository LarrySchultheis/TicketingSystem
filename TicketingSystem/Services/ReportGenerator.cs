using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;

namespace TicketingSystem.Services
{
    public class ReportGenerator
    {
        public List<TicketData> GenerateIncentveReport()
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    var data = context.TicketData.Where(u => u.TicketWorker.FullName == "Basic User").ToList();

                    return data;
                }
            }
            catch (Exception e)
            {
                ExceptionReporter er = new ExceptionReporter();
                er.DumpException(e);
                List<TicketData> data = new List<TicketData>();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Services;

namespace TicketingSystem.Models
{
    //Class to model report input when generating reports
    public class ReportInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReportName ReportName { get; set; }
        public ReportFormat ReportFormat { get; set; }
    }
}

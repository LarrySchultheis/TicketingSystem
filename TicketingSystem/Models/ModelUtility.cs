using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    public enum ReportFormat
    {
        PDF,
        CSV,
        DOCX
    }
    public enum ReportName
    {
        LaborHoursByJob,
        LaborHoursByJobAndEmployee,
        IncentiveReport
    }
    public class ModelUtility
    {
    }
}

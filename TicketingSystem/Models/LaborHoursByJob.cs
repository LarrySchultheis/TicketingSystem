using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    public class LaborHoursByJob
    {
        public JobType Job { get; set; }
        public int Pallets { get; set; }
        public int Cases { get; set; }
        public TimeSpan? TotalHours { get; set; }
    }
}

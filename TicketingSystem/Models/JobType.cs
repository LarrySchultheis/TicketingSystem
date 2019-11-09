using System;
using System.Collections.Generic;

namespace TicketingSystem.Models
{
    public partial class JobType
    {
        public JobType()
        {
            TicketData = new HashSet<TicketData>();
        }

        public int JobTypeId { get; set; }
        public string JobName { get; set; }

        public ICollection<TicketData> TicketData { get; set; }
    }
}

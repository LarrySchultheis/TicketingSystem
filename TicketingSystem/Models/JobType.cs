using System;
using System.Collections.Generic;

namespace MVCApp.Models
{
    public partial class JobType
    {
        public JobType()
        {
            TicketData = new HashSet<TicketData>();
        }

        public int JobTypeId { get; set; }
        public string JobType1 { get; set; }

        public virtual ICollection<TicketData> TicketData { get; set; }
    }
}

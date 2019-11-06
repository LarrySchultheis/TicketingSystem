using System;
using System.Collections.Generic;

namespace TicketingSystem.Models
{
    public partial class TicketData
    {
        public TicketData()
        {
            TicketDataLog = new HashSet<TicketDataLog>();
        }

        public int EntryId { get; set; }
        public int TripNumber { get; set; }
        public string StageNumber { get; set; }
        public int JobTypeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartTime { get; set; }
        public string Comments { get; set; }
        public DateTime EntryDate { get; set; }
        public string PalletNumber { get; set; }
        public DateTime? EndTime { get; set; }
        public string PalletType { get; set; }
        public int? PalletWrapsNumber { get; set; }
        public string Carrier { get; set; }
        public int? CasesNumber { get; set; }
        public int? TrailerNumber { get; set; }
        public bool IsClosed { get; set; }

        public virtual JobType JobType { get; set; }
        public virtual ICollection<TicketDataLog> TicketDataLog { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace TicketingSystem.Models
{
    public partial class TicketData
    {
        public int EntryId { get; set; }
        public int TripNum { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Comments { get; set; }
        public int PalletNum { get; set; }
        public string PalletType { get; set; }
        public int? PalletWrapNum { get; set; }
        public string Carrier { get; set; }
        public int CasesNum { get; set; }
        public int? TrailerNum { get; set; }
        public bool TicketClosed { get; set; }
        public string StageNum { get; set; }
        public int JobTypeId { get; set; }
        public int EntryAuthorId { get; set; }
        public DateTime EntryDate { get; set; }
        public string WorkerName { get; set; }
        public int TicketWorkerId { get; set; }

        public Users EntryAuthor { get; set; }
        public JobType JobType { get; set; }
        public Users TicketWorker { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace TicketingSystem.Models
{
    public partial class TicketDataLog
    {
        public int LogId { get; set; }
        public DateTime ChangeTime { get; set; }
        public string DataAction { get; set; }
        public int EntryId { get; set; }
        public string Details { get; set; }
    }
}

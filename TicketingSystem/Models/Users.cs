using System;
using System.Collections.Generic;

namespace TicketingSystem.Models
{
    public partial class Users
    {
        public Users()
        {
            TicketDataEntryAuthor = new HashSet<TicketData>();
            TicketDataTicketWorker = new HashSet<TicketData>();
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string ShiftType { get; set; }
        public bool? IsActive { get; set; }
        public string FullName { get; set; }
        public string Auth0Uid { get; set; }

        public ICollection<TicketData> TicketDataEntryAuthor { get; set; }
        public ICollection<TicketData> TicketDataTicketWorker { get; set; }
    }
}

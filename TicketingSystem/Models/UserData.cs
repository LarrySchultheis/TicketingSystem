using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    //Models basic data for use with the user management API
    public class UserData
    {
        public string user_id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
}

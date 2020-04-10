using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    //Models management API payload when adding a user to Auth0
    public class Auth0UserPayload
    {
        public string email { get; set; }
        public string name { get; set; }

        public string password { get; set; }
        public string connection { get; set; }
    }
}

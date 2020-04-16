using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    //Models permissions in Auth0
    public class UserPermission
    {
        public string permission_name { get; set; }
        public string description { get; set; }
    }
}

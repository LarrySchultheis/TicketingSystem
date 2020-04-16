using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    //Class used to model the payload when calling POST on roles in management API
    public class Auth0RolesPayload
    {
        public string[] roles { get; set; }
    }
}

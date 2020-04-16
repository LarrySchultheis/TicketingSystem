using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    //Class used to model roles consumed by the Auth0 Management API
    public class Auth0Role
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    //Models the token received from Auth0 for use with the management API
    public class TokenData
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    //Class that models a server error
    public class ServerErrorViewModel : ErrorViewModel
    {
        public string Guid { get; set; }
        public string Message { get; set; }
    }
}




  


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class DataEditor
    {
        public bool PostEdit(TicketData td)
        {
            try
            {
                using (var context = new TicketingSystemDBContext())
                {

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
    }
}

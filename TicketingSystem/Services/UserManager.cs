using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.ExceptionReport;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class UserManager
    {
        public bool CreateUser(Users newUser, UserData loggedInUser)
        {
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    Auth0APIClient.AddUser(newUser);
                }
                return true;
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
                return false;
            }

        }

        public IEnumerable<Users> GetUsers()
        {
            using (var db = new TicketingSystemDBContext())
            {
                return db.Users.ToList();
            }
        }
    }
}

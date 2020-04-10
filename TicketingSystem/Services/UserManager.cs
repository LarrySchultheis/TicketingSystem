using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using TicketingSystem.ExceptionReport;
using TicketingSystem.Models;

namespace TicketingSystem.Services
{
    public class UserManager
    {
        /// <summary>
        /// Function to create a new user in the databse and add the user to Auth0
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="loggedInUser"></param>
        /// <returns></returns>
        public bool CreateUser(Users newUser, UserData loggedInUser)
        {
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    string auth0ID = Auth0APIClient.AddUser(newUser);
                    Auth0APIClient.SetRole(auth0ID, newUser.ShiftType);
                }
                return true;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }

        }

        /// <summary>
        /// Function to return all of the users in the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Users> GetUsers()
        {
            try
            { 
                using (var db = new TicketingSystemDBContext())
                {
                    return db.Users.ToList();
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        /// <summary>
        /// Gets the user with the given ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Users GetUserByID(int userID)
        {
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    return db.Users.Find(userID);
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        public bool DeleteUser(int UserId)
        {
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    Users user = db.Users.Find(UserId);
                    string auth0ID = user.Auth0Uid;

                    db.Users.Remove(user);
                    db.SaveChanges();

                    Auth0APIClient.DeleteUser(auth0ID);

                    return true;
                }
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }
    }
}

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
                string tempPass = CreateAndAddUser(newUser);

                if (tempPass.Equals("Email Exists"))
                    return false;

                if (newUser.ShiftType != "Warehouse")
                {
                    string auth0ID = Auth0APIClient.AddUser(newUser, tempPass);
                    Auth0APIClient.SetRole(auth0ID, newUser.ShiftType);
                }  
                return true;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }

        }

        private string CreateAndAddUser(Users newUser)
        {
            using (var db = new TicketingSystemDBContext())
            {

                string tempPass = Guid.NewGuid().ToString().Substring(0, 12);
                string encrypted = Utility.Encrypt(tempPass);
                newUser.PassWrd = encrypted;
                newUser.IsActive = true;

                if (db.Users.Where(us => us.Email == newUser.Email).Any())
                {
                    return "Email Exists";
                }

                db.Users.Add(newUser);
                db.SaveChanges();

                return tempPass;
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

        /// <summary>
        /// Function to update users in the database from Auth0
        /// </summary>
        /// <returns></returns>
        public bool UpdateUsersFromAuth0()
        {
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    var users = Auth0APIClient.GetAllUsers();
                    foreach (var user in users)
                    {
                        Users dbuser = db.Users.Where(u => u.Auth0Uid == user.user_id).FirstOrDefault();
                        if (dbuser != null)
                        {
                            dbuser.Email = user.email;
                            dbuser.FullName = user.name;
                            var roles = Auth0APIClient.GetUserRole(dbuser.Auth0Uid);

                            dbuser.ShiftType = roles.ElementAt(0).name;

                            db.Users.Update(dbuser);
                            db.SaveChanges();
                        }

                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        public bool ImportUsersFromAuth0()
        {
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    var users = Auth0APIClient.GetAllUsers();
                    foreach(var u in users)
                    {
                        if(!db.Users.Where(usr => usr.Email == u.email).Any())
                        {
                            string shiftType = Auth0APIClient.GetUserRole(u.user_id)[0].name;

                            Users newUser = new Users();
                            newUser.Auth0Uid = u.user_id;
                            newUser.FullName = u.name;
                            newUser.Email = u.email;
                            newUser.ShiftType = shiftType;

                            CreateAndAddUser(newUser);
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }

        //Not supported due to management API restrictions

        //public bool UpdateUser(Users user)
        //{
        //    try
        //    {
        //        using (var db = new TicketingSystemDBContext())
        //        {
        //            Users oldUser = db.Users.Find(user.UserId);
        //            string oldShiftType = oldUser.ShiftType;
        //            oldUser.Email = user.Email;
        //            oldUser.FullName = user.FullName;
        //            oldUser.ShiftType = user.ShiftType;
        //            db.Users.Update(oldUser);
        //            db.SaveChanges();

        //            //  Auth0APIClient.UpdateUser(oldUser);
        //            Auth0APIClient.UpdateRole(oldUser.Auth0Uid, oldShiftType, user.ShiftType);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new HttpResponseException(Utility.CreateResponseMessage(e));
        //    }
        //    return true;
        //}

        /// <summary>
        /// Function to delete a user from DB and Auth0
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ToggleActivation(int UserId)
        {
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    Users user = db.Users.Find(UserId);
                    string auth0ID = user.Auth0Uid;
                    user.IsActive = !user.IsActive;
                    db.Users.Update(user);
                    db.SaveChanges();

                    return true;
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
                    db.Users.Remove(user);
                    db.SaveChanges();
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

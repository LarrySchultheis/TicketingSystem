using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Models;
using TicketingSystem.Services;
using System.Diagnostics;
using TicketingSystem.ExceptionReport;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;

namespace TicketingSystem.Controllers
{
    public class EditController : Controller
    {
        /// <summary>
        /// Gets Index page when Edit/Index is hit
        /// </summary>
        /// <returns>
        /// Index View
        /// </returns>
        public async Task<ViewResult> Index()
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {

                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }
        
            try
            {
                RecordRetriever rr = new RecordRetriever();
                var res = rr.RetrieveRecords(1000);
                return View("Index", res);
            }
            catch (HttpResponseException e)
            {
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return View("ServerError", error);
            }
            catch (Exception e)
            {
                var guid = ExceptionReporter.DumpException(e);
                ErrorViewModel error = Utility.CreateBasicExceptionView(e, guid);
                return View("Error", error);
            }
        }

        /// <summary>
        /// Displays EditForm view with TicketData instance as the model
        /// </summary>
        /// <param name="td">TicketData instance</param>
        /// <returns>EditForm View with specified TicketData entry</returns>
        public async Task<ViewResult> EditForm(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }

            try
            {
                RecordRetriever rr = new RecordRetriever();
                var tdRes = rr.GetRecordByID(td.EntryId);
                return View("EditForm", tdRes);
            }
            catch (HttpResponseException e)
            {
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return View("ServerError", error);
            }
            catch (Exception e)
            {
                var guid = ExceptionReporter.DumpException(e);
                ErrorViewModel error = Utility.CreateBasicExceptionView(e, guid);
                return View("Error", error);
            }

        }

        /// <summary>
        /// Gets the TicketData entry based on the entryID input in Index page
        /// </summary>
        /// <param name="entryId">Entry ID input from index</param>
        /// <returns>JSON holding redirect URL</returns>
        public async Task<JsonResult> GetRecord(string entryId)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return Json(new
                {
                    newUrl = Url.Action("Error", "Edit", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"))
                }) ; 
            }
            try
            {
                using (var context = new TicketingSystemDBContext())
                {
                    RecordRetriever rr = new RecordRetriever();
                    var result = rr.GetRecordByID(int.Parse(entryId));

                    return Json(new
                    {
                        newUrl = Url.Action("EditForm", "Edit", result)
                    });
                }
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return Json(new
                {
                    newUrl = Url.Action("ServerError", error)
                });
            }
            catch (Exception e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateBasicExceptionView(e, guid))
                });
            }
        }

        /// <summary>
        /// Endpoint to create a 401 error page
        /// </summary>
        /// <returns></returns>
        public ViewResult ServerError(ServerErrorViewModel error)
        {
            return View("Error", error);
        }

        public ViewResult Error(ErrorViewModel error)
        {
            return View("Error", error);
        }

        /// <summary>
        /// Posts edited ticket data entry to DataEditor service
        /// </summary>
        /// <param name="td">TicketData instance from edit form</param>
        /// <returns>Index View</returns>
        public async Task<ViewResult> PostEdit(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                return View("Error", Utility.CreateHttpErrorView(e, "You do not have the permissions to view this page"));
            }

            try
            {
                DataEditor de = new DataEditor();
                UserData loggedInUser = Auth0APIClient.GetUserData(User.Claims.First().Value);
                de.PostEditor(td, loggedInUser);
                RecordRetriever rr = new RecordRetriever();
                var res = rr.RetrieveRecords(1000);
                return View("Index", res);
            }
            catch (HttpResponseException e)
            {
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return View("ServerError", error);
            }
            catch (Exception e)
            {
                var guid = ExceptionReporter.DumpException(e);
                ErrorViewModel error = Utility.CreateBasicExceptionView(e, guid);
                return View("Error", error);
            }
        }


        /// <summary>
        /// Endpoint to delete an entry from the database
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public async Task<JsonResult> RemoveEntry(string entryId)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateHttpErrorView(e, "401 Unauthorized"))
                });
            }

            try
            {
                DataEditor de = new DataEditor();
                de.DeleteEntry(entryId, Auth0APIClient.GetUserData(User.Claims.First().Value));

                return Json(new
                {
                    newUrl = Url.Action("Index", "Edit"),
                    message = "Deleted entry",
                    id = entryId
                });
            }
            catch (HttpResponseException e)
            {
                string guid = ExceptionReporter.DumpException(e);
                ServerErrorViewModel error = await Utility.CreateServerErrorView(e);
                return Json(new
                {
                    newUrl = Url.Action("ServerError", error)
                });
            }
            catch (Exception e)
            {
                string guid = ExceptionReporter.DumpException(e);
                return Json(new
                {
                    newUrl = Url.Action("Error", Utility.CreateBasicExceptionView(e, guid))
                });
            }

        }

        /// <summary>
        /// Function to authorize the currently logged in user
        /// </summary>
        /// <returns></returns>
        public bool Authorize()
        {
            try
            {
                var userId = User.Claims.First().Value;
                UserData ud = Auth0APIClient.GetUserData(userId);
                List<UserPermission> permissions = Auth0APIClient.GetPermissions(ud.user_id);
                bool authorized = false;

                foreach (UserPermission perm in permissions)
                {
                    if (perm.permission_name == ModelUtility.AccessLevel1 || perm.permission_name == ModelUtility.AccessLevel2)
                    {
                        authorized = true;
                        break;
                    }
                }

                if (authorized == false)
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);

                return authorized;
            }
            catch(Exception e)
            {
                throw new HttpResponseException(Utility.CreateResponseMessage(e));
            }
        }
    }
}
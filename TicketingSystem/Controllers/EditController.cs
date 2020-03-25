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
        /// Gets Index page when HomePage/Index is hit
        /// </summary>
        /// <returns>
        /// Index View
        /// </returns>
        public IActionResult Index()
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                ErrorViewModel errorView = new ErrorViewModel();
                errorView.ErrorCode = "401";
                return View("Error", errorView);
            }
        
            RecordRetriever rr = new RecordRetriever();
            var res = rr.RetrieveRecords();
            return View("Index", res);
        }

        /// <summary>
        /// Displays EditForm view with TicketData instance as the model
        /// </summary>
        /// <param name="td">TicketData instance</param>
        /// <returns>EditForm View with specified TicketData entry</returns>
        public IActionResult EditForm(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                ErrorViewModel errorView = new ErrorViewModel();
                errorView.ErrorCode = "401";
                return View("Error", errorView);
            }
            RecordRetriever rr = new RecordRetriever();
            var tdRes = rr.GetRecordByID(td.EntryId);
            return View("EditForm", tdRes);
        }

        /// <summary>
        /// Gets the TicketData entry based on the entryID input in Index page
        /// </summary>
        /// <param name="entryId">Entry ID input from index</param>
        /// <returns>JSON holding redirect URL</returns>
        public JsonResult GetRecord(string entryId)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                ErrorViewModel errorView = new ErrorViewModel();
                errorView.ErrorCode = "401";
                return Json(new
                {
                    newUrl = Url.Action("Error", "Edit")
                }); 
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
            catch(Exception e)
            {
                ExceptionReporter er = new ExceptionReporter();
                er.DumpException(e);
                TicketData td = new TicketData();

                return Json(new
                {
                    newUrl = Url.Action("EntryClose", "Home", td)
                });
            }
        }

        public IActionResult Error()
        {
            ErrorViewModel error = new ErrorViewModel();
            error.ErrorCode = "401";
            return View("Error", error);
        }

        /// <summary>
        /// Posts edited ticket data entry to DataEditor service
        /// </summary>
        /// <param name="td">TicketData instance from edit form</param>
        /// <returns>Index View</returns>
        public IActionResult PostEdit(TicketData td)
        {
            try
            {
                Authorize();
            }
            catch (HttpResponseException e)
            {
                ErrorViewModel errorView = new ErrorViewModel();
                errorView.ErrorCode = "401";
                return View("Error", errorView);
            }

            DataEditor de = new DataEditor();
            de.PostEditor(td);
            RecordRetriever rr = new RecordRetriever();
            var res = rr.RetrieveRecords();
            return View("Index", res);
        }

        public bool Authorize()
        {
            var userId = User.Claims.First().Value;
            UserData ud = Auth0APIClient.GetUserData(userId);
            List<UserPermission> permissions = Auth0APIClient.GetPermissions(ud.user_id);
            bool authorized = false;

            foreach (UserPermission perm in permissions)
            {
                if (perm.permission_name == "access:lvl1")
                    authorized = true;
                if (perm.permission_name == "access:lvl2")
                    authorized = true;
            }
            
            if (authorized == false)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return authorized;
        }
    }
}
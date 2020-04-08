using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace TicketingSystem.Services
{

    public static class Utility
    {
        public static ErrorViewModel CreateErrorView(HttpResponseException exception, string reason)
        {
            ErrorViewModel errorView = new ErrorViewModel();

            try
            {
                int code = (int)exception.Response.StatusCode;
                string status = exception.Response.StatusCode.ToString();
                errorView.ErrorCode = code + " " + status;
                errorView.Reason = reason;
            }
            catch(Exception e)
            {
                ExceptionReporter.DumpException(e);
            }
            return errorView;
        }

        public static List<string> GetValidNames()
        {
            List<string> userNames = new List<string>();
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    var users = db.Users.ToList();
                    foreach (Users u in users)
                    {
                        userNames.Add(u.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
            }
            return userNames;

        }

        public static Users GetUserByName(string name)
        {
            try
            {
                using (var db = new TicketingSystemDBContext())
                {
                    return db.Users.Where(u => u.FullName == name).First();
                }
            }
            catch (Exception e)
            {
                ExceptionReporter.DumpException(e);
            }
            return new Users();
        }


    }
}

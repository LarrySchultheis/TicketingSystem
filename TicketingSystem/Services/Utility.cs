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

namespace TicketingSystem.Services
{

    public static class Utility
    {
        public static void Log(string log)
        {
            string path = Environment.CurrentDirectory;
            path += "\\logs";
            string now = DateTime.Now.ToString();
            now = now.Replace("/", "-");
            now = now.Replace(":", "-");

            using (StreamWriter outfile = new StreamWriter(Path.Combine(path, "Log_" + now + ".txt")))
            {
                outfile.WriteLine(log);
                outfile.Close();
            }
        }
        public static ErrorViewModel CreateErrorView(HttpResponseException exception, string reason)
        {
            ErrorViewModel errorView = new ErrorViewModel();
            int code = (int) exception.Response.StatusCode;
            string status = exception.Response.StatusCode.ToString();
            errorView.ErrorCode = code + " " + status;
            errorView.Reason = reason;
            return errorView;
        }

        public static List<string> GetValidNames()
        {
            using (var db = new TicketingSystemDBContext())
            {
                List<string> userNames = new List<string>();
                var users = db.Users.ToList();
                foreach (Users u in users)
                {
                    userNames.Add(u.FullName);
                }
                return userNames;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.ReportingServices.Interfaces;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http.Headers;

namespace TicketingSystem.Services
{
    public class ReportGenerator
    {
        protected static readonly string ssrsBaseURL = "http://localhost/reportserver?/TicketingSystemReporting/";
        protected static readonly string ssrsRestURL = "http://localhost/reports/api/v2.0";
        protected readonly string userName = "larry";
        protected readonly string password = "ls150682";


        public async Task<HttpResponseMessage> GenerateReport (ReportInput reportData)
        {
        
            HttpResponseMessage resp = null;
            string format, reportName;

            format = GetFormat(reportData);
            reportName = GetReportName(reportData);

            if (!reportName.Equals("Invalid"))
            {
                string startDate = reportData.StartDate.ToString("MM/dd/yyyy");
                string endDate = reportData.EndDate.ToString("MM/dd/yyyy");

                HttpClientHandler handler = new HttpClientHandler
                {
                    PreAuthenticate = true,
                    Credentials = new NetworkCredential(userName, password)
                };
                var client = new HttpClient(handler);

                resp = await client.GetAsync(BuildUrl(ssrsBaseURL, reportName, format, startDate, endDate));
                return resp;
            }
            else
            {
                Exception e = new Exception("Error, Report Type is not valid");
                throw e;
            }
           
        }

        private string BuildUrl (string baseUrl, string reportName, string format, string startDate, string endDate)
        {
            return baseUrl + reportName + "&rs:Format=" + format + "&StartDate=" + startDate + "&EndDate=" + endDate;
        }

        private string GetFormat(ReportInput reportInput)
        {
            switch (reportInput.ReportFormat)
            {
                case ReportFormat.PDF:
                    return "pdf";
                case ReportFormat.CSV:
                    return "csv";
                //case ReportFormat.DOCX:
                //    return "docx";
                default:
                    return "csv";
            }

        }

        private string GetReportName(ReportInput reportInput)
        {
            switch (reportInput.ReportName)
            {
                case ReportName.LaborHoursByJob:
                    return "LaborHoursByJob";
                case ReportName.LaborHoursByJobAndEmployee:
                    return "LaborHoursByJobAndEmployee";
                case ReportName.IncentiveReport:
                    return "IncentiveReport";
            }
            return "Invalid";
        }
    }
}

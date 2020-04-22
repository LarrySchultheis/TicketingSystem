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
        //base url of the report server
        protected static readonly string ssrsBaseURL = "http://localhost/reportserver?/TicketingSystemReporting/";

        //credentials used to access report server
        protected readonly string userName = "user";
        protected readonly string password = "pass";


        /// <summary>
        /// Function to generate and return the report binaries
        /// </summary>
        /// <param name="reportData">Report data containing date range, format, and report name</param>
        /// <returns>The HttpResponse from the report server</returns>
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

        /// <summary>
        /// Helper function to build the report request URL
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="reportName"></param>
        /// <param name="format"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>the built url for the requested report</returns>
        private string BuildUrl (string baseUrl, string reportName, string format, string startDate, string endDate)
        {
            return baseUrl + reportName + "&rs:Format=" + format + "&StartDate=" + startDate + "&EndDate=" + endDate;
        }

        /// <summary>
        /// Helper function to get the format of the report
        /// </summary>
        /// <param name="reportInput"></param>
        /// <returns>extension of the desired format</returns>
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

        /// <summary>
        /// Helper function to return the name of the report to generate
        /// </summary>
        /// <param name="reportInput"></param>
        /// <returns></returns>
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

using System;
using System.Collections.Generic;
using TicketingSystem.Models;
using TicketingSystem.ExceptionReport;
//using ReportService;
using ReportServerAPI;
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


        public void GenerateReport (ReportInput reportData)
        {
            HttpResponseMessage resp;
            string format, reportName;

            format = GetFormat(reportData);
            reportName = GetReportName(reportData);

            if (!reportName.Equals("Invalid"))
            {
                if (reportData.ReportType == 0)
                    GenerateIncentveReport(reportData, format, reportName);

                else if (reportData.ReportType == 1)
                    GenerateLaborHoursByJob(reportData, format, reportName);//reportData.StartDate, reportData.EndDate);

                else if (reportData.ReportType == 2)
                    GenerateLaborHoursByJobAndEmployee(reportData, format, reportName);

                else
                {
                    Exception e = new Exception("Error, Report Type is not valid");
                    throw e;
                }
            }
        }

        public async void GenerateLaborHoursByJob(ReportInput reportInput, string format, string reportName)//DateTime startDate, DateTime endDate)
        {
            string startDate = reportInput.StartDate.ToString("MM/dd/yyyy");
            string endDate = reportInput.EndDate.ToString("MM/dd/yyyy");
            
            HttpClientHandler handler = new HttpClientHandler
            {
                PreAuthenticate = true,
                Credentials = new NetworkCredential(userName, password)
            };
            var client = new HttpClient(handler);

            //var resp = await client.GetAsync(ssrsBaseURL + "LaborHoursByJob&rs:Format=csv&StartDate=" + start + "&EndDate=" + end);
            var resp = await client.GetAsync(BuildUrl(ssrsBaseURL, reportName, format, startDate, endDate));

            var data = await resp.Content.ReadAsByteArrayAsync();
            using (Stream file = File.OpenWrite("test." + format))
            {
                file.Write(data, 0, data.Length);
            }

            var y = 0;
        }

        public void GenerateIncentveReport(ReportInput reportInput, string format, string reportName)
        {
        }

        public void GenerateLaborHoursByJobAndEmployee(ReportInput reportInput, string format, string reportName)
        {
        }

        private byte[] ReadStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
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

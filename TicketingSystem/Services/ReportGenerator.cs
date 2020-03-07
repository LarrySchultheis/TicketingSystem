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
        protected static readonly string ssrsBaseURL = "http://localhost/reportserver";
        protected static readonly string ssrsRestURL = "http://localhost/reports/api/v2.0";



        public List<TicketData> GenerateIncentveReport(DateTime startDate, DateTime endDate)
        {

            return null;
        }

        public void GenerateReport (ReportInput reportData)
        {
            HttpResponseMessage resp;
            if (reportData.ReportType == 0)
                GenerateIncentveReport(reportData.StartDate, reportData.EndDate);

            else if (reportData.ReportType == 1)
                GenerateLaborHoursByJob(reportData.StartDate, reportData.EndDate);//reportData.StartDate, reportData.EndDate);

            else if (reportData.ReportType == 2)
                GenerateLaborHoursByJobAndEmployee(reportData.StartDate, reportData.EndDate);

            else
            {
                Exception e = new Exception("Error, Report Type is not valid");
                throw e;
            }
        }

        public async void GenerateLaborHoursByJob(DateTime startDate, DateTime endDate)//DateTime startDate, DateTime endDate)
        {
            string start, end;
            start = startDate.ToString("MM/dd/yyyy");
            end = endDate.ToString("MM/dd/yyyy");

            HttpClientHandler handler = new HttpClientHandler
            {
                PreAuthenticate = true,
                Credentials = new NetworkCredential("larry", "ls150682")
            };
            var client = new HttpClient();

            var resp = await client.GetAsync(ssrsBaseURL + "?/TicketingSystemReporting/LaborHoursByJob&rs:Format=csv&StartDate=" + start + "&EndDate=" + end);

            var data = await resp.Content.ReadAsByteArrayAsync();
            using (Stream file = File.OpenWrite("test.csv"))
            {
                file.Write(data, 0, data.Length);
            }

            var y = 0;
        }

        public void GenerateLaborHoursByJobAndEmployee(DateTime startDate, DateTime endDate)
        {
        }

        public LaborHoursByJob GetLaborHoursByJob(DateTime startDate, DateTime endDate)
        {
            LaborHoursByJob lh = new LaborHoursByJob();



            return lh;
        }

        private byte[] ReadStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}

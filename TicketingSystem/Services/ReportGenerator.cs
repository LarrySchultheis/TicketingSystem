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

namespace TicketingSystem.Services
{
    public class ReportGenerator
    {
        protected static readonly string ssrsBaseURL = "http://localhost/reportserver";


        public List<TicketData> GenerateIncentveReport(DateTime startDate, DateTime endDate)
        {

            return null;
        }

        public void GenerateReport (ReportInput reportData)
        {
            if (reportData.ReportType == 0)
                GenerateIncentveReport(reportData.StartDate, reportData.EndDate);

            else if (reportData.ReportType == 1)
                GenerateLaborHoursByJob();//reportData.StartDate, reportData.EndDate);

            else if (reportData.ReportType == 2)
                GenerateLaborHoursByJobAndEmployee(reportData.StartDate, reportData.EndDate);

            else
            {
                Exception e = new Exception("Error, Report Type is not valid");
                throw e;
            }
        }

        public async void GenerateLaborHoursByJob()//DateTime startDate, DateTime endDate)
        {
            //List<LaborHoursByJob> data = new List<LaborHoursByJob>();

            //using (var db = new TicketingSystemDBContext())
            //{
            //    var td = db.TicketData.Where(t => t.EntryDate >= startDate && t.EntryDate <= endDate).ToList();
            //    foreach (TicketData t in td)
            //    {
            //        LaborHoursByJob lh = new LaborHoursByJob();
            //        lh.Cases = t.CasesNum;
            //        lh.Pallets = t.PalletNum;
            //        lh.Job = db.JobType.Where(j => j.JobTypeId == t.JobTypeId).FirstOrDefault();
            //        if (t.EndTime == null)
            //        {
            //            lh.TotalHours = DateTime.Now - t.StartTime;
            //        }
            //        else
            //        {
            //            lh.TotalHours = t.EndTime - t.StartTime;
            //        }
            //        data.Add(lh);
            //    }
            //}

            //Call Report server endpoint with parameters
            //serve file via binary

            //paramlist.Add(new ReportParameter(""))

            //var x = 0;

            //BasicHttpBinding rsBinding = new BasicHttpBinding();
            //rsBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            //rsBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;

            //EndpointAddress rsEndpointAddress = new EndpointAddress("http://localhost:80/reportserver/ReportService2010.asmx");
            //ReportingService2010SoapClient rsClient = new ReportingService2010SoapClient(rsBinding, rsEndpointAddress);





            //if (output.Status == TaskStatus.RanToCompletion && output.Result.Length > 0)
            //{
            //    foreach (CatalogItem item in output.Result)
            //    {

            //    }
            //}

            //BasicHttpBinding rsBinding = new BasicHttpBinding();
            //rsBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            //rsBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;


            //EndpointAddress rsEndpointAddress = new EndpointAddress("http://localhost:80/reportserver/ReportService2005.asmx");
            //ReportExecutionServiceSoapClient rsc = new ReportExecutionServiceSoapClient(rsBinding, rsEndpointAddress);

            //ServerInfoHeader serverInfoHeader;
            //ExecutionInfo executionInfo;

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ssrsBaseURL);
            
            var resp = await httpClient.GetAsync(ssrsBaseURL + "/report");


            var y = 0;



        }

        //private static async Task<CatalogItem[]> rsListChildren(String ItemPath, ReportingService2010SoapClient rsClient)
        //{
        //    TrustedUserHeader trustedUserHeader = new TrustedUserHeader();
        //    ListChildrenResponse listChildrenResponse = null;
        //    try
        //    {
        //        listChildrenResponse = await rsClient.ListChildrenAsync(trustedUserHeader, ItemPath, false);
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message + exception.StackTrace);
        //        return new CatalogItem[0];
        //    }
        //    return listChildrenResponse.CatalogItems;
        //}

        public void GenerateLaborHoursByJobAndEmployee(DateTime startDate, DateTime endDate)
        {
        }

        public LaborHoursByJob GetLaborHoursByJob(DateTime startDate, DateTime endDate)
        {
            LaborHoursByJob lh = new LaborHoursByJob();



            return lh;
        }
    }
}

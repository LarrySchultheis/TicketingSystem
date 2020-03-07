using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System.IO;
using TicketingSystem.Services;

namespace Tests.AutomatedTests
{
    [TestFixture]
    class ReportGenerationTests
    {
        private IWebDriver driver;
        public string reportUrl;
        private readonly string chromeDriverPath = GetDriverDirectory();
    

        [Test(Description = "Run LaborHoursByJobReport")]
        public void LaborHoursByJobTest()
        {
            reportUrl = "https://localhost:44326/Report/Index";
            driver.Navigate().GoToUrl(reportUrl);
            
        }

        [TearDown]
        public void TearDownTest()
        {
            driver.Close();
        }

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver(chromeDriverPath);
        }

        private static string GetDriverDirectory()
        {
            return Directory.GetCurrentDirectory().Replace("bin\\Debug\\netcoreapp2.1", "");
        }
    }
}

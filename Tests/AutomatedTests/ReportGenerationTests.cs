using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System.IO;
using TicketingSystem.Services;
using Microsoft.Reporting.WebForms;

namespace Tests.AutomatedTests
{
    [TestFixture]
    class ReportGenerationTests
    {
        private IWebDriver driver;
        public string reportUrl;
        private readonly string chromeDriverPath = GetDriverDirectory();
    

        //[Test(Description = "Run LaborHoursByJobReport")]
        public void LaborHoursByJobTest()
        {
            reportUrl = "https://localhost:44326/Report/Index";
            driver.Navigate().GoToUrl(reportUrl);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            var startDate = driver.FindElement(By.Id("startDate"));
            var endDate = driver.FindElement(By.Id("endDate"));
            var submitButton = driver.FindElement(By.Id("subBtn"));

            startDate.SendKeys("1/20/2020");
            endDate.SendKeys("2/20/2020");
            submitButton.Click();
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

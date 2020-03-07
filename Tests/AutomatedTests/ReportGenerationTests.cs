using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;

namespace Tests.AutomatedTests
{
    [TestFixture]
    class ReportGenerationTests
    {
        private IWebDriver driver;
        public string reportUrl;
        private readonly string chromeDriverPath = "C:\\Program Files\\SeleniumChromeDriver";

        [Test(Description = "Run LaborHoursByJobReport")]
        public void LaborHoursByJobTest()
        {
            reportUrl = "https://localhost:44326/Report/Index";
            driver.Navigate().GoToUrl(reportUrl);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            
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
    }
}

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Copyscape
{
    public class Tests
    {
        private IWebDriver driver;
        private readonly By _inputurl = By.XPath("//input[@id='name']");
        private readonly By _buttongo = By.XPath("//input[@value='Go']");
        private readonly By _find_results_title = By.XPath("//div[@class='results_title']//b[1]");

        private const string _url = "https://tradercalculator.site/";
        
        private const string _expected_result = "No results";
        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-extensions"); // to disable extension
            options.AddArguments("--disable-notifications"); // to disable notification
            options.AddArguments("--disable-application-cache"); // to disable cache
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.copyscape.com/");
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test1()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(x => driver.FindElement(_inputurl));
            driver.FindElement(_inputurl).SendKeys(_url);

            var signin1 = driver.FindElement(_buttongo);
            signin1.Click();

            var actualresults = driver.FindElement(_find_results_title).Text;
            Assert.AreEqual(_expected_result, actualresults, "Test fail");

            if (_expected_result == actualresults)
            {
                driver.Quit();
            }
            else
            {
                IReadOnlyCollection<IWebElement> selecthreh = driver.FindElements(By.ClassName("result"));
                selecthreh.Click();
            }




        }

        [TearDown]
        public void TearDown()
        {
            
           // driver.Quit();
        }
    }
}
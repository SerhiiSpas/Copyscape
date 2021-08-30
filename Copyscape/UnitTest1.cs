using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using RestSharp;
using NUnit.Framework.Internal;
using OpenQA.Selenium.Chrome;
using RestSharp.Serialization.Json;
using System.Text.RegularExpressions;

namespace Copyscape
{

    public class Copyscape_Check
    {


        [Test]
        public void Copyscape()
        {

            var client = new RestClient("https://www.copyscape.com/?q=https%3A%2F%2Fwww.forexindikator.net%2F"); //+url
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"92\", \" Not A;Brand\";v=\"99\", \"Google Chrome\";v=\"92\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "navigate");
            request.AddHeader("Sec-Fetch-User", "?1");
            request.AddHeader("Sec-Fetch-Dest", "document");

            IRestResponse response = client.Execute(request);
            /*Console.WriteLine(response.Content);*/
            string response_html = response.Content;
            string No_results = "<b>No results</b>";
            string results_error_month = "The maximum number of scans per month has been reached for this site";
            string results_error_network = "Your computer or network has reached the limit for free Copyscape searches.";
            bool first_check = response_html.Contains(No_results);
            bool second_check = response_html.Contains(results_error_month);
            bool third_check = response_html.Contains(results_error_network);

            if (first_check is true)
            {
                Assert.AreEqual(true, first_check, "first_check fail");
                Console.WriteLine("Test passes, no copies found");
            }

            else if (second_check is true)
            {
                Assert.AreEqual(true, second_check, "second_check fail");
                Console.WriteLine("The maximum number of scans per month has been reached for this site");
            }

            else if (third_check is true)
            {
                Assert.AreEqual(true, third_check, "third_check fail");
                Console.WriteLine("Your computer or network has reached the limit for free Copyscape searches.");
            }

            else
            {
                IWebDriver driver = new ChromeDriver();
                driver.Url = @"https://www.copyscape.com/?q=https%3A%2F%2Fwww.forexindikator.net%2F";
                var _cookie = driver.Manage().Cookies.GetCookieNamed("PHPSESSID");



                IReadOnlyCollection<IWebElement> selectlink = driver.FindElements(By.XPath("//div[@class='result']//a"));

                foreach (IWebElement a in selectlink)
                {
                    // a.FindElement(By.XPath("//div[@class='result']//a")).Click();
                    IWebDriver driverTest = new ChromeDriver();    
                    driverTest.Url = a.GetAttribute("href");                    
                    driverTest.Manage().Cookies.AddCookie(_cookie);                    
                    driverTest.Navigate().GoToUrl(a.GetAttribute("href"));

                    Regex regex = new Regex(@"/\d{1, 3}/(%)>");
                    By _textresults = By.XPath("//form[@action='followup.php']");
                    string actualText = driver.FindElement(_textresults).Text;
                    MatchCollection matches = regex.Matches(actualText);

                     if (matches.Count > 0)
                     {
                         foreach (Match match in matches)
                             Console.WriteLine(match);

                     }

                     else
                     {
                         Console.WriteLine("Not found results");
                     }



                }
                Console.WriteLine("empty");
            }

        }
    }
}
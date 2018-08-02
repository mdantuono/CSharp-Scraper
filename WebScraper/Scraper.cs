using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebScraper
{
    public class Scraper
    {
        public Scraper()
        { }

        public void Scrape()
        {
            // Add (--headless) to chromeoptions to block window from popping up
            //ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless");

            // Initiate new ChromeDriver called driver and navigate to login URL
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://login.yahoo.com/config/login?.src=finance&.intl=us&.done=https%3A%2F%2Ffinance.yahoo.com%2F");

            // Input username field, submit
            IWebElement username = driver.FindElement(By.Name("username"));
            username.SendKeys("mikeishere3@intracitygeeks.org");
            username.Submit();

            // Initiate new driver wait to wait for page to load and elements to appear
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));

            // Wait, then Input password field, submit using click
            IWebElement password = wait.Until(d => driver.FindElement(By.Id("login-passwd")));
            password.SendKeys("scraper123");
            driver.FindElement(By.Id("login-signin")).Click();

            // Navigate to portfolio url once logged in
            driver.Navigate().GoToUrl("https://finance.yahoo.com/portfolio/p_0/view/v1");

            // Wait for tr elements to load
            wait.Until(d => driver.FindElement(By.TagName("tbody")));

            // Get a count of the stocks in the list
            int count;
            IWebElement list = driver.FindElement(By.TagName("tbody"));
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> items = list.FindElements(By.TagName("tr"));
            count = items.Count;

            Console.WriteLine("\nThere are " + count + " stocks in the list\n");

            //Loop to iterate through names and prices of stocks
            for (int i = 1; i <= count; i++)
            {
                var symbol = driver.FindElement(By.XPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[1]/span/a")).Text;
                var price = driver.FindElement(By.XPath("//*[@id=\"main\"]/section/section[2]/div[2]/table/tbody/tr[" + i + "]/td[2]/span")).Text;

                Console.WriteLine(symbol + " " + price);
            }

            Console.WriteLine("\nI gotchu fam.\n");
        }

        public void TestScrape()
        {
            return;
        }
    }

}
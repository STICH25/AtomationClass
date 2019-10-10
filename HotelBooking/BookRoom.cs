using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace HotelBooking
{
    public class GetARoom
    {
        public IWebDriver driver;
        public Actions actions;
        public WebDriverWait wait;
        

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));
            actions = new Actions(driver);
            wait.PollingInterval = TimeSpan.FromMilliseconds(5000);
            wait.IgnoreExceptionTypes(typeof(ElementNotInteractableException), typeof(NoSuchElementException),
                typeof(ElementNotVisibleException), typeof(ElementNotSelectableException), typeof(ElementClickInterceptedException));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.phptravels.net/");
        }

        [Test]
        public void ReachSite()
        {
            string pageTitle = "PHPTRAVELS | Travel Technology Partner";
            string title = driver.Title;
            Assert.AreEqual(title, pageTitle);
        }

        [Test]
        public void SearchHotel()
        {
            var myDate = DateTime.Now;
            string currentDay = myDate.ToString("d MM yyyy");
            var futureDay = DateTime.Now.AddDays(7);

           IWebElement cookies = wait.Until(drv => driver.FindElement(By.Id("cookyGotItBtn")));
           if (cookies.Displayed)
           {
               cookies.Click();
           }

           
           IWebElement searchBox = wait.Until(drv => driver.FindElement(By.XPath("//div[@id='s2id_location']//a[@class='select2-choice select2-default']")));
           searchBox.Click();
           searchBox.SendKeys("Tamp");
           Thread.Sleep(2000);
           IWebElement city = wait.Until(drv => driver.FindElement(By.XPath("//div[contains(text(),'a, United States')]")));
           actions.MoveToElement(city).Perform();
           city.Click();

           //Selecting date range
           IWebElement checkInDay = driver.FindElement(By.Name("checkin"));
           checkInDay.SendKeys(currentDay);
           checkInDay.Click();

           IWebElement checkoutDay = driver.FindElement(By.XPath("//input[@name='checkout']"));
           checkoutDay.SendKeys(futureDay.ToString("d MM yyyy"));
           checkoutDay.Click();

           //Search Button
           IWebElement searchBtn = wait.Until(drv => driver.FindElement(By.XPath("//button[@class='btn btn-lg btn-block btn-primary pfb0 loader']")));
           searchBtn.Submit();
           
           IList<IWebElement> hotelList = wait.Until(drv => driver.FindElements(By.XPath("//table[@id='listing']")));
           Assert.IsNotEmpty(hotelList); 


        }

        [TearDown]
        public void Close()
        {
            driver.Close();
            driver.Quit();
        }

    }
}
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumFramework.GuidedSellerAutomation
{
    public class Driver
    {
        public static IWebDriver Instance { get; set; }

        public static void Initialize()
        {
            Instance = new ChromeDriver("C:\\Users\\faiw\\SDrivers");
            //Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
        }

        public static void Close()
        {
            Instance.Close();
        }
    }
}
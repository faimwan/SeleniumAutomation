using System;
using System.Threading;
using GuidedSellerAutomation;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace SeleniumFramework.CountertopsAutomation
{
    public class Driver
    {
        public static IWebDriver Instance { get; set; }

        public static void Initialize()
        {
            //Instance = new ChromeDriver("C:\\Users\\faiw\\SDrivers");
            Instance = new InternetExplorerDriver("C:\\Users\\faiw\\SDrivers");
            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            //DesignBuilderPage.GoTo();
        }

        public static void Close()
        {
            Instance.Close();
        }

        public static void Wait(TimeSpan timeSpan)
        {
            Thread.Sleep((int) (timeSpan.TotalSeconds * 1000));
        }

    }
}
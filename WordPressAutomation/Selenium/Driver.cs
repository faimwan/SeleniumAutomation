using System;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WordPressAutomation
{
    public class Driver
    {
        public static IWebDriver Instance { get; set; }

        public static string BaseAddress
        {
            get { return "http://localhost:21272"; }
        }

        public static void Initialize()
        {
            Instance = new ChromeDriver("C:\\Users\\faiw\\SDrivers");
            TurnOnwait();
            LoginPage.GoTo();

            // example for SauceLab
            // DesiredCapabilities capabilities = DesiredCapabilities.Chrome();
            // capabilities.setCapabilities(CapabilityType.Platform, "Window 10");
            // capabilities.setCapabilities(CapabilityType.Version, "");
            // capabilities.setCapabilities("name", "Wordpress test");
            // capabilities.setCapabilities("username", "iamatester");
            // capabilities.setCapabilities("accessKey", "");
            //
            // Instance = new RemoteWebDriver(
            //              new uri("http://ondemand.saucelabs.com:80/wd/hub"), capabilities);

        }

        public static void Close()
        {
            Instance.Close();
        }

        public static void Wait(TimeSpan timeSpan)
        {
            Thread.Sleep((int) (timeSpan.TotalSeconds * 1000));
        }

        public static void Nowait(Action action)
        {
            TurnOffwait();
            action();
            TurnOnwait();
        }

        private static void TurnOnwait()
        {
            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
        }

        private static void TurnOffwait()
        {
            Instance.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }
    }
}
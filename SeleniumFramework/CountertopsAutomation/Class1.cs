using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace SeleniumFramework.CountertopsAutomation
{
    public class Class1
    {
        public void Go()
        {
            var driver = new ChromeDriver("C:\\Users\\faiw\\SDrivers");
            driver.Navigate().GoToUrl("http://google.com");

            Wait(10);

            Console.WriteLine("Waited for 5 second");
            driver.Close();
        }

        public static void Wait(int timeSpan)
        {
            Console.WriteLine("Inside Wait method");
            Console.WriteLine("value of time: " + timeSpan);
            Thread.Sleep((int)(timeSpan* 1000));
        }
    }
}

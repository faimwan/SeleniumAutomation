using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumFramework.CountertopsAutomation
{
    public class Countertops
    {
        public static void WaitForProgressBarToFinish()
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElements(By.CssSelector(".progress-linear.interminate")).Count == 0);
        }

        public static void WaitForConfiguratorToBeReady()
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElement(By.XPath("//div[@class='config-product-name']/span[@id='brand']")).Displayed);
        }
        
    }
}

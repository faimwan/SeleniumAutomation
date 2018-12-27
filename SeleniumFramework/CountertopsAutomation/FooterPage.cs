using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace SeleniumFramework.CountertopsAutomation
{
    public class FooterPage
    {
        public static void ValidateFooter()
        {
            var thdLogo = Driver.Instance.FindElements(By.CssSelector(".jss128>svg"));
            Assert.IsTrue(thdLogo.Count > 0, "Missing THD logo");

            var underLogoText = Driver.Instance.FindElement(By.CssSelector(".jss128>h2"));
            Assert.IsTrue(underLogoText.Text.Contains("All rights reserved"), "Missing rights reserved text");
        }
    }
}

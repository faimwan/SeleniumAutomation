using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace SeleniumFramework.CountertopsAutomation
{
    public class HeaderPage
    {
        public static void ValidateHeader()
        {
            var thdLogo = Driver.Instance.FindElements(By.CssSelector(".jss32>svg"));
            Assert.IsTrue(thdLogo.Count > 0, "Missing THD logo");

            var headerTitle = Driver.Instance.FindElement(By.CssSelector(".jss25>h2"));
            Assert.IsTrue(headerTitle.Text == "Design Builder", "Missing page header title");

            var needHelp = Driver.Instance.FindElement(By.CssSelector(".jss31"));
            Assert.IsTrue(needHelp.Text.Contains("Need help? Please call us at"), "Missing need help text");

            var helpIcon = Driver.Instance.FindElements(By.ClassName("icon_help_full_filled"));
            Assert.IsTrue(helpIcon.Count > 0, "Missing help (information?) icon");
        }
    }
}

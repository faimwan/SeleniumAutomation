using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuidedSellerAutomation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumFramework.CountertopsAutomation
{
    public class CountertopsPIPPage
    {
        private static Utilities util = new Utilities();
        private static CountertopsValidationMessages valMsg = new CountertopsValidationMessages();

        public static void ValidateProduct(string prodName)
        {
            // validate product name in header
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElement(By.CssSelector(".header-title>span")).Text == prodName);

            // validate actions in header (Overview, Colors, Description, Specifications)
            var headerActions =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'header-action')]/span"));
            string[] baseHeaderActions = valMsg.pip_headerActions;
            bool validHA = util.ValidateListElements(valMsg.pip_headerActions, headerActions);
            if (!validHA)
                Assert.IsTrue(-1 > 0, "Header actions list does not match");

            // validate main image
            var images = Driver.Instance.FindElements(By.XPath("//img"));
            Assert.IsTrue(images[0].Displayed, "Missing image.");

            // validate product name next to image
            string baseDesc = "//main/div[3]/div[2]/div/";
            var productName = Driver.Instance.FindElement(By.XPath(baseDesc + "div[1]/span"));
            Assert.IsTrue(productName.Text == prodName, "Product name did not match.");

            // validate product details
            var shortDesc = Driver.Instance.FindElement(By.XPath(baseDesc + "div[2]"));
            Assert.IsTrue(shortDesc.Displayed, "Missing short description.");

            // validate 'Read More' link
            var readMoreLink = Driver.Instance.FindElement(By.Id("read-more"));
            Assert.IsTrue(readMoreLink.Displayed, "Missing Read More link.");

            // validate 'Starting at' label
            //var startingAtLabel = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'ProductPrice')]//span"));
            //Assert.IsTrue(startingAtLabel.Text == "Starting at", "Missing 'Starting at' label");

            // validate price
            // var price = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'pip-price')]"));
            //Assert.IsTrue(price.Displayed, "Missing pricing.");

            // validate 'Configure Product' button
            var confProductButton =
                Driver.Instance.FindElement(By.XPath("//button[contains(@class,'pip-configure-product-button')]"));
            Assert.IsTrue(confProductButton.Displayed, "Missing Configure Product button.");

            // validate breadcrumbs
            ValidateBreadcrumbs(prodName);

        }

        public static void ValidateBreadcrumbs(string prodName)
        {
            // validate base breadcrumbs with the active breadcrumb
            var breadcrumbs = BuildBaseBreadcrumbs();
            string[] baseBreadcrumbs = valMsg.plp_breadcrumbs;
            bool valid = util.ValidateListElements(baseBreadcrumbs, breadcrumbs);
            if (!valid)
            {
                Assert.IsTrue(-1 > 0, "Base breadcrumbs did not match.");
            }
            else
            {
                var activebcrumb = Driver.Instance.FindElements(
                    By.XPath("//div[starts-with(@class,'Breadcrumbs')]/span[contains(@class,'active')]"));
                if (activebcrumb.Count > 0)
                {
                    Assert.IsTrue(activebcrumb[0].Text == prodName, "Active breadcrumb did not match.");
                }
            }
        }
        

        public static string[] BuildBaseBreadcrumbs()
        {
            string[] bcrumbs = new string[3];
            var breadcrumbs =
                Driver.Instance.FindElements(
                    By.XPath("//div[starts-with(@class,'Breadcrumbs')]/*[starts-with(@class,'breadcrumb')]/span"));
            if (breadcrumbs.Count > 0)
            {
                for (int i = 0; i < breadcrumbs.Count - 1; i++)
                    bcrumbs[i] = breadcrumbs[i].Text;
            }

            return bcrumbs;
        }

        public static void ConfigureProduct(string prodName)
        {
            // click on Configure Product button
            var confProductButton =
                Driver.Instance.FindElement(By.XPath("//button[contains(@class,'pip-configure-product-button')]"));
            confProductButton.Click();

            // check if configurator page is displayed
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElements(By.XPath("//header//h2"))[0].Text == "Configurator");

            // validate configurator page ui elements
            //ValidateConfiguratorUI(prodName);

        }

        public static void ValidateConfiguratorUI(string prodName)
        {
            // validate breadcrumbs
            ValidateBreadcrumbs(prodName);

            // check for measurement tool option label
            //      check for shape tabs
            // check for drawing shape in canvas
            //      check for rotate left and rotate right buttons
            // check for choose a surface (surface thickness) option label
            //      check for thickness choices list
            // check for select color option label
            //      check for color choices list
            // check for finshed corners & edges option label
            //      check for external corner choices
            //      check for internal corner choice
            // check for choose edge style option label
            //      check for edge style choice
            // check for summary card
            //      check for brand and product name
            //      check for price
            //      check for Add To Design button
            //      check for options labels:
            //          Total Surface Area, Select a Surface Thickness, Color, Finished Corners, 1/4" Rounded, 1" Rounded, 3" Rounded,
            //          Interior Angles, Edge Style, Total Finished Edge Length, Total Square Footage, Shape (name)
        }

    }
}

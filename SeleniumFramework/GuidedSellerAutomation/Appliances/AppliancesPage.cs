using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumFramework.GuidedSellerAutomation.Appliances
{
    public class AppliancesPage
    {
        private const string _allChoices = "//div[@class='parent-option']";

        private static GuidedSellerValidationMessages gsValMsg = new GuidedSellerValidationMessages();
        private static Utilities util = new Utilities();

        public static void GoTo()
        {
            //Driver.Instance.Navigate().GoToUrl("http://custom.hd-qa71.homedepotdev.com/guidedseller/configurator/540857");
            Driver.Instance.Navigate().GoToUrl("http://custom.homedepot.com/guidedseller/configurator/539890");
            Driver.Instance.Manage().Window.Maximize();
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.CssSelector(".parent-option>div")).Displayed);
        }

        public static bool IsAt
        {
            get
            {
                var mainQuestion = Driver.Instance.FindElement(By.CssSelector(".parent-option>div"));
                return mainQuestion.Text.Contains("Appliances are you looking for?");
            }
        }

        public static void Validate_Main_Choices()
        {
            string[] baseMainChoiceList = gsValMsg.AppliancesMainChoiceList;
            var allChoices = Driver.Instance.FindElements(By.ClassName("parent-option"));
            if (allChoices.Count > 0)
            {
                var mainChoiceList = allChoices[0].FindElements(By.XPath("div[2]//div[contains(@class,'ChoiceDescription')]/div"));
                Assert.IsTrue(mainChoiceList.Count.Equals(4), "Number of expected main choices does not match");
                bool vChoices = util.ValidateListElements(baseMainChoiceList, mainChoiceList);
                Assert.IsTrue(vChoices, "Main appliances choices do not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Could not find main choices");
            }               
        }

        public static void ChooseAStyle(string style)
        {
            // collect all style choices
            var styleChoices = Driver.Instance.FindElements(By.XPath("//div[contains(@class,'ChoiceDescription')]"));

            // find the one that match parameter and click on it
            int pos = util.GetItemPosition(styleChoices, style);
            if(pos > -1)
                styleChoices[pos].Click();
            else
            {
                Assert.IsTrue(-1 > 0, "Did not find provided style: " + style );
            }

            // wait for search results
            var wait = new WebDriverWait(Driver.Instance,TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElements(By.ClassName("plp-pod")).Any());
        }

        public static void ValidateSearchResultUI()
        {
            // validate Recommended Products text
            var recommendedProductsLabel =
                Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'ProductHeaderComponent__MainText')]"));
            Assert.IsTrue(recommendedProductsLabel.Text == "Recommended Products", "Missing Recommended Products label.");
                       
            // validate checkbox is displayed
            var getItFastCheckbox = Driver.Instance.FindElement(By.ClassName("checkbox-btn__label"));
            Assert.IsTrue(getItFastCheckbox.Displayed, "Missing get it fast checkbox");

            // validate localized store
            var localizaedStore = Driver.Instance.FindElements(By.ClassName("store-banner__localized"));
            Assert.IsTrue(localizaedStore.Count > 0, "Missing localized store");

            // validate 'Chnage Your Store' link
            var changeYourStoreLink = Driver.Instance.FindElement(By.ClassName("get-it-fast__change"));
            Assert.IsTrue(changeYourStoreLink.Text == "Change Your Store");

            // validate 'items on display at' string
            Assert.IsTrue(getItFastCheckbox.Text.Contains("items on display at"), "Missing on display at text");

            // validate 1st pod has: image, brand, product description, model, rating, pricing, delivery option
            var podList = Driver.Instance.FindElements(By.ClassName("plp-pod"));
            var brand = podList[0].FindElements(By.ClassName("plp-pod__brand-name"));
            Assert.IsTrue(brand.Count > 0, "Missing brand.");

            var prodDescription = podList[0].FindElements(By.CssSelector(".plp-pod__description>a"));
            Assert.IsTrue(prodDescription.Count > 0, "Missing product description.");

            var model = podList[0].FindElements(By.ClassName("pod-plp__model"));
            Assert.IsTrue(model.Count > 0, "Missing model info.");

            var price = podList[0].FindElements(By.CssSelector(".price.price--secondary"));
            Assert.IsTrue(price.Count > 0, "Missing pricing.");

            var addToCartButton = podList[0].FindElements(By.ClassName("button__content"));
            Assert.IsTrue(addToCartButton.Count == 0, "Add To Cart should not be displayed.");

            var deliveryOption = podList[0].FindElements(By.CssSelector(".fulfillment__message>div>div"));
            Assert.IsTrue(deliveryOption.Count > 0, "Missing delivery option.");
        }
    }
}

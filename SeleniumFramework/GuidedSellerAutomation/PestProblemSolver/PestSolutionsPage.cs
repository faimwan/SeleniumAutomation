using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using GuidedSellerAutomation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumFramework.GuidedSellerAutomation.PestProblemSolver
{
    public class PestSolutionsPage
    {
        private const string _mainChoices = "//div[contains(@class,'styles__ChoiceDescription')]/div";
        private const string _accountIcon = "//div[@class='MyAccount__Icon']";
        //private const string _accountIcon = "//div[@class='MyAccount__Icon']//svg[@class='HeaderIcon__primarySvg']";
        private readonly By _topLevelChoice = By.XPath("//div[@class='option-copmonent']");

        private static GuidedSellerValidationMessages gsValMsg = new GuidedSellerValidationMessages();
        private static Utilities util = new Utilities();

        public static void GoTo()
        {            
            Driver.Instance.Navigate().GoToUrl("http://custom.homedepot.com/guidedseller/configurator/537468");
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.ClassName("content")).Displayed);
        }

        public static bool IsAt
        {
            get
            {
                var msg = Driver.Instance.FindElements(By.ClassName("content"));
                if (msg.Count > 0)
                    return msg[0].Text.Contains("Make a selection above to view recommended products.");
                return false;
            }
        }

        public static void Validate_Main_Choices()
        {
            string[] mainChoiceList = gsValMsg.PestSolutionsMainChoiceList;
            By locator = By.XPath(_mainChoices);
            var currentChoiceList = Driver.Instance.FindElements(locator);            
            Console.WriteLine("Number of main choices: " + currentChoiceList.Count);
            Assert.IsTrue(currentChoiceList.Count.Equals(6), "Number of expected main choices does not match");
            bool vChoices = util.ValidateListElements(mainChoiceList, currentChoiceList);
            Assert.IsTrue(vChoices, "Main pest control choices does not match");
        }

        public static void Validate_Header()
        {
            var hdLogo = Driver.Instance.FindElements(By.ClassName("HeaderLogo"));
            Assert.IsTrue(hdLogo.Count > 0, "Missing Home Depot Logo");

            var storeLabel = Driver.Instance.FindElements(By.ClassName("MyStore__label"));
            Assert.IsTrue(storeLabel.Count > 0, "Missing Store label");

            var searchInput = Driver.Instance.FindElements(By.Id("headerSearch"));
            Assert.IsTrue(searchInput.Count > 0, "Missing search input field");

            var headerSearchButton = Driver.Instance.FindElements(By.Id("headerSearchButton"));
            Assert.IsTrue(headerSearchButton.Count > 0, "Missing search button");

            var myAccountLabel = Driver.Instance.FindElements(By.LinkText("My Account"));
            Assert.IsTrue(myAccountLabel.Count > 0, "Missing my account label");

            var myAccountIcon = Driver.Instance.FindElements(By.XPath("//div[@class='MyAccount__icon']"));
            Assert.IsTrue(myAccountIcon.Count > 0, "Missing my account icon");

            var myCartLabel = Driver.Instance.FindElements(By.ClassName("MyCart__label"));
            Assert.IsTrue(myCartLabel.Count > 0, "Missing cart label");

            var myCartItemCountLabel = Driver.Instance.FindElements(By.ClassName("MyCart__itemCount__label"));
            Assert.IsTrue(myCartItemCountLabel.Count > 0, "Missing cart item count label");

            var myCartIcon = Driver.Instance.FindElements(By.XPath("//div[@class='MyCart__icon']"));
            Assert.IsTrue(myCartIcon.Count > 0, "Missing cart icon");

            var storeFinderLink = Driver.Instance.FindElements(By.LinkText("Store Finder"));
            Assert.IsTrue(storeFinderLink.Count > 0, "Missing Store Finder link");

            var truckToolRentalLink = Driver.Instance.FindElements(By.LinkText("Truck & Tool Rental"));
            Assert.IsTrue(storeFinderLink.Count > 0, "Missing Truck & Tool Rental link");

            var forTheProLink = Driver.Instance.FindElements(By.LinkText("For the Pro"));
            Assert.IsTrue(forTheProLink.Count > 0, "Missing For the Pro link");

            var giftCardsLink = Driver.Instance.FindElements(By.LinkText("Gift Cards"));
            Assert.IsTrue(giftCardsLink.Count > 0, "Missing Gift Cards link");

            var creditServicesLink = Driver.Instance.FindElements(By.LinkText("Credit Services"));
            Assert.IsTrue(creditServicesLink.Count > 0, "Missing Credit Services link");

            var favoritesLink = Driver.Instance.FindElements(By.LinkText("Favorites"));
            Assert.IsTrue(favoritesLink.Count > 0, "Missing Favorites link");

            var trackOrderLink = Driver.Instance.FindElements(By.LinkText("Track Order"));
            Assert.IsTrue(trackOrderLink.Count > 0, "Missing Track Order link");

            var helpLink = Driver.Instance.FindElements(By.LinkText("Help"));
            Assert.IsTrue(helpLink.Count > 0, "Missing Help link");

            var allDepartmentsLink = Driver.Instance.FindElements(By.LinkText("All Departments"));
            Assert.IsTrue(allDepartmentsLink.Count > 0, "Missing All Departments link");

            var homeDecorFurnitureLink = Driver.Instance.FindElements(By.LinkText("Home Decor & Furniture"));
            Assert.IsTrue(homeDecorFurnitureLink.Count > 0, "Missing Home Decor & Furniture link");

            var diyProjectsIdeasLink = Driver.Instance.FindElements(By.LinkText("DIY Projects & Ideas"));
            Assert.IsTrue(diyProjectsIdeasLink.Count > 0, "Missing DIY Projects & Ideas link");

            var homeServicesLink = Driver.Instance.FindElements(By.LinkText("Home Services"));
            Assert.IsTrue(homeServicesLink.Count > 0, "Missing Home Services link");

            var specialsOffersLink = Driver.Instance.FindElements(By.LinkText("Specials & Offers"));
            Assert.IsTrue(specialsOffersLink.Count > 0, "Missing Specials & Offers link");

            var localAdLink = Driver.Instance.FindElements(By.LinkText("Local Ad"));
            Assert.IsTrue(localAdLink.Count > 0, "Missing Local Ad link");
        }

        public static void Validate_Footer()
        {
            var footerTagline = Driver.Instance.FindElements(By.Id("footerTagline"));
            Assert.IsTrue(footerTagline.Count > 0, "Missing footer tagline (More saving. More doing)");

            var footerPhoneLink = Driver.Instance.FindElements(By.ClassName("footerPhone__link"));
            Assert.IsTrue(footerPhoneLink.Count > 0, "Missing phone link");

            var customerService = Driver.Instance.FindElements(By.ClassName("footer__header"));
            Assert.IsTrue(customerService[0].Text == "Customer Service", "Missing Customer Services section");

            var resources = Driver.Instance.FindElements(By.ClassName("footer__header"));
            Assert.IsTrue(resources[1].Text == "Resources", "Missing Resources section");

            var aboutUs = Driver.Instance.FindElements(By.ClassName("footer__header"));
            Assert.IsTrue(aboutUs[2].Text == "About Us", "Missing About Us section");
        }
    }
}

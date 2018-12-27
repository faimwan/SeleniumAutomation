using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GuidedSellerAutomation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using String = System.String;

namespace SeleniumFramework.CountertopsAutomation
{

    public class CountertopsPLPPage
    {
        private static Utilities util = new Utilities();
        private static CountertopsValidationMessages valMsg = new CountertopsValidationMessages();

        public static void ValidatePLPUI(string zipCode)
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'ProductCard')]")).Count > 0);

            // validate 'Guide Me' button
            var guideMeButton = Driver.Instance.FindElements(By.XPath("//div[@class='SearchResult']/div[2]/button/span"));
            Assert.IsTrue(guideMeButton[0].Text == "Guide Me", "Missing Guide Me");

            // validate zipcode label
            var zipcodeLabel = Driver.Instance.FindElements(By.XPath("//label[@for='zipcodeValue']/span"));
            Assert.IsTrue(zipcodeLabel[0].Text == "Zipcode:", "Missing Zipcode label");

            // validate zipcode
            var zipcode = Driver.Instance.FindElements(By.CssSelector(".ProductSearchSort>Div>Div>Div>Span"));
            Assert.IsTrue(zipcode[0].Text == zipCode, "Zipcode does not match");

            // validate results label
            var result = Driver.Instance.FindElements(By.TagName("h3"));
            Assert.IsTrue(result[0].Text.Contains("results"), "Missing results");
           
            // validate Category label - has some issue with this validation - is not stable
            var categoryLabel = Driver.Instance.FindElements(By.XPath("//i[@id='facetToggle']/../h4"));
            Console.WriteLine("label: " + categoryLabel[0].Text);
            Assert.IsTrue(categoryLabel[0].Text == "Category", "Missing Category label");

            // validate filter
            var filterOption = Driver.Instance.FindElements(By.XPath("//div[@role='checkbox']/label/span"));
            Assert.IsTrue(filterOption[0].Text == "Countertops", "Missing Countertops filter");

            // validate sort by label
            var sortByLabel = Driver.Instance.FindElements(By.XPath("//label[@for='sortBy']/span"));
            Assert.IsTrue(sortByLabel[0].Text == "Sort By", "Missing Sort By label");

            // validate sort by dropdown field
            var sortByDropdown = Driver.Instance.FindElements(By.ClassName("Select-value"));
            Assert.IsTrue(sortByDropdown[0].Displayed, "Missing sort by dropwdown");

            // validate pods
            var pods = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'ProductCard')]"));
            Assert.IsTrue(pods.Count > 0, "No products in PLP page");
        }

        public static CountertopGotoPIPCommand SelectAProduct(string prodName)
        {
            return new CountertopGotoPIPCommand(prodName);
        }


        public static FacetCommand CollapseFacet(string category)
        {
            return new FacetCommand(category);
        }

        public static FacetFilterCommand CheckFilter(string checkValue)
        {
            return new FacetFilterCommand(checkValue);
        }

        public static void ValidateBreadcrumbs()
        {
            var breadcrumbs = BuildBreadcrumbs();
            string[] baseBreadcrumbs = valMsg.plp_breadcrumbs;
            bool valid = util.ValidateListElements(baseBreadcrumbs, breadcrumbs);
            if(!valid)
                Assert.IsTrue(-1 > 0, "Bradcrumbs did not match.");
        }

        public static string[] BuildBreadcrumbs()
        {
            string[] bcrumbs = new string[3];
            int lastElement = 2;
            var breadcrumbs =
                Driver.Instance.FindElements(
                    By.XPath("//div[starts-with(@class,'Breadcrumbs')]/*[starts-with(@class,'breadcrumb')]/span"));
            // need to add the previos elements to the array
            if (breadcrumbs.Count > 0)
            {
                for (int i=0; i< breadcrumbs.Count; i++)
                bcrumbs[i] = breadcrumbs[i].Text;
            }
            var activebcrumb = Driver.Instance.FindElements(
                By.XPath("//div[starts-with(@class,'Breadcrumbs')]/a[contains(@class,'active')]"));
            // need to add the active element to the array
            if (activebcrumb.Count > 0)
            {
                bcrumbs[lastElement] = activebcrumb[0].Text;
            }
            return bcrumbs;
        }
    }

    public class FacetFilterCommand
    {
        private readonly string checkFilter;
        private string uncheckFilter;

        public FacetFilterCommand(string checkValue)
        {
            this.checkFilter = checkValue;
        }

        public FacetFilterCommand UnCheckFilter(string uncheckValue)
        {
            this.uncheckFilter = uncheckValue;
            return this;
        }

        public void Perform()
        {
            var util = new Utilities();

            var wait = new WebDriverWait(Driver.Instance,TimeSpan.FromSeconds(3));
            wait.Until(d => d.FindElements(By.CssSelector(".FacetCategory>div>h4")).Count > 0);
            wait.Until(d => d.FindElements(By.XPath("//div[@role='checkbox']/label/span")).Count > 0);

            // locate facet filter named as checkValue and click on it
            var filterOptions = Driver.Instance.FindElements(By.XPath("//div[@role='checkbox']/label/span"));
            int pos = util.GetItemPosition(filterOptions, checkFilter);
            if (pos > -1)
            {
                filterOptions[pos].Click();              
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Filter not found");
            }

            // validatte checkValue checkbox is checked
            // Thread.Sleep(3000);
            // Driver.Wait(TimeSpan.FromSeconds(3));
            var filterCheckboxes = Driver.Instance.FindElements(By.XPath("//div[@role='checkbox']"));
            wait.Until(d => filterCheckboxes[pos].GetAttribute("aria-checked") == "true");
            // Console.WriteLine("checkbox value: " + filterCheckboxes[pos].GetAttribute("aria-checked"));
            Assert.IsTrue(filterCheckboxes[pos].GetAttribute("aria-checked") == "true", "Facet filter checkbox is not checked");

            // validate pill named as checkValue
            var filterPills = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'PillsSection')]/div/span"));
            int pillPos = util.GetItemPosition(filterPills, checkFilter);
            if (pillPos < 0)
                Assert.IsTrue(-1 > 0, "Filter pill was not found");

            // validate 'Clear All' link
            var clearAllLink = Driver.Instance.FindElements(By.XPath("//div[@id='filterHeading']/div/button"));
            if(clearAllLink.Count == 0)
                Assert.IsTrue(-1 > 0, "Clear All link not found");
               
            // Click on 'x' on checkValue pill
            var closePill = filterPills[pillPos].FindElement(By.TagName("i"));
            closePill.Click();

            // validate pill and 'Clear All' link are not displayed
            Assert.IsTrue(Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'PillsSection')]/div/span")).Count == 0);
            Assert.IsTrue(Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'PillsSection')]/button")).Count == 0);

            // validate checkValue checkbox unchecked
            // Thread.Sleep(2000);
            // Driver.Wait(TimeSpan.FromSeconds(2));
            var filterCBoxes = Driver.Instance.FindElements(By.XPath("//div[@role='checkbox']"));
            wait.Until(d => filterCheckboxes[pos].GetAttribute("aria-checked") == "false");
            //Console.WriteLine("checkbox value: " + filterCheckboxes[pos].GetAttribute("aria-checked"));
            Assert.IsTrue(filterCBoxes[pos].GetAttribute("aria-checked") == "false", "Facet filter checkbox is checked");
        }
    }

    public class FacetCommand
    {
        private readonly string collapseCategory;
        private string expandCategory;

        public FacetCommand (string category)
        {
            this.collapseCategory = category;
        }

        public FacetCommand ExpandFacet(string category)
        {
            this.expandCategory = category;
            return this;
        }

        public void Perform()
        {
            // collect number of filter option for facet Category
            var facetFilterCount = Driver.Instance.FindElements(By.XPath("//div[@class='FacetCategory']/div[2]//div[@role='checkbox']")).Count;
            Console.WriteLine("number of filter: " + facetFilterCount);

            // find facet category's toggle and click on it
            var facetCategory = Driver.Instance.FindElements(By.XPath("//div[@class='FacetCategory']/div[1]/i[@id='facetToggle']"));
            Console.WriteLine("number of objs: " + facetCategory.Count);
            facetCategory[0].Click();

            // collect number of filter option for facet Category and validate no filter is displayed
            var collapsedFacetFilterCount = Driver.Instance.FindElements(By.XPath("//div[@class='FacetCategory']/div[2]//div[@role='checkbox']")).Count;
            Assert.IsTrue(collapsedFacetFilterCount == 0);

            // click on toggle
            facetCategory[0].Click();

            // validate filter is displayed
            var expandedFacetFilterCount = Driver.Instance.FindElements(By.XPath("//div[@class='FacetCategory']/div[2]//div[@role='checkbox']")).Count;
            Assert.IsTrue(expandedFacetFilterCount > 0);
        }
    }

    public class CountertopGotoPIPCommand
    {
        private static Utilities util = new Utilities();

        private readonly string prodName;

        public CountertopGotoPIPCommand(string prodName)
        {
            this.prodName = prodName;
        }

        public void Configure()
        {
            // find the product and click 
        }

        public void ByName()
        {
            // generate list of pods
            var podNameList =
                Driver.Instance.FindElements(
                    By.CssSelector(".countertops-plp-link.countertops-plp-productname-link>h2"));
            int posPod = util.GetItemPosition(podNameList, prodName);
            if (posPod > -1)
            {
                //Console.WriteLine("Product name: " + podNameList[(posPod - 1)].Text);
                podNameList[posPod].Click();
                var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
                wait.Until(d => d.FindElement(By.CssSelector(".header-title>span")).Text == prodName);
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Product was not found");
            }
        }

        public void ByMoreDeatils()
        {
            // generate list of pods
            var podNameList =
                Driver.Instance.FindElements(
                    By.CssSelector(".countertops-plp-link.countertops-plp-productname-link>h2"));
            int posPod = util.GetItemPosition(podNameList, prodName);

            Console.WriteLine("posPod is: " + posPod);         
            if (posPod > -1)
            {
                var locator = "//div[@class='Product-grid']/div[" + (posPod + 1) + "]//a[@class='countertops-plp-link countertops-plp-details-link']/button";
                var viewDetailsBtns = Driver.Instance.FindElements(By.XPath(locator));
                viewDetailsBtns[0].Click();
                var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
                wait.Until(d => d.FindElement(By.CssSelector(".header-title>span")).Text == prodName);
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Product was not found");
            }
        }

        public void ByBuildAndBuy()
        {
            // generate list of pods
            var podNameList =
                Driver.Instance.FindElements(
                    By.CssSelector(".countertops-plp-link.countertops-plp-productname-link>h2"));
            int posPod = util.GetItemPosition(podNameList, prodName);
            var biuldAndBuyList = Driver.Instance.FindElements(
                By.XPath("//a[contains(@class,'countertops-plp-details-link')]/button"));
            if (posPod > -1)
            {

                Console.WriteLine("value of posPod: " + posPod);
                var locator = "//div[@class='Product-grid']/div[" + (posPod + 1) + "]//a[contains(@class,'button primary')]";
                var buildAndBuyBtns = Driver.Instance.FindElements(By.XPath(locator));
                buildAndBuyBtns[0].Click();
                var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
                wait.Until(d => d.FindElements(By.CssSelector(".progress-linear.interminate")).Count == 0);
                wait.Until(d => d.FindElement(By.XPath("//div[starts-with(@class,'Header ')]//h2")).Text == "Configurator");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Product was not found");
            }
        }
    }
}

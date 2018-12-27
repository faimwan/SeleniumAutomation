using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Console = System.Console;

namespace SeleniumFramework.CountertopsAutomation
{
    public class DesignBuilderPage
    {
        /*
        public static void GoToNewDesign()
        {
            Driver.Instance.Navigate().GoToUrl("http://custom.hd-qa71.homedepotdev.com/specialorders/designs/countertops/new");
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.Id("create-design-model-designname")).GetAttribute("value") == "Design 1");
            //d.SwitchTo().ActiveElement().GetAttribute("id") == "create-design-modal-zipcode"
        }
        */

        public static bool IsAt
        {
            get
            {
                var modalTitle = Driver.Instance.FindElement(By.CssSelector(".card-title>h2"));
                return modalTitle.Text == "Design Name";
            }
        }

        public static bool IsAtDesignBuilder
        {
            get
            {
                var designBuilderTitle = Driver.Instance.FindElements(By.TagName("h2"));
                if (designBuilderTitle.Count > 0)
                    return designBuilderTitle[0].Text == "Design Builder";
                return false;
            }
        }

        public static void GoToDesignBuilder()
        {
            Driver.Instance.Navigate().GoToUrl("http://custom.hd-qa71.homedepotdev.com/specialorders/designs/countertops/designbuilder");
            //var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            //wait.Until(d => d.FindElement(By.Id("create-design-model-designname")).GetAttribute("value") == "Design 1");
            //d.SwitchTo().ActiveElement().GetAttribute("id") == "create-design-modal-zipcode"
        }


        public static CreateDefaultDesignCommand CreateDeafultDesignWithZipcode(string zipcode)
        {
            return new CreateDefaultDesignCommand(zipcode);
        }       

        public static void ValidateBreadcrumbs()
        {
            var baseBreadcrumb = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'Breadcrumbs breadcrumb')]/span/span"));
            Assert.IsTrue(baseBreadcrumb.Text == "Special Orders", "Missing Special Orders breadcrumb");

            var activeBreadcrumb = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'Breadcrumbs breadcrumb')]/a"));
            Assert.IsTrue(activeBreadcrumb.Text == "Design Builder", "Missing Deisgn Builder breadcrumb");
        }

        public static AddDesignCommand AddDesignAs(string designName)
        {
            return new AddDesignCommand(designName);
        }

        public static void ValidateDefaultDesign()
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElements(By.CssSelector(".tab-effects>li>div>span")).Count > 0);

            var tagDesignName = Driver.Instance.FindElement(By.CssSelector(".tab-effects>li>div>span"));
            Assert.IsTrue(tagDesignName.Text == "Design 1", "Default design name 'Design 1' did not match");

            var containerTtitle = Driver.Instance.FindElement(By.Id("My Countertop"));
            Assert.IsTrue(containerTtitle.Text == "My Countertop", "Container title 'My Countertop' did not match");

            //var containerName = Driver.Instance.FindElement(By.XPath("//h4[contains(@class,'countertops-empty-item-name')]"));
            //Assert.IsTrue(containerName.Text == "Countertop 1", "Container name 'Countertop 1' did not match");

            var selectASurfaceLink = Driver.Instance.FindElement(By.CssSelector(".card-content-placeholder>div>span>button"));
            Assert.IsTrue(selectASurfaceLink.Text == "Select A Surface", "Missing Select A Surface link");

            var addAnotherCountertopButton = Driver.Instance.FindElement(By.CssSelector(".card-actions>button"));
            Assert.IsTrue(addAnotherCountertopButton.Text == "Add Another Countertop", "Missing Add Another Countertop button");

            var addNewDesignButton = Driver.Instance.FindElement(By.XPath("//div[@id='left-panel']/div[2]/button"));
            Assert.IsTrue(addNewDesignButton.Text == "Add New Design", "Missing Add New Design button");


        }

        public static ValidateDesignCommand ValidateDesignNameAs(string designName)
        {
            return new ValidateDesignCommand(designName);
        }

        

        public static void UpdateDesignNameAs(string newDesignName)
        {
            var designActions = Driver.Instance.FindElements(By.XPath("//span[starts-with(@class,'designActions')]"));
            var changeDesignNameAction = designActions[0];
            changeDesignNameAction.Click();

            // update design name and save change
            var designNameInput = Driver.Instance.FindElement(By.XPath("//input[@name='headlineModalConfirm']"));
            designNameInput.Clear();
            designNameInput.SendKeys(newDesignName);

            var saveButton = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[2]"));
            saveButton.Click();

            // validate updated design name
            var summaryDesignName = Driver.Instance.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]"));
            Assert.IsTrue(summaryDesignName.Text == newDesignName, "Design name '" + newDesignName + "' did not match");
        }

        public static void CancelUpdateDesignNameAs(string newDesignName)
        {
            var designActions = Driver.Instance.FindElements(By.XPath("//span[starts-with(@class,'designActions')]"));
            var changeDesignNameAction = designActions[0];
            changeDesignNameAction.Click();

            // update design name and save change
            var designNameInput = Driver.Instance.FindElement(By.XPath("//input[@name='headlineModalConfirm']"));
            designNameInput.Clear();
            designNameInput.SendKeys(newDesignName);

            var canceleButton = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[1]"));
            canceleButton.Click();

            var summaryDesignName = Driver.Instance.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]"));
            Assert.IsTrue(summaryDesignName.Text == "Design 1", "Design name was updated");
        }

        public static void AddDesign()
        {
            // save number of design tags            
            var designTags = Driver.Instance.FindElements(By.CssSelector(".tab-effects")).Count;
            Console.WriteLine("number of tags: " + designTags);

            // save deisgn name in summary card
            var summaryDesignName0 = Driver.Instance.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]")).Text;

            // click on Add Design button
            var addNewDesignButton = Driver.Instance.FindElement(By.XPath("//div[@id='left-panel']/div[2]/button"));
            addNewDesignButton.Click();

            // wait for modal
            // verify deisgn name and zipcode are populated
            var designName = Driver.Instance.FindElement(By.Id("create-design-model-designname")).GetAttribute("value");
            Assert.IsTrue(designName != "", "Design name is not pre-populated");

            var zipcode = Driver.Instance.FindElement(By.Id("create-design-modal-zipcode")).GetAttribute("value");
            Assert.IsTrue(zipcode != "", "Zipcode is not pre-populated");

            // click Add Design button
            var addDesignButton = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[2]"));
            addDesignButton.Click();

            // wait for summary card to update
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]")).Text != summaryDesignName0);

            // validate number of design tags
            var totalDesignTags = Driver.Instance.FindElements(By.CssSelector(".tab-effects")).Count;
            Console.WriteLine("number of tags: " + totalDesignTags);
            Assert.IsTrue(totalDesignTags > designTags, "Design was not added");
        }

        public static void CancelAddDesign()
        {
            // save number of design tags            
            var designTags = Driver.Instance.FindElements(By.CssSelector(".tab-effects")).Count;
            Console.WriteLine("number of tags: " + designTags);

            // click on Add Design button
            var addNewDesignButton = Driver.Instance.FindElement(By.XPath("//div[@id='left-panel']/div[2]/button"));
            addNewDesignButton.Click();

            // wait for modal
            // verify deisgn name and zipcode are populated
            var deisgnName = Driver.Instance.FindElement(By.Id("create-design-model-designname")).GetAttribute("value");
            Assert.IsTrue(deisgnName != "", "Design name is not pre-populated");

            var zipcode = Driver.Instance.FindElement(By.Id("create-design-modal-zipcode")).GetAttribute("value");
            Assert.IsTrue(zipcode != "", "Zipcode is not pre-populated");

            // click Add Design button
            var cancelButton = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[1]"));
            cancelButton.Click();

            // wait for summary card to update
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]")).Text == "Design 1");

            // validate number of design tags
            var totalDesignTags = Driver.Instance.FindElements(By.CssSelector(".tab-effects")).Count;
            Console.WriteLine("number of tags: " + totalDesignTags);
            Assert.IsTrue(totalDesignTags == designTags, "Design was added");
        }

        public static void SelectADesign(string designName)
        {
            /*
            // click on Add Design button
            var addNewDesignButton = Driver.Instance.FindElement(By.XPath("//div[@id='left-panel']/div[2]/button"));
            addNewDesignButton.Click();

            // click Add Design button
            var addDesignButton = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[2]"));
            addDesignButton.Click();

            // wait for summary card to update
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]")).Text == "Design 2");
            */
            
            // get all design tags list
            var totalDesignTags = Driver.Instance.FindElements(By.CssSelector(".tab-effects"));

            // find the design by design name and click on it
            var selectDesignTag = totalDesignTags[totalDesignTags.Count - 1].FindElement(By.TagName("span"));
            if (selectDesignTag.Text == designName)
            {
                Actions action = new Actions(Driver.Instance);
                action.MoveToElement(selectDesignTag);
                action.Click();
                action.Perform();
            }
            else
                Assert.IsTrue(-1 > 0, "Design not found");

            // wait for summary card to update and validate
            var wait1 = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait1.Until(d => d.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]")).Text == designName);

        }

        public static void CloseDesignByDesignTag()
        {
            // collect number of design tags
            var designTags = Driver.Instance.FindElements(By.CssSelector(".tab-effects")).Count;

            // click on close design
            var closeDesignIcon = Driver.Instance.FindElements(By.ClassName("icon_close"));
            if (closeDesignIcon.Count > 0)
            {
                Actions action = new Actions(Driver.Instance);
                action.MoveToElement(closeDesignIcon[0]);
                action.Click();
                action.Perform();
            }
            else
            {
                Assert.IsTrue(-1 > 0, "No design close icon found");
            }

            // wait to confirmation dialog
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[1]")).Displayed);

            // click on 'No' save button
            var noSaveButton = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[1]"));
            noSaveButton.Click();

            // verify number of design tags
            var totalDesignTags = Driver.Instance.FindElements(By.CssSelector(".tab-effects")).Count;
            //Console.WriteLine("number of tags: " + totalDesignTags);
            Assert.IsTrue(totalDesignTags == (designTags-1), "Design was added");
        }

        public static void CloseDesignByAction()
        {
            
            // collect number of design tags
            var designTags = Driver.Instance.FindElements(By.CssSelector(".tab-effects")).Count;

            var designActions = Driver.Instance.FindElements(By.XPath("//span[starts-with(@class,'designActions')]"));
            var closeeDesignAction = designActions[3];
            closeeDesignAction.Click();

            // wait to confirmation dialog
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[1]")).Displayed);

            // click on 'No' save button
            var noSaveButton = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[1]"));
            noSaveButton.Click();

            // verify number of design tags
            var totalDesignTags = Driver.Instance.FindElements(By.CssSelector(".tab-effects")).Count;
            //Console.WriteLine("number of tags: " + totalDesignTags);
            Assert.IsTrue(totalDesignTags == (designTags - 1), "Design was added");
        }

        public static void CopyDesign(string designName)
        {
            // click on copy design icon
            var designActions = Driver.Instance.FindElements(By.XPath("//span[starts-with(@class,'designActions')]"));
            var duplicateDesignNameAction = designActions[1];
            duplicateDesignNameAction.Click();

            // verify copy design modal is displayed
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.XPath("//div[@class='text-input-container']/input")).Displayed);

            // enter design name and click on 'Copy Design' button
            var designNameInput = Driver.Instance.FindElement(By.XPath("//div[@class='text-input-container']/input"));
            designNameInput.SendKeys(designName);

            
            //var wait1 = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[2]")).Enabled);

            // click on Coy Design button
            var copyDesignButton =
                Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[2]"));
            copyDesignButton.Click();

            // wait for copied design be active on summary card
            wait.Until(d => d.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]")).Text == designName);

            // validate copied design
            var activeDesignTag = Driver.Instance.FindElements(By.CssSelector(".tab-effects>li>div>span"));
            Assert.IsTrue(activeDesignTag[0].Text == designName, "Copy design did not match");

            // validate number of counter container
            var countertopContainer = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]"));
            Assert.IsTrue(countertopContainer.Count == 1, "Number of countertop container did not match");
        }

        public static void AddCountertop()
        {
            // collect number of countertops
            var countertopContainer = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count;

            // click on Add Another Countertop button
            var addAnotherCountertopButton = Driver.Instance.FindElement(By.XPath("//div[@class='card-actions']/button"));
            addAnotherCountertopButton.Click();

            // wait for countertop container to be increased
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count > countertopContainer);

            // verify newly added countertop name
            var totalCountertopContainer = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count;
            Assert.IsTrue(totalCountertopContainer > countertopContainer, "Countertop was not added");

            // verify newly added countertop name
            var countertopNames = Driver.Instance.FindElements(By.XPath("//h4"));
            if(countertopNames.Count >  0)
                Assert.IsTrue(countertopNames[(totalCountertopContainer-1)].Text == "Countertop 2");
            else
            {
                Assert.IsTrue(-1 > 0, "Countertop name did not match");
            }
        }

        public static void DeleteLastCountertop()
        {
            // collect number of countertops
            var countertopContainer = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count;

            // click on last 'Delete Countertop' link
            var deleteCountertopLinks = Driver.Instance.FindElements(By.CssSelector(".delete_Item.fakeLink"));
            if(deleteCountertopLinks.Count > 0)
                deleteCountertopLinks[(countertopContainer-1)].Click();
            else
                Assert.IsTrue(-1 > 0, "No Delete Countertop links were found");

            // wait for Delete Countertop button is displayed
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[2]")).Enabled);

            // click on 'Delete Countertop' button
            var deleteCountertopButton =
                Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[2]"));
            deleteCountertopButton.Click();

            // wait for number of countertops is decreased
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count < countertopContainer);
        }

        public static AddDesignCommand AddDesignNameAs(string designName)
        {
            return new AddDesignCommand(designName);
        }

        public static void SelectASurface()
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.CssSelector(".tab-effects>li>div>span")).Count > 0);
            wait.Until(d => d.FindElements(By.CssSelector(".card-content-placeholder>div>span"))[0].Enabled);

            var selectASurfaceButton =
                Driver.Instance.FindElements(By.CssSelector(".card-content-placeholder>div>span"));
            if (selectASurfaceButton.Count > 0)
                selectASurfaceButton[0].Click();                
            else
            {
                Assert.IsTrue(-1 > 0, "Select A Surface link was not found");
            }
        }

        public static void AddARoom()
        {
            var container = Driver.Instance.FindElement(By.Id("container"));
            var roomQty0 = container.FindElements(By.XPath("div/div[starts-with(@class,'card ')]")).Count; 
            var addRoomBtn = Driver.Instance.FindElement(By.CssSelector("#scrollto-bottom-of-page>button"));
            addRoomBtn.Click();
          
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card dialog')]")).Count > 0);
            var roomName = Driver.Instance.FindElement(By.CssSelector(".text-input-container>input")).GetAttribute("value");
            Assert.IsTrue(roomName != "", "Room name is not pre-populated");
            var addRoomModalBtn = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[2]"));
            addRoomModalBtn.Click();
            wait.Until(d => d.FindElements(By.XPath("//div[@id='container']//div[starts-with(@class,'card ')]")).Count > roomQty0);

            var roomQty1 = container.FindElements(By.XPath("div/div[starts-with(@class,'card ')]")).Count;
            Assert.IsTrue(roomQty1 > roomQty0, "Room did not add");
        }

        public static void CancelAddARoom()
        {
            var container = Driver.Instance.FindElement(By.Id("container"));
            var roomQty0 = container.FindElements(By.XPath("div/div[starts-with(@class,'card ')]")).Count;
            var addRoomBtn = Driver.Instance.FindElement(By.CssSelector("#scrollto-bottom-of-page>button"));
            addRoomBtn.Click();

            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card dialog')]")).Count > 0);
            var roomName = Driver.Instance.FindElement(By.CssSelector(".text-input-container>input")).GetAttribute("value");
            Assert.IsTrue(roomName != "", "Room name is not pre-populated");
            var cancelModalBtn = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[1]"));
            cancelModalBtn.Click();           

            var roomQty1 = container.FindElements(By.XPath("div/div[starts-with(@class,'card ')]")).Count;
            Assert.IsTrue(roomQty1 == roomQty0, "Room was added");
        }

        public static void DeleteARoom()
        {
            var container = Driver.Instance.FindElement(By.Id("container"));
            var roomQty0 = container.FindElements(By.XPath("div/div[starts-with(@class,'card ')]")).Count;
            var deleteRoomTrashIcon = Driver.Instance.FindElement(By.XPath("//i[@data-tip='Delete Room']"));
            deleteRoomTrashIcon.Click();

            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card dialog')]")).Count > 0);
            var deleteRoomModalBtn = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[2]"));
            deleteRoomModalBtn.Click();
            wait.Until(d => d.FindElements(By.XPath("//div[@id='container']//div[starts-with(@class,'card ')]")).Count < roomQty0);

            var roomQty1 = container.FindElements(By.XPath("div/div[starts-with(@class,'card ')]")).Count;
            Assert.IsTrue(roomQty1 < roomQty0, "Room was not deleted");
        }

        public static void CancelDeleteARoom()
        {
            var container = Driver.Instance.FindElement(By.Id("container"));
            var roomQty0 = container.FindElements(By.XPath("div/div[starts-with(@class,'card ')]")).Count;
            var deleteRoomTrashIcon = Driver.Instance.FindElement(By.XPath("//i[@data-tip='Delete Room']"));
            deleteRoomTrashIcon.Click();

            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card dialog')]")).Count > 0);
            var cancelModalBtn = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'card-actions right')]/button[1]"));
            cancelModalBtn.Click();

            var roomQty1 = container.FindElements(By.XPath("div/div[starts-with(@class,'card ')]")).Count;
            Assert.IsTrue(roomQty1 == roomQty0, "Room was deleted");
        }

        public static void ValidateAttachedDesignNameAs(string design)
        {
            // wait for Design Builder displayed and ready
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card dialog')]")).Count > 0);

            // vallidate swatch in line item
            var imageSwatch =
                Driver.Instance.FindElements(
                    By.XPath("//div[starts-with(@class,'card-content-new-item')]/div/div[1]/img"));
            Assert.IsTrue(imageSwatch.Count > 0, "Missing product swatch");

            // validate item line
            var itemDescriptions =
                Driver.Instance.FindElements(
                    By.XPath("//div[starts-with(@class,'card-content-new-item')]/div/div[3]/div/div/span"));
            Assert.IsTrue(itemDescriptions[0].Text.Contains("Total Square Footage:"));
            Assert.IsTrue(itemDescriptions[1].Text.Contains("Color:"));
            Assert.IsTrue(itemDescriptions[2].Text.Contains("Edge Style:"));

            // validate item line values
            var itemDescValues = Driver.Instance.FindElements(
                By.XPath("//div[starts-with(@class,'card-content-new-item')]/div/div[3]/div/div"));
            Assert.IsTrue(itemDescValues[0].Text.Contains("14"));
            Assert.IsTrue(itemDescValues[1].Text.Contains("Red"));
            Assert.IsTrue(itemDescValues[2].Text.Contains("Edge Style"));

            // validate menu items
            var menuItems = Driver.Instance.FindElements(By.XPath("//li[@role='menuitem']//p"));
            Console.WriteLine("number of menu items: " + menuItems.Count);
            Assert.IsTrue(menuItems.Count == 5, "Missing line item actions");

            Assert.IsTrue(menuItems[0].Text == "Configuration Details", "Missing Configuration Details action");
            Assert.IsTrue(menuItems[1].Text == "Edit Details", "Missing Edit Details action");
            Assert.IsTrue(menuItems[2].Text == "Duplicate", "Missing Duplicate action");
            Assert.IsTrue(menuItems[3].Text == "Change Product", "Missing Change Product action");
            Assert.IsTrue(menuItems[4].Text == "Delete", "Missing Delete action");

        }

        public static ValidateLineItemCommand ValidateLineItemCountertop(string countertop)
        {
            return new ValidateLineItemCommand(countertop);
        }

        public static void DeleteConfiguration()
        {
            // launch configuration details modal
            var menuItems = Driver.Instance.FindElements(By.XPath("//li[@role='menuitem']//p"));
            menuItems[4].Click();
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.XPath("//span[starts-with(@class,'card-title')]/h2")).Count > 0);

            // click on Delete Product
            var actionButtons =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-actions right')]/button"));
            wait.Until(d => actionButtons[1].Enabled);
            actionButtons[1].Click();

            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card-actions right')]")).Count == 0);
            // validate no product is attached to countertop 1
            // vallidate swatch in line item
            var imageSwatch =
                Driver.Instance.FindElements(
                    By.XPath("//div[starts-with(@class,'card-content-new-item')]/div/div[1]/img"));
            Assert.IsTrue(imageSwatch.Count == 0, "A product is still attached");

        }

        public static void CancelDeleteConfiguration()
        {
            // launch configuration details modal
            var menuItems = Driver.Instance.FindElements(By.XPath("//li[@role='menuitem']//p"));
            menuItems[4].Click();
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.XPath("//span[starts-with(@class,'card-title')]/h2")).Count > 0);

            // click on Cancel in Modal
            var actionButtons =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-actions right')]/button"));
            wait.Until(d => actionButtons[1].Enabled);
            actionButtons[0].Click();

            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card-actions right')]")).Count == 0);
            // validate product is still attached to countertop 1
            // vallidate swatch in line item
            var imageSwatch =
                Driver.Instance.FindElements(
                    By.XPath("//div[starts-with(@class,'card-content-new-item')]/div/div[1]/img"));
            Assert.IsTrue(imageSwatch.Count > 0, "No product is attached to countertop");

        }

        public static void DuplicateConfiguration()
        {
            // save number line items in design builder
            var countertopContainers =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count;

            // click on Duplicate button on menu actions
            var menuItems = Driver.Instance.FindElements(By.XPath("//li[@role='menuitem']//p"));
            var duplicateBtn = menuItems[2];
            duplicateBtn.Click();

            // wait until number of line items increases
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count > countertopContainers);           
        }

        public static void DuplicateMultipleConfiguration(int nCopy)
        {
            // save number line items in design builder
            var countertopContainers =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count;

            // menu action items
            var menuItems = Driver.Instance.FindElements(By.XPath("//li[@role='menuitem']"));

            // click on arrow on menu actions
            // click on Duplicate button on menu actions
            var arrowButton = Driver.Instance.FindElements(By.Id("chevron_up"));
            Actions action = new Actions(Driver.Instance);
            action.MoveToElement(arrowButton[0]);
            action.Click();
            action.Perform();

            // enter nCopy value
            var inputDuplicate = menuItems[2].FindElements(By.XPath("div[2]/div[1]/input"));
            var wait0 = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(2));
            wait0.Until(d => inputDuplicate[0].Enabled);
           
            action.MoveToElement(inputDuplicate[0]);
            action.Click();
            action.Perform();
            inputDuplicate[0].Clear();
            inputDuplicate[0].SendKeys(nCopy.ToString());
            
            // click on okay check mark
            var okCheckmarkBtn = menuItems[2].FindElements(By.XPath("div[2]/div[1]/button"));
            action.MoveToElement(okCheckmarkBtn[0]);
            action.Click();
            action.Perform();

            // wait for items be added to design builder
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count > countertopContainers);

            var countertopContainers1 =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]")).Count;
            Assert.IsTrue(countertopContainers1 == (countertopContainers + nCopy), "Added countertops number did not match");

        }

        public static ValidateConfigDetailsCommand ValidateConfigurationDetailsForCountertop(string countertop)
        {
            return new ValidateConfigDetailsCommand(countertop);
        }

        public static void EditCountertopConfiguration(string countertop)
        {
            // check if container is > 0
            // locate countertop
            // check menu items available
            // click on Edit Details button
            Utilities util = new Utilities();

            // validate countertop container
            var countertopNameList =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]//h4"));
            int posCountertop = util.GetItemPosition(countertopNameList, countertop);
            if (posCountertop > -1)
            {
                var countertopName = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//h4"));
                Console.WriteLine("Countertop name: " + countertopName[0].Text);
                Assert.IsTrue(countertopName[0].Text.Contains(countertop), "Countertop name does not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Falied to find " + countertop + " in line items");
            }

            // validate menu items
            var menuItems = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//li[@role='menuitem']//p"));
            Console.WriteLine("number of menu items: " + menuItems.Count);
            Assert.IsTrue(menuItems.Count == 5, "Missing line item actions");

            // launch configuration and wait for modal
            var EditDetailsBtn = menuItems[1];
            EditDetailsBtn.Click();
        }


        public static void ChangeProduct(string countertop)
        {
            // check if container is > 0
            // locate countertop
            // check menu items available
            // click on Change Product button
            Utilities util = new Utilities();

            // validate countertop container
            var countertopNameList =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]//h4"));
            int posCountertop = util.GetItemPosition(countertopNameList, countertop);
            if (posCountertop > -1)
            {
                var countertopName = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//h4"));
                Console.WriteLine("Countertop name: " + countertopName[0].Text);
                Assert.IsTrue(countertopName[0].Text.Contains(countertop), "Countertop name does not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Falied to find " + countertop + " in line items");
            }

            // validate menu items
            var menuItems = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//li[@role='menuitem']//p"));
            Console.WriteLine("number of menu items: " + menuItems.Count);
            Assert.IsTrue(menuItems.Count == 5, "Missing line item actions");

            // launch configuration and wait for modal
            var changeProductBtn = menuItems[3];
            changeProductBtn.Click();
        }
    }

    public class ValidateConfigDetailsCommand
    {
        private readonly string countertop;
        private string prodName;
        private string prodPrice;
        private string thickness;
        private string color;
        private int quarterRoundQty = 0;
        private int oneInchRoundQty = 0;
        private int threeInchRoundQty = 0;
        private int intAnglesQty = 0;
        private string edgeStyle;
        private int totalSquareFootage;
        private string totalPrice;
        private static Utilities util = new Utilities();


        public ValidateConfigDetailsCommand(string countertop)
        {
            this.countertop = countertop;
        }

        public ValidateConfigDetailsCommand WithProductNameAs(string prodName)
        {
            this.prodName = prodName;
            return this;
        }

        public ValidateConfigDetailsCommand WithProductPriceAs(string prodPrice)
        {
            this.prodPrice = prodPrice;
            return this;
        }

        public ValidateConfigDetailsCommand WithSurfaceThicknessAs(string thickness)
        {
            this.thickness = thickness;
            return this;
        }

        public ValidateConfigDetailsCommand WithColorAs(string color)
        {
            this.color = color;
            return this;
        }

        public ValidateConfigDetailsCommand WithQuarterRoundAs(int quarterRoundQty)
        {
            this.quarterRoundQty = quarterRoundQty;
            return this;
        }

        public ValidateConfigDetailsCommand WithOneInchRoundAs(int oneInchRoundQty)
        {
            this.oneInchRoundQty = oneInchRoundQty;
            return this;
        }

        public ValidateConfigDetailsCommand WithThreeInchRoundAs(int threeInchRoundQty)
        {
            this.threeInchRoundQty = threeInchRoundQty;
            return this;
        }

        public ValidateConfigDetailsCommand WithInteriorAngles(int intAnglesQty)
        {
            this.intAnglesQty = intAnglesQty;
            return this;
        }

        public ValidateConfigDetailsCommand WithEdgeStyleAs(string edgeStyle)
        {
            this.edgeStyle = edgeStyle;
            return this;
        }

        public ValidateConfigDetailsCommand WithTotalSquareFootageAs(int totalSquareFootage)
        {
            this.totalSquareFootage = totalSquareFootage;
            return this;
        }

        public ValidateConfigDetailsCommand WithTotalPriceAs(string totalPrice)
        {
            this.totalPrice = totalPrice;
            return this;
        }

        public void ValidateConfigDetails()
        {
            // validate countertop container
            var countertopNameList =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]//h4"));
            int posCountertop = util.GetItemPosition(countertopNameList, countertop);
            if (posCountertop > -1)
            {
                var countertopName = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//h4"));
                Console.WriteLine("Countertop name: " + countertopName[0].Text);
                Assert.IsTrue(countertopName[0].Text.Contains(countertop), "Countertop name does not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Falied to find " + countertop + " in line items");
            }

            // validate menu items
            var menuItems = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//li[@role='menuitem']//p"));
            Console.WriteLine("number of menu items: " + menuItems.Count);
            Assert.IsTrue(menuItems.Count == 5, "Missing line item actions");

            // launch configuration and wait for modal
            var configurationDetailsBtn = menuItems[0];
            configurationDetailsBtn.Click();
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.XPath("//span[@class='card-title']/h2")).Count > 0);

            //Driver.Wait(TimeSpan.FromMilliseconds(60));

            // validate section headers
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card configurationDialog')]//h2")).Count == 3);
            var sectionHeaders = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card configurationDialog')]//h2"));
            Console.WriteLine("number of section headers: " + sectionHeaders.Count);

            // validate configuration deatils title and section headers
            Assert.IsTrue(sectionHeaders[0].Text == "Configuration Details", "Missing Configuration Details title");
            Assert.IsTrue(sectionHeaders[1].Text == "Product", "Missing Product header section");
            Assert.IsTrue(sectionHeaders[2].Text == "Product Details", "Missing Product Details header section");

            // collect all tables in configuration details
            var tableData = Driver.Instance.FindElements(By.TagName("table"));
            Assert.IsTrue(tableData.Count == 3, "Expected table data does not match");

            // validate product data section
            var productData = tableData[0].FindElements(By.XPath("tbody//td"));
           
            // validate product name
            Console.WriteLine("product name in report: " + productData[1].Text);
            Assert.IsTrue(productData[1].Text.Contains(prodName), "Product name did not match");
            // validate product price
            Assert.IsTrue(productData[2].Text.Contains(prodPrice), "Product price did not match");

            // validate product details data section
            var optionNames = tableData[1].FindElements(By.XPath("tbody/tr/td[1]"));

            // validate surface thickness
            int posThickness = util.GetItemPosition(optionNames, "Select a Surface Thickness");
            if (posThickness > -1)
            {
                var thicknessData = tableData[1].FindElements(By.XPath("tbody/tr[" + (posThickness + 1) + "]/td"));
                Assert.IsTrue(thicknessData[1].Text == thickness, "Surface thickness did not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Could not find surface thickness value in configuration details report");
            }

            // validate color
            int posColor = util.GetItemPosition(optionNames, "Color");
            if (posColor > -1)
            {
                var colorData = tableData[1].FindElements(By.XPath("tbody/tr[" + (posColor + 1) + "]/td"));
                Assert.IsTrue(colorData[1].Text == color, "Color did not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Could not find surface thickness value in configuration details report");
            }

            // validate quarterRoundCorner if quantity > 0
            if (quarterRoundQty > 0)
            {
                int posQuarterRound = util.GetItemPosition(optionNames, "¼\" Rounded");
                if (posQuarterRound > -1)
                {
                    var quarterRoundData = tableData[1].FindElements(By.XPath("tbody/tr[" + (posQuarterRound + 1) + "]/td"));
                    Assert.IsTrue(quarterRoundData[1].Text == quarterRoundQty.ToString(), "¼\" Rounded Corners value did not match");
                }
                else
                {
                    Assert.IsTrue(-1 > 0, "Could not find ¼\" Rounded Corners value in configuration details report");
                }
            }

            // validate oneInchRoundCorner if quantity > 0
            if (oneInchRoundQty > 0)
            {
                int posOneInchRound = util.GetItemPosition(optionNames, "1\" Rounded");
                if (posOneInchRound > -1)
                {
                    var oneInchRoundData = tableData[1].FindElements(By.XPath("tbody/tr[" + (posOneInchRound + 1) + "]/td"));
                    Assert.IsTrue(oneInchRoundData[1].Text == oneInchRoundQty.ToString(), "1\" Rounded Corners value did not match");
                }
                else
                {
                    Assert.IsTrue(-1 > 0, "Could not find 1\" Rounded Corners value in configuration details report");
                }
            }

            // validate threeInchRoundCorner if quantity > 0
            if (threeInchRoundQty > 0)
            {
                int posThreeInchRound = util.GetItemPosition(optionNames, "3\" Rounded");
                if (posThreeInchRound > -1)
                {
                    var threeInchRoundData = tableData[1].FindElements(By.XPath("tbody/tr[" + (posThreeInchRound + 1) + "]/td"));
                    Assert.IsTrue(threeInchRoundData[1].Text == threeInchRoundQty.ToString(), "3\" Rounded Corners value did not match");
                }
                else
                {
                    Assert.IsTrue(-1 > 0, "Could not find 3\" Rounded Corners value in configuration details report");
                }
            }

            // validate interior angles if quantity > 0
            if (intAnglesQty > 0)
            {
                int posInteriorAngles = util.GetItemPosition(optionNames, "Interior Angles");
                if (posInteriorAngles > -1)
                {
                    var intriorAnglesData = tableData[1].FindElements(By.XPath("tbody/tr[" + (posInteriorAngles + 1) + "]/td"));
                    Assert.IsTrue(intriorAnglesData[1].Text == intAnglesQty.ToString(), "Interior Angles value did not match");
                }
                else
                {
                    Assert.IsTrue(-1 > 0, "Could not find Interior Angles value in configuration details report");
                }
            }

            // validate edge style
            int posEdgeStyle = util.GetItemPosition(optionNames, "Edge Style");
            if (posEdgeStyle > -1)
            {
                var edgeStyleData = tableData[1].FindElements(By.XPath("tbody/tr[" + (posEdgeStyle + 1) + "]/td"));
                Assert.IsTrue(edgeStyleData[1].Text == edgeStyle, "Edge Style value did not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Could not Edge Style value in configuration details report");
            }

            // validate total square footage
            int posTotalSquareFootage = util.GetItemPosition(optionNames, "Total Square Footage");
            if (posTotalSquareFootage > -1)
            {
                var totalSquareFootageData = tableData[1].FindElements(By.XPath("tbody/tr[" + (posTotalSquareFootage + 1) + "]/td"));
                Assert.IsTrue(totalSquareFootageData[1].Text == totalSquareFootage.ToString(), "Total Square Footage value did not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Could not find Total Square Footage value in configuration details report");
            }

            // validate total data section
            var totalData = tableData[2].FindElements(By.XPath("tbody/tr/td[1]"));

            // validate total price
            int posTotalPrice = util.GetItemPosition(totalData, "Total");
            if (posTotalPrice > -1)
            {
                var totalPriceData = tableData[2].FindElements(By.XPath("tbody/tr[" + (posTotalPrice + 1) + "]/td"));
                Assert.IsTrue(totalPriceData[2].Text.Contains(totalPrice), "Total price value did not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Could not find Total price value in configuration details report");
            }

            // close configuration details modal
            var closeModal = Driver.Instance.FindElement(By.XPath("//i[starts-with(@class,'icon_close')]"));
            Actions action = new Actions(Driver.Instance);
            action.MoveToElement(closeModal);
            action.Click();
            action.Perform();
            
        }
    }

    public class ValidateLineItemCommand
    {
        private readonly string countertop;
        private int squareFeet;
        private string color;
        private string edgeStyle;
        private string productName;
        private static Utilities util = new Utilities();

        public ValidateLineItemCommand(string countertop)
        {
            this.countertop = countertop;
        }

        public ValidateLineItemCommand WithProductAs(string productName)
        {
            this.productName = productName;
            return this;
        }

        public ValidateLineItemCommand WithTotalSuqareFootageAs(int squareFeet)
        {
            this.squareFeet = squareFeet;
            return this;
        }

        public ValidateLineItemCommand WithColorAs(string color)
        {
            this.color = color;
            return this;
        }

        public ValidateLineItemCommand WithEdgeStyleAs(string edgeStyle)
        {
            this.edgeStyle = edgeStyle;
            return this;
        }

        public void ValidateLineItem()
        {
            // wait for Design Builder displayed and ready
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElements(By.CssSelector(".progress-linear.interminate")).Count == 0);
            wait.Until(d => d.FindElements(By.XPath("//div[starts-with(@class,'card-content-new-item')]/div/div[1]/img")).Count > 0);

            // validate countertop container
            var countertopNameList =
                Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'card-content-toolbar')]//h4"));
            int posCountertop = util.GetItemPosition(countertopNameList, countertop);
            if (posCountertop > -1)
            {
                var countertopName = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//h4"));
                Console.WriteLine("Countertop name: " + countertopName[0].Text);
                Assert.IsTrue(countertopName[0].Text.Contains(countertop), "Countertop name does not match");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Falied to find " + countertop + " in line items");
            }

            // validate product name
            var prodName = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//div[starts-with(@class,'card-content-new-item')]/div/div[2]/span[2]"));
            Assert.IsTrue(prodName[0].Text == productName, "Porduct name does not match");

            // vallidate swatch in line item
            var imageSwatch = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//div[starts-with(@class,'card-content-new-item')]/div/div[1]/img"));
            Assert.IsTrue(imageSwatch.Count > 0, "Missing product swatch");

            // validate item line
            var itemDescriptions = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//div[starts-with(@class,'card-content-new-item')]/div/div[3]/div/div/span"));
            Assert.IsTrue(itemDescriptions[0].Text.Contains("Total Square Footage:"));
            Assert.IsTrue(itemDescriptions[1].Text.Contains("Color:"));
            Assert.IsTrue(itemDescriptions[2].Text.Contains("Edge Style:"));

            // validate item line values
            var itemDescValues = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//div[starts-with(@class,'card-content-new-item')]/div/div[3]/div/div"));
            Assert.IsTrue(itemDescValues[0].Text.Contains(squareFeet.ToString()), "Countertop square footage value does not match");
            Assert.IsTrue(itemDescValues[1].Text.Contains(color), "Countertop color does not match");
            Assert.IsTrue(itemDescValues[2].Text.Contains(edgeStyle), "Countertop edge style doe snot match");

            // validate menu items
            var menuItems = Driver.Instance.FindElements(By.XPath("//div[@class='card-content']/div[" + (posCountertop + 1) + "]//li[@role='menuitem']//p"));
            Console.WriteLine("number of menu items: " + menuItems.Count);
            Assert.IsTrue(menuItems.Count == 5, "Missing line item actions");

            Assert.IsTrue(menuItems[0].Text == "Configuration Details", "Missing Configuration Details action");
            Assert.IsTrue(menuItems[1].Text == "Edit Details", "Missing Edit Details action");
            Assert.IsTrue(menuItems[2].Text == "Duplicate", "Missing Duplicate action");
            Assert.IsTrue(menuItems[3].Text == "Change Product", "Missing Change Product action");
            Assert.IsTrue(menuItems[4].Text == "Delete", "Missing Delete action");
        }

        
    }

    public class ValidateDesignCommand
    {
        private readonly string designName;
        private string zipcode;

        public ValidateDesignCommand(string designName)
        {
            this.designName = designName;
        }

        public ValidateDesignCommand WithZipcodeAs(string zipcode)
        {
            this.zipcode = zipcode;
            return this;
        }

        public void ValidateDesign()
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            //wait.Until(d => d.FindElements(By.ClassName("jss138")).Count > 0); -- this is not consistent when run test on different browser
            wait.Until(d => d.FindElements(By.CssSelector(".tab-effects>li>div")).Count > 0);
            Console.WriteLine("number of rows: " + Driver.Instance.FindElements(By.ClassName("jss138")).Count);

            //left-panel
            var tagDesignName = Driver.Instance.FindElements(By.CssSelector(".tab-effects>li>div>span"));
            Assert.IsTrue(tagDesignName[0].Text == designName, "Design name '" + designName + "' did not match");

            var containerTtitle = Driver.Instance.FindElement(By.Id("Room 1"));
            Assert.IsTrue(containerTtitle.Text == "Room 1", "Container title 'Room 1' did not match");

            var editCountertopIcon = Driver.Instance.FindElement(By.XPath("//div[starts-with(@class,'design-items')]//i"));
            Assert.IsTrue(editCountertopIcon.Displayed, "Missing edit 'Room 1' icon");

            //var containerName = Driver.Instance.FindElement(By.XPath("//h4[contains(@class,'countertops-empty-item-name')]"));
            //Assert.IsTrue(containerName.Text == "Countertop 1", "Container name 'Countertop 1' did not match");

            var chooseACountertopLink = Driver.Instance.FindElement(By.CssSelector(".card-content-placeholder>div>span>button"));
            Assert.IsTrue(chooseACountertopLink.Text == "Choose A Countertop", "Missing Choose A Countertop link");

            var addAnotherCountertopButton = Driver.Instance.FindElement(By.CssSelector(".card-actions>button"));
            Assert.IsTrue(addAnotherCountertopButton.Text == "Add Countertop", "Missing Add Countertop button");

            var addNewDesignButton = Driver.Instance.FindElement(By.XPath("//div[@id='left-panel']/div[2]/button"));
            Assert.IsTrue(addNewDesignButton.Text == "Add New Design", "Missing Add New Design button");

            //right-panel(summary card)
            var summaryDesignName = Driver.Instance.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]"));
            Assert.IsTrue(summaryDesignName.Text == designName, "Design name '" + designName + "' did not match");

            var summaryZipcode = Driver.Instance.FindElements(By.XPath("//div[@id='right-panel']//header//dd"));
            if(summaryZipcode.Count > 0)
                Assert.IsTrue(summaryZipcode[1].Text == zipcode, "Zipcode did not match");
        }

    }

    public class AddDesignCommand
    {
        private readonly string designName;
        private string zipcode;

        public AddDesignCommand(string designName)
        {
            this.designName = designName;
        }

        public AddDesignCommand WithZipcodeAs(string zipcode)
        {
            this.zipcode = zipcode;
            return this;
        }

        public void AddDesign()
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.XPath("//div[@id='left-panel']//button")).Enabled);

            var addNewDeisgnButton = Driver.Instance.FindElement(By.XPath("//div[@id='left-panel']//button"));
            Actions action = new Actions(Driver.Instance);
            action.MoveToElement(addNewDeisgnButton);
            action.Click();
            action.Perform();
            //addNewDeisgnButton.Click();

            wait.Until(d => d.FindElement(By.Id("create-design-model-designname")).Displayed);

            var designNameInput = Driver.Instance.FindElement(By.Id("create-design-model-designname"));
            designNameInput.Clear();
            designNameInput.SendKeys(designName);

            var propertyZipcodeInput = Driver.Instance.FindElement(By.Id("create-design-modal-zipcode"));
            propertyZipcodeInput.Clear();
            propertyZipcodeInput.SendKeys(zipcode);

            wait.Until(d => d.FindElement(By.XPath("//div[@class='card-actions right']/button[2]")).Enabled);

            var addDesignButton = Driver.Instance.FindElement(By.XPath("//div[@class='card-actions right']/button[2]"));
            addDesignButton.Click();

            // wait until summary card design name is updated
            wait.Until(d => d.FindElement(By.XPath("//h2[contains(@class,'custom-design-name')]")).Text == designName);
        }       
    }

    public class CreateDefaultDesignCommand
    {

        private readonly string zipcode;

        public CreateDefaultDesignCommand(string zipcode)
        {
            this.zipcode = zipcode;
        }

        public void CreateDesign()
        {
            var propertyZipcodeInput = Driver.Instance.FindElement(By.Id("create-design-modal-zipcode"));
            propertyZipcodeInput.SendKeys(zipcode);

            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.CssSelector(".card-actions.right>button")).Enabled);

            var addDesignButton = Driver.Instance.FindElement(By.CssSelector(".card-actions.right>button"));
            addDesignButton.Click();
        }
    }
}

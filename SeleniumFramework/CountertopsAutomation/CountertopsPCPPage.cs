using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuidedSellerAutomation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SeleniumFramework.CountertopsAutomation
{
    public class CountertopsPCPPage
    {
        private static Utilities util = new Utilities();
        private static CountertopsValidationMessages valMsg = new CountertopsValidationMessages();

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
                var bcrumbs =
                    Driver.Instance.FindElements(
                        By.XPath("//div[starts-with(@class,'Breadcrumbs')]/*[starts-with(@class,'breadcrumb')]/span"));
                if (bcrumbs.Count > 0)
                {
                    // validate product name in breadcrumbs
                    var prodCrumbPos = bcrumbs.Count - 1;
                    //Console.WriteLine("prod crumb: " + bcrumbs[prodCrumbPos].Text);
                    Assert.IsTrue(bcrumbs[prodCrumbPos].Text == prodName, "Product name was missing in breadcrumbs");
                    var activebcrumb = Driver.Instance.FindElements(
                        By.XPath("//div[starts-with(@class,'Breadcrumbs')]/span[contains(@class,'active')]"));
                    if (activebcrumb.Count > 0)
                    {
                        Assert.IsTrue(activebcrumb[0].Text == "Configure Product", "Active breadcrumb did not match.");
                    }
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

        public static void ValidateConfiguratorUI(string prodName)
        {
            // configurator does not display correctly for chrome, works fine for IE?
            // workaround is refreshing the page
            // Driver.Instance.Navigate().Refresh();
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElement(By.XPath("//div[@class='config-product-name']/span[@id='brand']")).Displayed);

            // validate breadcrumbs
            ValidateBreadcrumbs(prodName);
         
            // check for measurement tool option label
            string cmtLabel = "Countertop Measurement Tool";
            var cmtLabelDisplayed = Driver.Instance.FindElement(By.CssSelector("#extras>h2"));
            Assert.IsTrue(cmtLabel == cmtLabelDisplayed.Text, "Missing Countertop Measurement Tool option");                        

            //      check for shape tabs            
            var shapeTabs = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'styles__Slider')]/div"));
            //Console.WriteLine("number of shape tabs: " + shapeTabs.Count);
            if (shapeTabs.Count < 1)
            {
                Assert.IsTrue(-1 > 0, "Missing Shape Tags");
            }

            //      check for Add Shape button
            var addAShapeBtn = Driver.Instance.FindElement(By.XPath("//div[@id='extras']/div[2]/button"));
            Assert.IsTrue(addAShapeBtn.Text == "Add A Shape", "Add A Shape button was not found");

            //      check for SIDEs, Length, Wall, Backslpah, Backsplash Height

            
            // check for drawing shape in canvas
            //      check for shapes
            var shapes = Driver.Instance.FindElements(By.CssSelector("#canvasGroup>g"));
            Assert.IsTrue(shapes.Count > 0, "No shape was displayed in canvas");

            //      check for rotate left and rotate right buttons
            var rotateButtons = Driver.Instance.FindElements(By.CssSelector("#buttonStrip>button"));
            Assert.IsTrue(rotateButtons[0].Text == "Rotate Left", "Missing Rotate Left button in canvas");
            Assert.IsTrue(rotateButtons[1].Text == "Rotate Right", "Missing Rotate Right button in canvas");

            // check for choose a surface (surface thickness) option label
            string casLabel = "Choose A Surface";
            var casLabelDisplayed = Driver.Instance.FindElement(By.CssSelector("#ChooseASurface>h2"));
            Assert.IsTrue(casLabel == casLabelDisplayed.Text, "Missing Choose A Surface option");

            //      check for thickness choices list
            var thicknessChoices = Driver.Instance.FindElements(By.XPath("//input[@type='radio']"));
            Assert.IsTrue(thicknessChoices.Count > 1, "Missing surface thickness choices");

            // check for select color option label
            string scLabel = "Select Color";
            var scLabelDisplayed = Driver.Instance.FindElement(By.XPath("//div[@id='ChooseASurface']/div//div[@class='card-content']/h2"));
            //var scLabelExist = CheckOptionLabelInList(scLabel, optionLabels);
            Assert.IsTrue(scLabelDisplayed.Text == scLabel, "Missing Select Color option");

            //      check for color choices list
            var colorNamesList = Driver.Instance.FindElements(By.CssSelector(".color-card-footer"));
            Assert.IsTrue(colorNamesList.Count > 0, "Missing color choices");

            // check for finshed corners & edges option label
            string fcneLabel = "Finished Corners & Edges";
            var fcneLabelDisplayed = Driver.Instance.FindElement(By.CssSelector("#FinishedCornersEdges>h2"));
            Assert.IsTrue(fcneLabelDisplayed.Text == fcneLabel, "Missing Finished Corners & Edges option");

            //      check for external corner choices
            string baseFCNE = "//div[@id='FinishedCornersEdges']/div/";
            string corners = baseFCNE + "div[1]//div[starts-with(@class,'ChoiceBoxSection')][contains(@title,'Rounded Corners')]";
            var finishedCorners = Driver.Instance.FindElements(By.XPath(corners));
            Assert.IsTrue(finishedCorners.Count > 0, "Missing Finished Corners choices");

            //      check for internal angle choice
            string intAngles = baseFCNE + "div[2]//div[starts-with(@class,'ChoiceBoxSection')][(@title='Interior Angles')]";
            var interiorAngles = Driver.Instance.FindElements(By.XPath(intAngles));
            Assert.IsTrue(interiorAngles.Count > 0, "Missing Interior Angles choices");
            
            // check for choose edge style option label
            string cesLabel = "Choose Edge Style";
            var cedLabelDisplayed =
                Driver.Instance.FindElement(
                    By.XPath("//div[contains(@class,'configurator-card-content')]/div[4]/div/h2"));
            Assert.IsTrue(cedLabelDisplayed.Text == cesLabel, "Missing Choose Edge Style option");

            //      check for edge style choice
            var edgeStyleChoices = Driver.Instance.FindElements(By.XPath("//div[contains(@class,'configurator-card-content')]/div[4]//div[@class='null-template-choice']"));
            Assert.IsTrue(edgeStyleChoices.Count > 0, "Missing edge style choices");

            // check for summary card
            //      check for brand and product name
            //      brand name sometimes return empty????
            string brandText = "Granite-R-Us";
            wait.Until(d => d.FindElements(By.XPath("//div[@class='config-product-name']/span[@id='brand']")).Count > 0);
            var brand = Driver.Instance.FindElement(By.XPath("//div[@class='config-product-name']/span[@id='brand']"));
            Console.WriteLine("Brand: " + brand.Text);
            Assert.IsTrue(brand.Text == brandText, "Brand did not match");

            string productNameText = "Granite";
            wait.Until(d => d.FindElements(By.Id("productName")).Count > 0);
            var productName = Driver.Instance.FindElement(By.Id("productName"));
            Assert.IsTrue(productName.Text == productNameText, "Product name did not match");

            //      check for price
            var price = Driver.Instance.FindElements(By.Id("config-original-price"));
            Assert.IsTrue(price.Count > 0, "Pricing is missing");

            //      check for Add To Design button
            var addToDesignBtn = Driver.Instance.FindElements(By.CssSelector(".button.primary"));
            Assert.IsTrue(addToDesignBtn.Count > 0, "Add To Design button is missing");

            //      check for options labels:
            //          Total Surface Area, Select a Surface Thickness, Color, Finished Corners, 1/4" Rounded, 1" Rounded, 3" Rounded,
            //          Interior Angles, Edge Style, Total Finished Edge Length, Total Square Footage, Shape (name)
            var optionSummaryList = Driver.Instance.FindElements(By.XPath("//td[@class='config-summary-name']/span"));
            string tsaLabel = "Total Surface Area";
            string sastLabel = "Select a Surface Thickness";
            string colorLabel = "Color";
            string finishedCornersLabel = "Finished Corners";
            string quarterRoundedLabel = "¼\" Rounded";
            string oneInchRoundedLabel = "1\" Rounded";
            string threeInchRoundedLabel = "3\" Rounded";
            string interiorAnglesLabel = "Interior Angles";
            string edgeStyleLabel = "Edge Style";
            string tfelLabel = "Total Finished Edge Length";
            string tsfLabel = "Total Square Footage";

            bool isTotalSurfaceAreaLabelFound = CheckOptionLabelInList(tsaLabel, optionSummaryList);
            Assert.IsTrue(isTotalSurfaceAreaLabelFound, "Missing Total Surface Area in summary card");

            bool isSelectASurfaceThicknessLabelFound = CheckOptionLabelInList(sastLabel, optionSummaryList);
            Assert.IsTrue(isSelectASurfaceThicknessLabelFound, "Missing Select a Surface Thickness in summary card");

            bool isColorLabelFound = CheckOptionLabelInList(colorLabel, optionSummaryList);
            Assert.IsTrue(isColorLabelFound, "Missing Color in summary card");

            bool isFinishedCornersLabelFound = CheckOptionLabelInList(finishedCornersLabel, optionSummaryList);
            Assert.IsTrue(isFinishedCornersLabelFound, "Missing Finished Corners in summary card");

            bool isquarterRoundedLabelFound = CheckOptionLabelInList(quarterRoundedLabel, optionSummaryList);
            Assert.IsTrue(isquarterRoundedLabelFound, "Missing ¼\" Rounded in summary card");

            bool isOneInchRoundedLabelFound = CheckOptionLabelInList(oneInchRoundedLabel, optionSummaryList);
            Assert.IsTrue(isOneInchRoundedLabelFound, "Missing 1\" Rounded in summary card");

            bool isThreeInchRoundedLabelFound = CheckOptionLabelInList(threeInchRoundedLabel, optionSummaryList);
            Assert.IsTrue(isThreeInchRoundedLabelFound, "Missing 3\" Rounded in summary card");

            bool isInteriorAnglesLabelFound = CheckOptionLabelInList(interiorAnglesLabel, optionSummaryList);
            Assert.IsTrue(isInteriorAnglesLabelFound, "Missing Interior Angles in summary card");

            bool isEdgeStyleLabelFound = CheckOptionLabelInList(edgeStyleLabel, optionSummaryList);
            Assert.IsTrue(isEdgeStyleLabelFound, "Missing EdgeS Style in summary card");

            bool isTotalFinishedEdgeLengthLabelFound = CheckOptionLabelInList(tfelLabel, optionSummaryList);
            Assert.IsTrue(isTotalFinishedEdgeLengthLabelFound, "Missing Total Finished Edge Length in summary card");

            bool isTotalSquareFootageLabelFound = CheckOptionLabelInList(tsfLabel, optionSummaryList);
            Assert.IsTrue(isTotalSquareFootageLabelFound, "Missing Total Square Footage in summary card");
        }

        public static bool CheckOptionLabelInList(string label, IList<IWebElement> sList)
        {
            bool isFound = false;
            var nItems = sList.Count;
            for (int i = 0; i < nItems; i++)
            {
                if (sList[i].Text == label)
                {
                    isFound = true;
                    break;
                }
            }
            return isFound;
        }

        public static void ChangeDefaultShapeTo(string shape)
        {
            // verify default shape style is Galley
            var shapeCountertopLayout = Driver.Instance.FindElement(By.XPath("//div[@class='option-group headless']"));
            var selectedShape =
                shapeCountertopLayout.FindElement(By.XPath("div/div//div[starts-with(@class,'selected ')]/span"));
            Console.WriteLine("selected shape name: " + selectedShape.Text);

            // collect all shape styles - there is more than I expected
            var shapeStyleList = shapeCountertopLayout.FindElements(By.XPath("div/div/div[2]/div/span"));
            Console.WriteLine("number of shape types: " + shapeStyleList.Count);

            int posShape = util.GetItemPosition(shapeStyleList, shape);
            if (posShape > -1)
            {
                var shapeBtn = shapeCountertopLayout.FindElement(By.XPath("div/div/div[2]/div[" + (posShape + 1) + "]/span"));
                shapeBtn.Click();

                var selectedShape1 =
                    shapeCountertopLayout.FindElement(By.XPath("div/div//div[starts-with(@class,'selected ')]/span"));
                Console.WriteLine("selected shape name: " + selectedShape1.Text);
                Assert.IsTrue(selectedShape1.Text == shape);
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Could not find " + shape + " to select");
            }
        }

        public static CreateCountertopConfigurationCommand CreateCountertopWithSurface(string thickness)
        {
            return new CreateCountertopConfigurationCommand(thickness);
        }

        public static UpdateConfigurationCommand UpdateConfigurationWithSurfaceAs(string thickness)
        {
            return new UpdateConfigurationCommand(thickness);
        }

        public static void AddShapes(int addShapeQty)
        {
            // wait for configurator is ready
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElement(By.XPath("//div[@class='config-product-name']/span[@id='brand']")).Displayed);

            // save number of shape tabs and in canvas     
            var shapeTabs0 = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'styles__Slider')]/div"));
            var shapeInCanvas0 = Driver.Instance.FindElements(By.CssSelector("#canvasGroup>g"));

            // add shapes based on parameter
            var addAShapeBtn = Driver.Instance.FindElement(By.XPath("//div[@id='extras']/div[2]/button"));
            if (addShapeQty > 0)
            {
                for (int i=0; i < addShapeQty; i++)
                {
                    addAShapeBtn.Click();
                    Driver.Wait(TimeSpan.FromMilliseconds(10));
                }
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Add shapes quantity must be greater than 0");
            }

            // validate total shapes tabs and shapes in canvas
            var shapeTabs1 = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'styles__Slider')]/div"));
            Assert.IsTrue((shapeTabs0.Count + addShapeQty) == shapeTabs1.Count, "Added Shape tabs did not match");
            var shapeInCanvas1 = Driver.Instance.FindElements(By.CssSelector("#canvasGroup>g"));
            Assert.IsTrue((shapeInCanvas0.Count + addShapeQty) == shapeInCanvas1.Count, "Added Shape in canvas did not match");
        }

        public static void SelectShapeByTab(string shape)
        {
            // collect all tabs and find tab name by shape
            var shapeTabs = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'styles__Slider')]/div"));
            Console.WriteLine("number of shape tabs: " + shapeTabs.Count);
            int posShapeTab = util.GetItemPosition(shapeTabs, shape);

            if (posShapeTab > -1)
            {
                // click on shape tab name
                var shapeTabBtn = Driver.Instance.FindElements(By.XPath("//div[starts-with(@class,'styles__Slider')]/div[" + (posShapeTab + 1) + "]//span"));
                shapeTabBtn[0].Click();

                // validate selected shape object in canvas
                var selectedShapeObjectName =
                    Driver.Instance.FindElement(
                        By.CssSelector(".selectedShape.shapeGroup>text"));
                Assert.IsTrue(selectedShapeObjectName.Text == shape, "Active shape object did not match");
                
                var selectedShapeObjectColor = Driver.Instance.FindElement(
                    By.CssSelector(".selectedShape.shapeGroup>path")).GetAttribute("fill");
                //Console.WriteLine("Color fill data: " + selectedShapeObjectColor); 
                Assert.IsTrue(selectedShapeObjectColor == "#f96303", "Active shape did not have the right color");
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Did not find shape");
            }
        }

        public static RotateLeftCommand RotateToLeft(string shape)
        {
            return new RotateLeftCommand(shape);
        }

        public static RotateRightCommand RotateToRight(string shape)
        {
            return new RotateRightCommand(shape);
        }
    }

    public class RotateRightCommand
    {
        private readonly string shape;

        public RotateRightCommand(string shape)
        {
            this.shape = shape;
        }

        public void ValidateOrientation(int degree)
        {
            // wait for configurator is ready
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElement(By.XPath("//div[@class='config-product-name']/span[@id='brand']")).Displayed);

            // validate selected shape object in canvas
            var selectedShapeObjectName =
                Driver.Instance.FindElement(
                    By.CssSelector(".selectedShape.shapeGroup>text"));
            Assert.IsTrue(selectedShapeObjectName.Text == shape, "Active shape object did not match");

            // collect original orientation of shape
            var selectedShapeObjectOrientation0 = Driver.Instance.FindElement(
                By.CssSelector(".selectedShape.shapeGroup")).GetAttribute("transform");
            Console.WriteLine("Shape orientation: " + selectedShapeObjectOrientation0);

            // click on Rorate Left button
            var rotateBtns = Driver.Instance.FindElements(By.CssSelector("#buttonStrip>button"));
            var rotateRightBtn = rotateBtns[1];
            rotateRightBtn.Click();

            Driver.Wait(TimeSpan.FromMilliseconds(10));

            // collect new orientation of shape
            var selectedShapeObjectOrientation1 = Driver.Instance.FindElement(
                By.CssSelector(".selectedShape.shapeGroup")).GetAttribute("transform");
            Console.WriteLine("Shape orientation: " + selectedShapeObjectOrientation1);

            // get shape degree
            var shapeDegrees = selectedShapeObjectOrientation1.Split(' ');
            Console.WriteLine("shape degree: " + shapeDegrees[0]);

            // validate coordinates of shape before and after rotate
            Assert.IsTrue(selectedShapeObjectOrientation0 != selectedShapeObjectOrientation1, "Shape did not rotate");
            Assert.IsTrue(shapeDegrees[0].Contains(degree.ToString()), "Shape did not rotate 45 degree to the right");
        }
    }

    public class RotateLeftCommand
    {
        private readonly string shape;

        public RotateLeftCommand(string shape)
        {
            this.shape = shape;
        }

        public void ValidateOrientation(int degree)
        {
            // wait for configurator is ready
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElement(By.XPath("//div[@class='config-product-name']/span[@id='brand']")).Displayed);

            // validate selected shape object in canvas
            var selectedShapeObjectName =
                Driver.Instance.FindElement(
                    By.CssSelector(".selectedShape.shapeGroup>text"));
            Assert.IsTrue(selectedShapeObjectName.Text == shape, "Active shape object did not match");

            // collect original orientation of shape
            var selectedShapeObjectOrientation0 = Driver.Instance.FindElement(
                By.CssSelector(".selectedShape.shapeGroup")).GetAttribute("transform");
            Console.WriteLine("Shape orientation: " + selectedShapeObjectOrientation0);

            // click on Rorate Left button
            var rotateBtns = Driver.Instance.FindElements(By.CssSelector("#buttonStrip>button"));
            var rotateLeftBtn = rotateBtns[0];
            rotateLeftBtn.Click();

            Driver.Wait(TimeSpan.FromMilliseconds(10));

            // collect new orientation of shape
            var selectedShapeObjectOrientation1 = Driver.Instance.FindElement(
                By.CssSelector(".selectedShape.shapeGroup")).GetAttribute("transform");
            Console.WriteLine("Shape orientation: " + selectedShapeObjectOrientation1);

            // get shape degree
            var shapeDegrees = selectedShapeObjectOrientation1.Split(' ');
            Console.WriteLine("shape degree: " + shapeDegrees[0]);

            // validate coordinates of shape before and after rotate
            Assert.IsTrue(selectedShapeObjectOrientation0 != selectedShapeObjectOrientation1, "Shape did not rotate");
            Assert.IsTrue(shapeDegrees[0].Contains(degree.ToString()), "Shape did not rotate to the correct degree");
        }
    }

    public class UpdateConfigurationCommand
    {
        private readonly string thickness;
        private string color;
        private int quarterRoundedCornersQty;

        public UpdateConfigurationCommand(string thickness)
        {
            this.thickness = thickness;
        }

        public UpdateConfigurationCommand WithColor(string color)
        {
            this.color = color;
            return this;
        }

        public UpdateConfigurationCommand WithQuarterRoundedCorners(int quarterRoundedCornersQty)
        {
            this.quarterRoundedCornersQty = quarterRoundedCornersQty;
            return this;
        }

        public void UpdateDesign()
        {
            Utilities util = new Utilities();
            // wait to configurator to load
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(50));
            wait.Until(d => d.FindElement(By.XPath("//div[@class='config-product-name']/span[@id='brand']")).Displayed);

            // move to Choose surface thickness
            string casLabel = "Choose A Surface";
            var casLabelDisplayed = Driver.Instance.FindElement(By.CssSelector("#ChooseASurface>h2"));
            Actions action = new Actions(Driver.Instance);
            action.MoveToElement(casLabelDisplayed);
            action.Perform();

            // set surface thickness
            //      check for thickness choices list
            var thicknessChoiceList = Driver.Instance.FindElements(By.XPath("//span[@class='radio-btn__label']/span[2]"));
            int PosThickness = util.GetItemPosition(thicknessChoiceList, thickness);

            var thicknessChoice = Driver.Instance.FindElements(By.XPath("//div[@class='headless']/div/div/div[3]/div[" + (PosThickness + 1) + "]//span[@class='radio-btn__label']/span[2]"));
            thicknessChoice[0].Click();

            // validate thickness in summary
            var thicknessInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
            Console.WriteLine("thickness choice value: " + thicknessInSummary[1].Text);
            Assert.IsTrue(thicknessInSummary[1].Text == thickness, "Surface thickness choice was not set properly");

            // Move to Select Color label
            string scLabel = "Select Color";
            var scLabelDisplayed = Driver.Instance.FindElement(By.XPath("//div[@id='ChooseASurface']/div//div[@class='card-content']/h2"));
            action.MoveToElement(scLabelDisplayed);
            action.Perform();

            // set color
            var colorChoiceList = Driver.Instance.FindElements(By.XPath("//div[@class='color-card-footer']/p"));
            int posColor = util.GetItemPosition(colorChoiceList, color);
            Console.WriteLine("Position of color: " + posColor);

            if (posColor > -1)
            {
                var colorChoice = Driver.Instance.FindElement(By.XPath("//div[@class='option-group null-template']/div[" + (posColor + 1) + "]//div[@class='color-card-footer']/p"));
                Console.WriteLine("color name: " + colorChoice.Text);
                action.MoveToElement(colorChoice);
                action.Click();
                action.Perform();
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Color not found");
            }

            // validate color in summary
            var colorInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
            Assert.IsTrue(colorInSummary[2].Text.ToUpper() == color, "Color choice was not set properly");

            // reset quarterRoundedCorners if different than 0
            var stepperDecrementBtns = Driver.Instance.FindElements(By.CssSelector(".quantity-btn.decrement"));
            var quarterRoundInSummary0 = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span")).Count;
            if (quarterRoundInSummary0 > 0)
            {
                var quarterRoundDec = stepperDecrementBtns[0];
                action.MoveToElement(quarterRoundDec);
                for (int i = 0; i < quarterRoundInSummary0; i++)
                {
                    action.Click();
                    action.Perform();
                }
                // validate quarter round corner value in summary is reset to 0
                var quarterRoundInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
                Assert.IsTrue(quarterRoundInSummary[4].Text == "0", "¼\" Rounded corner choice was not reset properly");
            }

            // set quarterROundedCorners
            var stepperIncrementBtns = Driver.Instance.FindElements(By.CssSelector(".quantity-btn.increment"));
            // set corners
            if (quarterRoundedCornersQty > 0)
            {
                var quarterRoundInc = stepperIncrementBtns[0];
                action.MoveToElement(quarterRoundInc);
                for (int i = 0; i < quarterRoundedCornersQty; i++)
                {
                    action.Click();
                    action.Perform();
                }
                // validate quarter round corner value in summary
                var quarterRoundInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
                Assert.IsTrue(quarterRoundInSummary[4].Text == quarterRoundedCornersQty.ToString(), "¼\" Rounded corner choice was not set properly");
            }

            Driver.Wait(TimeSpan.FromMilliseconds(30));

            // wait for pricing update
            var waitPricing = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            waitPricing.Until(d => d.FindElement(By.Id("config-original-price")).Text.Contains("$"));

            // click on Update My Design
            wait.Until(d => d.FindElement(By.CssSelector(".button.primary")).Enabled);
            var updateMyDeisgnBtn = Driver.Instance.FindElements(By.CssSelector(".button.primary"));
            updateMyDeisgnBtn[0].Click();
        }
    }

    public class CreateCountertopConfigurationCommand
    {
        private readonly string thickness;
        private string color;
        private string corner;
        private int quarterRoundQty = 0;
        private int oneInchRoundQty = 0;
        private int threeInchRoundQty = 0;
        private int intAngleQty = 0;
        private string edgeOption;
        private static Utilities util = new Utilities();

        public CreateCountertopConfigurationCommand(string thickness)
        {
            this.thickness = thickness;
        }

        public CreateCountertopConfigurationCommand WithColor(string color)
        {
            this.color = color;
            return this;
        }

        public CreateCountertopConfigurationCommand WithQuarterRoundedCorners(int quarterRoundQty)
        {
            this.quarterRoundQty = quarterRoundQty;
            return this;
        }

        public CreateCountertopConfigurationCommand WithOneInchRoundedCorners(int oneInchRoundQty)
        {
            this.oneInchRoundQty = oneInchRoundQty;
            return this;
        }

        public CreateCountertopConfigurationCommand WithThreeInchRoundedCorners(int threeInchRoundQty)
        {
            this.threeInchRoundQty = threeInchRoundQty;
            return this;
        }

        public CreateCountertopConfigurationCommand WithInteriorAngles(int intAngleQty)
        {
            this.intAngleQty = intAngleQty;
            return this;
        }

        public CreateCountertopConfigurationCommand WithEdgeStyle(string edgeOption)
        {
            this.edgeOption = edgeOption;
            return this;
        }

        public void AddToDesign()
        {
            // wait to configurator to load
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElements(By.CssSelector(".progress-linear.interminate")).Count == 0);
            wait.Until(d => d.FindElement(By.XPath("//div[@class='config-product-name']/span[@id='brand']")).Displayed);

            // check if Choose surface thickness label is visible
            string casLabel = "Choose A Surface";
            var casLabelDisplayed = Driver.Instance.FindElement(By.CssSelector("#ChooseASurface>h2"));
            Actions action = new Actions(Driver.Instance);
            action.MoveToElement(casLabelDisplayed);
            action.Perform();

            // set surface thickness
            //      check for thickness choices list
            var thicknessChoiceList = Driver.Instance.FindElements(By.XPath("//span[@class='radio-btn__label']/span[2]"));
            int PosThickness = util.GetItemPosition(thicknessChoiceList, thickness);

            var thicknessChoice = Driver.Instance.FindElements(By.XPath("//div[@class='headless']/div/div/div[3]/div[" + (PosThickness + 1) + "]//span[@class='radio-btn__label']/span[2]"));
            thicknessChoice[0].Click();

            // validate thickness in summary
            var thicknessInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
            Console.WriteLine("thickness choice value: " + thicknessInSummary[1].Text);
            Assert.IsTrue(thicknessInSummary[1].Text == thickness, "Surface thickness choice was not set properly");

            // Move to Select Color label
            string scLabel = "Select Color";
            var scLabelDisplayed = Driver.Instance.FindElement(By.XPath("//div[@id='ChooseASurface']/div//div[@class='card-content']/h2"));
            action.MoveToElement(scLabelDisplayed);
            action.Perform();

            // set color
            var colorChoiceList = Driver.Instance.FindElements(By.XPath("//div[@class='color-card-footer']/p"));
            int posColor = util.GetItemPosition(colorChoiceList, color);
            Console.WriteLine("Position of color: " + posColor);

            if (posColor > -1)
            {
                var colorChoice = Driver.Instance.FindElement(By.XPath("//div[@class='option-group null-template']/div[" + (posColor + 1) + "]//div[@class='color-card-footer']/p"));
                Console.WriteLine("color name: " + colorChoice.Text);
                
                action.MoveToElement(colorChoice);
                action.Click();
                action.Perform();
            }
            else
            {
                Assert.IsTrue(-1 > 0, "Color not found");
            }

            // validate color in summary
            var colorInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
            Assert.IsTrue(colorInSummary[2].Text.ToUpper() == color, "Color choice was not set properly");

            var stepperIncrementBtns = Driver.Instance.FindElements(By.CssSelector(".quantity-btn.increment"));
            // set corners
            if (quarterRoundQty > 0)
            {
                var quarterRoundInc = stepperIncrementBtns[0];
                action.MoveToElement(quarterRoundInc);
                for (int i = 0; i < quarterRoundQty; i++)
                {
                    action.Click();
                    action.Perform();
                }
                // validate quarter round corner value in summary
                var quarterRoundInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
                Assert.IsTrue(quarterRoundInSummary[4].Text == quarterRoundQty.ToString(), "¼\" Rounded corner choice was not set properly");
            }

            if (oneInchRoundQty > 0)
            {
                var oneInchRoundInc = stepperIncrementBtns[1];
                action.MoveToElement(oneInchRoundInc);
                for (int i = 0; i < oneInchRoundQty; i++)
                {
                    action.Click();
                    action.Perform();
                }
                // validate 1 inch round corner value in summary
                var oneInchRoundInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
                Assert.IsTrue(oneInchRoundInSummary[5].Text == oneInchRoundQty.ToString(), "1\" Rounded corner choice was not set properly");
            }

            if (threeInchRoundQty > 0)
            {
                var threeInchRoundInc = stepperIncrementBtns[2];
                action.MoveToElement(threeInchRoundInc);
                for (int i = 0; i < threeInchRoundQty; i++)
                {
                    action.Click();
                    action.Perform();
                }
                // validate 3 inch round corner value in summary
                var threeInchRoundInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
                Assert.IsTrue(threeInchRoundInSummary[6].Text == threeInchRoundQty.ToString(), "3\" Rounded corner choice was not set properly");
            }

            // set interior Angles
            if (intAngleQty > 0)
            {
                var intAngleInc = stepperIncrementBtns[3];
                action.MoveToElement(intAngleInc);
                for (int i = 0; i < intAngleQty; i++)
                {
                    action.Click();
                    action.Perform();
                }
                // validate 3 inch round corner value in summary
                var intAngleInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
                Assert.IsTrue(intAngleInSummary[7].Text == intAngleQty.ToString(), "Interior Angles choice was not set properly");
            }

            // set edge
            if (edgeOption == "yes")
            {
                var edgeStyleChoices = Driver.Instance.FindElements(By.XPath("//div[contains(@class,'configurator-card-content')]/div[4]//div[@class='null-template-choice']"));
                edgeStyleChoices[0].Click();
                // validate edge style value in summary
                var edgeStyleInSummary = Driver.Instance.FindElements(By.CssSelector(".config-summary-choice-value>span"));
                Assert.IsTrue(edgeStyleInSummary[8].Text == "Edge Style", "Edge Style choice was not set properly");
            }

            Driver.Wait(TimeSpan.FromMilliseconds(30));

            // wait for pricing update
            var waitPricing = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(60));
            waitPricing.Until(d => d.FindElement(By.Id("config-original-price")).Text.Contains("$"));

            // Add to Deisgn
            wait.Until(d => d.FindElement(By.CssSelector(".button.primary")).Enabled);
            var addToDesignBtn = Driver.Instance.FindElements(By.CssSelector(".button.primary"));
            addToDesignBtn[0].Click();
         
        }


    }
}

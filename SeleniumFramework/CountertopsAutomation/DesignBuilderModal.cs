using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumFramework;

namespace SeleniumFramework.CountertopsAutomation
{
    public class DesignBuilderModal
    {
        public static void GoToNewDesign()
        {
            Driver.Instance.Navigate().GoToUrl("http://custom.hd-qa71.homedepotdev.com/specialorders/designs/countertops/new");
            Driver.Instance.Manage().Window.Maximize();
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
            wait.Until(d => d.SwitchTo().ActiveElement().GetAttribute("value") == "Design 1");
            //wait.Until(d => d.FindElement(By.Id("create-design-model-designname")).GetAttribute("value") == "Design 1");
            //d.SwitchTo().ActiveElement().GetAttribute("id") == "create-design-modal-zipcode"

        }

        public static CreateDefaultDesignCommand CreateDeafultDesignWithZipcode(string zipcode)
        {
            return new CreateDefaultDesignCommand(zipcode);
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

                var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(30));
                wait.Until(d => d.FindElement(By.CssSelector(".card-actions.right>button")).Enabled);

                var addDesignButton = Driver.Instance.FindElements(By.CssSelector(".card-actions.right>button"));
                wait.Until(d => addDesignButton.Count > 0);
                addDesignButton[0].Click();
            }
        }

        public static CreateDesignCommand CreateDesignAs(string designName)
        {
            return new CreateDesignCommand(designName);
        }

        public class CreateDesignCommand
        {
            private readonly string designName;
            private string zipcode;

            public CreateDesignCommand(string designName)
            {
                this.designName = designName;
            }

            public CreateDesignCommand WithZipcode(string zipcode)
            {
                this.zipcode = zipcode;
                return this;
            }

            public void CreateDesign()
            {
                var designNameInput = Driver.Instance.FindElement(By.Id("create-design-model-designname"));
                designNameInput.Clear();
                designNameInput.SendKeys(designName);

                var propertyZipcodeInput = Driver.Instance.FindElement(By.Id("create-design-modal-zipcode"));
                propertyZipcodeInput.SendKeys(zipcode);

                //design name input delay because zipcode validation messes up design name
                var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(20));
                wait.Until(d => d.FindElement(By.CssSelector(".card-actions.right>button")).Enabled);

                var addDesignButton = Driver.Instance.FindElement(By.CssSelector(".card-actions.right>button"));
                addDesignButton.Click();
            }

        }
    }
}

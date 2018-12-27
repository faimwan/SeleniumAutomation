using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumFramework.GuidedSellerAutomation;

namespace SeleniumFramework
{
    public class Utilities
    {
        public IList<IWebElement> GetListElements(By locator)
        {
            IList<IWebElement> myReturnList = Driver.Instance.FindElements(locator);
            return myReturnList;
        }

        public int GetNumberItems(By locator)
        {
            IList<IWebElement> myItemList = Driver.Instance.FindElements(locator);
            return myItemList.Count;
        }

        public bool ValidateListElements(string[] list, string[] list1)
        {
            bool valid = true;
            if (list.Length == list1.Length)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] != list1[i])
                    {
                        //Console.WriteLine("Current: " + myCurrentList[i].Text);
                        //Console.WriteLine("Expected: " + list[i]);
                        valid = false;
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Number of items in list: " + list.Length + ", number of items in list1: " +
                                  list1.Length);
                valid = false;
            }

            return valid;
        }

        public bool ValidateListElements(string[] list, IList<IWebElement> list1)
        {
            bool valid = true;
            if (list.Length == list1.Count)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] != list1[i].Text)
                    {
                        Console.WriteLine("Current: " + list1[i].Text);
                        Console.WriteLine("Expected: " + list[i]);
                        valid = false;
                        break;
                    }
                }
            }
            return valid;
        }

        public int GetItemPosition(IList<IWebElement> list, string item)
        {
            var pos = -1;
            int total = list.Count;
            for (int i = 0; i < total; i++)
            {
                //Console.WriteLine("Item name: " + list[i].Text);
                if (list[i].Text == item)
                {
                    Console.WriteLine("Found it: " + item + " at position: " + i);
                    pos = i;
                    break;
                }
            }
            return pos;
        }

        public void WaitForAjax()
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(15));
            wait.Until(d => (bool)(d as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"));
        }

        public void WaitForReady()
        {
            //WebDriverWait wait = new WebDriverWait(webDriver, waitForElement);
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(15));
            wait.Until(driver => (bool)((IJavaScriptExecutor)driver).
                ExecuteScript("return jQuery.active == 0"));
        }

        public static void MoveToElemnt(IWebElement elem)
        {
            Actions action = new Actions(Driver.Instance);
            action.MoveToElement(elem);
            action.Perform();
        }
    }
}

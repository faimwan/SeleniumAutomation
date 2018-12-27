using System;
using WordPressAutomation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WordPressTests
{
    [TestClass]
    public class LoginTests : WordPressTest
    {
        [TestMethod]
        public void Admin_User_Can_Login()
        {
            Assert.IsTrue(DashboardPage.IsAt, "Failed to login");
        }
    }
}

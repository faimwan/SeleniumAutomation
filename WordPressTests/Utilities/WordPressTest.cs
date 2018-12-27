using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;
using WordPressAutomation.Workflows;

namespace WordPressTests
{
    [TestClass]
    public class WordPressTest
    {
        [TestInitialize]
        public void Init()
        {
            Driver.Initialize();
            PostCreator.Initialize();
            LoginPage.GoTo();
            LoginPage.LoginAs("admin").WithPassword("password").Login();
        }

        [TestMethod]
        public void Cleanup()
        {
            PostCreator.Cleanup();
            Driver.Close();
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests
{
    [TestClass]
    public class PageTests : WordPressTest
    {
        [TestMethod]
        public void Can_Edit_A_Page()
        {
            ListPostsPage.GoTo(PostType.Page);
            ListPostsPage.SelectPost("Sample Page");

            Assert.IsTrue(NewPostPage.IsInEditMode(), "Wasn't in edit mode");
            Assert.AreEqual("Sample Page", NewPostPage.Title, "Title dit not match");
        }
    }
}

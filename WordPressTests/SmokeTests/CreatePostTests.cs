using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;

namespace WordPressTests
{
    [TestClass]
    public class CreatePostTests : WordPressTest
    {
        [TestMethod]
        public void Can_Create_A_Basic_Post()
        {
            NewPostPage.GoTo();
            NewPostPage.CreatePost("This is the post title").WithBody("Hi, this is the body").Publish();

            NewPostPage.GoToNewPost();
            Assert.AreEqual(PostPage.Title, "This is the test title", "Ttitle did not match new post");
        }
    }
}

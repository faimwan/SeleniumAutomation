using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressAutomation;
using WordPressAutomation.Workflows;

namespace WordPressTests.PostsTests
{
    [TestClass]
    public class AllPostsTests : WordPressTest
    {
        // Added post show up in all posts
        // Can activate excerpt mode
        // Can Add new post

        // Single post selections

        // Can select a posy by title
        // Can select a post by edit
        // Can select a pos\t by QuickEdit
        // Can trash a post
        // Can view a post
        // Can filter by author
        // Can filter by category
        // Can filter by tag
        // Can go to post comments

        // Bulk actions

        // Can edit muktiple posts
        // Can trash muktiple posts
        // Can select all posts

        // Drop down filters

        // Can filter by month
        // Can filter by category
        // Can view published only
        // Can view drafts only
        // Can view trash only

        // Can search posts

        // Added posts show up in all posts
        // Can trash a post
        // Can search posts

        [TestMethod]
        public void Added_Posts_Show_Up()
        {
            // Go to posts, get post count, store
            ListPostsPage.GoTo(PostType.Posts);
            ListPostsPage.StoreCount();

            // Add a new post
            PostCreator.CreatePost();

            /*
            NewPostPage.GoTo();
            NewPostPage.CreatePost("Added posts show up, title")
                .WithBody("Added posts show up, body")
                .Publish();
            */

            // Go to posts, get new post count
            ListPostsPage.GoTo(PostType.Posts);
            Assert.AreEqual(ListPostsPage.PreviousPostCount + 1, ListPostsPage.CurrentPostCount, "Count of posts did not increase");

            // Check for added post
            Assert.IsTrue(ListPostsPage.DoesPostExistWithTitle(PostCreator.PreviousTitle),"Post was not added");

            // trash post (clean up)
            ListPostsPage.TrashPost(PostCreator.PreviousTitle);
            Assert.AreEqual(ListPostsPage.PreviousPostCount, ListPostsPage.CurrentPostCount, "Couldn't trash post");

        }

        [TestMethod]
        public void Can_Search_Posts()
        {
            // Create a new post
            PostCreator.CreatePost();

            // Go to list posts
            // ListPostsPage.GoTo(PostType.Posts);

            // Search for post
            ListPostsPage.SearchForPost(PostCreator.PreviousTitle);

            // Check that post shows up in results
            Assert.IsTrue(ListPostsPage.DoesPostExistWithTitle(PostCreator.PreviousTitle));

            // Cleanup (trash post)
            // Is already part of the PostCreator.CleanUp
        }
    }
}

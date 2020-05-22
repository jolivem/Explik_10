using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.UT.Repository
{
    [TestClass]
    public class PageRepositoryTests
	{
		private Page _page1;
		private PageContent _pageContent1;
		private PageContent _pageContent2;
		private DateTime _createdDate;
		private DateTime _editedDate;
        protected IRepository Repository;
        protected ApplicationSettings ApplicationSettings;

        protected string ConnectionString { get; set; }
        protected virtual DataStoreType DataStoreType { get { return null; } }

        [TestInitialize]
		public void SetUp()
		{
            ConnectionString = "tuti";
            ApplicationSettings = new ApplicationSettings() { ConnectionString = ConnectionString, DataStoreType = DataStoreType };

            Repository = new LightSpeedRepository(ApplicationSettings);

            Repository.DeleteAllPages();

            // Create 5 Pages with 2 versions of content each
            _createdDate = DateTime.Today.ToUniversalTime().AddDays(-1);
			_editedDate = _createdDate.AddHours(1);

			_page1 = NewPage("admin", "homepage, newpage");
            _page1.ControlledBy = "bob";
			_pageContent1 = Repository.AddNewPage(_page1, "text", "admin", _createdDate);
			_page1 = _pageContent1.Page;
			_pageContent2 = Repository.AddNewPageContentVersion(_page1, "v2", _editedDate, 2);
			_page1 = _pageContent2.Page; // update the modified date


			Page page2 = NewPage("editor1");
			PageContent pageContent2 = Repository.AddNewPage(page2, "text", "editor1", _createdDate);
			Repository.AddNewPageContentVersion(pageContent2.Page, "v2",  _editedDate, 1);

			Page page3 = NewPage("editor2");
			PageContent pageContent3 = Repository.AddNewPage(page3, "text", "editor2", _createdDate);
			Repository.AddNewPageContentVersion(pageContent3.Page, "v2", _editedDate, 1);

			Page page4 = NewPage("editor3");
			PageContent pageContent4 = Repository.AddNewPage(page4, "text", "editor3", _createdDate);
			Repository.AddNewPageContentVersion(pageContent4.Page, "v2", _editedDate, 1);

			Page page5 = NewPage("editor4");
			PageContent pageContent5 = Repository.AddNewPage(page5, "text", "editor4", _createdDate);
			Repository.AddNewPageContentVersion(pageContent5.Page, "v2", _editedDate, 1);
		}

		protected Page NewPage(string author, string tags = "tag1,tag2,tag3", string title = "Title")
		{
			Page page = new Page()
			{
				Title = title,
				CreatedOn = _createdDate,
				CreatedBy = author,
				ControlledBy = author,
				PublishedOn = _createdDate,
				Tags = tags
			};

			return page;
		}

		[TestMethod]
		public void AllPages()
		{
			// Arrange


			// Act
			List<Page> actualList = Repository.AllPages().ToList();

			// Assert
			Assert.IsTrue(actualList.Count==5);
			Assert.IsTrue(actualList[0] != null);
			Assert.IsTrue(actualList[1] != null);
			Assert.IsTrue(actualList[2] != null);
			Assert.IsTrue(actualList[3] != null);
			Assert.IsTrue(actualList[4] != null);
		}

		[TestMethod]
		public void GetPageById()
		{
			// Arrange


			// Act
			Page actualPage = Repository.GetPageById(_page1.Id);

			// Assert
			Assert.IsTrue(actualPage != null);
			Assert.IsTrue(actualPage.Id == (_page1.Id));
			Assert.IsTrue(actualPage.CreatedBy == (_page1.CreatedBy));
			Assert.IsTrue(actualPage.CreatedOn == (_page1.CreatedOn));
			Assert.IsTrue(actualPage.IsLocked == (_page1.IsLocked));
			Assert.IsTrue(actualPage.ControlledBy == (_page1.ControlledBy));
			Assert.IsTrue(actualPage.PublishedOn == (_page1.PublishedOn));
			Assert.IsTrue(actualPage.Tags == (_page1.Tags));
			Assert.IsTrue(actualPage.Title == (_page1.Title));
		}

		[TestMethod]
		public void FindPagesCreatedBy()
		{
			// Arrange


			// Act
			List<Page> actualPages = Repository.FindPagesCreatedBy("admin").ToList();

			// Assert
			Assert.IsTrue(actualPages.Count == (1));
			Assert.IsTrue(actualPages[0].Id == (_page1.Id));
			Assert.IsTrue(actualPages[0].CreatedBy == (_page1.CreatedBy));
			Assert.IsTrue(actualPages[0].CreatedOn == (_page1.CreatedOn));
			Assert.IsTrue(actualPages[0].IsLocked == (_page1.IsLocked));
			Assert.IsTrue(actualPages[0].ControlledBy == (_page1.ControlledBy));
			Assert.IsTrue(actualPages[0].PublishedOn == (_page1.PublishedOn));
			Assert.IsTrue(actualPages[0].Tags == (_page1.Tags));
			Assert.IsTrue(actualPages[0].Title == (_page1.Title));
		}

		[TestMethod]
		public void FindPagesByControlledBy()
		{
			// Arrange
			PageContent newContent = Repository.AddNewPageContentVersion(_page1, "new text", _createdDate, 3);
			Page expectedPage = newContent.Page;

            // Act
            List<Page> _actualPages = Repository.AllPages().ToList();
            List<Page> actualPages = Repository.FindPagesControlledBy("bob").ToList();

            // Assert
            Assert.IsTrue(actualPages.Count == (0));
            //After a new page content, the controlledBy attribute is set to ""
			//Assert.IsTrue(actualPages[0].Id == (expectedPage.Id));
			//Assert.IsTrue(actualPages[0].CreatedBy == (expectedPage.CreatedBy));
			//Assert.IsTrue(actualPages[0].CreatedOn == (expectedPage.CreatedOn));
			//Assert.IsTrue(actualPages[0].IsLocked == (expectedPage.IsLocked));
			//Assert.IsTrue(actualPages[0].ControlledBy == ("bob"));
			//Assert.IsTrue(actualPages[0].PublishedOn == (expectedPage.PublishedOn));
			//Assert.IsTrue(actualPages[0].Tags == (expectedPage.Tags));
			//Assert.IsTrue(actualPages[0].Title == (expectedPage.Title));
		}

		[TestMethod]
		public void FindPagesContainingTag()
		{
			// Arrange


			// Act
			List<Page> actualPages = Repository.FindPagesContainingTag("tag1").ToList();


			// Assert
			Assert.IsTrue(actualPages.Count == (4));
		}

		//[TestMethod]
		//public void AllTags()
		//{
  //          // Arrange


  //          // Act
  //          List<string> actual = Repository.AllTags().ToList();

		//	// Assert
		//	Assert.IsTrue(actual.Count == (5)); // homepage, newpage, tag1, tag2, tag3
		//}

		[TestMethod]
		public void GetPageByTitle()
		{
			// Arrange
			string title = "page title";
			Page expectedPage = NewPage("admin", "tag1", title);
			PageContent newContent = Repository.AddNewPage(expectedPage, "sometext", "admin", _createdDate);
			expectedPage.Id = newContent.Page.Id; // get the new identity

			// Act
			Page actualPage = Repository.GetPageByTitle(title);

			// Assert
			Assert.IsTrue(actualPage.Id == (expectedPage.Id));
			Assert.IsTrue(actualPage.CreatedBy == (expectedPage.CreatedBy));
			Assert.IsTrue(actualPage.CreatedOn == (expectedPage.CreatedOn));
			Assert.IsTrue(actualPage.IsLocked == (expectedPage.IsLocked));
			Assert.IsTrue(actualPage.ControlledBy == (expectedPage.ControlledBy));
			Assert.IsTrue(actualPage.PublishedOn == (expectedPage.PublishedOn));
			Assert.IsTrue(actualPage.Tags == (expectedPage.Tags));
			Assert.IsTrue(actualPage.Title == (expectedPage.Title));
		}

		[TestMethod]
		public void GetLatestPageContent()
		{
			// Arrange
			PageContent expectedContent = _pageContent2;
			Page expectedPage = _pageContent2.Page;

			// Act
			PageContent actualContent = Repository.GetLatestPageContent(_pageContent2.Page.Id);
			Page actualPage = actualContent.Page;

			// Assert
			Assert.IsTrue(actualContent.ControlledBy == (expectedContent.ControlledBy));
			Assert.IsTrue(actualContent.EditedOn == (expectedContent.EditedOn));
			Assert.IsTrue(actualContent.Id == (expectedContent.Id));
			Assert.IsTrue(actualContent.Text == (expectedContent.Text));
			Assert.IsTrue(actualContent.VersionNumber == (expectedContent.VersionNumber));

			Assert.IsTrue(actualPage.Id == (expectedPage.Id));
			Assert.IsTrue(actualPage.CreatedBy == (expectedPage.CreatedBy));
			Assert.IsTrue(actualPage.CreatedOn == (expectedPage.CreatedOn));
			Assert.IsTrue(actualPage.IsLocked == (expectedPage.IsLocked));
			Assert.IsTrue(actualPage.ControlledBy == (expectedPage.ControlledBy));
			Assert.IsTrue(actualPage.PublishedOn == (expectedPage.PublishedOn));
			Assert.IsTrue(actualPage.Tags == (expectedPage.Tags));
			Assert.IsTrue(actualPage.Title == (expectedPage.Title));
		}

		[TestMethod]
		public void GetPageContentById()
		{
			// Arrange
			PageContent expectedContent = _pageContent2;
			Page expectedPage = _pageContent2.Page;

			// Act
			PageContent actualContent = Repository.GetPageContentById(expectedContent.Id);
			Page actualPage = actualContent.Page;

			// Assert
			Assert.IsTrue(actualContent.ControlledBy == (expectedContent.ControlledBy));
			Assert.IsTrue(actualContent.EditedOn == (expectedContent.EditedOn));
			Assert.IsTrue(actualContent.Id == (expectedContent.Id));
			Assert.IsTrue(actualContent.Text == (expectedContent.Text));
			Assert.IsTrue(actualContent.VersionNumber == (expectedContent.VersionNumber));

			Assert.IsTrue(actualPage.Id == (expectedPage.Id));
			Assert.IsTrue(actualPage.CreatedBy == (expectedPage.CreatedBy));
			Assert.IsTrue(actualPage.CreatedOn == (expectedPage.CreatedOn));
			Assert.IsTrue(actualPage.IsLocked == (expectedPage.IsLocked));
			Assert.IsTrue(actualPage.ControlledBy == (expectedPage.ControlledBy));
			Assert.IsTrue(actualPage.PublishedOn == (expectedPage.PublishedOn));
			Assert.IsTrue(actualPage.Tags == (expectedPage.Tags));
			Assert.IsTrue(actualPage.Title == (expectedPage.Title));
		}

		[TestMethod]
		public void GetPageContentByPageIdAndVersionNumber()
		{
			// Arrange
			PageContent expectedContent = _pageContent2;
			Page expectedPage = _pageContent2.Page;

			// Act
			PageContent actualContent = Repository.GetPageContentByPageIdAndVersionNumber(expectedPage.Id, expectedContent.VersionNumber);
			Page actualPage = actualContent.Page;

			// Assert
			Assert.IsTrue(actualContent.ControlledBy == (expectedContent.ControlledBy));
			Assert.IsTrue(actualContent.EditedOn == (expectedContent.EditedOn));
			Assert.IsTrue(actualContent.Id == (expectedContent.Id));
			Assert.IsTrue(actualContent.Text == (expectedContent.Text));
			Assert.IsTrue(actualContent.VersionNumber == (expectedContent.VersionNumber));

			Assert.IsTrue(actualPage.Id == (expectedPage.Id));
			Assert.IsTrue(actualPage.CreatedBy == (expectedPage.CreatedBy));
			Assert.IsTrue(actualPage.CreatedOn == (expectedPage.CreatedOn));
			Assert.IsTrue(actualPage.IsLocked == (expectedPage.IsLocked));
			Assert.IsTrue(actualPage.ControlledBy == (expectedPage.ControlledBy));
			Assert.IsTrue(actualPage.PublishedOn == (expectedPage.PublishedOn));
			Assert.IsTrue(actualPage.Tags == (expectedPage.Tags));
			Assert.IsTrue(actualPage.Title == (expectedPage.Title));
		}

		//[TestMethod]
		//public void GetPageContentByEditedBy()
		//{
		//	// Arrange

		//	// Act
		//	List<PageContent> allContent = Repository.GetPageContentByEditedBy("admin").ToList();

		//	// Assert
		//	Assert.IsTrue(allContent.Count == (2));
		//}

		[TestMethod]
		public void FindPageContentsByPageId()
		{
			// Arrange


			// Act
			List<PageContent> pagesContents = Repository.FindPageContentsByPageId(_page1.Id).ToList();

			// Assert
			Assert.IsTrue(pagesContents.Count == (2));
			Assert.IsTrue(pagesContents[0] != null);

			PageContent expectedPageContent = pagesContents.FirstOrDefault(x => x.Id == _pageContent1.Id);
			Assert.IsTrue(expectedPageContent != null);
		}

		//[TestMethod]
		//public void FindPageContentsEditedBy()
		//{
		//	// Arrange


		//	// Act
		//	List<PageContent> pagesContents = Repository.FindPageContentsEditedBy("admin").ToList();

		//	// Assert
		//	Assert.IsTrue(pagesContents.Count == (2));
		//	Assert.IsTrue(pagesContents[0] != null);

		//	PageContent expectedPageContent = pagesContents.FirstOrDefault(x => x.Id == _pageContent1.Id);
		//	Assert.IsTrue(expectedPageContent != null);
		//}

		[TestMethod]
		public void AllPageContents()
		{
			// Arrange


			// Act
			List<PageContent> pagesContents = Repository.AllPageContents().ToList();

			// Assert
			Assert.IsTrue(pagesContents.Count == (10)); // five pages with 2 versions
			Assert.IsTrue(pagesContents[0] != null);

			PageContent expectedPageContent = pagesContents.FirstOrDefault(x => x.Id == _pageContent1.Id);
			Assert.IsTrue(expectedPageContent != null);
		}

        [TestMethod]
        public void DeletePage_Test()
        {
            // Arrange
            Page page = Repository.GetPageById(_page1.Id);

            // Act
            Repository.DeletePage(page.Id);

            // Assert
            Assert.IsTrue(page != null);
            Assert.IsTrue(Repository.GetPageById(1) == null);
        }

        [TestMethod]
        public void DeletePageContent()
        {
            // Arrange
            PageContent pageContent = Repository.GetLatestPageContent(_page1.Id);
            Guid id = pageContent.Id;

            // Act
            Repository.DeletePageContent(pageContent);

            // Assert
            Assert.IsTrue(Repository.GetPageContentById(id) == null);
        }


        [TestMethod]
		public void SaveOrUpdatePage()
		{
			// Arrange
			Page newPage = NewPage("admin", "tag1, 3, 4");
			DateTime modifiedDate = _createdDate.AddMinutes(1);

			Page existingPage = _page1;
			existingPage.Title = "new title";
			existingPage.ControlledBy = "editor1";
			existingPage.PublishedOn = modifiedDate;

			// Act
			Repository.SaveOrUpdatePage(newPage);
			Repository.SaveOrUpdatePage(existingPage);

			// Assert
			Assert.IsTrue(Repository.AllPages().Count() == (6));

			Page actualPage = Repository.GetPageById(existingPage.Id);
			Assert.IsTrue(actualPage.Title == ("new title"));
			Assert.IsTrue(actualPage.ControlledBy == ("editor1"));
			Assert.IsTrue(actualPage.PublishedOn == (modifiedDate));
		}

		[TestMethod]
		public void AddNewPage()
		{
			// Arrange
			Page newPage = NewPage("admin", "tag1,3,4");
			newPage.PublishedOn = _createdDate;

			// Act
			PageContent pageContent = Repository.AddNewPage(newPage, "my text", "admin", _createdDate);

			// Assert
			Assert.IsTrue(Repository.AllPages().Count() == (6));
			Assert.IsTrue(pageContent != null);
			Assert.IsTrue(pageContent.Id != Guid.Empty);
			Assert.IsTrue(pageContent.Text == ("my text"));
			Assert.IsTrue(pageContent.EditedOn == (_createdDate));
			Assert.IsTrue(pageContent.VersionNumber == (1));

			Page actualPage = Repository.GetPageById(pageContent.Page.Id);
			Assert.IsTrue(actualPage.Title == ("Title"));
			Assert.IsTrue(actualPage.Tags == ("tag1,3,4"));
			Assert.IsTrue(actualPage.CreatedOn == (_createdDate));
			Assert.IsTrue(actualPage.CreatedBy == ("admin"));
			Assert.IsTrue(actualPage.ControlledBy == ("admin"));
			Assert.IsTrue(actualPage.PublishedOn == (_createdDate));
		}

		[TestMethod]
		public void AddNewPageContentVersion()
		{
			// Arrange
			Page existingPage = _page1;

			// Act
			PageContent newContent = Repository.AddNewPageContentVersion(existingPage, "new text", _createdDate, 2);

			// Assert
			Assert.IsTrue(Repository.AllPageContents().Count() == (11));
			Assert.IsTrue(newContent != null);
			Assert.IsTrue(newContent.Id != Guid.Empty);
			Assert.IsTrue(newContent.Text == ("new text"));
			Assert.IsTrue(newContent.EditedOn == (_createdDate));
			Assert.IsTrue(newContent.VersionNumber == (2));

			PageContent latestContent = Repository.GetPageContentById(newContent.Id);
			Assert.IsTrue(latestContent.Id == (newContent.Id));
			Assert.IsTrue(latestContent.Text == (newContent.Text));
			Assert.IsTrue(latestContent.EditedOn == (newContent.EditedOn));
			Assert.IsTrue(latestContent.VersionNumber == (newContent.VersionNumber));
		}

		[TestMethod]
		public void UpdatePageContent()
		{
			// Arrange
			DateTime editedDate = _editedDate.AddMinutes(10);

			PageContent existingContent = _pageContent1;
			int versionNumber = 2;
			int pageId = existingContent.Page.Id;

			existingContent.Text = "new text";
			existingContent.ControlledBy = "editor1";
			existingContent.EditedOn = editedDate;
			existingContent.VersionNumber = versionNumber;

			// Act
			Repository.UpdatePageContent(existingContent);
			PageContent actualContent = Repository.GetPageContentById(existingContent.Id);

			// Assert
			Assert.IsTrue(actualContent != null);
			Assert.IsTrue(actualContent.Text == ("new text"));
			Assert.IsTrue(actualContent.ControlledBy == ("editor1"));
			Assert.IsTrue(actualContent.EditedOn == (editedDate));
			Assert.IsTrue(actualContent.VersionNumber == (versionNumber));
		}

		public void DeleteAllPages()
		{
			// Arrange


			// Act
			Repository.DeleteAllPages();

			// Assert
			Assert.IsTrue(Repository.AllPages().Count() == (0));
			Assert.IsTrue(Repository.AllPageContents().Count() == (0));
		}
	}
}

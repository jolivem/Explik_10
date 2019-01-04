using System;
using System.Collections.Generic;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Services
{
	public interface IPageService
	{
        /// <summary>
        /// Adds the page to the database.
        /// </summary>
        /// <param name="model">The summary details for the page.</param>
        /// <returns>A <see cref="PageViewModel"/> for the newly added page.</returns>
        /// <exception cref="DatabaseException">An database error occurred while saving.</exception>
        /// <exception cref="SearchException">An error occurred adding the page to the search index.</exception>
        PageViewModel AddPage(PageViewModel model);
        void AddSeveralPagesForTests();

        /// <summary>
        /// Retrieves a list of all pages in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An database error occurred while retrieving the list.</exception>
        IEnumerable<PageViewModel> AllPages(bool loadPageContent = false);

        /// <summary>
        /// Retrieves a list of all new pages in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An database error occurred while retrieving the list.</exception>
        IEnumerable<PageViewModel> AllNewPages(bool loadPageContent = false);

        /// <summary>
        /// Retrieves a list of all my pages in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An database error occurred while retrieving the list.</exception>
        IEnumerable<PageViewModel> MyPages(string id, bool loadPageContent = false);

        /// <summary>
        /// Retrieves the list of pages with alerts.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An database error occurred while retrieving the list.</exception>
        IEnumerable<PageViewModel> AllPagesWithAlerts(bool loadPageContent = false);

        /// <summary>
        /// Gets alls the pages created by a user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>All pages created by the provided user, or an empty list if none are found.</returns>
        /// <exception cref="DatabaseException">An database error occurred while saving.</exception>
        IEnumerable<PageViewModel> AllPagesCreatedBy(string userName);

        List<PageViewModel> PagesMostRecent(int number);
        List<PageViewModel> PagesBestRated(int number);
        List<PageViewModel> PagesMostViewed(int number);

		/// <summary>
		/// Retrieves a list of all tags in the system.
		/// </summary>
		/// <returns>A <see cref="IEnumerable{TagViewModel}"/> for the tags.</returns>
		/// <exception cref="DatabaseException">An database error occurred while getting the tags.</exception>
		IEnumerable<TagViewModel> AllTags();

        /// <summary>
        /// Deletes a page from the database.
        /// </summary>
        /// <param name="pageId">The id of the page to remove.</param>
        /// <exception cref="DatabaseException">An database error occurred while deleting the page.</exception>
        void DeletePage(int pageId);

        /// <summary>
        /// Set Draft.
        /// </summary>
        /// <param name="pageId">The id of the page to remove.</param>
        /// <exception cref="DatabaseException">An database error occurred while deleting the page.</exception>
        //void SetDraft(int pageId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId">The id of the page of the alert.</param>
        /// <exception cref="DatabaseException">An database error occurred while deleting the page.</exception>
        void AddAlert(Alert alert);

        /// <summary>
        /// Submit a page from the database.
        /// </summary>
        /// <param name="pageId">The id of the page to remove.</param>
        /// <exception cref="DatabaseException">An database error occurred while deleting the page.</exception>
        //void SubmitPage(int pageId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="controllername"></param>
        /// <param name="rating"></param>
        /// <param name="tags"></param>
        void ValidatePage(int pageId, string controllername, int rating, string tags=null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId">The id of the page to reject.</param>
        /// <exception cref="DatabaseException">An database error occurred while deleting the page.</exception>
        //void RejectPage(int pageId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId">The id of the page to reset.</param>
        /// <exception cref="DatabaseException">An database error occurred while deleting the page.</exception>
        //void DeletPageAlerts(int pageId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId">The id of the page to reset.</param>
        /// <exception cref="DatabaseException">An database error occurred while deleting the page.</exception>
        void DeletCommentAlerts(Guid commentGuid);

        /// <summary>
        /// Exports all pages in the database, including content, to an XML format.
        /// </summary>
        /// <returns>An XML string.</returns>
        /// <exception cref="DatabaseException">An database error occurred while getting the list.</exception>
        /// <exception cref="InvalidOperationException">An XML serialiation occurred exporting the page content.</exception>
        string ExportToXml();

		/// <summary>
		/// Finds all pages with the given tag.
		/// </summary>
		/// <param name="tag">The tag to search for.</param>
		/// <returns>A <see cref="IEnumerable{PageViewModel}"/> of pages tagged with the provided tag.</returns>
		/// <exception cref="DatabaseException">An database error occurred while getting the list.</exception>
		IEnumerable<PageViewModel> FindByTag(string tag);

		/// <summary>
		/// Finds the first page with the tag 'homepage'. Any pages that are locked by an administrator take precedence.
		/// </summary>
		/// <returns>The homepage.</returns>
		PageViewModel FindHomePage();

		/// <summary>
		/// Finds a page by its title
		/// </summary>
		/// <param name="title">The page title</param>
		/// <returns>A <see cref="PageViewModel"/> for the page.</returns>
		/// <exception cref="DatabaseException">An databaseerror occurred while getting the page.</exception>
		PageViewModel FindByTitle(string title);

		/// <summary>
		/// Retrieves the page by its id.
		/// </summary>
		/// <param name="id">The id of the page</param>
		/// <param name="loadContent">True if the page's content should also be loaded, which will also run all text plugins.</param>
		/// <returns>A <see cref="PageViewModel"/> for the page.</returns>
		/// <exception cref="DatabaseException">An database error occurred while getting the page.</exception>
		PageViewModel GetById(int id, bool loadContent = false);

		/// <summary>
		/// Retrieves the current text content for a page.
		/// </summary>
		/// <param name="pageId">The id of the page.</param>
		/// <returns>The <see cref="PageContent"/> for the page.</returns>
		PageContent GetCurrentContent(int pageId);

		/// <summary>
		/// Retrieves the <see cref="MarkupConverter"/> used by this IPageService.
		/// </summary>
		/// <returns></returns>
		Converters.MarkupConverter GetMarkupConverter();

		/// <summary>
		/// Renames a tag by changing all pages that reference the tag to use the new tag name.
		/// </summary>
		/// <exception cref="DatabaseException">An database error occurred while saving one of the pages.</exception>
		/// <exception cref="SearchException">An error occurred updating the search index.</exception>
		void RenameTag(string oldTagName, string newTagName);

		/// <summary>
		/// Updates all links in pages to another page, when that page's title is changed.
		/// </summary>
		/// <param name="oldTitle">The previous page title.</param>
		/// <param name="newTitle">The new page title.</param>
		void UpdateLinksToPage(string oldTitle, string newTitle);

		/// <summary>
		/// Updates the provided page.
		/// </summary>
		/// <param name="model">The page model.</param>
		/// <exception cref="DatabaseException">An database error occurred while updating.</exception>
		/// <exception cref="SearchException">An error occurred adding the page to the search index.</exception>
		void UpdatePage(PageViewModel model);

		/// <summary>
		/// Gets the menu HTML, which is stored in the database.
		/// </summary>
		string GetMenu(IUserContext userContext);

		/// <summary>
		/// Clears all pages and page content from the database.
		/// </summary>
		/// <exception cref="DatabaseException">A datastore error occurred while clearing the page data.</exception>
		void ClearPageTables();

	    /// <summary>
	    /// Get information about user activity given a pageId
	    /// </summary>
        /// <param name="username"></param>
	    /// <returns></returns>
        UserActivity GetUserActivity(string username);

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="pageId"></param>
	    /// <param name="username"></param>
	    /// <param name="rating"></param>
        void SetPageRatingForUser(int pageId, string username, int rating);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="username"></param>
        /// <param name="rating"></param>
        void SetPageCommentForUser(int pageId, string username, string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUsername"></param>
        /// <returns></returns>
        int GetPageRatingFromUser(int id, string username);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetUserIp();


    }
}

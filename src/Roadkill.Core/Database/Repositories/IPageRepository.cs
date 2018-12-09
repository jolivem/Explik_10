using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Database
{
	public interface IPageRepository
	{
        //TODO add ini of NbRatings... in AddNewPage...
		PageContent AddNewPage(Page page, string text, string editedBy, DateTime editedOn);
		PageContent AddNewPageContentVersion(Page page, string text, string editedBy, DateTime editedOn, int version);
        /// <summary>
        /// Returns a list of tags for all pages. Each item is a list of tags seperated by ,
        /// e.g. { "tag1, tag2, tag3", "blah, blah2" } 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Page> AllPages();
        IEnumerable<Page> AllNewPages();
        IEnumerable<Page> MyPages(string username);
        IEnumerable<PageContent> AllPageContents();
		IEnumerable<string> AllTags();
        void DeletePage(int pageId);
        void SetDraft(int pageId);
        void SubmitPage(int pageId);

	    void RejectPage(int pageId);
        /// <summary>
		/// Removes a single version of page contents by its id.
		/// </summary>
		/// <param name="pageContent"></param>
		void DeletePageContent(PageContent pageContent);
		void DeleteAllPages();

        IEnumerable<Page> FindMostRecentPages(int number);

	    IEnumerable<Page> FindPagesMostViewed(int number);
	    IEnumerable<Page> FindPagesBestRated(int number);


        IEnumerable<Page> FindPagesCreatedBy(string username);
        IEnumerable<Page> FindPagesWithAlerts();
        IEnumerable<Page> FindPagesControlledBy(string username);
		IEnumerable<Page> FindPagesContainingTag(string tag);
		IEnumerable<PageContent> FindPageContentsByPageId(int pageId);
		IEnumerable<PageContent> FindPageContentsEditedBy(string username);
		PageContent GetLatestPageContent(int pageId);
		Page GetPageById(int id);

		/// <summary>
		/// Case insensitive search by page title
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		Page GetPageByTitle(string title);
		PageContent GetPageContentById(Guid id);
		PageContent GetPageContentByPageIdAndVersionNumber(int id, int versionNumber);
		IEnumerable<PageContent> GetPageContentByEditedBy(string username);
		Page SaveOrUpdatePage(Page page);
		void UpdatePageContent(PageContent content); // no new version

        void IncrementNbView(int pageId);
        void SetNbView(int pageId, int nbView);
        void SetRating(int pageId, int nbRating, int totalRating);

	}
}

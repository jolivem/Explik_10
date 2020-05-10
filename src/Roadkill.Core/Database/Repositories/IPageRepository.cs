using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Database
{
	public interface IPageRepository
	{
		PageContent AddNewPage(Page page, string text, string editedBy, DateTime editedOn);
		PageContent AddNewPageContentVersion(Page page, string text, DateTime editedOn, int version);
        /// <summary>
        /// Returns a list of tags for all pages. Each item is a list of tags seperated by ,
        /// e.g. { "tag1, tag2, tag3", "blah, blah2" } 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Page> AllPages();
        IEnumerable<Page> AllNewPages();
        IEnumerable<Page> MyPages(string username);
        IEnumerable<PageContent> AllPageContents();
		//IEnumerable<string> AllTags();
        IEnumerable<string> AllControlledTags(bool checkCompetition = false);
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
        IEnumerable<Page> FindControlledPagesByCompetitionId(int competitionId);
        IEnumerable<Page> FindPagesByCompetitionId(int competitionId);
        void DeletCompetitionPages(int competitionId);
        void DeletCompetitionPage(int pageId);

        void CleanPagesForCompetitionId(int competitionId);

        IEnumerable<Page> FindPagesCreatedBy(string username);
        IEnumerable<Page> FindPagesWithAlerts();
        IEnumerable<Page> FindPagesControlledBy(string username);
		IEnumerable<Page> FindPagesContainingTag(string tag);
        IEnumerable<Page> FindControlledPagesByTag(string tag);
        IEnumerable<PageContent> FindPageContentsByPageId(int pageId);
		//IEnumerable<PageContent> FindPageContentsEditedBy(string username);
		PageContent GetLatestPageContent(int pageId);
		Page GetPageById(int id);

		/// <summary>
		/// Case insensitive search by page title
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		Page GetPageByTitle(string title);
        string GetPageTitle(int pageId);

        PageContent GetPageContentById(Guid id);
		PageContent GetPageContentByPageIdAndVersionNumber(int id, int versionNumber);
		//IEnumerable<PageContent> GetPageContentByEditedBy(string username);
		Page SaveOrUpdatePage(Page page);
		void UpdatePageContent(PageContent content); // no new version

        void IncrementNbView(int pageId);
        void SetNbView(int pageId, int nbView);
        void SetCompetitionId(int pageId, int competitionId);
        void SetRating(int pageId, int nbRating, int totalRating);
        void AddPageRating(int pageId, int rating);
        void RemovePageRating(int pageId, int rating);

    }
}

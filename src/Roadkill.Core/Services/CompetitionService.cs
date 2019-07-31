using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Roadkill.Core.Mvc.ViewModels;
using static Roadkill.Core.Mvc.ViewModels.CompetitionViewModel;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Services
{
    public class CompetitionService : ServiceBase, ICompetitionService
    {
        private MarkupConverter _markupConverter;
        private PageService _pageService;
        private SearchService _searchService;

        public CompetitionService(ApplicationSettings settings, IRepository repository, MarkupConverter markupConverter,
            PageService pageService, SearchService searchService)
            : base(settings, repository)
        {
            _markupConverter = markupConverter;
            _pageService = pageService;
            _searchService = searchService;
        }


        private bool AddUserForFake(string email, string username)
        {
            try
            {
                User user = Repository.GetUserByUsernameOrEmail(username, email);
                if (user == null)
                {
                    user = new User();
                    user.Email = email;
                    user.Username = username;
                    user.SetPassword(username);
                    user.IsAdmin = false;
                    user.IsEditor = true;
                    user.IsController = false;
                    user.AttachmentsPath = DateTime.UtcNow.ToString("yyyy-MM") + "/" + username;
                    user.IsBlackListed = false;
                    user.IsActivated = true;
                    Repository.SaveOrUpdateUser(user);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (DatabaseException ex)
            {
                throw new SecurityException(ex, "An error occurred while adding the new user {0}", email);
            }
        }
        /// <summary>
        /// Add a competition
        /// </summary>
        /// <param name="model"></param>
        public void AddCompetition(CompetitionViewModel model, bool debug=false)
        {
            try
            {
                Competition competition = new Competition();
                competition.Status = (int)model.Status;
                competition.PublicationStart = model.PublicationStart;
                competition.PublicationStop = model.PublicationStop;
                competition.RatingStart = model.RatingStart;
                competition.RatingStop = model.RatingStop;
                competition.PageTag = model.PageTag;
                competition.PageId = GetPageIdFromTag(model.Id, model.PageId, model.PageTag);

                Repository.AddCompetition(competition);

                competition = Repository.GetCompetitionByPageTag(model.PageTag);

                if (debug)
                {

                    // add users
                    AddUserForFake("u0", "u0@e");
                    AddUserForFake("u1", "u1@e");
                    AddUserForFake("u2", "u2@e");

                    //add competition page
                    Page page = new Page();
                    page.CreatedBy ="Admin";
                    page.CreatedOn = DateTime.Now;
                    page.Tags = model.PageTag;
                    page.IsLocked = true;
                    page.IsControlled = true;
                    page.CompetitionId = competition.Id;
                    page.Title = "Nouvelle competition 2";
                    
                    Repository.AddNewPage(page, "Bienvenue pour cette noubelle competition", "Admin", DateTime.UtcNow);

                    //add participation pages
                    page = new Page();
                    page.CreatedBy = "u0";
                    page.CreatedOn = DateTime.Now;
                    page.Tags = "tag1, tag2";
                    page.IsLocked = false;
                    page.IsControlled = false;
                    page.Title = "Titre ma clé";
                    Repository.AddNewPage(page, "clé not controllée 21", "Admin", DateTime.UtcNow);

                    //add participation pages
                    page = new Page();
                    page.CreatedBy = "u1";
                    page.CreatedOn = DateTime.Now;
                    page.Tags = "tag1, tag2";
                    page.IsLocked = false;
                    page.IsControlled = true;
                    page.Title = "Titre ma clé";
                    Repository.AddNewPage(page, "clé controllée 22", "u1", DateTime.UtcNow);

                    //add participation pages
                    page = new Page();
                    page.CreatedBy = "u2";
                    page.CreatedOn = DateTime.Now;
                    page.Tags = "tag3";
                    page.IsLocked = false;
                    page.IsControlled = true;
                    page.Title = "Titre ma clé";
                    Repository.AddNewPage(page, "clé controllée 23", "u2", DateTime.UtcNow);

                }
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while updating a competition.");
            }
        }

        /// <summary>
        /// Update the dates of the competition, or th state
        /// </summary>
        /// <param name="model"></param>
        public void UpdateCompetition(CompetitionViewModel model)
        {
            try
            {
                Competition competition = Repository.GetCompetitionById(model.Id);
                Statuses oldStatus = (Statuses)competition.Status;
                competition.Status = (int)model.Status;
                competition.PublicationStart = model.PublicationStart;
                competition.PublicationStop = model.PublicationStop;
                competition.RatingStart = model.RatingStart;
                competition.RatingStop = model.RatingStop;
                competition.PageTag = model.PageTag;
                competition.PageId = GetPageIdFromTag(model.Id, model.PageId, model.PageTag);

                Repository.UpdateCompetition(competition);

                if ((oldStatus == Statuses.PauseBeforeAchieved || oldStatus == Statuses.RatingOngoing) && 
                    competition.Status == (int)Statuses.Achieved)
                {
                    Achieve(model.Id);
                    return;
                }

            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while updating a competition.");
            }
        }

        /// <summary>
        /// Handles pageId from pageTag, save pageId if not done
        /// </summary>
        /// <param name="competitionId"></param>
        /// <param name="pageId"></param>
        /// <param name="pageTag"></param>
        /// <returns></returns>
        private int GetPageIdFromTag(int competitionId, int pageId, string pageTag)
        {
            if (pageId != -1)
            {
                // already set and saved
                return pageId;
            }

            try
            {
                Page page = Repository.FindPagesContainingTag(pageTag).Single();
                if (page != null)
                {
                    // recently set but not saved
                    Repository.UpdateCompetitionPageId(competitionId, page.Id);
                    return page.Id;
                }
            }
            catch (Exception exception)
            {
                return -1;
            }

            // not set yet
            return -1;
        }

        /// <summary>
        /// Get all compeitions for display
        /// </summary>
        /// <returns></returns>
        public List<CompetitionViewModel> GetCompetitions( bool forAdmin = false)
        {
            try
            {
                IEnumerable<Competition> list = Repository.GetCompetitions(forAdmin);
                if (list != null)
                {
                    List<CompetitionViewModel> competitionModels;

                    competitionModels = (from competition in list
                        select new CompetitionViewModel(competition)).ToList();

                    competitionModels.Sort(new ComparerByDate());

                    // fill pageId
                    foreach (var model in competitionModels)
                    {
                        SetPageInfo(model);
                    }

                    return competitionModels;
                }

                return null;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while getting all competitions.");
            }
        }

        /// <summary>
        /// Set Page info (Id, text and Title) from page tag
        /// </summary>
        /// <param name="model"></param>
        void SetPageInfo(CompetitionViewModel model)
        {
            try
            {
                int pageId = GetPageIdFromTag(model.Id, model.PageId, model.PageTag);
                if (pageId != -1)
                {
                    Page page = Repository.GetPageById(pageId);
                    model.PageId = page.Id;
                    model.PageTitle = page.Title;
                    model.PageText = Repository.GetLatestPageContent(page.Id).Text;
                }
            }
            catch (Exception exception)
            {
                model.PageId = -1;
                model.PageTitle = "";
                model.PageText = "";
            }
        }

        /// <summary>
        /// Get a CompetitionViewModel knowing its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompetitionViewModel GetById(int id)
        {
            try
            {
                Competition competition = Repository.GetCompetitionById(id);
                if (competition == null)
                {
                    return null;
                }

                CompetitionViewModel model = new CompetitionViewModel(competition);
                SetPageInfo(model); // presentation page
                 
                return model;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, $"An exception occurred while getting competitions id = {id}");
            }
        }

        /// <summary>
        /// Get a CompetitionViewModel knowing its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<PageViewModel> GetCompetitionPages(int id)
        {
            try
            {
                List<PageViewModel> model = new List<PageViewModel>();

                List<CompetitionPage> competitionPages = Repository.GetCompetitionPages(id).ToList();
                if (competitionPages.Count > 0)
                {
                    // Get all pages and sort according to ratings
                    IComparer<CompetitionPage> comparer = new ComparerByRating();
                    competitionPages.Sort(comparer);
                    foreach (CompetitionPage competitionPage in competitionPages)
                    {
                        // check that page is still present (has no been deleted)
                        if (Repository.GetPageById(competitionPage.PageId) != null)
                        {
                            PageViewModel page = new PageViewModel(Repository.GetLatestPageContent(competitionPage.PageId), _markupConverter);

                            // Replace current rating of the page by the one at the end of the competition
                            page.NbRating = competitionPage.NbRating;
                            page.TotalRating = competitionPage.TotalRating;
                            page.Ranking = Repository.GetPageRanking(competitionPage.PageId);
                            model.Add(page);
                        }
                    }
                }
                return model;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, $"An exception occurred while getting competitions id = {id}");
            }
        }

        /// <summary>
        /// Start rating:
        /// Fill the list of pages that have competited
        /// </summary>
        /// <param name="competitionId"></param>
        //public void StartRating(int competitionId)
        //{
        //    try
        //    {
        //        // Clean competition id (remove if the page has not been controlled)
        //        //Repository.CleanPagesForCompetitionId(competitionId);

        //        Competition competition = Repository.GetCompetitionById(competitionId);
        //        if (competition == null)
        //        {
        //            return;
        //        }
 
        //    }

        //    catch (DatabaseException ex)
        //    {
        //        throw new DatabaseException(ex, $"An exception occurred while starting rating of competitions id = {competitionId}");
        //    }

        //}

        ///// <summary>
        ///// Start rating:
        ///// Fill the list of pages that have competited
        ///// </summary>
        ///// <param name="competitionId"></param>
        //public void StartPublication(int competitionId)
        //{
        //    try
        //    {
        //        Competition competition = Repository.GetCompetitionById(competitionId);
        //        if (competition == null)
        //        {
        //            return;
        //        }

        //        // Change status to achieved
        //        competition.Status = (int)CompetitionViewModel.Statuses.PublicationOngoing;
        //        Repository.UpdateCompetition(competition);
        //    }

        //    catch (DatabaseException ex)
        //    {
        //        throw new DatabaseException(ex, $"An exception occurred while starting publication of competitions id = {competitionId}");
        //    }

        //}

        /// <summary>
        /// Achieve a competition:
        /// . remove pages that have not been validated
         /// </summary>
        /// <param name="competitionId"></param>
        public void Achieve(int competitionId)
        {
            try
            {
                
                Competition competition = Repository.GetCompetitionById(competitionId);
                if (competition == null)
                {
                    return;
                }

                // Remove previous achievement if any
                Repository.DeletCompetitionPages(competitionId);

                List<Page> pages = Repository.FindPagesByCompetitionId(competitionId).ToList();
                if (pages.Count > 0)
                {
                    foreach (Page page in pages)
                    {
                        // Fill the CompetitionPage table with all the pages that have competitited
                        // so that they can be viewed by visitors
                        Repository.ArchiveCompetitionPage(competitionId, page);

                        // Add the pages to Lucene search engine
                        PageViewModel pageModel = _pageService.GetById(page.Id, true);
                        try
                        {
                            _searchService.Add(pageModel);
                        }
                        catch (SearchException)
                        {
                            // TODO: log
                        }

                    }
                }
                
                // Set ranking
                List<CompetitionPage> competitionPages = Repository.GetCompetitionPages(competitionId).ToList();
                if (competitionPages.Count > 0)
                {
                    int ranking = 1;
                    ComparerByRating comparer = new ComparerByRating();
                    competitionPages.Sort(comparer);

                    foreach (var competitionPage in competitionPages)
                    {
                        Page page = Repository.GetPageById(competitionPage.PageId);
                        if (page != null)
                        {
                            Repository.UpdateCompetitionPageRanking(competitionId, page.Id, ranking);
                        }

                        ranking++;
                    }
                }
            }

            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, $"An exception occurred while achieving competitions id = {competitionId}");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public Competition GetCompetitionByStatus(CompetitionViewModel.Statuses status)
        {
            try
            {
                 return Repository.GetCompetitionByStatus((int) status);
            }
            catch ( Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ComparerByRating : IComparer<CompetitionPage>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0 if equal ; 1 if x > y ; -1 if y > x</returns>
        public int Compare(CompetitionPage x, CompetitionPage y)
        {
            // both are not rated
            if (x.NbRating == 0 && y.NbRating == 0)
            {
                return 0;
            }

            // x is not rated
            if (x.NbRating == 0)
            {
                return -1;
            }

            // y is not rated
            if (y.NbRating == 0)
            {
                return 1;
            }

            // both are rated

            double xRate = (double)x.TotalRating / (double)x.NbRating;
            double yRate = (double)y.TotalRating / (double)y.NbRating;
            return yRate.CompareTo(xRate);
        }
    }
    ///// <summary>
    ///// 
    ///// </summary>
    //internal class ComparerByPageId : IComparer<CompetitionPage>
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="x"></param>
    //    /// <param name="y"></param>
    //    /// <returns>0 if equal ; 1 if x > y ; -1 if y > x</returns>
    //    public int Compare(CompetitionPage x, CompetitionPage y)
    //    {
    //        return x.PageId.CompareTo(y.PageId);
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    internal class ComparerByRanking : IComparer<CompetitionPage>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0 if equal ; 1 if x > y ; -1 if y > x</returns>
        public int Compare(CompetitionPage x, CompetitionPage y)
        {
            return x.Ranking.CompareTo(y.Ranking);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ComparerByDate : IComparer<CompetitionViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0 if equal ; 1 if x > y ; -1 if y > x</returns>
        public int Compare(CompetitionViewModel x, CompetitionViewModel y)
        {
            return y.PublicationStart.CompareTo(x.PublicationStart);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Roadkill.Core.Converters;
using Roadkill.Core.Database;
using Roadkill.Core.Cache;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Configuration;
using System.Web;
using System.Windows.Forms;

using Lucene.Net.Support;

using Roadkill.Core.Attachments;
using Roadkill.Core.Logging;
using Roadkill.Core.Mvc;
using Roadkill.Core.Text;
using Roadkill.Core.Plugins;
using static Roadkill.Core.Mvc.ViewModels.CompetitionViewModel;

namespace Roadkill.Core.Services
{
    /// <summary>
    /// Provides a set of tasks for wiki page management.
    /// </summary>
    public class PageService : ServiceBase, IPageService
    {
        private SearchService _searchService;
        private MarkupConverter _markupConverter;
        private PageHistoryService _historyService;
        private IUserContext _context;
        private ListCache _listCache;
        private PageViewModelCache _pageViewModelCache;
        private SiteCache _siteCache;
        private IPluginFactory _pluginFactory;
        private MarkupLinkUpdater _markupLinkUpdater;

        public PageService(ApplicationSettings settings, IRepository repository, SearchService searchService,
            PageHistoryService historyService, IUserContext context,
            ListCache listCache, PageViewModelCache pageViewModelCache, SiteCache sitecache, IPluginFactory pluginFactory)
            : base(settings, repository)
        {
            _searchService = searchService;
            _markupConverter = new MarkupConverter(settings, repository, pluginFactory);
            _historyService = historyService;
            _context = context;
            _listCache = listCache;
            _pageViewModelCache = pageViewModelCache;
            _siteCache = sitecache;
            _pluginFactory = pluginFactory;
            _markupLinkUpdater = new MarkupLinkUpdater(_markupConverter.Parser);

            //            foreach (Page page in pages)
            //{
            //     page.FilePath = _attachmentPathUtil.ConvertUrlPathToPhysicalPath

            //}
            //TODO change file path
        }

        /// <summary>
        /// Adds the page to the database.
        /// </summary>
        /// <param name="model">The summary details for the page.</param>
        /// <returns>A <see cref="PageViewModel"/> for the newly added page.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while saving.</exception>
        /// <exception cref="SearchException">An error occurred adding the page to the search index.</exception>
        public PageViewModel AddPage(PageViewModel model)
        {
            try
            {
                string currentUser = _context.CurrentUsername;

                Page page = new Page();
                page.Title = model.Title;
                page.Tags = model.CommaDelimitedTags();
                page.CreatedBy = AppendIpForDemoSite(currentUser);
                page.CreatedOn = DateTime.UtcNow;
                page.PublishedOn = DateTime.UtcNow;
                page.ControlledBy = AppendIpForDemoSite(currentUser);
                page.ControllerRating = model.ControllerRating;
                page.IsControlled = false;
                page.IsRejected = false;
                page.IsSubmitted = false;
                page.VideoUrl = model.VideoUrl;
                page.Pseudonym = model.Pseudonym;
                page.FilePath = model.FilePath; // attachment path + partial guid

                Competition competition = Repository.GetCompetitionByStatus((int)CompetitionViewModel.Statuses.PublicationOngoing);
                if (model.IsInCompetition && competition != null)
                {
                    page.CompetitionId = competition.Id;
                }
                else
                {
                    page.CompetitionId = -1;
                }


                // Double check, incase the HTML form was faked.
                if (_context.IsAdmin)
                    page.IsLocked = model.IsLocked;

                PageContent pageContent = Repository.AddNewPage(page, model.Content, AppendIpForDemoSite(currentUser), DateTime.UtcNow);

                _listCache.RemoveAll();
                _pageViewModelCache.RemoveAll(); // completely clear the cache to update any reciprocal links.

                // Update the lucene index
                PageViewModel savedModel = new PageViewModel(pageContent, _markupConverter);

                //Update Lucene only when controlled
                //try
                //{
                //    _searchService.Add(savedModel);
                //}
                //catch (SearchException)
                //{
                //    // TODO: log
                //}

                return savedModel;
            }
            catch (DatabaseException e)
            {
                throw new DatabaseException(e, "An error occurred while adding page '{0}' to the database", model.Title);
            }
        }

        /// <summary>
        /// Adds the page to the database.
        /// </summary>
        /// <param name="model">The summary details for the page.</param>
        /// <returns>A <see cref="PageViewModel"/> for the newly added page.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while saving.</exception>
        /// <exception cref="SearchException">An error occurred adding the page to the search index.</exception>
        public void AddSeveralPagesForTests()
        {
            try
            {
                for (int i = 1; i < 10; i++)
                {

                    string currentUser = "user" + i;
                    for (int p = 1; p < 5; p++)
                    {
                        Page page = new Page();
                        page.Title = "Title of this wondefull page n° " + p;
                        //page.Summary = "This is a short summary to say that it is a page test and nothing more";
                        page.Tags = "tage" + i;
                        page.CreatedBy = AppendIpForDemoSite(currentUser);
                        page.CreatedOn = DateTime.UtcNow;
                        page.PublishedOn = DateTime.UtcNow;
                        page.ControlledBy = AppendIpForDemoSite(currentUser);
                        page.FilePath = DateTime.UtcNow.ToString("yyyy-MM") + "/" + _context.CurrentUsername;


                        // Double check, incase the HTML form was faked.
                        //if (_context.IsAdmin)
                        //    page.IsLocked = model.IsLocked;

                        var content = "Le mot coucou est un terme du vocabulaire courant qui peut désigner différentes espèces d'oiseaux ayant généralement " +
                            "un chant qui correspond à l'onomatopée « coucou ». Ce nom ne correspond donc pas à un niveau précis de la classification scientifique " +
                            "des espèces. Autrement dit, il s'agit d'un nom vernaculaire dont le sens est ambigu en biologie car il désigne une partie seulement " +
                            "des espèces appartenant soit à la sous-famille des Cuculinae (coucous de l'Ancien Monde), soit à celle des Coccyzinae " +
                            "(coucous du Nouveau Monde). Le plus souvent toutefois, en disant « coucou » les francophones font référence au Coucou gris(Cuculus canorus). ";

                        PageContent pageContent = Repository.AddNewPage(page, content, AppendIpForDemoSite(currentUser), DateTime.UtcNow);

                        //nb view
                        Random r = new Random();
                        Repository.SetNbView(pageContent.Page.Id, r.Next(0, 100000));

                        //rating
                        Repository.SetRating(pageContent.Page.Id, 10, r.Next(10, 500));

                        ValidatePage(pageContent.Page.Id, "Controller", 3, false, "");

                        _listCache.RemoveAll();
                        _pageViewModelCache.RemoveAll(); // completely clear the cache to update any reciprocal links.

                        // Update the lucene index
                        PageViewModel savedModel = new PageViewModel(pageContent, _markupConverter);
                        try
                        {
                            _searchService.Add(savedModel);
                        }
                        catch (SearchException)
                        {
                            // TODO: log
                        }
                    }
                }

                return;
            }
            catch (DatabaseException e)
            {
                throw new DatabaseException(e, "An error occurred while adding page to the database");
            }
        }

        /// <summary>
        /// Retrieves a list of all pages in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while retrieving the list.</exception>
        public IEnumerable<PageViewModel> AllPages(bool loadPageContent = false)
        {
            try
            {
                string cacheKey = "";
                IEnumerable<PageViewModel> pageModels;

                if (loadPageContent)
                {
                    cacheKey = CacheKeys.AllPagesWithContent();
                    pageModels = _listCache.Get<PageViewModel>(cacheKey);

                    if (pageModels == null)
                    {
                        IEnumerable<Page> pages = Repository.AllPages().OrderBy(p => p.Title);
                        pageModels = from page in pages
                                     select new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);

                        _listCache.Add<PageViewModel>(cacheKey, pageModels);
                    }
                }
                else
                {
                    cacheKey = CacheKeys.AllPages();
                    pageModels = _listCache.Get<PageViewModel>(cacheKey);

                    if (pageModels == null)
                    {
                        IEnumerable<Page> pages = Repository.AllPages().OrderBy(p => p.Title);
                        pageModels = from page in pages
                                     select new PageViewModel() { Id = page.Id, Title = page.Title };

                        _listCache.Add<PageViewModel>(cacheKey, pageModels);
                    }
                }

                return pageModels;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving all pages from the database");
            }
        }

        /// <summary>
        /// Retrieves a list of all pages in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while retrieving the list.</exception>
        public IEnumerable<PageViewModel> AllNewPages(bool loadPageContent = false)
        {
            try
            {
                string cacheKey = "";
                IEnumerable<PageViewModel> pageModels;

                if (loadPageContent)
                {
                    cacheKey = CacheKeys.AllPagesWithContent();
                    pageModels = _listCache.Get<PageViewModel>(cacheKey);

                    if (pageModels == null)
                    {
                        IEnumerable<Page> pages = Repository.AllNewPages().OrderBy(p => p.Title);
                        pageModels = from page in pages
                                     select new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);

                        _listCache.Add<PageViewModel>(cacheKey, pageModels);
                    }
                }
                else
                {
                    cacheKey = CacheKeys.AllNewPages();
                    pageModels = _listCache.Get<PageViewModel>(cacheKey);

                    if (pageModels == null)
                    {
                        IEnumerable<Page> pages = Repository.AllNewPages().OrderBy(p => p.Title);
                        pageModels = from page in pages
                                     select new PageViewModel() { Id = page.Id, Title = page.Title };

                        _listCache.Add<PageViewModel>(cacheKey, pageModels);
                    }
                }

                return pageModels;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving all new pages from the database");
            }
        }

        /// <summary>
        /// Retrieves a list of all pages in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while retrieving the list.</exception>
        //public IEnumerable<PageViewModel> AllPagesWithAlerts(bool loadPageContent = false)
        //{
        //    try
        //    {
        //        IEnumerable<PageViewModel> pageModels;

        //        IEnumerable<Page> pages = Repository.FindPagesWithAlerts();
        //        pageModels = from page in pages select new PageViewModel(page);

        //        return pageModels;
        //    }
        //    catch (DatabaseException ex)
        //    {
        //        throw new DatabaseException(ex, "An error occurred while retrieving all new pages from the database");
        //    }
        //}

        /// <summary>
        /// Retrieves a list of all pages in the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while retrieving the list.</exception>
        public IEnumerable<PageViewModel> MyPages(string id)
        {
            try
            {
                string cacheKey = "";
                IEnumerable<PageViewModel> pageModels;

                cacheKey = CacheKeys.MyPages();
                pageModels = _listCache.Get<PageViewModel>(cacheKey);

                if (pageModels == null)
                {
                    IEnumerable<Page> pages = Repository.MyPages(id).OrderByDescending(p => p.Id);
                    pageModels = from page in pages
                                 select new PageViewModel(page);

                    _listCache.Add<PageViewModel>(cacheKey, pageModels);
                }

                return pageModels;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving my pages from the database");
            }
        }

        /// <summary>
        /// Gets alls the pages created by a user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>All pages created by the provided user, or an empty list if none are found.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while retrieving the list.</exception>
        public IEnumerable<PageViewModel> AllPagesCreatedBy(string userName)
        {
            try
            {
                string cacheKey = string.Format("allpages.createdby.{0}", userName);

                IEnumerable<PageViewModel> models = _listCache.Get<PageViewModel>(cacheKey);
                if (models == null)
                {
                    IEnumerable<Page> pages = Repository.FindPagesCreatedBy(userName);
                    models = from page in pages
                             select new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);

                    _listCache.Add<PageViewModel>(cacheKey, models);
                }

                return models;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving all pages created by {0} from the database", userName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<PageViewModel> PagesMostRecent(int number)
        {
            List<PageViewModel> models = new List<PageViewModel>();
            IEnumerable<Page> pages = Repository.FindMostRecentPages(number);
            foreach (Page page in pages)
            {
                // ignore pages whose competition is ongoing, add only if competition is Achived
                //if (page.CompetitionId != -1)
                //{
                //    Competition competition = Repository.GetCompetitionById(page.CompetitionId);
                //    if (competition.Status == (int)Statuses.Achieved)
                //    {
                //        models.Add(GetById(page.Id, true));
                //    }
                //}
                //else
                //{
                    models.Add(GetById(page.Id, true));
                //}

            }
            return models;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<PageViewModel> PagesBestRated(int number)
        {
            List<PageViewModel> models = new List<PageViewModel>();
            IEnumerable<Page> pages = Repository.FindPagesBestRated(number);
            foreach (Page page in pages)
            {
                models.Add(GetById(page.Id, true));
            }
            return models;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<PageViewModel> PagesMostViewed(int number)
        {
            List<PageViewModel> models = new List<PageViewModel>();
            IEnumerable<Page> pages = Repository.FindPagesMostViewed(number);
            foreach (Page page in pages)
            {
                models.Add(GetById(page.Id, true));
            }
            return models;
        }

        /// <summary>
        /// Retrieves a list of all tags in the system.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{TagViewModel}"/> for the tags.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while getting the tags.</exception>
        public IEnumerable<TagViewModel> AllControlledTags(bool checkCompetition = false)
        {
            try
            {
                string cacheKey = "alltags";

                List<TagViewModel> tags = _listCache.Get<TagViewModel>(cacheKey);
                if (tags == null)
                {
                    IEnumerable<string> tagList = Repository.AllControlledTags(checkCompetition);
                    tags = new List<TagViewModel>();

                    foreach (string item in tagList)
                    {
                        foreach (string tagName in PageViewModel.ParseTags(item))
                        {
                            if (!string.IsNullOrEmpty(tagName))
                            {
                                // tags starting with "___" are reserved (___contact, ___about, ___privacy, ...)
                                if (tagName.Length >= 2 && tagName.Substring(0, 2) != "__")
                                {
                                    TagViewModel tagModel = new TagViewModel(tagName);
                                    int index = tags.IndexOf(tagModel);

                                    if (index < 0)
                                    {
                                        tags.Add(tagModel);
                                    }
                                    else
                                    {
                                        tags[index].Count++;
                                    }
                                }
                            }
                        }
                    }

                    //_listCache.Add<TagViewModel>(cacheKey, tags);
                }

                return tags;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving all tags from the database");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        public void SetDraft(int pageId)
        {
            PageViewModel model = GetById(pageId, true);
            if (model.IsPublished)
            {
                // update index
                try
                {
                    _searchService.Delete(model);

                    //update caches

                    // remove tag caches (no matter the other pages for the tag)
                    foreach (string tag in model.Tags)
                    {
                        if (!string.IsNullOrWhiteSpace(tag))
                        {
                            string cacheKey = CacheKeys.PagesByTagKey(tag);
                            _listCache.Remove(cacheKey);
                        }
                    }

                    // remove page

                }
                catch (SearchException ex)
                {
                    Log.Error(ex, "Unable to delete page with id {0} from the lucene index", pageId);
                }
            }
            Repository.SetDraft(pageId);
        }


        /// <summary>
        /// Deletes a page from the database.
        /// </summary>
        /// <param name="pageId">The id of the page to remove.</param>
        /// <exception cref="DatabaseException">An databaseerror occurred while deleting the page.</exception>
        public void DeletePage(int pageId)
        {
            try
            {
                // Avoid grabbing all the pagecontents coming back each time a page is requested, it has no inverse relationship.
                Page page = Repository.GetPageById(pageId);

                // Update the lucene index before we actually delete the page.
                try
                {
                    PageViewModel model = new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
                    _searchService.Delete(model);
                }
                catch (SearchException ex)
                {
                    Log.Error(ex, "Unable to delete page with id {0} from the lucene index", pageId);
                }

                IList<PageContent> children = Repository.FindPageContentsByPageId(pageId).ToList();
                for (int i = 0; i < children.Count; i++)
                {
                    Repository.DeletePageContent(children[i]);
                }

                Repository.DeletePage(pageId);

                // Remove everything for now, to avoid reciprocal link issues
                _listCache.RemoveAll();
                _pageViewModelCache.RemoveAll();

                // Remove from competitions
                Repository.DeletCompetitionPage(pageId);

                // Remove comments and ratings
                Repository.DeleteComments(pageId);

                // Remove  alerts
                Repository.DeletePageAlerts(pageId);

                // Remove from courses
                Repository.DeleteCoursePagesforPageId(pageId);

            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while deleting the page id {0} from the database", pageId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="controllerName"></param>
        /// <param name="controllerRating"></param>
        /// <param name="isInCompetition">-1 if not involved in a competition</param>
        /// <param name="tags"></param>
        public void ValidatePage(int pageId, string controllerName, int controllerRating, bool isInCompetition, string tags = null)
        {
            try
            {
                Page page = Repository.GetPageById(pageId);
                page.IsControlled = true;
                page.IsRejected = false;
                page.IsSubmitted = false;
                page.ControlledBy = controllerName;
                page.PublishedOn = DateTime.UtcNow;
                //TODO check competition

                // Nb view,
                // after validation, nb view = min 2
                if (page.NbView < 2)
                {
                    page.NbView = 2;
                }

                // Controller Rating
                // take ControllerRating into account only if there is no rating yet, i.e. first control
                // and if the controller has set the rating
                if (page.NbRating == 0 && controllerRating > 0)
                {
                    page.ControllerRating = controllerRating;
                    page.TotalRating = controllerRating;
                    page.NbRating = 1;
                }

                if (tags != null)
                {
                    page.Tags = tags; //TODO add controller tags to user tags, dont ecrase
                }

                // save competition Id if participation
                page.CompetitionId = -1;
                if (isInCompetition)
                {
                    var competition = Repository.GetCompetitionByStatus((int)CompetitionViewModel.Statuses.PublicationOngoing);
                    if (competition != null)
                    {

                        page.CompetitionId = competition.Id;
                    }
                }

                Repository.SaveOrUpdatePage(page);

                //Update Lucene only when controlled, but not in competition (see CompetitionService.Achieve()
                if (!isInCompetition)
                {
                    PageViewModel model = GetById(pageId, true);
                    try
                    {
                        _searchService.Add(model);
                    }
                    catch (SearchException)
                    {
                        // TODO: log
                    }
                }
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while validating the page id {0} from the database", pageId);
            }
        }

        /// <summary>
        /// Validates a page from the database.
        /// </summary>
        /// <param name="pageId">The id of the page to validate.</param>
        /// <exception cref="DatabaseException">An databaseerror occurred while deleting the page.</exception>
        public void RejectPage(int pageId)
        {
            try
            {
                // update Lucene index
                PageViewModel model = GetById(pageId, true);
                if (model.IsPublished)
                {
                    try
                    {
                        _searchService.Delete(model);
                    }
                    catch (SearchException)
                    {
                        // TODO: log
                    }
                }

                Repository.RejectPage(pageId);

            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while validating the page id {0} from the database", pageId);
            }
        }

        public void DeletCommentAlerts(Guid commentGuid)
        {
            return;
        }

        /// <summary>
        /// Exports all pages in the database, including content, to an XML format.
        /// </summary>
        /// <returns>An XML string.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while getting the list.</exception>
        /// <exception cref="InvalidOperationException">An XML serialiation occurred exporting the page content.</exception>
        public string ExportToXml()
        {
            try
            {
                List<PageViewModel> list = AllPages().ToList();

                XmlSerializer serializer = new XmlSerializer(typeof(List<PageViewModel>));

                StringBuilder builder = new StringBuilder();
                using (StringWriter writer = new StringWriter(builder))
                {
                    serializer.Serialize(writer, list);
                    return builder.ToString();
                }
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "A database error occurred while exporting the pages to XML");
            }
        }

        /// <summary>
        /// Finds the first page with the tag 'homepage'. Any pages that are locked by an administrator take precedence.
        /// </summary>
        /// <returns>The homepage.</returns>
        //public PageViewModel FindHomePage()
        //{
        //    try
        //    {
        //        PageViewModel pageModel = _pageViewModelCache.GetHomePage();
        //        if (pageModel == null)
        //        {

        //            Page page = Repository.FindPagesContainingTag("homepage").FirstOrDefault(x => x.IsLocked == true);
        //            if (page == null)
        //            {
        //                page = Repository.FindPagesContainingTag("homepage").FirstOrDefault();
        //            }

        //            if (page != null)
        //            {
        //                pageModel = new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
        //                _pageViewModelCache.UpdateHomePage(pageModel);
        //            }
        //        }

        //        return pageModel;
        //    }
        //    catch (DatabaseException ex)
        //    {
        //        throw new DatabaseException(ex, "An error occurred finding the tag 'homepage' in the database");
        //    }
        //}

        /// <summary>
        /// Finds the first page with the tag 'homepage'. Any pages that are locked by an administrator take precedence.
        /// </summary>
        /// <returns>The homepage.</returns>
        public PageViewModel FindPageWithTag(string tag)
        {
            try
            {
                PageViewModel pageModel = _pageViewModelCache.GetPageWithTag(tag);
                if (pageModel == null)
                {

                    Page page = Repository.FindPagesContainingTag(tag).FirstOrDefault(x => x.IsLocked == true);
                    if (page == null)
                    {
                        page = Repository.FindPagesContainingTag(tag).FirstOrDefault();
                    }

                    if (page != null)
                    {
                        pageModel = new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
                        _pageViewModelCache.UpdatePageWithTag(tag, pageModel);
                    }
                }

                return pageModel;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred finding the tag 'homepage' in the database");
            }
        }
        /// <summary>
        /// Finds all pages with the given tag.
        /// </summary>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>A <see cref="IEnumerable{PageViewModel}"/> of pages tagged with the provided tag.</returns>
        /// <exception cref="DatabaseException">An database error occurred while getting the list.</exception>
        public IEnumerable<PageViewModel> FindByTag(string tag)
        {
            try
            {
                string cacheKey = string.Format("pagesbytag.{0}", tag);

                IEnumerable<PageViewModel> models = _listCache.Get<PageViewModel>(cacheKey);
                if (models == null)
                {

                    IEnumerable<Page> pages = Repository.FindPagesContainingTag(tag).OrderBy(p => p.Title);
                    models = from page in pages
                             select new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);

                    _listCache.Add<PageViewModel>(cacheKey, models);
                }

                return models;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred finding the tag '{0}' in the database", tag);
            }
        }

        /// <summary>
        /// Finds controlled pages with the given tag.
        /// </summary>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>A <see cref="IEnumerable{PageViewModel}"/> of pages tagged with the provided tag.</returns>
        /// <exception cref="DatabaseException">An database error occurred while getting the list.</exception>
        public IEnumerable<PageViewModel> FindControlledPagesByTag(string tag)
        {
            try
            {
                string cacheKey = string.Format("pagesbytag.{0}", tag);

                IEnumerable<PageViewModel> models = _listCache.Get<PageViewModel>(cacheKey);
                if (models == null)
                {

                    IEnumerable<Page> pages = Repository.FindControlledPagesByTag(tag).OrderBy(p => p.Title);
                    models = from page in pages
                             select new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);

                    _listCache.Add<PageViewModel>(cacheKey, models);
                }

                return models;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred finding the tag '{0}' in the database", tag);
            }
        }
        /// <summary>
        /// Finds a page by its title
        /// </summary>
        /// <param name="title">The page title</param>
        /// <returns>A <see cref="PageViewModel"/> for the page.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while getting the page.</exception>
        public PageViewModel FindByTitle(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                    return null;

                Page page = Repository.GetPageByTitle(title);

                if (page == null)
                    return null;
                else
                    return new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred finding the page with title '{0}' in the database", title);
            }
        }

        /// <summary>
        /// Retrieves the page by its id.
        /// </summary>
        /// <param name="id">The id of the page</param>
        /// <returns>A <see cref="PageViewModel"/> for the page.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while getting the page.</exception>
        public PageViewModel GetById(int id, bool loadContent = false)
        {
            try
            {
                //TODO use cache !!!!!
                //PageViewModel pageModel = _pageViewModelCache.Get(id);
                //if (pageModel != null)
                //{
                //    return pageModel;
                //}
                //else
                PageViewModel pageModel;
                if (true)
                {
                    Page page = Repository.GetPageById(id);

                    if (page == null)
                    {
                        return null;
                    }
                    else
                    {
                        // If object caching is enabled, ignore the "loadcontent" parameter as the cache will be 
                        // used on the second call anyway, so performance isn't an issue.
                        if (ApplicationSettings.UseObjectCache)
                        {
                            pageModel = new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
                        }
                        else
                        {
                            if (loadContent)
                            {
                                pageModel = new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
                            }
                            else
                            {
                                pageModel = new PageViewModel(page);
                            }
                        }

                        // needed for display through url /wiki/Id
                        pageModel.AllComments = FindAllCommentByPage(id);
                        pageModel.Ranking = Repository.GetPageRanking(id);
                        pageModel.UserHits = Repository.GetUserHits(page.CreatedBy);

                        // get courses of the page
                        var courses = Repository.FindCoursesByPageId(page.Id);
                        pageModel.Courses = (from course in courses
                                            select new CourseViewModel(course)).ToList();

                        //CoursePage coursePage = coursePages.SingleOrDefault(x => x.PageId == page.Id);

                        _pageViewModelCache.Add(id, pageModel);

                        return pageModel;
                    }
                }
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred getting the page with id '{0}' from the database", id);
            }
        }

        /// <summary>
        /// Retrieves the page given the competition id.
        /// </summary>
        /// <param name="id">The id of the page</param>
        /// <returns>A <see cref="PageViewModel"/> for the page.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while getting the page.</exception>
        public PageViewModel GetByCompetitionId(int id, bool loadContent = false)
        {
            try
            {
                PageViewModel pageModel;
                if (true)
                {
                    Page page = Repository.GetPageById(id);

                    if (page == null)
                    {
                        return null;
                    }
                    else
                    {
                        // If object caching is enabled, ignore the "loadcontent" parameter as the cache will be 
                        // used on the second call anyway, so performance isn't an issue.
                        if (ApplicationSettings.UseObjectCache)
                        {
                            pageModel = new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
                        }
                        else
                        {
                            if (loadContent)
                            {
                                pageModel = new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
                            }
                            else
                            {
                                pageModel = new PageViewModel(page);
                            }
                        }

                        // needed for display through url /wiki/Id
                        pageModel.AllComments = FindAllCommentByPage(id);
                        pageModel.Ranking = Repository.GetPageRanking(id);
                        pageModel.UserHits = Repository.GetUserHits(page.CreatedBy);

                        _pageViewModelCache.Add(id, pageModel);

                        return pageModel;
                    }
                }
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred getting the page with id '{0}' from the database", id);
            }
        }

        /// <summary>
        /// Updates the provided page.
        /// </summary>
        /// <param name="model">The summary.</param>
        /// <exception cref="DatabaseException">An databaseerror occurred while updating.</exception>
        /// <exception cref="SearchException">An error occurred adding the page to the search index.</exception>
        public void UpdatePage(PageViewModel model)
        {
            try
            {
                string currentUser = _context.CurrentUsername;

                Page page = Repository.GetPageById(model.Id);
                page.Title = model.Title;
                page.Tags = model.CommaDelimitedTags();
                page.PublishedOn = DateTime.UtcNow;
                page.ControlledBy = "";//AppendIpForDemoSite(currentUser);

                page.IsControlled = model.IsControlled;
                page.IsRejected = model.IsRejected;
                page.IsSubmitted = model.IsSubmitted;

                // The participation to competition can be set only when publication is ongoing
                // "update" implies participation to the current competition --> it depends on the competition status
                page.CompetitionId = -1;
                if (model.IsInCompetition)
                {
                    // save competition Id
                    var competition = Repository.GetCompetitionByStatus((int)CompetitionViewModel.Statuses.PublicationOngoing);
                    if (competition != null)
                    {
                        page.CompetitionId = competition.Id;
                    }
                }

                //page.ControllerRating = model.ControllerRating; edit page doesn't change the controller rating
                //page.IsVideo = model.IsVideo;
                page.VideoUrl = model.VideoUrl;

                page.Pseudonym = model.Pseudonym;

                // A second check to ensure a fake IsLocked POST doesn't work.
                if (_context.IsAdmin)
                    page.IsLocked = model.IsLocked;

                Repository.SaveOrUpdatePage(page);

                //
                // Update the cache - updating a page is expensive for the cache right now
                // this could be improved by updating the item in the listcache instead of invalidating it
                //
                _pageViewModelCache.Remove(model.Id, 0);

                if (model.Tags.Contains(CacheKeys.ABOUTPAGE))
                    _pageViewModelCache.RemovePageWithTag(CacheKeys.ABOUTPAGE);
                if (model.Tags.Contains(CacheKeys.CONTACTPAGE))
                    _pageViewModelCache.RemovePageWithTag(CacheKeys.CONTACTPAGE);
                if (model.Tags.Contains(CacheKeys.PRIVACYPAGE))
                    _pageViewModelCache.RemovePageWithTag(CacheKeys.PRIVACYPAGE);
                if (model.Tags.Contains(CacheKeys.WARNINGSPAGE))
                    _pageViewModelCache.RemovePageWithTag(CacheKeys.WARNINGSPAGE);
                if (model.Tags.Contains(CacheKeys.HOMEPAGE))
                    _pageViewModelCache.RemovePageWithTag(CacheKeys.HOMEPAGE);
                if (model.Tags.Contains(CacheKeys.COMPETITIONPAGE))
                    _pageViewModelCache.RemovePageWithTag(CacheKeys.COMPETITIONPAGE);
                //if (model.Tags.Contains(CacheKeys.))
                //    _pageViewModelCache.RemovePageWithTag(CacheKeys.COMPETITIONPAGE);

                _listCache.RemoveAll();

                int newVersion = _historyService.MaxVersion(model.Id) + 1;
                //PageContent pageContent = Repository.AddNewPageContentVersion(page, model.Content, AppendIpForDemoSite(currentUser), DateTime.UtcNow, newVersion);
                PageContent pageContent = Repository.AddNewPageContentVersion(page, model.Content, DateTime.UtcNow, newVersion);

                // Update all links to this page (if it has had its title renamed). Case changes don't need any updates.
                if (model.PreviousTitle != null && model.PreviousTitle.ToLower() != model.Title.ToLower())
                {
                    UpdateLinksToPage(model.PreviousTitle, model.Title);
                }

                // Update the lucene index
                // DO NOT UPDATE, the lucene index is handled by published or not published states
                //PageViewModel updatedModel = new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);
                //_searchService.Update(updatedModel);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred updating the page with title '{0}' in the database", model.Title);
            }
        }

        /// <summary>
        /// Renames a tag by changing all pages that reference the tag to use the new tag name.
        /// </summary>
        /// <exception cref="DatabaseException">An databaseerror occurred while saving one of the pages.</exception>
        /// <exception cref="SearchException">An error occurred updating the search index.</exception>
        public void RenameTag(string oldTagName, string newTagName)
        {
            if (string.IsNullOrEmpty(oldTagName) || string.IsNullOrEmpty(newTagName))
                return;

            try
            {
                IEnumerable<PageViewModel> pageModels = FindByTag(oldTagName);

                foreach (PageViewModel model in pageModels)
                {
                    _searchService.Delete(model);

                    string tags = model.CommaDelimitedTags();

                    if (tags.IndexOf(",") != -1)
                    {
                        tags = tags.Replace(oldTagName + ",", newTagName + ",");
                    }
                    else if (tags.IndexOf(";") != -1)
                    {
                        // legacy
                        tags = tags.Replace(oldTagName + ";", newTagName + ";");
                    }
                    else
                    {
                        // Single tag
                        tags = tags.Replace(oldTagName, newTagName);
                    }

                    model.RawTags = tags;
                    UpdatePage(model);
                }

                string cacheKey = CacheKeys.PagesByTagKey(oldTagName);
                _listCache.Remove(cacheKey);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while changing the tagname {0} to {1}", oldTagName, newTagName);
            }
        }

        /// <summary>
        /// Retrieves the current text content for a page.
        /// </summary>
        /// <param name="pageId">The id of the page.</param>
        /// <returns>The <see cref="PageContent"/> for the page.</returns>
        public PageContent GetCurrentContent(int pageId)
        {
            return Repository.GetLatestPageContent(pageId);
        }

        /// <summary>
        /// Adds an IP address after the username for any demo site vandalism.
        /// </summary>
        private string AppendIpForDemoSite(string username)
        {
            string result = username;

            if (ApplicationSettings.IsDemoSite)
            {
                if (!_context.IsAdmin)
                {
                    string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (string.IsNullOrEmpty(ip))
                        ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                    result = string.Format("{0} ({1})", username, ip);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetUserIp()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            return ip;
        }

        /// <summary>
        /// Updates all links in pages to another page, when that page's title is changed.
        /// </summary>
        /// <param name="oldTitle">The previous page title.</param>
        /// <param name="newTitle">The new page title.</param>
        public void UpdateLinksToPage(string oldTitle, string newTitle)
        {
            bool shouldClearCache = false;

            foreach (PageContent content in Repository.AllPageContents())
            {
                if (_markupLinkUpdater.ContainsPageLink(content.Text, oldTitle))
                {
                    content.Text = _markupLinkUpdater.ReplacePageLinks(content.Text, oldTitle, newTitle);
                    Repository.UpdatePageContent(content);

                    shouldClearCache = true;
                }
            }

            if (shouldClearCache)
            {
                _pageViewModelCache.RemoveAll();
                _listCache.RemoveAll();
            }
        }

        /// <summary>
        /// Retrieves the (usually left) menu containing the new page, settings etc. options
        /// </summary>
        public string GetMenu(IUserContext userContext)
        {
            MenuParser parser = new MenuParser(_markupConverter, Repository, _siteCache, userContext);

            // TODO: turn this into a theme-based bit of template HTML
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<div id=\"leftmenu\">");
            builder.AppendLine(parser.GetMenu());
            builder.AppendLine("</div>");

            return builder.ToString();
        }

        /// <summary>
        /// Retrieves the (usually left) menu containing the new page, settings etc. options
        /// </summary>
        public string GetBootStrapNavMenu(IUserContext userContext)
        {
            MenuParser parser = new MenuParser(_markupConverter, Repository, _siteCache, userContext);

            // TODO: turn this into a theme-based bit of template HTML
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<nav id=\"leftmenu\" class=\"navbar navbar-default\" role=\"navigation\">");
            builder.Append(GetCollapsableMenuHtml());

            builder.AppendLine(@"<div id=""left-menu-toggle"" class=""collapse navbar-collapse"">");

            // Add bootstrap into the <ul>
            string menuHtml = parser.GetMenu();
            menuHtml = menuHtml.Replace("<ul>", "<ul class =\"nav navbar-nav\">");
            builder.AppendLine(menuHtml);

            builder.AppendLine("</div>");
            builder.AppendLine("</nav>");

            return builder.ToString();
        }

        /// <summary>
        /// Adds the Adidas bar to the nav bar so it can be collapsed on mobile devices
        /// </summary>
        /// <returns></returns>
        private string GetCollapsableMenuHtml()
        {
            string html = @"<div class=""navbar-header"">
					<button type=""button"" class=""navbar-toggle"" data-toggle=""collapse"" data-target=""#left-menu-toggle"">
						<span class=""sr-only"">Toggle navigation</span>
						<span class=""icon-bar""></span>
						<span class=""icon-bar""></span>
						<span class=""icon-bar""></span>
					</button>
				</div>";

            return html;
        }

        /// <summary>
        /// Retrieves the <see cref="MarkupConverter"/> used by this IPageService.
        /// </summary>
        /// <returns></returns>
        public MarkupConverter GetMarkupConverter()
        {
            return new MarkupConverter(ApplicationSettings, Repository, _pluginFactory);
        }

        /// <summary>
        /// Clears all pages and page content from the database.
        /// </summary>
        /// <exception cref="DatabaseException">An datastore error occurred while clearing the page data.</exception>
        public void ClearPageTables()
        {
            try
            {
                Repository.DeleteAllPages();
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while clearing all page tables.");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        public void AddComment(Comment comment)
        {
            try
            {
                Repository.AddComment(comment);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while adding a comment.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        public void AddAlert(Alert alert)
        {
            try
            {
                Repository.AddAlert(alert);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while adding a comment.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        public void DeleteComment(int commentId)
        {
            try
            {
                Repository.DeleteComment(commentId);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while deleting a comment.");
            }
        }

        /// <summary>
        /// FindAllCommentByPage
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public List<Comment> FindAllCommentByPage(int pageId)
        {
            try
            {
                List<Comment> comments = Repository.FindCommentsByPage(pageId).ToList();
                return comments;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while getting comments by page.");
            }
        }

        /// <summary>
        /// FindCommentByPageAndUser
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public Comment FindCommentByPageAndUser(int pageId, string userName)
        {
            try
            {
                return Repository.FindCommentByPageAndUser(pageId, userName);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while getting comments by page.");
            }
        }

        public void IncrementNbView(int pageId)
        {
            try
            {
                Repository.IncrementNbView(pageId);
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An exception occurred while incrementing nb views.");
            }

        }

        public Page FindById(int id) //TODO handle cache ???
        {
            try
            {
                return Repository.GetPageById(id);
            }
            catch (DatabaseException ex)
            {
                //throw new DatabaseException(ex, "An error occurred getting the page with id '{0}' from the database", id);
                return null;
            }
        }

        public UserActivity GetUserActivity(string username)
        {
            List<Page> pages = (List<Page>)Repository.FindPagesCreatedBy(username);

            if (pages.Count > 0)
            {
                // nb publications
                UserActivity userActivity = new UserActivity();
                userActivity.NbPublications = pages.Count(p => p.IsControlled);

                // gloabl rating
                long sumRatings = pages.Sum(p => p.TotalRating);
                long nbRatings = pages.Sum(p => p.NbRating);
                userActivity.GlobalRating = 3.5; //TODO beware divided by zero sumRatings / nbRatings;

                // oldest
                Page oldestPage = pages.OrderBy(p => p.PublishedOn).First();
                userActivity.OldestPageDate = oldestPage.PublishedOn; // PublishedOn

                return userActivity;
            }

            return null;
        }

        /// <summary>
        /// SetPageRatingForUser
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="username"></param>
        /// <param name="rating">if 0, remove rating</param>
        public void SetPageRatingForUser(int pageId, string username, int rating)
        {
            if (string.IsNullOrEmpty(username))
            {
                // use the IP address instead of the user
                username = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(username))
                    username = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            Comment comment = FindCommentByPageAndUser(pageId, username);
            if (comment != null)
            {
                // if already rated, remove
                if (comment.Rating != 0)
                {
                    Repository.RemovePageRating(pageId, comment.Rating);
                }
                // if not rated or rating removed, add
                else
                {
                    Repository.AddPageRating(comment.PageId, rating);
                }

                // add new rating only if it is a rating (rating != 0)
                Repository.UpdateCommentRating(comment.Id, rating);
            }
            else
            {
                comment = new Comment(pageId, username, rating, "");
                AddComment(comment);

                // update page table
                Repository.AddPageRating(comment.PageId, comment.Rating);

            }
        }

        /// <summary>
        /// SetPageRatingForUser
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="username"></param>
        /// <param name="rating">if 0, remove rating</param>
        public void SetPageCommentForUser(int pageId, string username, string text)
        {
            Comment comment = FindCommentByPageAndUser(pageId, username);
            if (comment != null)
            {
                Repository.UpdateComment(comment.Id, text);
            }
            else
            {
                comment = new Comment(pageId, username, 0, text);
                AddComment(comment);
            }

            // because comment has changed, force reloading
            _pageViewModelCache.Remove(pageId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetPageRatingFromUser(int id, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                // use the IP address instead of the user
                username = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(username))
                    username = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            Comment comment = FindCommentByPageAndUser(id, username);
            if (comment != null)
            {
                return comment.Rating;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetPageCommentFromUser(int id, string username)
        {
            Comment comment = FindCommentByPageAndUser(id, username);
            if (comment != null)
            {
                return comment.Text;
            }
            return "";
        }

        public string GetPageAlertFromUser(int id, string username)
        {
            Alert alert = Repository.FindAlertByPageAndUser(id, username);
            if (alert != null)
            {
                return alert.Ilk;
            }
            return "";
        }

        /// <summary>
        /// Get pages for rating, get the rating of the user if nay
        /// </summary>
        /// <param name="competitionId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<PageAndUserRatingViewModel> FindControlledfPagesByCompetition(int competitionId, string userName)
        {
            try
            {
                List<PageAndUserRatingViewModel> list = new List<PageAndUserRatingViewModel>();
                List<Page> pages = Repository.FindControlledPagesByCompetitionId(competitionId).ToList();
                foreach (Page page in pages)
                {
                    // only controlled pages
                    if (page.IsControlled)
                    {
                        int rating = 0;
                        if (userName != null && userName != "")
                        {
                            rating = Repository.GetRatingByPageAndUser(page.Id, userName);
                        }
                        list.Add(new PageAndUserRatingViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter, rating));
                    }
                }

                return list;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving all pages created by {0} from the database", userName);
            }
        }

        /// <summary>
        /// Get pages for rating, get the rating of the user if nay
        /// </summary>
        /// <param name="competitionId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IEnumerable<PageViewModel> FindPagesByCompetitionId(int competitionId)
        {
            try
            {
                IEnumerable<PageViewModel> pageModels;

                IEnumerable<Page> pages = Repository.FindPagesByCompetitionId(competitionId).ToList();
                pageModels = from page in pages
                             select new PageViewModel(Repository.GetLatestPageContent(page.Id), _markupConverter);

                return pageModels;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving all new pages from the database");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        //public CourseViewModel FindCourseByPage(int pageId)
        //{
        //    try
        //    {
        //        var list = Repository.FindCoursesByPageId(pageId);
        //        if ( list != null)
        //        {
        //            return new CourseViewModel(list.First());
        //        }
        //        return null;
        //    }
        //    catch (DatabaseException ex)
        //    {
        //        // TODO add log
        //        //throw new DatabaseException(ex, "An error occurred while retrieving all new pages from the database");
        //        return null;
        //    }
            
        //}
    }
}

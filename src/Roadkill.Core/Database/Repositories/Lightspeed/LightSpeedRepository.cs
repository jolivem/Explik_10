using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Caching;
using Mindscape.LightSpeed.Linq;
using Mindscape.LightSpeed.Logging;
using Mindscape.LightSpeed.MetaData;
using Mindscape.LightSpeed.Querying;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database.Schema;
using Roadkill.Core.Logging;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Plugins;
using StructureMap;
using static Roadkill.Core.Mvc.ViewModels.CompetitionViewModel;
using PluginSettings = Roadkill.Core.Plugins.Settings;

namespace Roadkill.Core.Database.LightSpeed
{
    public class LightSpeedRepository : Roadkill.Core.Database.IRepository
    {
        private ApplicationSettings _applicationSettings;

        #region IQueryable

        internal IQueryable<PageEntity> Pages
        {
            get { return UnitOfWork.Query<PageEntity>(); }
        }

        internal IQueryable<PageContentEntity> PageContents
        {
            get { return UnitOfWork.Query<PageContentEntity>(); }
        }

        internal IQueryable<UserEntity> Users
        {
            get { return UnitOfWork.Query<UserEntity>(); }
        }

        internal IQueryable<CommentEntity> Comments
        {
            get { return UnitOfWork.Query<CommentEntity>(); }
        }

        internal IQueryable<AlertEntity> Alerts
        {
            get { return UnitOfWork.Query<AlertEntity>(); }
        }

        internal IQueryable<CompetitionEntity> Competitions
        {
            get { return UnitOfWork.Query<CompetitionEntity>(); }
        }

        internal IQueryable<CompetitionPageEntity> CompetitionPages
        {
            get { return UnitOfWork.Query<CompetitionPageEntity>(); }
        }

        internal IQueryable<CourseEntity> Courses
        {
            get { return UnitOfWork.Query<CourseEntity>(); }
        }

        internal IQueryable<CoursePageEntity> CoursePages
        {
            get { return UnitOfWork.Query<CoursePageEntity>(); }
        }

        #endregion

        public virtual LightSpeedContext Context
        {
            get
            {
                LightSpeedContext context = ObjectFactory.GetInstance<LightSpeedContext>();
                if (context == null)
                    throw new DatabaseException("The context for Lightspeed is null - has Startup() been called?", null);

                return context;
            }
        }

        public virtual IUnitOfWork UnitOfWork
        {
            get
            {
                EnsureConectionString();

                IUnitOfWork unitOfWork = ObjectFactory.GetInstance<IUnitOfWork>();
                if (unitOfWork == null)
                    throw new DatabaseException("The IUnitOfWork for Lightspeed is null - has Startup() been called?", null);

                return unitOfWork;
            }
        }

        public LightSpeedRepository(ApplicationSettings settings)
        {
            _applicationSettings = settings;
        }

        #region IRepository

        public void Startup(DataStoreType dataStoreType, string connectionString, bool enableCache)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                LightSpeedContext context = new LightSpeedContext();
                context.ConnectionString = connectionString;
                context.DataProvider = dataStoreType.LightSpeedDbType;
                context.IdentityMethod = IdentityMethod.GuidComb;
                context.CascadeDeletes = true;
                context.VerboseLogging = true;
                context.Logger = new DatabaseLogger();

                if (enableCache)
                    context.Cache = new CacheBroker(new DefaultCache());

                ObjectFactory.Configure(x =>
                {
                    x.For<LightSpeedContext>().Singleton().Use(context);
                    x.For<IUnitOfWork>().HybridHttpOrThreadLocalScoped()
                        .Use(ctx => ctx.GetInstance<LightSpeedContext>().CreateUnitOfWork());
                });
            }
            else
            {
                Log.Warn("LightSpeedRepository.Startup skipped as no connection string was provided");
            }
        }

        public void TestConnection(DataStoreType dataStoreType, string connectionString)
        {
            LightSpeedContext context = ObjectFactory.GetInstance<LightSpeedContext>();
            if (context == null)
                throw new InvalidOperationException("Repository.Test failed - LightSpeedContext was null from the ObjectFactory");

            using (IDbConnection connection = context.DataProviderObjectFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
            }
        }

        #endregion

        #region ISettingsRepository

        public void Install(DataStoreType dataStoreType, string connectionString, bool enableCache)
        {
            LightSpeedContext context = ObjectFactory.GetInstance<LightSpeedContext>();
            if (context == null)
                throw new InvalidOperationException(
                    "Repository.Install failed - LightSpeedContext was null from the ObjectFactory");

            using (IDbConnection connection = context.DataProviderObjectFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                IDbCommand command = context.DataProviderObjectFactory.CreateCommand();
                command.Connection = connection;

                dataStoreType.Schema.Drop(command);
                dataStoreType.Schema.Create(command);
            }
        }

        public void Upgrade(ApplicationSettings settings)
        {
            try
            {
                using (IDbConnection connection = Context.DataProviderObjectFactory.CreateConnection())
                {
                    connection.ConnectionString = settings.ConnectionString;
                    connection.Open();

                    IDbCommand command = Context.DataProviderObjectFactory.CreateCommand();
                    command.Connection = connection;

                    settings.DataStoreType.Schema.Upgrade(command);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Upgrade failed: {0}", ex);
                throw new UpgradeException("A problem occurred upgrading the database schema.\n\n", ex);
            }

            try
            {
                SaveSiteSettings(new SiteSettings());
            }
            catch (Exception ex)
            {
                Log.Error("Upgrade failed: {0}", ex);
                throw new UpgradeException("A problem occurred saving the site preferences.\n\n", ex);
            }
        }

        public SiteSettings GetSiteSettings()
        {
            SiteSettings siteSettings = new SiteSettings();
            SiteConfigurationEntity entity = UnitOfWork.Find<SiteConfigurationEntity>()
                .FirstOrDefault(x => x.Id == SiteSettings.SiteSettingsId);

            if (entity != null)
            {
                siteSettings = SiteSettings.LoadFromJson(entity.Content);
            }
            else
            {
                Log.Warn("No site settings could be found in the database, using a default instance");
            }

            return siteSettings;
        }

        public PluginSettings GetTextPluginSettings(Guid databaseId)
        {
            PluginSettings pluginSettings = null;
            SiteConfigurationEntity entity = UnitOfWork.Find<SiteConfigurationEntity>()
                .FirstOrDefault(x => x.Id == databaseId);

            if (entity != null)
            {
                pluginSettings = PluginSettings.LoadFromJson(entity.Content);
            }

            return pluginSettings;
        }

        public void SaveSiteSettings(SiteSettings siteSettings)
        {
            SiteConfigurationEntity entity = UnitOfWork.Find<SiteConfigurationEntity>()
                .FirstOrDefault(x => x.Id == SiteSettings.SiteSettingsId);

            if (entity == null || entity.Id == Guid.Empty)
            {
                entity = new SiteConfigurationEntity();
                entity.Id = SiteSettings.SiteSettingsId;
                entity.Version = ApplicationSettings.ProductVersion.ToString();
                entity.Content = siteSettings.GetJson();
                UnitOfWork.Add(entity);
            }
            else
            {
                entity.Version = ApplicationSettings.ProductVersion.ToString();
                entity.Content = siteSettings.GetJson();
            }

            UnitOfWork.SaveChanges();
        }

        public void SaveTextPluginSettings(TextPlugin plugin)
        {
            string version = plugin.Version;
            if (string.IsNullOrEmpty(version))
                version = "1.0.0";

            SiteConfigurationEntity entity = UnitOfWork.Find<SiteConfigurationEntity>()
                .FirstOrDefault(x => x.Id == plugin.DatabaseId);

            if (entity == null || entity.Id == Guid.Empty)
            {
                entity = new SiteConfigurationEntity();
                entity.Id = plugin.DatabaseId;
                entity.Version = version;
                entity.Content = plugin.Settings.GetJson();
                UnitOfWork.Add(entity);
            }
            else
            {
                entity.Version = version;
                entity.Content = plugin.Settings.GetJson();
            }

            UnitOfWork.SaveChanges();
        }

        #endregion

        #region IPageRepository

        public PageContent AddNewPage(Page page, string text, string editedBy, DateTime editedOn)
        {
            PageEntity pageEntity = new PageEntity();
            ToEntity.FromPage(page, pageEntity);
            pageEntity.Id = 0;
            UnitOfWork.Add(pageEntity);
            UnitOfWork.SaveChanges();

            PageContentEntity pageContentEntity = new PageContentEntity()
            {
                Id = Guid.NewGuid(),
                Page = pageEntity,
                Text = text,
                ControlledBy = "",
                EditedOn = editedOn,
                VersionNumber = 1,
            };

            UnitOfWork.Add(pageContentEntity);
            UnitOfWork.SaveChanges();

            PageContent pageContent = FromEntity.ToPageContent(pageContentEntity);
            pageContent.Page = FromEntity.ToPage(pageEntity);
            return pageContent;
        }

        public PageContent AddNewPageContentVersion(Page page, string text, DateTime editedOn, int version)
        {
            if (version < 1)
                version = 1;

            PageEntity pageEntity = UnitOfWork.FindById<PageEntity>(page.Id);
            if (pageEntity != null)
            {
                // Update the content
                PageContentEntity pageContentEntity = new PageContentEntity()
                {
                    Id = Guid.NewGuid(),
                    Page = pageEntity,
                    Text = text,
                    ControlledBy = "",
                    EditedOn = editedOn,
                    VersionNumber = version,
                };

                UnitOfWork.Add(pageContentEntity);
                UnitOfWork.SaveChanges();

                // The page modified fields
                pageEntity.PublishedOn = editedOn;
                pageEntity.ControlledBy = "";
                UnitOfWork.SaveChanges();

                // Turn the content database entity back into a domain object
                PageContent pageContent = FromEntity.ToPageContent(pageContentEntity);
                pageContent.Page = FromEntity.ToPage(pageEntity);

                return pageContent;
            }

            Log.Error("Unable to update page content for page id {0} (not found)", page.Id);
            return null;
        }

        public IEnumerable<Page> AllPages()
        {
            List<PageEntity> entities = Pages.ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> AllNewPages()
        {
            List<PageEntity> entities = Pages.Where(p =>
                p.IsRejected == false && p.IsSubmitted == true && p.IsControlled == false && p.IsCopied == false).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> MyPages(string createdBy)
        {
            List<PageEntity> entities = Pages.Where(p => p.CreatedBy == createdBy).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<PageContent> AllPageContents()
        {
            List<PageContentEntity> entities = PageContents.ToList();
            return FromEntity.ToPageContentList(entities);
        }

        //public IEnumerable<string> AllTags()
        //{
        //    return new List<string>(Pages.Select(p => p.Tags));
        //}

        public IEnumerable<string> AllControlledTags(bool checkCompetition = false)
        {
            int competitionId = -1;
            if (checkCompetition == true)
            {
                // get current competitionID
                competitionId = GetOnGoingCompetitionId();
                if (competitionId != -1)
                {
                    // page tags out of competition
                    return new List<string>(Pages.Where(p => p.IsControlled && p.CompetitionId != competitionId).Select(p => p.Tags));
                }

            }

            // all tags
            return new List<string>(Pages.Where(p => p.IsControlled).Select(p => p.Tags));


        }

        public void DeleteAllPages()
        {
            UnitOfWork.Remove(new Query(typeof(PageEntity)));
            UnitOfWork.SaveChanges();

            UnitOfWork.Remove(new Query(typeof(PageContentEntity)));
            UnitOfWork.SaveChanges();
        }

        public void DeletePage(int pageId)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        public void SetDraft(int pageId)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            entity.IsControlled = false;
            entity.IsSubmitted = false;
            entity.IsRejected = false;
            UnitOfWork.SaveChanges();
        }

        public void SubmitPage(int pageId)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            entity.IsControlled = false;
            entity.IsRejected = false;
            entity.IsSubmitted = true;
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        public void RejectPage(int pageId)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            entity.IsControlled = false;
            entity.IsRejected = true;
            entity.IsSubmitted = false;
            UnitOfWork.SaveChanges();
        }


        public void DeletePageContent(PageContent pageContent)
        {
            PageContentEntity entity = UnitOfWork.FindById<PageContentEntity>(pageContent.Id);
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Page> FindMostRecentPages(int number)
        {
            // ignore all pages that are in the ongoing competition
            int id = GetOnGoingCompetitionId();
            if (id != -1)
            {
                List<PageEntity> entities = Pages
                    .Where(p => p.IsControlled && !p.IsLocked && p.CompetitionId != id)
                    .OrderByDescending(p => p.PublishedOn)
                    .Take(number)
                    .ToList();
                return FromEntity.ToPageList(entities);
            }
            else
            {
                List<PageEntity> entities = Pages
                    .Where(p => p.IsControlled && !p.IsLocked)
                    .OrderByDescending(p => p.PublishedOn)
                    .Take(number)
                    .ToList();
                return FromEntity.ToPageList(entities);
            }
            
        }

        public IEnumerable<Page> FindPagesBestRated(int number)
        {
            List<PageEntity> entities = Pages
                .Where(p => p.IsControlled && !p.IsLocked)
                .OrderByDescending(p => p.NbRating == 0 ? 0 : (float)p.TotalRating / (float)p.NbRating) //TODO : use also explikRating
                .Take(number)
                .ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesMostViewed(int number)
        {
            List<PageEntity> entities = Pages
                .Where(p => p.IsControlled && !p.IsLocked)
                .OrderByDescending(p => p.NbView)
                .Take(number)
                .ToList();
            return FromEntity.ToPageList(entities);
        }

        public string GetPageTitle(int pageId)
        {
            PageEntity entity = Pages.FirstOrDefault(p => p.Id == pageId);
            if (entity != null)
                return entity.Title;
            else
                return "";
        }

        public IEnumerable<Page> FindPagesCreatedBy(string username)
        {
            List<PageEntity> entities = Pages.Where(p => p.CreatedBy == username).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindControlledPagesByCompetitionId(int competitionId)
        {
            List<PageEntity> entities = Pages.Where(p => p.CompetitionId == competitionId && p.IsControlled == true).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesByCompetitionId(int competitionId)
        {
            List<PageEntity> entities = Pages.Where(p => p.CompetitionId == competitionId).ToList();
            return FromEntity.ToPageList(entities);
        }

        public void DeletCompetitionPages(int competitionId)
        {
            List<CompetitionPageEntity> entities = CompetitionPages.Where(x => x.CompetitionId == competitionId).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.Remove(entity);
            }

            UnitOfWork.SaveChanges();
        }

        public void DeletCompetitionPage(int pageId)
        {
            List<CompetitionPageEntity> entities = CompetitionPages.Where(x => x.PageId == pageId).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.Remove(entity);
            }

            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Page> FindPagesControlledBy(string username)
        {
            List<PageEntity> entities = Pages.Where(p => p.ControlledBy == username).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesContainingTag(string tag)
        {
            IEnumerable<PageEntity>
                entities = Pages.Where(p =>
                    p.Tags.ToLower().Contains(tag.ToLower())); // Lightspeed doesn't support ToLowerInvariant
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindControlledPagesByTag(string tag)
        {
            IEnumerable<PageEntity>
                entities = Pages.Where(p =>
                    p.Tags.ToLower().Contains(tag.ToLower())// Lightspeed doesn't support ToLowerInvariant
                    && p.CompetitionId == -1 // ignore if in competition
                    && p.IsControlled == true);
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesWithAlerts()
        {
            List<int> pageIds = Alerts.GroupBy(a => a.PageId).Select(a => a.First()).Select(a => a.PageId).ToList();
            IEnumerable<PageEntity>
                entities = Pages.Where(p => pageIds.Contains(p.Id)); // Lightspeed doesn't support ToLowerInvariant
            return FromEntity.ToPageList(entities);

        }

        public IEnumerable<PageContent> FindPageContentsByPageId(int pageId)
        {
            List<PageContentEntity> entities = PageContents.Where(p => p.Page.Id == pageId).ToList();
            return FromEntity.ToPageContentList(entities);
        }

        //public IEnumerable<PageContent> FindPageContentsEditedBy(string username)
        //{
        //	List<PageContentEntity> entities = PageContents.Where(p => p.ControlledBy == username).ToList();
        //	return FromEntity.ToPageContentList(entities);
        //}

        public Page GetPageById(int id)
        {
            PageEntity entity = Pages.FirstOrDefault(p => p.Id == id);
            return FromEntity.ToPage(entity);
        }

        public Page GetPageByTitle(string title)
        {
            PageEntity entity = Pages.FirstOrDefault(p => p.Title.ToLower() == title.ToLower());
            return FromEntity.ToPage(entity);
        }

        public PageContent GetLatestPageContent(int pageId)
        {
            PageContentEntity entity = PageContents.Where(x => x.Page.Id == pageId).OrderByDescending(x => x.EditedOn)
                .FirstOrDefault();
            return FromEntity.ToPageContent(entity);
        }

        public PageContent GetPageContentById(Guid id)
        {
            PageContentEntity entity = PageContents.FirstOrDefault(p => p.Id == id);
            return FromEntity.ToPageContent(entity);
        }

        public PageContent GetPageContentByPageIdAndVersionNumber(int id, int versionNumber)
        {
            PageContentEntity entity = PageContents.FirstOrDefault(p => p.Page.Id == id && p.VersionNumber == versionNumber);
            return FromEntity.ToPageContent(entity);
        }

        //public IEnumerable<PageContent> GetPageContentByEditedBy(string username)
        //{
        //	List<PageContentEntity> entities = PageContents.Where(p => p.EditedBy == username).ToList();
        //	return FromEntity.ToPageContentList(entities);
        //}

        public Page SaveOrUpdatePage(Page page)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(page.Id);
            if (entity == null)
            {
                entity = new PageEntity();
                ToEntity.FromPage(page, entity);
                UnitOfWork.Add(entity);
                UnitOfWork.SaveChanges();
                page = FromEntity.ToPage(entity);
            }
            else
            {
                ToEntity.FromPage(page, entity);
                UnitOfWork.SaveChanges();
                page = FromEntity.ToPage(entity);
            }

            return page;
        }

        /// <summary>
        /// This updates an existing set of text and is used for page rename updates.
        /// To add a new version of a page, use AddNewPageContentVersion
        /// </summary>
        /// <param name="content"></param>
        public void UpdatePageContent(PageContent content)
        {
            PageContentEntity entity = UnitOfWork.FindById<PageContentEntity>(content.Id);
            if (entity != null)
            {
                ToEntity.FromPageContent(content, entity);
                UnitOfWork.SaveChanges();
                content = FromEntity.ToPageContent(entity);
            }
        }

        public void IncrementNbView(int pageId)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            if (entity != null)
            {
                entity.NbView++;
                UnitOfWork.SaveChanges();
            }
        }

        public void SetNbView(int pageId, int nbView)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            if (entity != null)
            {
                entity.NbView = nbView;
                UnitOfWork.SaveChanges();
            }
        }

        public void SetCompetitionId(int pageId, int competitionId)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            if (entity != null)
            {
                entity.CompetitionId = competitionId;
                UnitOfWork.SaveChanges();
            }
        }
        public void SetRating(int pageId, int nbRating, int totalRating)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            if (entity != null)
            {
                entity.TotalRating = totalRating;
                entity.NbRating = nbRating;
                UnitOfWork.SaveChanges();
            }
        }

        public void AddPageRating(int pageId, int rating)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            if (entity != null)
            {
                entity.TotalRating += rating;
                entity.NbRating++;
                UnitOfWork.SaveChanges();
            }
        }

        public void RemovePageRating(int pageId, int rating)
        {
            PageEntity entity = UnitOfWork.FindById<PageEntity>(pageId);
            if (entity != null)
            {
                entity.TotalRating -= rating;
                entity.NbRating--;
                if (entity.TotalRating<=0 || entity.NbRating<=0)
                {
                    // todo log error
                    entity.TotalRating = 0;
                    entity.NbRating=0;
                }
                UnitOfWork.SaveChanges();
            }
        }

        /// <summary>
        /// Remove the competitionId if the pages have not been controlled
        /// </summary>
        /// <param name="competitionId"></param>
        public void CleanPagesForCompetitionId(int competitionId)
        {
            List<PageEntity> entities = Pages.Where(x => x.CompetitionId == competitionId && !x.IsControlled).ToList();
            foreach (var entity in entities)
            {
                entity.CompetitionId = -1;
            }

            UnitOfWork.SaveChanges();
        }


        #endregion

        #region IUserRepository

        public void DeleteUser(User user)
        {
            UserEntity entity = UnitOfWork.FindById<UserEntity>(user.Id);
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteAllUsers()
        {
            UnitOfWork.Remove(new Query(typeof(UserEntity)));
            UnitOfWork.SaveChanges();
        }

        public User GetAdminById(Guid id)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.Id == id && x.IsAdmin);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByActivationKey(string key)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.ActivationKey == key && x.IsActivated == false);
            return FromEntity.ToUser(entity);
        }

        public User GetEditorById(Guid id)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.Id == id && x.IsEditor);
            return FromEntity.ToUser(entity);
        }

        public User GetControllerById(Guid id)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.Id == id && x.IsController);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByEmail(string email, bool? isActivated = null)
        {
            UserEntity entity;

            if (isActivated.HasValue)
                entity = Users.FirstOrDefault(x => x.Email == email && x.IsActivated == isActivated);
            else
                entity = Users.FirstOrDefault(x => x.Email == email);

            return FromEntity.ToUser(entity);
        }

        public User GetUserById(Guid id, bool? isActivated = null)
        {
            UserEntity entity;

            if (isActivated.HasValue)
                entity = Users.FirstOrDefault(x => x.Id == id && x.IsActivated == isActivated);
            else
                entity = Users.FirstOrDefault(x => x.Id == id);

            return FromEntity.ToUser(entity);
        }

        public User GetUserByPasswordResetKey(string key)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.PasswordResetKey == key);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByUsername(string username)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.Username == username);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByUsernameOrEmail(string username, string email)
        {
            UserEntity entity = Users.FirstOrDefault(x => x.Username == username || x.Email == email);
            return FromEntity.ToUser(entity);
        }

        public IEnumerable<User> FindAllEditors()
        {
            List<UserEntity> entities = Users.Where(x => x.IsEditor).ToList();
            return FromEntity.ToUserList(entities);
        }

        public IEnumerable<User> FindAllControllers()
        {
            List<UserEntity> entities = Users.Where(x => x.IsController).ToList();
            return FromEntity.ToUserList(entities);
        }

        public IEnumerable<User> FindAllAdmins()
        {
            List<UserEntity> entities = Users.Where(x => x.IsAdmin).ToList();
            return FromEntity.ToUserList(entities);
        }

        public User SaveOrUpdateUser(User user)
        {
            UserEntity entity = UnitOfWork.FindById<UserEntity>(user.Id);
            if (entity == null)
            {
                // Turn the domain object into a database entity
                entity = new UserEntity();
                ToEntity.FromUser(user, entity);
                UnitOfWork.Add(entity);
                UnitOfWork.SaveChanges();

                user = FromEntity.ToUser(entity);
            }
            else
            {
                ToEntity.FromUser(user, entity);
                UnitOfWork.SaveChanges();
            }

            return user;
        }

        #endregion

        #region ICommentRepository

        public void DeleteComment(Guid commentId)
        {
            CommentEntity entity = Comments.Where(x => x.Id == commentId).Single();
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="rating"></param>
        public void UpdateCommentRating(Guid commentId, int rating)
        {
            CommentEntity entity = Comments.Where(x => x.Id == commentId).Single();
            entity.Rating = rating;
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="text"></param>
        public void UpdateComment(Guid commentId, string text)
        {
            CommentEntity entity = Comments.Where(x => x.Id == commentId).Single();
            entity.Text = text;
            entity.IsControlled = false;
            entity.IsRejected = false;
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        public void ValidateComment(Guid commentId)
        {
            CommentEntity entity = Comments.Where(x => x.Id == commentId).Single();
            entity.IsControlled = true;
            entity.IsRejected = false;
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        public void RejectComment(Guid commentId)
        {
            CommentEntity entity = Comments.Where(x => x.Id == commentId).Single();
            entity.IsControlled = true;
            entity.IsRejected = true;
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> FindCommentsByPage(int pageId)
        {
            List<CommentEntity> entities = Comments.Where(x => x.PageId == pageId &&
                                                               x.Text != "" &&
                                                               x.IsControlled == true &&
                                                               x.IsRejected == false).ToList();
            return FromEntity.ToCommentList(entities);
        }

        /// <summary>
        /// Get the rating of a given page by a given user
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetRatingByPageAndUser(int pageId, string username)
        {
            CommentEntity comment = Comments.Where(x => x.CreatedBy == username && x.PageId == pageId).SingleOrDefault();
            if (comment != null)
            {
                return comment.Rating;
            }
            return 0; // not rated
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> FindCommentsToControl()
        {
            List<CommentEntity> entities = Comments.Where(x => x.IsControlled == false && x.IsRejected == false && x.Text != null && x.Text != "").ToList();
            return FromEntity.ToCommentList(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        public void AddComment(Comment comment)
        {
            CommentEntity entity = new CommentEntity();
            ToEntity.FromComment(comment, entity);
            UnitOfWork.Add(entity);
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Comment FindCommentByPageAndUser(int pageId, string username)
        {
            List<CommentEntity> entities = Comments.Where(x => x.PageId == pageId && x.CreatedBy == username).ToList();
            if (entities.Count > 0)
            {
                return FromEntity.ToComment(entities[0]);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        public void DeleteComments(int pageId)
        {
            var entities = Comments.Where(x => x.PageId == pageId).ToList();
            foreach (Entity entity in entities)
            {
                UnitOfWork.Remove(entity);
            }

            UnitOfWork.SaveChanges();
        }

        #endregion

        #region IAlertRepository

        public void DeleteAlert(Guid alertId)
        {
            AlertEntity entity = Alerts.Where(x => x.Id == alertId).Single();
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Alert> FindAlertsByPage(int pageId)
        {
            List<AlertEntity> entities = Alerts.Where(x => x.PageId == pageId).ToList();
            return FromEntity.ToAlertList(entities);
        }

        public Alert FindAlertByPageAndUser(int pageId, string username)
        {
            AlertEntity entitie = Alerts.Where(x => x.PageId == pageId && x.CreatedBy == username).FirstOrDefault();
            if (entitie != null)
            {
                return FromEntity.ToAlert(entitie);
            }

            return null;
        }

        public IEnumerable<Alert> GetAlerts()
        {
            List<AlertEntity> entities = Alerts.ToList();
            if (entities != null)
            {
                return FromEntity.ToAlertList(entities);
            }

            return null;
        }

        public IEnumerable<Alert> FindAlertsByComment(Guid commentGuid)
        {
            List<AlertEntity> entities = Alerts.Where(x => x.CommentId == commentGuid).ToList();
            return FromEntity.ToAlertList(entities);
        }

        public void AddAlert(Alert alert)
        {
            AlertEntity entity = new AlertEntity();
            ToEntity.FromAlert(alert, entity);
            UnitOfWork.Add(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeletePageAlerts(int pageId)
        {
            List<AlertEntity> entities = Alerts.Where(x => x.PageId == pageId).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.Remove(entity);
            }

            UnitOfWork.SaveChanges();
        }

        public void DeletPageAlertsByUser(int pageId, string username)
        {
            List<AlertEntity> entities = Alerts.Where(x => x.PageId == pageId && x.CreatedBy == username).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.Remove(entity);
            }

            UnitOfWork.SaveChanges();
        }

        public void DeleteCommentAlerts(Guid commentId)
        {
            List<AlertEntity> entities = Alerts.Where(x => x.CommentId == commentId).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.Remove(entity);
            }

            UnitOfWork.SaveChanges();
        }

        #endregion

        #region ICompetitionRepository

        public void DeleteCompetition(int id)
        {
            CompetitionEntity entity = Competitions.Where(x => x.Id == id).Single();
            UnitOfWork.Remove(entity);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Competition> GetCompetitions(bool forAdmin = false)
        {
            List<CompetitionEntity> entities;
            if (forAdmin)
            {
                // for admin, get all competitions
                entities = Competitions.ToList();
            }
            else
            {
                // for other users, hide Pause and Init statuses
                entities = Competitions.Where(c => 
                c.Status == (int)Statuses.Achieved ||
                c.Status == (int)Statuses.PublicationOngoing ||
                c.Status == (int)Statuses.RatingOngoing).ToList();
            }

            if (entities != null)
            {
                return FromEntity.ToCompetitionList(entities);
            }
            return null;
        }

        public Competition GetCompetitionByPageTag(string tag)
        {
            CompetitionEntity entity = Competitions.SingleOrDefault(x => x.PageTag == tag);
            if (entity != null)
            {
                return FromEntity.ToCompetition(entity);
            }

            return null;
        }
        public Competition GetCompetitionById(int id)
        {
            CompetitionEntity entity = Competitions.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                return FromEntity.ToCompetition(entity);
            }

            return null;
        }

        public Competition GetCompetitionByStatus(int status)
        {
            CompetitionEntity entity = Competitions.SingleOrDefault(x => x.Status == status);
            if (entity != null)
            {
                return FromEntity.ToCompetition(entity);
            }

            return null;
        }

        public int GetOnGoingCompetitionId()
        {
            CompetitionEntity entity = Competitions.SingleOrDefault(x =>
              x.Status == (int)Statuses.PublicationOngoing ||
              x.Status == (int)Statuses.PauseBeforeRating ||
              x.Status == (int)Statuses.RatingOngoing ||
              x.Status == (int)Statuses.PauseBeforeAchieved);

            if (entity != null)
            {
                return entity.Id;
            }

            return -1;
        }

        public void AddCompetition(Competition competition)
        {
            CompetitionEntity entity = new CompetitionEntity();
            ToEntity.FromCompetition(competition, entity);
            UnitOfWork.Add(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateCompetition(Competition competition)
        {
            CompetitionEntity entity = Competitions.SingleOrDefault(x => x.Id == competition.Id);
            ToEntity.FromCompetition(competition, entity);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<CompetitionPage> GetCompetitionPages(int competitionId)
        {
            List<CompetitionPageEntity> entities = CompetitionPages.Where(x => x.CompetitionId == competitionId).ToList();
            if (entities != null)
            {
                return FromEntity.ToCompetitionPageList(entities);
            }

            return null;
        }

        public void UpdateCompetitionPageRanking(int competitionId, int pageId, int ranking)
        {
            var entity = CompetitionPages.SingleOrDefault(x => x.CompetitionId == competitionId && x.PageId == pageId);
            entity.Ranking = ranking;
            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public int GetPageRanking(int pageId)
        {
            var entity = CompetitionPages.SingleOrDefault(x => x.PageId == pageId);
            if (entity != null)
            {
                return entity.Ranking;
            }

            return 0; // no participation in a competition
        }

        public void UpdateCompetitionPageId(int competitionId, int pageId)
        {
            CompetitionEntity entity = Competitions.SingleOrDefault(x => x.Id == competitionId);
            entity.PageId = pageId;
            UnitOfWork.SaveChanges();
        }

        public int[] GetUserHits(string username)
        {
            int[] hits = new int[] { 0, 0, 0 };

            hits[0] = CompetitionPages.Count(x => x.UserName == username && x.Ranking == 1);
            hits[1] = CompetitionPages.Count(x => x.UserName == username && x.Ranking == 2);
            hits[2] = CompetitionPages.Count(x => x.UserName == username && x.Ranking == 3);
            return hits;
        }

        public void ArchiveCompetitionPage(int competitionId, Page page)
        {
            CompetitionPageEntity entity = new CompetitionPageEntity();
            //ToEntity.FromCompetitionPage(competition, entity);
            entity.CompetitionId = competitionId;
            entity.PageId = page.Id;
            entity.NbRating = page.NbRating;
            entity.TotalRating = page.TotalRating;
            entity.UserName = page.CreatedBy;
            UnitOfWork.Add(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region ICourseRepository
        public Course GetCourseById(int id)
        {
            CourseEntity entity = Courses.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                return FromEntity.ToCourse(entity);
            }

            return null;
        }

        public int AddNewCourse(Course course)
        {
            CourseEntity entity = new CourseEntity();
            ToEntity.FromCourse(course, entity);

            entity.Id = 0;
            UnitOfWork.Add(entity);
            UnitOfWork.SaveChanges();
            return entity.Id;
        }

        public void UpdateCourse(Course Course)
        {
            throw new NotImplementedException();
        }

        public void UpdateCoursePageId(int courseId, int pageId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> GetCoursesByUser(string createdBy)
        {
            List<CourseEntity> entities = Courses.Where(x => x.CreatedBy == createdBy).ToList();
            if (entities != null)
            {
                return FromEntity.ToCourseList(entities);
            }

            return null;
        }

        public Course GetCourseByPage(string tag)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CoursePage> GetCoursePages(int courseId)
        {
            var entities = CoursePages.Where(x => x.CourseId == courseId);
            if (entities != null)
            {
                return FromEntity.ToCoursePageList(entities.ToList());
            }
            return null;
        }

        public IEnumerable<Page> GetPagesByCourseId(int courseId)
        {
            List<int> pageIds = CoursePages.Where(x => x.CourseId == courseId).Select(x => x.PageId).ToList();
            List<PageEntity> entities = Pages.Where(x => pageIds.Contains (x.Id)).ToList();
            if (entities != null)
            {
                return FromEntity.ToPageList(entities);
            }
            return null;
        }

        public void DeleteCourse(int id)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region IDisposable
        public void Dispose()
		{
			UnitOfWork.SaveChanges();
			UnitOfWork.Dispose();
		}
		#endregion

		private void EnsureConectionString()
		{
			if (_applicationSettings.Installed && string.IsNullOrEmpty(_applicationSettings.ConnectionString))
				throw new DatabaseException("The connection string is empty in the web.config file (and the roadkill.config's installed=true).", null);
		}
    }
}

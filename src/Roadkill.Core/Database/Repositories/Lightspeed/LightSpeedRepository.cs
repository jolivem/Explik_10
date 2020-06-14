using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database.Repositories;
using Roadkill.Core.Database.Repositories.Entities;
using Roadkill.Core.Database.Schema;
using Roadkill.Core.Logging;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Plugins;
using StructureMap;
using static Roadkill.Core.Mvc.ViewModels.CompetitionViewModel;
using PluginSettings = Roadkill.Core.Plugins.Settings;

namespace Roadkill.Core.Database.LightSpeed
{
    public class LightSpeedRepository : IRepository
    {
        //using PageContes = uow.PageContentRepository;
        private ApplicationSettings _applicationSettings;
        private string connectionString;

        #region IQueryable


        #endregion

        //public virtual LightSpeedContext Context
        //{
        //    get
        //    {
        //        return uow.Co²
        //        LightSpeedContext context = ObjectFactory.GetInstance<LightSpeedContext>();
        //        if (context == null)
        //            throw new DatabaseException("The context for Lightspeed is null - has Startup() been called?", null);

        //        return context;
        //    }
        //}
        //public UnitOfWork UnitOfWork;

        //public virtual UnitOfWork UnitOfWork
        //{
        //    get
        //    {
        //        EnsureConectionString();

        //        UnitOfWork unitOfWork = ObjectFactory.GetInstance<IUnitOfWork>();
        //        if (unitOfWork == null)
        //            throw new DatabaseException("The IUnitOfWork for Lightspeed is null - has Startup() been called?", null);

        //        return unitOfWork;
        //    }
        //}

        public LightSpeedRepository(ApplicationSettings settings)
        {
            _applicationSettings = settings;
            connectionString = _applicationSettings.ConnectionString;
        }

        #region IRepository

        public void Startup( string connectionString_)
        {
            if (!string.IsNullOrEmpty(connectionString_))
            {
                connectionString = connectionString_;
            }
            else
            {
                Log.Warn("LightSpeedRepository.Startup skipped as no connection string was provided");
            }
        }

        public bool TestConnection( string connectionString)
        {
            try
            {
                var uow = new UnitOfWork(connectionString);
                return uow.ConnectionSuccessful();
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region ISettingsRepository

        public SiteSettings GetSiteSettings()
        {
            var uow = new UnitOfWork(connectionString);
            SiteSettings siteSettings = new SiteSettings();
            
            explik_siteconfiguration entity = uow.SiteConfigurationRepository.FirstOrDefault(x => x.Id == SiteSettings.SiteSettingsId.ToString());

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
            var uow = new UnitOfWork(connectionString);
            PluginSettings pluginSettings = null;
            explik_siteconfiguration entity = uow.SiteConfigurationRepository.FirstOrDefault(x => x.Id == databaseId.ToString());

            if (entity != null)
            {
                pluginSettings = PluginSettings.LoadFromJson(entity.Content);
            }

            return pluginSettings;
        }

        public void SaveSiteSettings(SiteSettings siteSettings)
        {
            var uow = new UnitOfWork(connectionString);
            explik_siteconfiguration entity = uow.SiteConfigurationRepository.FirstOrDefault(x => x.Id == SiteSettings.SiteSettingsId.ToString());

            if (entity == null || entity.Id == Guid.Empty.ToString())
            {
                entity = new explik_siteconfiguration();
                entity.Id = SiteSettings.SiteSettingsId.ToString();
                entity.Version = ApplicationSettings.ProductVersion.ToString();
                entity.Content = siteSettings.GetJson();
                uow.SiteConfigurationRepository.Insert(entity);
            }
            else
            {
                entity.Version = ApplicationSettings.ProductVersion.ToString();
                entity.Content = siteSettings.GetJson();
            }

            uow.Save();
        }

        public void SaveTextPluginSettings(TextPlugin plugin)
        {
            string version = plugin.Version;
            if (string.IsNullOrEmpty(version))
                version = "1.0.0";

            var uow = new UnitOfWork(connectionString);
            explik_siteconfiguration entity = uow.SiteConfigurationRepository.FirstOrDefault(x => x.Id == plugin.DatabaseId.ToString());

            if (entity == null || entity.Id == Guid.Empty.ToString())
            {
                entity = new explik_siteconfiguration();
                entity.Id = plugin.DatabaseId.ToString();
                entity.Version = version;
                entity.Content = plugin.Settings.GetJson();
                uow.SiteConfigurationRepository.Add(entity);
            }
            else
            {
                entity.Version = version;
                entity.Content = plugin.Settings.GetJson();
            }

            uow.Save();
        }

        #endregion

        #region IPageRepository

        public PageContent AddNewPage(Page page, string text, string editedBy, DateTime editedOn)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages pageEntity = new explik_pages();
            ToEntity.FromPage(page, pageEntity);
            pageEntity.Id = 0;
            uow.PagesRepository.Add(pageEntity);
            uow.Save();

            explik_pagecontent pageContentEntity = new explik_pagecontent()
            {
                //Id = Guid.NewGuid().ToString(),
                PageId = pageEntity.Id,
                Text = text,
                ControlledBy = "",
                EditedOn = editedOn,
                VersionNumber = 1,
            };

            uow.PageContentRepository.Add(pageContentEntity);
            uow.Save();

            PageContent pageContent = FromEntity.ToPageContent(pageContentEntity, pageEntity);
            pageContent.Page = FromEntity.ToPage(pageEntity);
            return pageContent;
        }

        public PageContent AddNewPageContentVersion(Page page, string text, DateTime editedOn, int version)
        {
            if (version < 1)
                version = 1;
            var uow = new UnitOfWork(connectionString);
            explik_pages pageEntity = uow.PagesRepository.GetById(page.Id);
            if (pageEntity != null)
            {
                // Update the content
                explik_pagecontent pageContentEntity = new explik_pagecontent()
                {
                    //Id = Guid.NewGuid().ToString(),
                    PageId = page.Id,
                    Text = text,
                    ControlledBy = "",
                    EditedOn = editedOn,
                    VersionNumber = version,
                };

                uow.PageContentRepository.Add(pageContentEntity);
                uow.Save();

                // The page modified fields
                pageEntity.PublishedOn = editedOn;
                pageEntity.ControlledBy = "";
                uow.Save();

                // Turn the content database entity back into a domain object
                PageContent pageContent = FromEntity.ToPageContent(pageContentEntity, pageEntity);
                pageContent.Page = FromEntity.ToPage(pageEntity);

                return pageContent;
            }

            Log.Error("Unable to update page content for page id {0} (not found)", page.Id);
            return null;
        }

        public IEnumerable<Page> AllPages()
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages.ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> AllNewPages()
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages.Where(p =>
                p.IsRejected == false && p.IsSubmitted == true && p.IsControlled == false && p.IsCopied == false).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> MyPages(string createdBy)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages.Where(p => p.CreatedBy == createdBy).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<PageContent> AllPageContents()
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pagecontent> entities = uow.PageContents.ToList();
            return FromEntity.ToPageContentList(entities, null);
        }

        //public IEnumerable<string> AllTags()
        //{
        //    return new List<string>(uow.Pages.Select(p => p.Tags));
        //}

        public IEnumerable<string> AllControlledTags(bool checkCompetition = false)
        {
            var uow = new UnitOfWork(connectionString);
            int competitionId = -1;
            if (checkCompetition == true)
            {
                // get current competitionID
                competitionId = GetOnGoingCompetitionId();
                if (competitionId != -1)
                {
                    // page tags out of competition
                    return new List<string>(uow.Pages.Where(p => p.IsControlled && p.CompetitionId != competitionId).Select(p => p.Tags));
                }

            }

            // all tags
            return new List<string>(uow.Pages.Where(p => p.IsControlled).Select(p => p.Tags));


        }

        public void DeleteAllPages()
        {
            var uow = new UnitOfWork(connectionString);
            uow.PagesRepository.DeleteAll();
            uow.Save();

            uow.PageContentRepository.DeleteAll();
            uow.Save();
        }

        public void DeletePage(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
            uow.PagesRepository.Delete(entity);
            uow.Save();
        }

        public void SetDraft(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
            entity.IsControlled = false;
            entity.IsSubmitted = false;
            entity.IsRejected = false;
            uow.Save();
        }

        public void SubmitPage(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
            entity.IsControlled = false;
            entity.IsRejected = false;
            entity.IsSubmitted = true;
            uow.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        public void RejectPage(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
            entity.IsControlled = false;
            entity.IsRejected = true;
            entity.IsSubmitted = false;
            uow.Save();
        }


        public void DeletePageContent(PageContent pageContent)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pagecontent entity = uow.PageContentRepository.GetById(pageContent.Id.ToString());
            uow.PageContentRepository.Delete(entity);
            uow.Save();
        }

        public IEnumerable<Page> FindMostRecentPages(int number)
        {
            var uow = new UnitOfWork(connectionString);
            // ignore all pages that are in the ongoing competition
            int id = GetOnGoingCompetitionId();
            if (id != -1)
            {
                List<explik_pages> entities = uow.Pages
                    .Where(p => p.IsControlled && !p.IsLocked && p.CompetitionId != id)
                    .OrderByDescending(p => p.PublishedOn)
                    .Take(number)
                    .ToList();
                return FromEntity.ToPageList(entities);
            }
            else
            {
                List<explik_pages> entities = uow.Pages
                    .Where(p => p.IsControlled && !p.IsLocked)
                    .OrderByDescending(p => p.PublishedOn)
                    .Take(number)
                    .ToList();
                return FromEntity.ToPageList(entities);
            }
            
        }

        public IEnumerable<Page> FindPagesBestRated(int number)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages
                .Where(p => p.IsControlled && !p.IsLocked)
                .OrderByDescending(p => p.NbRating == 0 ? 0 : (float)p.TotalRating / (float)p.NbRating) //TODO : use also explikRating
                .Take(number)
                .ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesMostViewed(int number)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages
                .Where(p => p.IsControlled && !p.IsLocked)
                .OrderByDescending(p => p.NbView)
                .Take(number)
                .ToList();
            return FromEntity.ToPageList(entities);
        }

        public string GetPageTitle(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.Pages.FirstOrDefault(p => p.Id == pageId);
            if (entity != null)
                return entity.Title;
            else
                return "";
        }

        public IEnumerable<Page> FindPagesCreatedBy(string username)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages.Where(p => p.CreatedBy == username).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindControlledPagesByCompetitionId(int competitionId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages.Where(p => p.CompetitionId == competitionId && p.IsControlled == true).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesByCompetitionId(int competitionId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages.Where(p => p.CompetitionId == competitionId).ToList();
            return FromEntity.ToPageList(entities);
        }

        public void DeletCompetitionPages(int competitionId)
        {

            var uow = new UnitOfWork(connectionString);
            List<explik_competitionpage> entities = uow.CompetitionPages.Where(x => x.CompetitionId == competitionId).ToList();
            foreach (var entity in entities)
            {
                uow.CompetitionPagesRepository.Delete(entity);
            }

            uow.Save();
        }

        public void DeletCompetitionPage(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_competitionpage> entities = uow.CompetitionPages.Where(x => x.PageId == pageId).ToList();
            foreach (var entity in entities)
            {
                uow.CompetitionPagesRepository.Delete(entity);
            }

            uow.Save();
        }

        public IEnumerable<Page> FindPagesControlledBy(string username)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages.Where(p => p.ControlledBy == username).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesContainingTag(string tag)
        {
            var uow = new UnitOfWork(connectionString);
            IEnumerable<explik_pages>
                entities = uow.Pages.Where(p =>
                    p.Tags.ToLower().Contains(tag.ToLower())); // Lightspeed doesn't support ToLowerInvariant
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindControlledPagesByTag(string tag)
        {
            var uow = new UnitOfWork(connectionString);
            IEnumerable<explik_pages>
                entities = uow.Pages.Where(p =>
                    p.Tags.ToLower().Contains(tag.ToLower())// Lightspeed doesn't support ToLowerInvariant
                    && p.CompetitionId == -1 // ignore if in competition
                    && p.IsControlled == true);
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesWithAlerts()
        {
            var uow = new UnitOfWork(connectionString);
            List<int> pageIds = uow.Alerts.GroupBy(a => (int)a.PageId).Select(a => a.First()).Select(a => (int)a.PageId).ToList();
            IEnumerable<explik_pages>
                entities = uow.Pages.Where(p => pageIds.Contains(p.Id)); // Lightspeed doesn't support ToLowerInvariant
            return FromEntity.ToPageList(entities);

        }

        public IEnumerable<PageContent> FindPageContentsByPageId(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pagecontent> entities = uow.PageContents.Where(p => p.PageId == pageId).ToList();
            explik_pages pageEntity = uow.PagesRepository.GetById(pageId);
            return FromEntity.ToPageContentList(entities, pageEntity);
        }

        //public IEnumerable<PageContent> FindPageContentsEditedBy(string username)
        //{
        //	List<explik_pagecontent> entities = PageContents.Where(p => p.ControlledBy == username).ToList();
        //	return FromEntity.ToPageContentList(entities);
        //}

        public Page GetPageById(int id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.Pages.FirstOrDefault(p => p.Id == id);
            return FromEntity.ToPage(entity);
        }

        public Page GetPageByTitle(string title)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.Pages.FirstOrDefault(p => p.Title.ToLower() == title.ToLower());
            return FromEntity.ToPage(entity);
        }

        public PageContent GetLatestPageContent(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pagecontent entity = uow.PageContents.Where(x => x.PageId == pageId).OrderByDescending(x => x.EditedOn)
                .FirstOrDefault();
            explik_pages pageEntity = uow.PagesRepository.GetById(pageId);
            return FromEntity.ToPageContent(entity, pageEntity);
        }

        public PageContent GetPageContentById(Guid id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pagecontent entity = uow.PageContents.FirstOrDefault(p => p.Id == id.ToString());
            if (entity == null)
            {
                return null;
            }
            explik_pages pageEntity = uow.PagesRepository.GetById(entity.PageId);
            return FromEntity.ToPageContent(entity, pageEntity);
        }

        public PageContent GetPageContentByPageIdAndVersionNumber(int id, int versionNumber)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pagecontent entity = uow.PageContents.FirstOrDefault(p => p.PageId == id && p.VersionNumber == versionNumber);
            explik_pages pageEntity = uow.PagesRepository.GetById(entity.PageId);
            return FromEntity.ToPageContent(entity, pageEntity);
        }

        //public IEnumerable<PageContent> GetPageContentByEditedBy(string username)
        //{
        //	List<explik_pagecontent> entities = PageContents.Where(p => p.EditedBy == username).ToList();
        //	return FromEntity.ToPageContentList(entities);
        //}

        public Page SaveOrUpdatePage(Page page)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(page.Id);
            if (entity == null)
            {
                entity = new explik_pages();
                ToEntity.FromPage(page, entity);
                uow.PagesRepository.Add(entity);
                uow.Save();
                page = FromEntity.ToPage(entity);
            }
            else
            {
                ToEntity.FromPage(page, entity);
                uow.Save();
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
            var uow = new UnitOfWork(connectionString);
            explik_pagecontent entity = uow.PageContentRepository.GetById(content.Id.ToString());
            if (entity != null)
            {
                ToEntity.FromPageContent(content, entity);
                uow.Save();
                explik_pages pageEntity = uow.PagesRepository.GetById(entity.PageId);
                content = FromEntity.ToPageContent(entity, pageEntity);
            }
        }

        public void IncrementNbView(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.NbView++;
                uow.Save();
            }
        }

        public void SetNbView(int pageId, int nbView)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.NbView = nbView;
                uow.Save();
            }
        }

        public void SetCompetitionId(int pageId, int competitionId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.CompetitionId = competitionId;
                uow.Save();
            }
        }
        public void SetRating(int pageId, int nbRating, int totalRating)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.TotalRating = totalRating;
                entity.NbRating = nbRating;
                uow.Save();
            }
        }

        public void AddPageRating(int pageId, int rating)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.TotalRating += rating;
                entity.NbRating++;
                uow.Save();
            }
        }

        public void RemovePageRating(int pageId, int rating)
        {
            var uow = new UnitOfWork(connectionString);
            explik_pages entity = uow.PagesRepository.GetById(pageId);
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
                uow.Save();
            }
        }

        /// <summary>
        /// Remove the competitionId if the pages have not been controlled
        /// </summary>
        /// <param name="competitionId"></param>
        public void CleanPagesForCompetitionId(int competitionId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_pages> entities = uow.Pages.Where(x => x.CompetitionId == competitionId && !x.IsControlled).ToList();
            foreach (var entity in entities)
            {
                entity.CompetitionId = -1;
            }

            uow.Save();
        }


        #endregion

        #region IUserRepository

        public void DeleteUser(User user)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity = uow.UsersRepository.GetById(user.Id.ToString());
            uow.UsersRepository.Delete(entity);
            uow.Save();
        }

        public void DeleteAllUsers()
        {
            var uow = new UnitOfWork(connectionString);
            uow.UsersRepository.DeleteAll();
            uow.Save();
        }

        public User GetAdminById(Guid id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity = uow.Users.FirstOrDefault(x => x.Id == id.ToString() && x.IsAdmin);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByActivationKey(string key)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity = uow.Users.FirstOrDefault(x => x.ActivationKey == key && x.IsActivated == false);
            return FromEntity.ToUser(entity);
        }

        public User GetEditorById(Guid id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity = uow.Users.FirstOrDefault(x => x.Id == id.ToString() && x.IsEditor);
            return FromEntity.ToUser(entity);
        }

        public User GetControllerById(Guid id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity = uow.Users.FirstOrDefault(x => x.Id == id.ToString() && x.IsController);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByEmail(string email, bool? isActivated = null)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity;

            if (isActivated.HasValue)
                entity = uow.Users.FirstOrDefault(x => x.Email == email && x.IsActivated == isActivated);
            else
                entity = uow.Users.FirstOrDefault(x => x.Email == email);

            return FromEntity.ToUser(entity);
        }

        public User GetUserById(Guid id, bool? isActivated = null)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity;

            if (isActivated.HasValue)
                entity = uow.Users.FirstOrDefault(x => x.Id == id.ToString() && x.IsActivated == isActivated);
            else
                entity = uow.Users.FirstOrDefault(x => x.Id == id.ToString());

            return FromEntity.ToUser(entity);
        }

        public User GetUserByPasswordResetKey(string key)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity = uow.Users.FirstOrDefault(x => x.PasswordResetKey == key);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByUsername(string username)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity = uow.Users.FirstOrDefault(x => x.Username == username);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByUsernameOrEmail(string username, string email)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity = uow.Users.FirstOrDefault(x => x.Username == username || x.Email == email);
            return FromEntity.ToUser(entity);
        }

        public IEnumerable<User> FindAllEditors()
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_users> entities = uow.Users.Where(x => x.IsEditor).ToList();
            return FromEntity.ToUserList(entities);
        }

        public IEnumerable<User> FindAllControllers()
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_users> entities = uow.Users.Where(x => x.IsController).ToList();
            return FromEntity.ToUserList(entities);
        }

        public IEnumerable<User> FindAllAdmins()
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_users> entities = uow.Users.Where(x => x.IsAdmin).ToList();
            return FromEntity.ToUserList(entities);
        }

        public User SaveOrUpdateUser(User user)
        {
            var uow = new UnitOfWork(connectionString);
            explik_users entity = uow.UsersRepository.GetById(user.Id.ToString());
            if (entity == null)
            {
                // Turn the domain object into a database entity
                entity = new explik_users();
                ToEntity.FromUser(user, entity);
                uow.UsersRepository.Add(entity);
                uow.Save();

                user = FromEntity.ToUser(entity);
            }
            else
            {
                ToEntity.FromUser(user, entity);
                uow.Save();
            }

            return user;
        }

        #endregion

        #region ICommentRepository

        public void DeleteComment(int commentId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_comments entity = uow.Comments.Where(x => x.Id == commentId).Single();
            uow.CommentsRepository.Delete(entity);
            uow.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="rating"></param>
        public void UpdateCommentRating(int commentId, int rating)
        {
            var uow = new UnitOfWork(connectionString);
            explik_comments entity = uow.Comments.Where(x => x.Id == commentId).Single();
            entity.Rating = rating;
            uow.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="text"></param>
        public void UpdateComment(int commentId, string text)
        {
            var uow = new UnitOfWork(connectionString);
            explik_comments entity = uow.Comments.Where(x => x.Id == commentId).Single();
            entity.Text = text;
            entity.IsControlled = false;
            entity.IsRejected = false;
            uow.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        public void ValidateComment(int commentId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_comments entity = uow.Comments.Where(x => x.Id == commentId).Single();
            entity.IsControlled = true;
            entity.IsRejected = false;
            uow.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        public void RejectComment(int commentId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_comments entity = uow.Comments.Where(x => x.Id == commentId).Single();
            entity.IsControlled = true;
            entity.IsRejected = true;
            uow.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> FindCommentsByPage(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_comments> entities = uow.Comments.Where(x => x.PageId == pageId &&
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
            var uow = new UnitOfWork(connectionString);
            explik_comments comment = uow.Comments.Where(x => x.CreatedBy == username && x.PageId == pageId).SingleOrDefault();
            if (comment != null)
            {
                return (int)comment.Rating;
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
            var uow = new UnitOfWork(connectionString);
            List<explik_comments> entities = uow.Comments.Where(x => x.IsControlled == false && x.IsRejected == false && x.Text != null && x.Text != "").ToList();
            return FromEntity.ToCommentList(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        public void AddComment(Comment comment)
        {
            var uow = new UnitOfWork(connectionString);
            explik_comments entity = new explik_comments();
            ToEntity.FromComment(comment, entity);
            uow.CommentsRepository.Add(entity);
            uow.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Comment FindCommentByPageAndUser(int pageId, string username)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_comments> entities = uow.Comments.Where(x => x.PageId == pageId && x.CreatedBy == username).ToList();
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
            var uow = new UnitOfWork(connectionString);
            var entities = uow.Comments.Where(x => x.PageId == pageId).ToList();
            foreach (explik_comments entity in entities)
            {
                uow.CommentsRepository.Delete(entity);
            }

            uow.Save();
        }

        #endregion

        #region IAlertRepository

        public void DeleteAlert(int alertId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_alerts entity = uow.Alerts.Where(x => x.Id == alertId).Single();
            uow.AlertsRepository.Delete(entity);
            uow.Save();
        }

        public IEnumerable<Alert> FindAlertsByPage(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_alerts> entities = uow.Alerts.Where(x => x.PageId == pageId).ToList();
            return FromEntity.ToAlertList(entities);
        }

        public Alert FindAlertByPageAndUser(int pageId, string username)
        {
            var uow = new UnitOfWork(connectionString);
            explik_alerts entitie = uow.Alerts.Where(x => x.PageId == pageId && x.CreatedBy == username).FirstOrDefault();
            if (entitie != null)
            {
                return FromEntity.ToAlert(entitie);
            }

            return null;
        }

        public IEnumerable<Alert> GetAlerts()
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_alerts> entities = uow.Alerts.ToList();
            if (entities != null)
            {
                return FromEntity.ToAlertList(entities);
            }

            return null;
        }

        //public IEnumerable<Alert> FindAlertsByComment(Guid commentGuid)
        //{
        //    var uow = new UnitOfWork(connectionString);
        //    List<explik_alerts> entities = uow.Alerts.Where(x => x.CommentId == commentGuid.ToString()).ToList();
        //    return FromEntity.ToAlertList(entities);
        //}

        public void AddAlert(Alert alert)
        {
            var uow = new UnitOfWork(connectionString);
            explik_alerts entity = new explik_alerts();
            ToEntity.FromAlert(alert, entity);
            uow.AlertsRepository.Add(entity);
            uow.Save();
        }

        public void DeletePageAlerts(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_alerts> entities = uow.Alerts.Where(x => x.PageId == pageId).ToList();
            foreach (var entity in entities)
            {
                uow.AlertsRepository.Delete(entity);
            }

            uow.Save();
        }

        public void DeletPageAlertsByUser(int pageId, string username)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_alerts> entities = uow.Alerts.Where(x => x.PageId == pageId && x.CreatedBy == username).ToList();
            foreach (var entity in entities)
            {
                uow.AlertsRepository.Delete(entity);
            }

            uow.Save();
        }

        //public void DeleteCommentAlerts(Guid commentId)
        //{
        //    var uow = new UnitOfWork(connectionString);
        //    List<explik_alerts> entities = uow.Alerts.Where(x => x.CommentId == commentId.ToString()).ToList();
        //    foreach (var entity in entities)
        //    {
        //        uow.AlertsRepository.Delete(entity);
        //    }

        //    uow.Save();
        //}

        #endregion

        #region ICompetitionRepository

        public void DeleteCompetition(int id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_competition entity = uow.Competitions.Where(x => x.Id == id).Single();
            uow.CompetitionRepository.Delete(entity);
            uow.Save();
        }

        public IEnumerable<Competition> GetCompetitions(bool forAdmin = false)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_competition> entities;
            if (forAdmin)
            {
                // for admin, get all competitions
                entities = uow.Competitions.ToList();
            }
            else
            {
                // for other users, hide Pause and Init statuses
                entities = uow.Competitions.Where(c => 
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
            var uow = new UnitOfWork(connectionString);
            explik_competition entity = uow.Competitions.SingleOrDefault(x => x.PageTag == tag);
            if (entity != null)
            {
                return FromEntity.ToCompetition(entity);
            }

            return null;
        }
        public Competition GetCompetitionById(int id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_competition entity = uow.Competitions.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                return FromEntity.ToCompetition(entity);
            }

            return null;
        }

        public Competition GetCompetitionByStatus(int status)
        {
            var uow = new UnitOfWork(connectionString);
            explik_competition entity = uow.Competitions.SingleOrDefault(x => x.Status == status);
            if (entity != null)
            {
                return FromEntity.ToCompetition(entity);
            }

            return null;
        }

        public int GetOnGoingCompetitionId()
        {
            var uow = new UnitOfWork(connectionString);
            explik_competition entity = uow.Competitions.SingleOrDefault(x =>
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
            var uow = new UnitOfWork(connectionString);
            explik_competition entity = new explik_competition();
            ToEntity.FromCompetition(competition, entity);
            uow.CompetitionRepository.Add(entity);
            uow.Save();
        }

        public void UpdateCompetition(Competition competition)
        {
            var uow = new UnitOfWork(connectionString);
            explik_competition entity = uow.Competitions.SingleOrDefault(x => x.Id == competition.Id);
            ToEntity.FromCompetition(competition, entity);
            uow.Save();
        }

        public IEnumerable<CompetitionPage> GetCompetitionPages(int competitionId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_competitionpage> entities = uow.CompetitionPages.Where(x => x.CompetitionId == competitionId).ToList();
            if (entities != null)
            {
                return FromEntity.ToCompetitionPageList(entities);
            }

            return null;
        }

        public void UpdateCompetitionPageRanking(int competitionId, int pageId, int ranking)
        {
            var uow = new UnitOfWork(connectionString);
            var entity = uow.CompetitionPages.SingleOrDefault(x => x.CompetitionId == competitionId && x.PageId == pageId);
            entity.Ranking = ranking;
            uow.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public int GetPageRanking(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            var entity = uow.CompetitionPages.SingleOrDefault(x => x.PageId == pageId);
            if (entity != null)
            {
                return (int)entity.Ranking;
            }

            return 0; // no participation in a competition
        }

        public void UpdateCompetitionPageId(int competitionId, int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_competition entity = uow.Competitions.SingleOrDefault(x => x.Id == competitionId);
            entity.PageId = pageId;
            uow.Save();
        }

        public int[] GetUserHits(string username)
        {
            var uow = new UnitOfWork(connectionString);
            int[] hits = new int[] { 0, 0, 0 };

            hits[0] = uow.CompetitionPages.Count(x => x.UserName == username && x.Ranking == 1);
            hits[1] = uow.CompetitionPages.Count(x => x.UserName == username && x.Ranking == 2);
            hits[2] = uow.CompetitionPages.Count(x => x.UserName == username && x.Ranking == 3);
            return hits;
        }

        public void ArchiveCompetitionPage(int competitionId, Page page)
        {
            var uow = new UnitOfWork(connectionString);
            explik_competitionpage entity = new explik_competitionpage();
            //ToEntity.FromCompetitionPage(competition, entity);
            entity.CompetitionId = competitionId;
            entity.PageId = page.Id;
            entity.NbRating = (int)page.NbRating;
            entity.TotalRating = (int)page.TotalRating;
            entity.UserName = page.CreatedBy;
            uow.CompetitionPagesRepository.Add(entity);
            uow.Save();
        }

        #endregion

        #region ICourseRepository

        public Course GetCourseById(int id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_course entity = uow.Courses.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                return FromEntity.ToCourse(entity);
            }

            return null;
        }

        public int AddCourse(Course course)
        {
            var uow = new UnitOfWork(connectionString);
            explik_course entity = new explik_course();
            ToEntity.FromCourse(course, entity);

            entity.Id = 0;
            uow.CourseRepository.Add(entity);
            uow.Save();
            course.Id = entity.Id;
            return entity.Id;
        }

        public void UpdateCourse(Course course)
        {
            var uow = new UnitOfWork(connectionString);
            explik_course entity = uow.Courses.SingleOrDefault(x => x.Id == course.Id);
            if (entity != null)
            {
                ToEntity.FromCourse(course, entity);
                uow.Save();
            }
        }

        public void UpdateCourseTitle(int id, string title)
        {
            var uow = new UnitOfWork(connectionString);
            explik_course entity = uow.Courses.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                entity.Title = title;
                uow.Save();
            }
        }

        public CoursePage GetCoursePageById(int id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_coursepage entity = uow.CoursePages.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                return FromEntity.ToCoursePage(entity);
            }

            return null;
        }

        public int AddCoursePage(CoursePage coursePage)
        {
            var uow = new UnitOfWork(connectionString);
            explik_coursepage entity = new explik_coursepage();
            ToEntity.FromCoursePage(coursePage, entity);

            entity.Id = 0;
            uow.CoursePageRepository.Add(entity);
            uow.Save();
            coursePage.Id = entity.Id;
            return entity.Id;

        }
        public void UpdateCoursePageOrder(int coursePageId, int order)
        {
            var uow = new UnitOfWork(connectionString);
            explik_coursepage entity = uow.CoursePages.SingleOrDefault(x => x.Id == coursePageId);
            if (entity != null)
            {
                entity.Order = order;
                uow.Save();
            }
        }

        public IEnumerable<Course> GetCoursesByUser(string createdBy)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_course> entities = uow.Courses.Where(x => x.CreatedBy == createdBy).ToList();
            if (entities != null)
            {
                return FromEntity.ToCourseList(entities);
            }

            return null;
        }

        //public Course GetCourseByPage(string tag)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable<CoursePage> GetCoursePages(int courseId)
        {
            var uow = new UnitOfWork(connectionString);
            var entities = uow.CoursePages.Where(x => x.CourseId == courseId);
            if (entities != null)
            {
                return FromEntity.ToCoursePageList(entities.ToList());
            }
            return null;
        }

        public IEnumerable<Page> GetPagesByCourseId(int courseId)
        {
            var uow = new UnitOfWork(connectionString);
            List<int> pageIds = uow.CoursePages.Where(x => x.CourseId == courseId).Select(x => x.PageId).ToList();
            List<explik_pages> entities = uow.Pages.Where(x => pageIds.Contains (x.Id)).ToList();
            if (entities != null)
            {
                return FromEntity.ToPageList(entities);
            }
            return null;
        }

        // Delete a course and its course pages
        public void DeleteCourse(int id)
        {
            var uow = new UnitOfWork(connectionString);
            explik_course entity = uow.CourseRepository.GetById(id);
            uow.CourseRepository.Delete(entity);

            uow.Save();
        }

        public void DeleteCoursePage(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_coursepage entity = uow.CoursePageRepository.GetById(pageId);
            uow.CoursePageRepository.Delete(entity);

            uow.Save();
        }

        public void DeleteCoursePages(int courseId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_coursepage> entities = uow.CoursePages.Where(x => x.CourseId == courseId).ToList();
            foreach (var pageEntity in entities)
            {
                uow.CoursePageRepository.Delete(pageEntity);
            }

            uow.Save();
        }

        public void DeleteCourses()
        {
            var uow = new UnitOfWork(connectionString);
            uow.CourseRepository.DeleteAll();
            uow.Save();

            uow.CoursePageRepository.DeleteAll();
            uow.Save();
        }

        public void DeleteCoursePagesforPageId(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            List<explik_coursepage> entities = uow.CoursePages.Where(x => x.PageId == pageId).ToList();
            foreach (var entity in entities)
            {
                uow.CoursePageRepository.Delete(entity);
            }

            uow.Save();
        }

        public Course FindCourseByPageId(int pageId)
        {
            var uow = new UnitOfWork(connectionString);
            explik_coursepage entitiy = uow.CoursePages.FirstOrDefault(x => x.PageId == pageId);
            if (entitiy != null)
            {
                explik_course entity = uow.Courses.SingleOrDefault(x => x.Id == entitiy.CourseId);
                if (entity != null)
                {
                    return FromEntity.ToCourse(entity);
                }
            }
 
            return null;
        }

        #endregion


        #region IDisposable
        public void Dispose()
		{
			//uow.Save();
			//uow.Dispose();
		}
		#endregion

		private void EnsureConectionString()
		{
			if (_applicationSettings.Installed && string.IsNullOrEmpty(_applicationSettings.ConnectionString))
				throw new DatabaseException("The connection string is empty in the web.config file (and the roadkill.config's installed=true).", null);
		}
    }
}

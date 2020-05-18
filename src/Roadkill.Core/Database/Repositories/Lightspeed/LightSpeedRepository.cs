﻿using System;
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
        private ApplicationSettings _applicationSettings;

        #region IQueryable

        internal IQueryable<explik_pages> Pages
        {
            get { return UnitOfWork.PagesRepository.GetAll(); }
        }

        internal IQueryable<explik_pagecontent> PageContents
        {
            get { return UnitOfWork.PageContentRepository.GetAll(); }
        }

        internal IQueryable<explik_users> Users
        {
            get { return UnitOfWork.UsersRepository.GetAll(); }
        }

        internal IQueryable<explik_comments> Comments
        {
            get { return UnitOfWork.CommentsRepository.GetAll(); }
        }

        internal IQueryable<explik_alerts> Alerts
        {
            get { return UnitOfWork.AlertsRepository.GetAll(); }
        }

        internal IQueryable<explik_competition> Competitions
        {
            get { return UnitOfWork.CompetitionRepository.GetAll(); }
        }

        internal IQueryable<explik_competitionpage> CompetitionPages
        {
            get { return UnitOfWork.CompetitionPagesRepository.GetAll(); }
        }

        internal IQueryable<explik_course> Courses
        {
            get { return UnitOfWork.CourseRepository.GetAll(); }
        }

        internal IQueryable<explik_coursepage> CoursePages
        {
            get { return UnitOfWork.CoursePageRepository.GetAll(); }
        }

        #endregion

        //public virtual LightSpeedContext Context
        //{
        //    get
        //    {
        //        return UnitOfWork.Co²
        //        LightSpeedContext context = ObjectFactory.GetInstance<LightSpeedContext>();
        //        if (context == null)
        //            throw new DatabaseException("The context for Lightspeed is null - has Startup() been called?", null);

        //        return context;
        //    }
        //}
        public UnitOfWork UnitOfWork;

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
        }

        #region IRepository

        //public void Startup(DataStoreType dataStoreType, string connectionString, bool enableCache)
        //{
        //    if (!string.IsNullOrEmpty(connectionString))
        //    {
        //        LightSpeedContext context = new LightSpeedContext();
        //        context.ConnectionString = connectionString;
        //        context.DataProvider = dataStoreType.LightSpeedDbType;
        //        context.IdentityMethod = IdentityMethod.GuidComb;
        //        context.CascadeDeletes = true;
        //        context.VerboseLogging = true;
        //        context.Logger = new DatabaseLogger();

        //        if (enableCache)
        //            context.Cache = new CacheBroker(new DefaultCache());

        //        ObjectFactory.Configure(x =>
        //        {
        //            x.For<LightSpeedContext>().Singleton().Use(context);
        //            x.For<IUnitOfWork>().HybridHttpOrThreadLocalScoped()
        //                .Use(ctx => ctx.GetInstance<LightSpeedContext>().CreateUnitOfWork());
        //        });
        //    }
        //    else
        //    {
        //        Log.Warn("LightSpeedRepository.Startup skipped as no connection string was provided");
        //    }
        //}

        //public void TestConnection(DataStoreType dataStoreType, string connectionString)
        //{
        //    LightSpeedContext context = ObjectFactory.GetInstance<LightSpeedContext>();
        //    if (context == null)
        //        throw new InvalidOperationException("Repository.Test failed - LightSpeedContext was null from the ObjectFactory");

        //    using (IDbConnection connection = context.DataProviderObjectFactory.CreateConnection())
        //    {
        //        connection.ConnectionString = connectionString;
        //        connection.Open();
        //    }
        //}

        #endregion

        #region ISettingsRepository



        public SiteSettings GetSiteSettings()
        {
            SiteSettings siteSettings = new SiteSettings();
            explik_siteconfiguration entity = UnitOfWork.SiteConfigurationRepository.FirstOrDefault(x => x.Id == SiteSettings.SiteSettingsId.ToString());

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
            explik_siteconfiguration entity = UnitOfWork.SiteConfigurationRepository.FirstOrDefault(x => x.Id == databaseId.ToString());

            if (entity != null)
            {
                pluginSettings = PluginSettings.LoadFromJson(entity.Content);
            }

            return pluginSettings;
        }

        public void SaveSiteSettings(SiteSettings siteSettings)
        {
            explik_siteconfiguration entity = UnitOfWork.SiteConfigurationRepository.FirstOrDefault(x => x.Id == SiteSettings.SiteSettingsId.ToString());

            if (entity == null || entity.Id == Guid.Empty.ToString())
            {
                entity = new explik_siteconfiguration();
                entity.Id = SiteSettings.SiteSettingsId.ToString();
                entity.Version = ApplicationSettings.ProductVersion.ToString();
                entity.Content = siteSettings.GetJson();
                UnitOfWork.SiteConfigurationRepository.Insert(entity);
            }
            else
            {
                entity.Version = ApplicationSettings.ProductVersion.ToString();
                entity.Content = siteSettings.GetJson();
            }

            UnitOfWork.Save();
        }

        public void SaveTextPluginSettings(TextPlugin plugin)
        {
            string version = plugin.Version;
            if (string.IsNullOrEmpty(version))
                version = "1.0.0";

            explik_siteconfiguration entity = UnitOfWork.SiteConfigurationRepository.FirstOrDefault(x => x.Id == plugin.DatabaseId.ToString());

            if (entity == null || entity.Id == Guid.Empty.ToString())
            {
                entity = new explik_siteconfiguration();
                entity.Id = plugin.DatabaseId.ToString();
                entity.Version = version;
                entity.Content = plugin.Settings.GetJson();
                UnitOfWork.SiteConfigurationRepository.Add(entity);
            }
            else
            {
                entity.Version = version;
                entity.Content = plugin.Settings.GetJson();
            }

            UnitOfWork.Save();
        }

        #endregion

        #region IPageRepository

        public PageContent AddNewPage(Page page, string text, string editedBy, DateTime editedOn)
        {
            explik_pages pageEntity = new explik_pages();
            ToEntity.FromPage(page, pageEntity);
            pageEntity.Id = 0;
            UnitOfWork.PagesRepository.Add(pageEntity);
            UnitOfWork.Save();

            explik_pagecontent pageContentEntity = new explik_pagecontent()
            {
                Id = Guid.NewGuid().ToString(),
                PageId = pageEntity.Id,
                Text = text,
                ControlledBy = "",
                EditedOn = editedOn,
                VersionNumber = 1,
            };

            UnitOfWork.PageContentRepository.Add(pageContentEntity);
            UnitOfWork.Save();

            PageContent pageContent = FromEntity.ToPageContent(pageContentEntity, pageEntity);
            pageContent.Page = FromEntity.ToPage(pageEntity);
            return pageContent;
        }

        public PageContent AddNewPageContentVersion(Page page, string text, DateTime editedOn, int version)
        {
            if (version < 1)
                version = 1;

            explik_pages pageEntity = UnitOfWork.PagesRepository.GetById(page.Id);
            if (pageEntity != null)
            {
                // Update the content
                explik_pagecontent pageContentEntity = new explik_pagecontent()
                {
                    Id = Guid.NewGuid().ToString(),
                    PageId = page.Id,
                    Text = text,
                    ControlledBy = "",
                    EditedOn = editedOn,
                    VersionNumber = version,
                };

                UnitOfWork.PageContentRepository.Add(pageContentEntity);
                UnitOfWork.Save();

                // The page modified fields
                pageEntity.PublishedOn = editedOn;
                pageEntity.ControlledBy = "";
                UnitOfWork.Save();

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
            List<explik_pages> entities = Pages.ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> AllNewPages()
        {
            List<explik_pages> entities = Pages.Where(p =>
                p.IsRejected == false && p.IsSubmitted == true && p.IsControlled == false && p.IsCopied == false).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> MyPages(string createdBy)
        {
            List<explik_pages> entities = Pages.Where(p => p.CreatedBy == createdBy).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<PageContent> AllPageContents()
        {
            List<explik_pagecontent> entities = PageContents.ToList();
            return FromEntity.ToPageContentList(entities, null);
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
            UnitOfWork.PagesRepository.DeleteAll();
            UnitOfWork.Save();

            UnitOfWork.PageContentRepository.DeleteAll();
            UnitOfWork.Save();
        }

        public void DeletePage(int pageId)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
            UnitOfWork.PagesRepository.Delete(entity);
            UnitOfWork.Save();
        }

        public void SetDraft(int pageId)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
            entity.IsControlled = false;
            entity.IsSubmitted = false;
            entity.IsRejected = false;
            UnitOfWork.Save();
        }

        public void SubmitPage(int pageId)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
            entity.IsControlled = false;
            entity.IsRejected = false;
            entity.IsSubmitted = true;
            UnitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        public void RejectPage(int pageId)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
            entity.IsControlled = false;
            entity.IsRejected = true;
            entity.IsSubmitted = false;
            UnitOfWork.Save();
        }


        public void DeletePageContent(PageContent pageContent)
        {
            explik_pagecontent entity = UnitOfWork.PageContentRepository.GetById(pageContent.Id);
            UnitOfWork.PageContentRepository.Delete(entity);
            UnitOfWork.Save();
        }

        public IEnumerable<Page> FindMostRecentPages(int number)
        {
            // ignore all pages that are in the ongoing competition
            int id = GetOnGoingCompetitionId();
            if (id != -1)
            {
                List<explik_pages> entities = Pages
                    .Where(p => p.IsControlled && !p.IsLocked && p.CompetitionId != id)
                    .OrderByDescending(p => p.PublishedOn)
                    .Take(number)
                    .ToList();
                return FromEntity.ToPageList(entities);
            }
            else
            {
                List<explik_pages> entities = Pages
                    .Where(p => p.IsControlled && !p.IsLocked)
                    .OrderByDescending(p => p.PublishedOn)
                    .Take(number)
                    .ToList();
                return FromEntity.ToPageList(entities);
            }
            
        }

        public IEnumerable<Page> FindPagesBestRated(int number)
        {
            List<explik_pages> entities = Pages
                .Where(p => p.IsControlled && !p.IsLocked)
                .OrderByDescending(p => p.NbRating == 0 ? 0 : (float)p.TotalRating / (float)p.NbRating) //TODO : use also explikRating
                .Take(number)
                .ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesMostViewed(int number)
        {
            List<explik_pages> entities = Pages
                .Where(p => p.IsControlled && !p.IsLocked)
                .OrderByDescending(p => p.NbView)
                .Take(number)
                .ToList();
            return FromEntity.ToPageList(entities);
        }

        public string GetPageTitle(int pageId)
        {
            explik_pages entity = Pages.FirstOrDefault(p => p.Id == pageId);
            if (entity != null)
                return entity.Title;
            else
                return "";
        }

        public IEnumerable<Page> FindPagesCreatedBy(string username)
        {
            List<explik_pages> entities = Pages.Where(p => p.CreatedBy == username).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindControlledPagesByCompetitionId(int competitionId)
        {
            List<explik_pages> entities = Pages.Where(p => p.CompetitionId == competitionId && p.IsControlled == true).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesByCompetitionId(int competitionId)
        {
            List<explik_pages> entities = Pages.Where(p => p.CompetitionId == competitionId).ToList();
            return FromEntity.ToPageList(entities);
        }

        public void DeletCompetitionPages(int competitionId)
        {
            List<explik_competitionpage> entities = CompetitionPages.Where(x => x.CompetitionId == competitionId).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.CompetitionPagesRepository.Delete(entity);
            }

            UnitOfWork.Save();
        }

        public void DeletCompetitionPage(int pageId)
        {
            List<explik_competitionpage> entities = CompetitionPages.Where(x => x.PageId == pageId).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.CompetitionPagesRepository.Delete(entity);
            }

            UnitOfWork.Save();
        }

        public IEnumerable<Page> FindPagesControlledBy(string username)
        {
            List<explik_pages> entities = Pages.Where(p => p.ControlledBy == username).ToList();
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesContainingTag(string tag)
        {
            IEnumerable<explik_pages>
                entities = Pages.Where(p =>
                    p.Tags.ToLower().Contains(tag.ToLower())); // Lightspeed doesn't support ToLowerInvariant
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindControlledPagesByTag(string tag)
        {
            IEnumerable<explik_pages>
                entities = Pages.Where(p =>
                    p.Tags.ToLower().Contains(tag.ToLower())// Lightspeed doesn't support ToLowerInvariant
                    && p.CompetitionId == -1 // ignore if in competition
                    && p.IsControlled == true);
            return FromEntity.ToPageList(entities);
        }

        public IEnumerable<Page> FindPagesWithAlerts()
        {
            List<int> pageIds = Alerts.GroupBy(a => (int)a.PageId).Select(a => a.First()).Select(a => (int)a.PageId).ToList();
            IEnumerable<explik_pages>
                entities = Pages.Where(p => pageIds.Contains(p.Id)); // Lightspeed doesn't support ToLowerInvariant
            return FromEntity.ToPageList(entities);

        }

        public IEnumerable<PageContent> FindPageContentsByPageId(int pageId)
        {
            List<explik_pagecontent> entities = PageContents.Where(p => p.PageId == pageId).ToList();
            explik_pages pageEntity = UnitOfWork.PagesRepository.GetById(pageId);
            return FromEntity.ToPageContentList(entities, pageEntity);
        }

        //public IEnumerable<PageContent> FindPageContentsEditedBy(string username)
        //{
        //	List<explik_pagecontent> entities = PageContents.Where(p => p.ControlledBy == username).ToList();
        //	return FromEntity.ToPageContentList(entities);
        //}

        public Page GetPageById(int id)
        {
            explik_pages entity = Pages.FirstOrDefault(p => p.Id == id);
            return FromEntity.ToPage(entity);
        }

        public Page GetPageByTitle(string title)
        {
            explik_pages entity = Pages.FirstOrDefault(p => p.Title.ToLower() == title.ToLower());
            return FromEntity.ToPage(entity);
        }

        public PageContent GetLatestPageContent(int pageId)
        {
            explik_pagecontent entity = PageContents.Where(x => x.PageId == pageId).OrderByDescending(x => x.EditedOn)
                .FirstOrDefault();
            explik_pages pageEntity = UnitOfWork.PagesRepository.GetById(pageId);
            return FromEntity.ToPageContent(entity, pageEntity);
        }

        public PageContent GetPageContentById(Guid id)
        {
            explik_pagecontent entity = PageContents.FirstOrDefault(p => p.Id == id.ToString());
            explik_pages pageEntity = UnitOfWork.PagesRepository.GetById(entity.PageId);
            return FromEntity.ToPageContent(entity, pageEntity);
        }

        public PageContent GetPageContentByPageIdAndVersionNumber(int id, int versionNumber)
        {
            explik_pagecontent entity = PageContents.FirstOrDefault(p => p.PageId == id && p.VersionNumber == versionNumber);
            explik_pages pageEntity = UnitOfWork.PagesRepository.GetById(entity.PageId);
            return FromEntity.ToPageContent(entity, pageEntity);
        }

        //public IEnumerable<PageContent> GetPageContentByEditedBy(string username)
        //{
        //	List<explik_pagecontent> entities = PageContents.Where(p => p.EditedBy == username).ToList();
        //	return FromEntity.ToPageContentList(entities);
        //}

        public Page SaveOrUpdatePage(Page page)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(page.Id);
            if (entity == null)
            {
                entity = new explik_pages();
                ToEntity.FromPage(page, entity);
                UnitOfWork.PagesRepository.Add(entity);
                UnitOfWork.Save();
                page = FromEntity.ToPage(entity);
            }
            else
            {
                ToEntity.FromPage(page, entity);
                UnitOfWork.Save();
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
            explik_pagecontent entity = UnitOfWork.PageContentRepository.GetById(content.Id);
            if (entity != null)
            {
                ToEntity.FromPageContent(content, entity);
                UnitOfWork.Save();
                explik_pages pageEntity = UnitOfWork.PagesRepository.GetById(entity.PageId);
                content = FromEntity.ToPageContent(entity, pageEntity);
            }
        }

        public void IncrementNbView(int pageId)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.NbView++;
                UnitOfWork.Save();
            }
        }

        public void SetNbView(int pageId, int nbView)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.NbView = nbView;
                UnitOfWork.Save();
            }
        }

        public void SetCompetitionId(int pageId, int competitionId)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.CompetitionId = competitionId;
                UnitOfWork.Save();
            }
        }
        public void SetRating(int pageId, int nbRating, int totalRating)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.TotalRating = totalRating;
                entity.NbRating = nbRating;
                UnitOfWork.Save();
            }
        }

        public void AddPageRating(int pageId, int rating)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
            if (entity != null)
            {
                entity.TotalRating += rating;
                entity.NbRating++;
                UnitOfWork.Save();
            }
        }

        public void RemovePageRating(int pageId, int rating)
        {
            explik_pages entity = UnitOfWork.PagesRepository.GetById(pageId);
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
                UnitOfWork.Save();
            }
        }

        /// <summary>
        /// Remove the competitionId if the pages have not been controlled
        /// </summary>
        /// <param name="competitionId"></param>
        public void CleanPagesForCompetitionId(int competitionId)
        {
            List<explik_pages> entities = Pages.Where(x => x.CompetitionId == competitionId && !x.IsControlled).ToList();
            foreach (var entity in entities)
            {
                entity.CompetitionId = -1;
            }

            UnitOfWork.Save();
        }


        #endregion

        #region IUserRepository

        public void DeleteUser(User user)
        {
            explik_users entity = UnitOfWork.UsersRepository.GetById(user.Id);
            UnitOfWork.UsersRepository.Delete(entity);
            UnitOfWork.Save();
        }

        public void DeleteAllUsers()
        {
            UnitOfWork.UsersRepository.DeleteAll();
            UnitOfWork.Save();
        }

        public User GetAdminById(Guid id)
        {
            explik_users entity = Users.FirstOrDefault(x => x.Id == id.ToString() && x.IsAdmin);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByActivationKey(string key)
        {
            explik_users entity = Users.FirstOrDefault(x => x.ActivationKey == key && x.IsActivated == false);
            return FromEntity.ToUser(entity);
        }

        public User GetEditorById(Guid id)
        {
            explik_users entity = Users.FirstOrDefault(x => x.Id == id.ToString() && x.IsEditor);
            return FromEntity.ToUser(entity);
        }

        public User GetControllerById(Guid id)
        {
            explik_users entity = Users.FirstOrDefault(x => x.Id == id.ToString() && x.IsController);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByEmail(string email, bool? isActivated = null)
        {
            explik_users entity;

            if (isActivated.HasValue)
                entity = Users.FirstOrDefault(x => x.Email == email && x.IsActivated == isActivated);
            else
                entity = Users.FirstOrDefault(x => x.Email == email);

            return FromEntity.ToUser(entity);
        }

        public User GetUserById(Guid id, bool? isActivated = null)
        {
            explik_users entity;

            if (isActivated.HasValue)
                entity = Users.FirstOrDefault(x => x.Id == id.ToString() && x.IsActivated == isActivated);
            else
                entity = Users.FirstOrDefault(x => x.Id == id.ToString());

            return FromEntity.ToUser(entity);
        }

        public User GetUserByPasswordResetKey(string key)
        {
            explik_users entity = Users.FirstOrDefault(x => x.PasswordResetKey == key);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByUsername(string username)
        {
            explik_users entity = Users.FirstOrDefault(x => x.Username == username);
            return FromEntity.ToUser(entity);
        }

        public User GetUserByUsernameOrEmail(string username, string email)
        {
            explik_users entity = Users.FirstOrDefault(x => x.Username == username || x.Email == email);
            return FromEntity.ToUser(entity);
        }

        public IEnumerable<User> FindAllEditors()
        {
            List<explik_users> entities = Users.Where(x => x.IsEditor).ToList();
            return FromEntity.ToUserList(entities);
        }

        public IEnumerable<User> FindAllControllers()
        {
            List<explik_users> entities = Users.Where(x => x.IsController).ToList();
            return FromEntity.ToUserList(entities);
        }

        public IEnumerable<User> FindAllAdmins()
        {
            List<explik_users> entities = Users.Where(x => x.IsAdmin).ToList();
            return FromEntity.ToUserList(entities);
        }

        public User SaveOrUpdateUser(User user)
        {
            explik_users entity = UnitOfWork.UsersRepository.GetById(user.Id);
            if (entity == null)
            {
                // Turn the domain object into a database entity
                entity = new explik_users();
                ToEntity.FromUser(user, entity);
                UnitOfWork.UsersRepository.Add(entity);
                UnitOfWork.Save();

                user = FromEntity.ToUser(entity);
            }
            else
            {
                ToEntity.FromUser(user, entity);
                UnitOfWork.Save();
            }

            return user;
        }

        #endregion

        #region ICommentRepository

        public void DeleteComment(Guid commentId)
        {
            explik_comments entity = Comments.Where(x => x.Id == commentId.ToString()).Single();
            UnitOfWork.CommentsRepository.Delete(entity);
            UnitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="rating"></param>
        public void UpdateCommentRating(Guid commentId, int rating)
        {
            explik_comments entity = Comments.Where(x => x.Id == commentId.ToString()).Single();
            entity.Rating = rating;
            UnitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="text"></param>
        public void UpdateComment(Guid commentId, string text)
        {
            explik_comments entity = Comments.Where(x => x.Id == commentId.ToString()).Single();
            entity.Text = text;
            entity.IsControlled = false;
            entity.IsRejected = false;
            UnitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        public void ValidateComment(Guid commentId)
        {
            explik_comments entity = Comments.Where(x => x.Id == commentId.ToString()).Single();
            entity.IsControlled = true;
            entity.IsRejected = false;
            UnitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        public void RejectComment(Guid commentId)
        {
            explik_comments entity = Comments.Where(x => x.Id == commentId.ToString()).Single();
            entity.IsControlled = true;
            entity.IsRejected = true;
            UnitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> FindCommentsByPage(int pageId)
        {
            List<explik_comments> entities = Comments.Where(x => x.PageId == pageId &&
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
            explik_comments comment = Comments.Where(x => x.CreatedBy == username && x.PageId == pageId).SingleOrDefault();
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
            List<explik_comments> entities = Comments.Where(x => x.IsControlled == false && x.IsRejected == false && x.Text != null && x.Text != "").ToList();
            return FromEntity.ToCommentList(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        public void AddComment(Comment comment)
        {
            explik_comments entity = new explik_comments();
            ToEntity.FromComment(comment, entity);
            UnitOfWork.CommentsRepository.Add(entity);
            UnitOfWork.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Comment FindCommentByPageAndUser(int pageId, string username)
        {
            List<explik_comments> entities = Comments.Where(x => x.PageId == pageId && x.CreatedBy == username).ToList();
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
            foreach (explik_comments entity in entities)
            {
                UnitOfWork.CommentsRepository.Delete(entity);
            }

            UnitOfWork.Save();
        }

        #endregion

        #region IAlertRepository

        public void DeleteAlert(Guid alertId)
        {
            explik_alerts entity = Alerts.Where(x => x.Id == alertId.ToString()).Single();
            UnitOfWork.AlertsRepository.Delete(entity);
            UnitOfWork.Save();
        }

        public IEnumerable<Alert> FindAlertsByPage(int pageId)
        {
            List<explik_alerts> entities = Alerts.Where(x => x.PageId == pageId).ToList();
            return FromEntity.ToAlertList(entities);
        }

        public Alert FindAlertByPageAndUser(int pageId, string username)
        {
            explik_alerts entitie = Alerts.Where(x => x.PageId == pageId && x.CreatedBy == username).FirstOrDefault();
            if (entitie != null)
            {
                return FromEntity.ToAlert(entitie);
            }

            return null;
        }

        public IEnumerable<Alert> GetAlerts()
        {
            List<explik_alerts> entities = Alerts.ToList();
            if (entities != null)
            {
                return FromEntity.ToAlertList(entities);
            }

            return null;
        }

        public IEnumerable<Alert> FindAlertsByComment(Guid commentGuid)
        {
            List<explik_alerts> entities = Alerts.Where(x => x.CommentId == commentGuid.ToString()).ToList();
            return FromEntity.ToAlertList(entities);
        }

        public void AddAlert(Alert alert)
        {
            explik_alerts entity = new explik_alerts();
            ToEntity.FromAlert(alert, entity);
            UnitOfWork.AlertsRepository.Add(entity);
            UnitOfWork.Save();
        }

        public void DeletePageAlerts(int pageId)
        {
            List<explik_alerts> entities = Alerts.Where(x => x.PageId == pageId).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.AlertsRepository.Delete(entity);
            }

            UnitOfWork.Save();
        }

        public void DeletPageAlertsByUser(int pageId, string username)
        {
            List<explik_alerts> entities = Alerts.Where(x => x.PageId == pageId && x.CreatedBy == username).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.AlertsRepository.Delete(entity);
            }

            UnitOfWork.Save();
        }

        public void DeleteCommentAlerts(Guid commentId)
        {
            List<explik_alerts> entities = Alerts.Where(x => x.CommentId == commentId.ToString()).ToList();
            foreach (var entity in entities)
            {
                UnitOfWork.AlertsRepository.Delete(entity);
            }

            UnitOfWork.Save();
        }

        #endregion

        #region ICompetitionRepository

        public void DeleteCompetition(int id)
        {
            explik_competition entity = Competitions.Where(x => x.Id == id).Single();
            UnitOfWork.CompetitionRepository.Delete(entity);
            UnitOfWork.Save();
        }

        public IEnumerable<Competition> GetCompetitions(bool forAdmin = false)
        {
            List<explik_competition> entities;
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
            explik_competition entity = Competitions.SingleOrDefault(x => x.PageTag == tag);
            if (entity != null)
            {
                return FromEntity.ToCompetition(entity);
            }

            return null;
        }
        public Competition GetCompetitionById(int id)
        {
            explik_competition entity = Competitions.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                return FromEntity.ToCompetition(entity);
            }

            return null;
        }

        public Competition GetCompetitionByStatus(int status)
        {
            explik_competition entity = Competitions.SingleOrDefault(x => x.Status == status);
            if (entity != null)
            {
                return FromEntity.ToCompetition(entity);
            }

            return null;
        }

        public int GetOnGoingCompetitionId()
        {
            explik_competition entity = Competitions.SingleOrDefault(x =>
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
            explik_competition entity = new explik_competition();
            ToEntity.FromCompetition(competition, entity);
            UnitOfWork.CompetitionRepository.Add(entity);
            UnitOfWork.Save();
        }

        public void UpdateCompetition(Competition competition)
        {
            explik_competition entity = Competitions.SingleOrDefault(x => x.Id == competition.Id);
            ToEntity.FromCompetition(competition, entity);
            UnitOfWork.Save();
        }

        public IEnumerable<CompetitionPage> GetCompetitionPages(int competitionId)
        {
            List<explik_competitionpage> entities = CompetitionPages.Where(x => x.CompetitionId == competitionId).ToList();
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
            UnitOfWork.Save();
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
                return (int)entity.Ranking;
            }

            return 0; // no participation in a competition
        }

        public void UpdateCompetitionPageId(int competitionId, int pageId)
        {
            explik_competition entity = Competitions.SingleOrDefault(x => x.Id == competitionId);
            entity.PageId = pageId;
            UnitOfWork.Save();
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
            explik_competitionpage entity = new explik_competitionpage();
            //ToEntity.FromCompetitionPage(competition, entity);
            entity.CompetitionId = competitionId;
            entity.PageId = page.Id;
            entity.NbRating = (int)page.NbRating;
            entity.TotalRating = (int)page.TotalRating;
            entity.UserName = page.CreatedBy;
            UnitOfWork.CompetitionPagesRepository.Add(entity);
            UnitOfWork.Save();
        }

        #endregion

        #region ICourseRepository
        public Course GetCourseById(int id)
        {
            explik_course entity = Courses.SingleOrDefault(x => x.Id == id);
            if (entity != null)
            {
                return FromEntity.ToCourse(entity);
            }

            return null;
        }

        public int AddNewCourse(Course course)
        {
            explik_course entity = new explik_course();
            ToEntity.FromCourse(course, entity);

            entity.Id = 0;
            UnitOfWork.CourseRepository.Add(entity);
            UnitOfWork.Save();
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
            List<explik_course> entities = Courses.Where(x => x.CreatedBy == createdBy).ToList();
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
            List<explik_pages> entities = Pages.Where(x => pageIds.Contains (x.Id)).ToList();
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
			UnitOfWork.Save();
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

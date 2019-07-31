using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roadkill.Core;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Plugins;
using PluginSettings = Roadkill.Core.Plugins.Settings;

namespace Roadkill.Tests.Unit
{
	public class RepositoryMock : IRepository
	{
        public List<Page> Pages { get; set; }
        public List<Page> NewPages { get; set; }
        public List<PageContent> PageContents { get; set; }
		public List<User> Users { get; set; }
		public SiteSettings SiteSettings { get; set; }
		public List<TextPlugin> TextPlugins { get; set; }

		// If this is set, GetTextPluginSettings returns it instead of a lookup
		public PluginSettings PluginSettings { get; set; }

        public List<Alert> Alerts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Competition> Competitions { get; set; }
        public List<CompetitionPage> CompetitionPages { get; set; }

        public bool Installed { get; set; }
		public DataStoreType InstalledDataStoreType { get; private set; }
		public string InstalledConnectionString { get; private set; }
		public bool InstalledEnableCache { get; private set; }

		public RepositoryMock()
		{
			Pages = new List<Page>();
			PageContents = new List<PageContent>();
			Users = new List<User>();
			SiteSettings = new SiteSettings();
			TextPlugins = new List<TextPlugin>();
            Comments = new List<Comment>();
            Alerts = new List<Alert>();
            Competitions = new List<Competition>();
            CompetitionPages = new List<CompetitionPage>();
        }

		#region IRepository Members

		public void DeletePage(Page page)
		{
			Pages.Remove(page);
		}

		public void DeletePageContent(PageContent pageContent)
		{
			PageContents.Remove(pageContent);
		}

		public void DeleteUser(User user)
		{
			Users.Remove(user);
		}

		public void DeleteAllPages()
		{
			Pages = new List<Page>();
			PageContents = new List<PageContent>();
		}

		public void DeleteAllUsers()
		{
			Users = new List<User>();
		}

		public Page SaveOrUpdatePage(Page page)
		{
			Page existingPage = Pages.FirstOrDefault(x => x.Id == page.Id);

			if (existingPage == null)
			{
				page.Id = Pages.Count + 1;
				Pages.Add(page);
				existingPage = page;
			}
			else
			{
				existingPage.CreatedBy = page.CreatedBy;
				existingPage.CreatedOn = page.CreatedOn;
				existingPage.IsLocked = page.IsLocked;
				existingPage.ControlledBy = page.ControlledBy;
				existingPage.PublishedOn = page.PublishedOn;
				existingPage.Tags = page.Tags;
				existingPage.Title = page.Title;
			}

			return existingPage;
		}

		public PageContent AddNewPage(Page page, string text, string editedBy, DateTime editedOn)
		{
			page.Id = Pages.Count + 1;
			Pages.Add(page);

			PageContent content = new PageContent();
			content.Id = Guid.NewGuid();
			content.EditedOn = editedOn;
			content.Page = page;
			content.Text = text;
			content.VersionNumber = 1;
			PageContents.Add(content);

			return content;
		}

		public PageContent AddNewPageContentVersion(Page page, string text, string editedBy, DateTime editedOn, int version)
		{
			PageContent content = new PageContent();
			content.Id = Guid.NewGuid();
			page.PublishedOn = content.EditedOn = editedOn;
			content.Page = page;
			content.Text = text;
			content.VersionNumber = FindPageContentsByPageId(page.Id).Max(x => x.VersionNumber) +1;
			PageContents.Add(content);

			return content;
		}

		public void UpdatePageContent(PageContent content)
		{
			PageContent existingContent = PageContents.FirstOrDefault(x => x.Id == content.Id);

			if (existingContent == null)
			{
				// Do nothing
			}
			else
			{
				existingContent.EditedOn = content.EditedOn;
				existingContent.Text = content.Text;
				existingContent.VersionNumber = content.VersionNumber;
			}
		}

		public User SaveOrUpdateUser(User user)
		{
			User existingUser = Users.FirstOrDefault(x => x.Id == user.Id);

			if (existingUser == null)
			{
				user.Id = Guid.NewGuid();
				Users.Add(user);
			}
			else
			{
				user.ActivationKey = user.ActivationKey;
				user.Email = user.Email;
				user.Firstname = user.Firstname;
				user.IsActivated = user.IsActivated;
				user.IsAdmin = user.IsAdmin;
				user.IsEditor = user.IsEditor;
				user.Lastname = user.Lastname;
				user.Password = user.Password;
				user.PasswordResetKey = user.PasswordResetKey;
				user.Username = user.Username;
				user.Salt = user.Salt;
			}

			return user;
		}

		public void SaveSiteSettings(SiteSettings settings)
		{
			SiteSettings = settings;
		}

		public SiteSettings GetSiteSettings()
		{
			return SiteSettings;
		}

		public void SaveTextPluginSettings(TextPlugin plugin)
		{
			int index = TextPlugins.IndexOf(plugin);

			if (index == -1)
				TextPlugins.Add(plugin);
			else
				TextPlugins[index] = plugin;
		}

		public PluginSettings GetTextPluginSettings(Guid databaseId)
		{
			if (PluginSettings != null)
				return PluginSettings;

			TextPlugin savedPlugin = TextPlugins.FirstOrDefault(x => x.DatabaseId == databaseId);

			if (savedPlugin != null)
				return savedPlugin._settings; // DON'T CALL Settings - you'll get a StackOverflowException
			else
				return null;
		}

		public void Startup(DataStoreType dataStoreType, string connectionString, bool enableCache)
		{
			
		}

		public virtual void Install(DataStoreType dataStoreType, string connectionString, bool enableCache)
		{
			Installed = true;
			InstalledDataStoreType = dataStoreType;
			InstalledConnectionString = connectionString;
			InstalledEnableCache = enableCache;
		}

		public virtual void TestConnection(DataStoreType dataStoreType, string connectionString)
		{
			
		}

		public void Upgrade(ApplicationSettings settings)
		{

		}

        #endregion

        #region IPageRepository Members

        public IEnumerable<Page> AllPages()
        {
            return Pages;
        }

        public IEnumerable<Page> AllNewPages()
        {
            return Pages; //TODO MJO repository mock
        }

        //public IEnumerable<Page> Alerts()
        //{
        //    return Pages; //TODO MJO repository mock
        //}

        public IEnumerable<Page> MyPages(string id)
        {
            return Pages; //TODO MJO repository mock
        }

        public Page GetPageById(int id)
		{
			return Pages.FirstOrDefault(p => p.Id == id);
		}

		public IEnumerable<Page> FindPagesCreatedBy(string username)
		{
			return Pages.Where(p => p.CreatedBy == username);
		}

		public IEnumerable<Page> FindPagesControlledBy(string username)
		{
			return Pages.Where(p => p.ControlledBy == username);
		}

		public IEnumerable<Page> FindPagesContainingTag(string tag)
		{
			return Pages.Where(p => p.Tags.ToLower().Contains(tag.ToLower()));
		}

		public IEnumerable<string> AllTags()
		{
			return Pages.Select(x => x.Tags);
		}

		public Page GetPageByTitle(string title)
		{
			return Pages.FirstOrDefault(p => p.Title == title);
		}

		public PageContent GetLatestPageContent(int pageId)
		{
			return PageContents.Where(p => p.Page.Id == pageId).OrderByDescending(x => x.EditedOn).FirstOrDefault();
		}

		public PageContent GetPageContentById(Guid id)
		{
			return PageContents.FirstOrDefault(p => p.Id == id);
		}

		public PageContent GetPageContentByPageIdAndVersionNumber(int id, int versionNumber)
		{
			return PageContents.FirstOrDefault(p => p.Page.Id == id && p.VersionNumber == versionNumber);
		}

		public PageContent GetPageContentByVersionId(Guid versionId)
		{
			return PageContents.FirstOrDefault(p => p.Id == versionId);
		}

		public IEnumerable<PageContent> FindPageContentsByPageId(int pageId)
		{
			return PageContents.Where(p => p.Page.Id == pageId).ToList();
		}

		public IEnumerable<PageContent> AllPageContents()
		{
			return PageContents;
		}

		#endregion

		#region IUserRepository Members

		public User GetAdminById(Guid id)
		{
			return Users.FirstOrDefault(x => x.Id == id && x.IsAdmin);
		}

		public User GetUserByActivationKey(string key)
		{
			return Users.FirstOrDefault(x => x.ActivationKey == key && x.IsActivated == false);
		}

		public User GetEditorById(Guid id)
		{
			return Users.FirstOrDefault(x => x.Id == id && x.IsEditor);
		}

		public User GetUserByEmail(string email, bool? isActivated = null)
		{
			if (isActivated.HasValue)
				return Users.FirstOrDefault(x => x.Email == email && x.IsActivated == isActivated);
			else
				return Users.FirstOrDefault(x => x.Email == email);
		}

		public User GetUserById(Guid id, bool? isActivated = null)
		{
			if (isActivated.HasValue)
				return Users.FirstOrDefault(x => x.Id == id && x.IsActivated == isActivated);
			else
				return Users.FirstOrDefault(x => x.Id == id);
		}

		public User GetUserByPasswordResetKey(string key)
		{
			return Users.FirstOrDefault(x => x.PasswordResetKey == key);
		}

		public User GetUserByUsername(string username)
		{
			return Users.FirstOrDefault(x => x.Username == username);
		}

		public User GetUserByUsernameOrEmail(string username, string email)
		{
			return Users.FirstOrDefault(x => x.Username == username || x.Email == email);
		}

		public IEnumerable<User> FindAllEditors()
		{
			return Users.Where(x => x.IsEditor).ToList();
		}

		public IEnumerable<User> FindAllAdmins()
		{
			return Users.Where(x => x.IsAdmin).ToList();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			
		}

        public void AddPageRating(int pageId, int rating)
        {
            Page page = Pages.Find(p => p.Id == pageId);
            page.TotalRating += rating;
            page.NbRating++;
        }

        public void RemovePageRating(int pageId, int rating)
        {
            Page page = Pages.Find(p => p.Id == pageId);
            page.TotalRating -= rating;
            page.NbRating--;
        }

        public void DeletePage(int pageId)
        {
            Page page = Pages.Find(p => p.Id == pageId);
            Pages.Remove(page);
        }

        public void SetDraft(int pageId)
        {
            Page page = Pages.Find(p => p.Id == pageId);
            page.IsControlled = false;
            page.IsSubmitted = false;
            page.IsRejected = false;
        }

        public void SubmitPage(int pageId)
        {
            Page page = Pages.Find(p => p.Id == pageId);
            page.IsSubmitted = false;
        }

        public void RejectPage(int pageId)
        {
            Page page = Pages.Find(p => p.Id == pageId);
            page.IsControlled = false;
            page.IsRejected = false;
        }

        public IEnumerable<Page> FindMostRecentPages(int number)
        {
            return new List<Page>();
        }

        public IEnumerable<Page> FindPagesMostViewed(int number)
        {
            return new List<Page>();
        }

        public IEnumerable<Page> FindPagesBestRated(int number)
        {
            return new List<Page>();
        }

        public IEnumerable<Page> FindPagesWithAlerts()
        {
            return new List<Page>();
        }

        public void IncrementNbView(int pageId)
        {
            Page page = Pages.Find(p => p.Id == pageId);
            page.NbView++;
        }

        public void SetNbView(int pageId, int nbView)
        {
            Page page = Pages.Find(p => p.Id == pageId);
            page.NbView = nbView;
        }

        public void SetRating(int pageId, int nbRating, int totalRating)
        {
            Page page = Pages.Find(p => p.Id == pageId);
            page.TotalRating = totalRating;
            page.NbRating = nbRating;
        }

        public User GetControllerById(Guid id)
        {
            User user = Users.FirstOrDefault(x => x.Id == id && x.IsController);
            return user;
        }

        public void DeleteComment(Guid commentId)
        {
            Comment comment = Comments.Find(c => c.Id == commentId);
            Comments.Remove(comment);
        }

        public IEnumerable<Comment> FindCommentsByPage(int pageId)
        {
            IEnumerable<Comment> comments = Comments.Where(c => c.PageId == pageId);
            return(comments);
        }

        public void AddComment(Comment comment)
        {
            Comments.Add(comment);
        }

        public void UpdateCommentRating(Guid commentId, int rating)
        {
            Comment comment = Comments.Find(c => c.Id == commentId);
            comment.Rating = rating;
        }

        public Comment FindCommentByPageAndUser(int pageId, string username)
        {
            Comment comment = Comments.Find(c => c.PageId == pageId && c.CreatedBy == username);
            return (comment);
        }

        public void DeleteAlert(Guid alertId)
        {
            Alert alert = Alerts.Find(a => a.Id == alertId);
            Alerts.Remove(alert);
        }

        public void DeletePageAlerts(int pageId)
        {
            Alert alert = Alerts.Find(a => a.PageId == pageId);
            Alerts.Remove(alert);
        }

        public void DeleteCommentAlerts(Guid commentId)
        {
            Alert alert = Alerts.Find(a => a.CommentId == commentId);
            Alerts.Remove(alert);
        }

        public IEnumerable<Alert> FindAlertsByPage(int pageId)
        {
            IEnumerable<Alert> alerts = Alerts.Where(a => a.PageId == pageId);
            return alerts;
        }

        public IEnumerable<Alert> FindAlertsByComment(Guid commentGuid)
        {
            IEnumerable<Alert> alerts = Alerts.Where(a => a.CommentId == commentGuid);
            return alerts;
        }

        public void AddAlert(Alert alert)
        {
            Alerts.Add( alert);
        }

        public PageContent AddNewPageContentVersion(Page page, string text, DateTime editedOn, int version)
        {
            // TODO find the page
            return new PageContent()
            {
                Id = Guid.NewGuid(),
                VersionNumber = version,
                Text = text,
                EditedOn = editedOn,
            };
        }

        public IEnumerable<Page> FindPagesByCompetitionId(int competitionId)
        {
            IEnumerable<Page> pages = Pages.Where(p => p.CompetitionId == competitionId);
            return pages;
        }

        public void CleanPagesForCompetitionId(int competitionId)
        {
            IEnumerable<Page> pages = Pages.Where(p => p.CompetitionId == competitionId && !p.IsControlled);
            foreach (var page in pages)
            {
                page.CompetitionId = -1;
            }
        }

        public void SetCompetitionId(int pageId, int competitionId)
        {
            Page page = Pages.Where(p => p.Id == pageId).SingleOrDefault();
            page.CompetitionId = competitionId;
        }

        public void ValidateComment(Guid commentId)
        {
            Comment comment = Comments.Find(c => c.Id == commentId);
            comment.IsControlled = true;
            comment.IsRejected = false;

        }

        public void RejectComment(Guid commentId)
        {
            Comment comment = Comments.Find(c => c.Id == commentId);
            comment.IsControlled = true;
            comment.IsRejected = true;
        }

        public void UpdateComment(Guid commentId, string text)
        {
            Comment comment = Comments.Find(c => c.Id == commentId);
            comment.Text = text;
            comment.IsControlled = false;
            comment.IsRejected = false;
        }

        public void DeleteComments(int pageId)
        {
            Comments.RemoveAll(c => c.PageId == pageId);
        }

        public IEnumerable<Comment> FindCommentsToControl()
        {
            IEnumerable<Comment> comments = Comments.Where(c => c.IsControlled == false);
            return comments;
        }

        public Alert FindAlertByPageAndUser(int pageId, string username)
        {
            Alert alert = Alerts.SingleOrDefault(a => a.PageId == pageId && a.CreatedBy == username);
            return alert;

        }

        public void DeletPageAlertsByUser(int pageId, string username)
        {
            Alerts.RemoveAll(a => a.PageId == pageId && a.CreatedBy == username);
            
        }

        public IEnumerable<Alert> GetAlerts()
        {
            return Alerts;
        }

        public void AddCompetition(Competition competition)
        {
            Competitions.Add( competition);
        }

        public void UpdateCompetition(Competition competition_)
        {
            Competition competition = Competitions.Find(c => c.Id == competition_.Id);
            competition.Status = competition_.Status;
            competition.RatingStart = competition_.RatingStart;
            competition.RatingStop = competition_.RatingStop;
            competition.PublicationStart = competition_.PublicationStart;
            competition.PublicationStop = competition_.PublicationStop;
            competition.PageId = competition_.PageId;
            competition.PageTag = competition_.PageTag;
        }

        public void UpdateCompetitionPageId(int competitionId, int pageId)
        {
            CompetitionPage cp_ = CompetitionPages.SingleOrDefault(cp => cp.CompetitionId == competitionId);
            if (cp_ != null)
            {
                cp_.PageId = pageId;
            }

        }

        public IEnumerable<Competition> GetCompetitions(bool forAdmin = false)
        {
            return new List<Competition>()
            {
                new Competition()
                {
                    Id = 1,
                    ObjectId = Guid.NewGuid(),
                    PageId = 1,
                    PageTag = "___competitionTag",
                    PublicationStart = DateTime.Now,
                    PublicationStop = DateTime.Now,
                    RatingStart = DateTime.Now,
                    RatingStop = DateTime.Now,
                    Status = 1,
                }
            };
        }
    

        public Competition GetCompetitionById(int id)
        {
            return new Competition()
            {
                Id = id,
                ObjectId = Guid.NewGuid(),
                PageId = 1,
                PageTag = "___competitionTag",
                PublicationStart = DateTime.Now,
                PublicationStop = DateTime.Now,
                RatingStart = DateTime.Now,
                RatingStop = DateTime.Now,
                Status = 1,
            };
        }

        public Competition GetCompetitionByStatus(int status)
        {
            return new Competition()
            {
                Id = 1,
                ObjectId = Guid.NewGuid(),
                PageId = 1,
                PageTag = "___competitionTag",
                PublicationStart = DateTime.Now,
                PublicationStop = DateTime.Now,
                RatingStart = DateTime.Now,
                RatingStop = DateTime.Now,
                Status = 1,
            };
        }

        public Competition GetCompetitionByPageTag(string tag)
        {
            return new Competition()
            {
                Id = 2,
                ObjectId = Guid.NewGuid(),
                PageId = 1,
                PageTag = tag,
                PublicationStart = DateTime.Now,
                PublicationStop = DateTime.Now,
                RatingStart = DateTime.Now,
                RatingStop = DateTime.Now,
                Status = 1,
            };
        }
        public IEnumerable<CompetitionPage> GetCompetitionPages(int competitionId)
        {
            return CompetitionPages.Where(cp => cp.CompetitionId == competitionId);

        }

        public void DeleteCompetition(int id)
        {
            Competitions.RemoveAll(c => c.Id == id);
        }

        public void ArchiveCompetitionPage(int competitionId, int pageId)
        {
            CompetitionPages.Add( new CompetitionPage()
            {
                CompetitionId = competitionId,
                PageId = pageId,
            });
        }

 
        public void UpdateCompetitionPageRanking(int competitionId, int pageId, int ranking)
        {
            CompetitionPage cp = CompetitionPages.SingleOrDefault(c => c.CompetitionId == competitionId);
            cp.Ranking = ranking;
        }

        public int GetPageRanking(int pageId)
        {
            int ranking = 0;
            return 0;
        }


        public int GetRatingByPageAndUser(int pageId, string username)
        {
            throw new NotImplementedException();
        }

        public void DeletCompetitionPages(int competitionId)
        {
            throw new NotImplementedException();
        }

        public void DeletCompetitionPage(int pageId)
        {
            throw new NotImplementedException();
        }

        public void ArchiveCompetitionPage(int competitionId, Page page)
        {
            throw new NotImplementedException();
        }

        public int[] GetUserHits(string username)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

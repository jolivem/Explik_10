using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	/// <summary>
	/// Maps Lightspeed entity classes to the Roadkill domain objects.
	/// </summary>
	/// <remarks>(AutoMapper was tried for this, but had problems with the Mindscape.LightSpeed.Entity class)</remarks>
	public class FromEntity
	{
		public static Page ToPage(PageEntity entity)
		{
			if (entity == null)
				return null;

			Page page = new Page();
			page.Id = entity.Id;
			page.CreatedBy = entity.CreatedBy;
			page.CreatedOn = entity.CreatedOn;
            page.IsLocked = entity.IsLocked;
            page.IsSubmitted = entity.IsSubmitted;
            page.IsControlled = entity.IsControlled;
            page.IsRejected = entity.IsRejected;
            page.IsCopied = entity.IsCopied;
            page.IsVideo = entity.IsVideo;
            page.ControlledBy = entity.ControlledBy;
			page.PublishedOn = entity.PublishedOn;
			page.Tags = entity.Tags;
            page.Title = entity.Title;
            page.Summary = entity.Summary;
            page.NbRating = entity.NbRating;
            page.NbView = entity.NbView;
            page.TotalRating = entity.TotalRating;
            page.FilePath = entity.FilePath;
            page.VideoUrl = entity.VideoUrl;
            page.Pseudonym = entity.Pseudonym;
            page.ControllerRating = entity.ControllerRating;
            page.CompetitionId = entity.CompetitionId;

            return page;
		}

		public static PageContent ToPageContent(PageContentEntity entity)
		{
			if (entity == null)
				return null;

			PageContent pageContent = new PageContent();
			pageContent.Id = entity.Id;
			pageContent.EditedOn = entity.EditedOn;
			pageContent.ControlledBy = entity.ControlledBy;
			pageContent.Text = entity.Text;
			pageContent.VersionNumber = entity.VersionNumber;
			pageContent.Page = ToPage(entity.Page);

			return pageContent;
		}

		/// <summary>
		/// Intentionally doesn't populate the User.Password property (as this is only ever stored).
		/// </summary>
		public static User ToUser(UserEntity entity)
		{
			if (entity == null)
				return null;

			User user = new User();
			user.Id = entity.Id;
			user.ActivationKey = entity.ActivationKey;
			user.Email = entity.Email;
			user.Firstname = entity.Firstname;
			user.IsActivated = entity.IsActivated;
            user.Contribution = entity.Contribution;
            user.DisplayFlags = entity.DisplayFlags;
			user.IsAdmin = entity.IsAdmin;
            user.IsEditor = entity.IsEditor;
            user.IsController = entity.IsController;
		    user.AttachmentsPath = entity.AttachmentsPath;
            user.Lastname = entity.Lastname;
			user.Password = entity.Password;
			user.PasswordResetKey = entity.PasswordResetKey;
			user.Username = entity.Username;
			user.Salt = entity.Salt;

			return user;
		}

        /// <summary>
        /// Intentionally doesn't populate the User.Password property (as this is only ever stored).
        /// </summary>
        public static Comment ToComment(CommentEntity entity)
        {
            if (entity == null)
                return null;

            Comment comment = new Comment();
            comment.Id = entity.Id;
            comment.CreatedBy = entity.CreatedBy;
            comment.CreatedOn = entity.CreatedOn;
            comment.PageId = entity.PageId;
            comment.Rating = entity.Rating;
            comment.ControlledBy = entity.ControlledBy;
            comment.IsControlled = entity.IsControlled;
            comment.IsRejected = entity.IsRejected;
            comment.Text = entity.Text;

            return comment;
        }

        /// <summary>
        /// Intentionally doesn't populate the User.Password property (as this is only ever stored).
        /// </summary>
        public static Alert ToAlert(AlertEntity entity)
        {
            if (entity == null)
                return null;

            Alert alert = new Alert();
            alert.Id = entity.Id;
            alert.PageId = entity.PageId;
            alert.CommentId = entity.CommentId;
            alert.CreatedBy = entity.CreatedBy;
            alert.CreatedOn = entity.CreatedOn;
            alert.Ilk = entity.Ilk;

            return alert;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Competition ToCompetition(CompetitionEntity entity)
        {
            if (entity == null)
                return null;

            Competition competition = new Competition();
            competition.Id = entity.Id;
            competition.PageTag = entity.PageTag;
            competition.PageId = entity.PageId;
            competition.PublicationStart = entity.PublicationStart;
            competition.PublicationStop = entity.PublicationStop;
            competition.RatingStart = entity.RatingStart;
            competition.RatingStop = entity.RatingStop;
            competition.Status = entity.Status;

            return competition;
        }

        public static CompetitionPage ToCompetitionPage(CompetitionPageEntity entity)
        {
            if (entity == null)
                return null;

            CompetitionPage competitionPage = new CompetitionPage();
            competitionPage.Id = entity.Id;
            competitionPage.PageId = entity.PageId;
            competitionPage.NbRating = entity.NbRating;
            competitionPage.TotalRating = entity.TotalRating;
            competitionPage.UserName = entity.UserName;
            competitionPage.Ranking = entity.Ranking;

            return competitionPage;
        }

        public static IEnumerable<PageContent> ToPageContentList(IEnumerable<PageContentEntity> entities)
		{
			List<PageContent> list = new List<PageContent>();
			foreach (PageContentEntity entity in entities)
			{
				PageContent pageContent = ToPageContent(entity);
				list.Add(pageContent);
			}

			return list;
		}

		public static IEnumerable<Page> ToPageList(IEnumerable<PageEntity> entities)
		{
			List<Page> list = new List<Page>();
			foreach (PageEntity entity in entities)
			{
				Page page = ToPage(entity);
				list.Add(page);
			}

			return list;
		}

		public static IEnumerable<User> ToUserList(List<UserEntity> entities)
		{
			List<User> list = new List<User>();
			foreach (UserEntity entity in entities)
			{
				User user = ToUser(entity);
				list.Add(user);
			}

			return list;
		}

        public static IEnumerable<Comment> ToCommentList(List<CommentEntity> entities)
        {
            List<Comment> list = new List<Comment>();
            foreach (CommentEntity entity in entities)
            {
                Comment comment = ToComment(entity);
                list.Add(comment);
            }

            return list;
        }

        public static IEnumerable<Alert> ToAlertList(List<AlertEntity> entities)
        {
            List<Alert> list = new List<Alert>();
            foreach (AlertEntity entity in entities)
            {
                Alert alert = ToAlert(entity);
                list.Add(alert);
            }

            return list;
        }

        public static IEnumerable<Competition> ToCompetitionList(List<CompetitionEntity> entities)
        {
            List<Competition> list = new List<Competition>();
            foreach (CompetitionEntity entity in entities)
            {
                Competition competition = ToCompetition(entity);
                list.Add(competition);
            }

            return list;
        }

        public static IEnumerable<CompetitionPage> ToCompetitionPageList(List<CompetitionPageEntity> entities)
        {
            List<CompetitionPage> list = new List<CompetitionPage>();
            foreach (CompetitionPageEntity entity in entities)
            {
                CompetitionPage competition = ToCompetitionPage(entity);
                list.Add(competition);
            }

            return list;
        }

    }
}

using Roadkill.Core.Database.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Roadkill.Core.Database
{
	/// <summary>
	/// Maps Lightspeed entity classes to the Roadkill domain objects.
	/// </summary>
	/// <remarks>(AutoMapper was tried for this, but had problems with the Mindscape.LightSpeed.Entity class)</remarks>
	public class FromEntity
	{
		public static Page ToPage(explik_pages entity)
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
			page.PublishedOn = (DateTime)entity.PublishedOn;
			page.Tags = entity.Tags;
            page.Title = entity.Title;
            page.Summary = entity.Summary;
            page.NbRating = (int)entity.NbRating;
            page.NbView = (int)entity.NbView;
            page.TotalRating = (int)entity.TotalRating;
            page.FilePath = entity.FilePath;
            page.VideoUrl = entity.VideoUrl;
            page.Pseudonym = entity.Pseudonym;
            page.ControllerRating = (int)entity.ControllerRating;
            page.CompetitionId = (int)entity.CompetitionId;

            return page;
		}

		public static PageContent ToPageContent(explik_pagecontent entity, explik_pages pagEntity)
		{
			if (entity == null)
				return null;

			PageContent pageContent = new PageContent();
			pageContent.Id = new Guid(entity.Id);
			pageContent.EditedOn = entity.EditedOn;
			pageContent.ControlledBy = entity.ControlledBy;
			pageContent.Text = entity.Text;
			pageContent.VersionNumber = entity.VersionNumber;
			pageContent.Page = ToPage(pagEntity);

			return pageContent;
		}

		/// <summary>
		/// Intentionally doesn't populate the User.Password property (as this is only ever stored).
		/// </summary>
		public static User ToUser(explik_users entity)
		{
			if (entity == null)
				return null;

			User user = new User();
			user.Id = new Guid(entity.Id);
			user.ActivationKey = entity.ActivationKey;
			user.Email = entity.Email;
			user.Firstname = entity.Firstname;
			user.IsActivated = entity.IsActivated;
            user.Contribution = (long)entity.Contribution;
            user.DisplayFlags = (long)entity.DisplayFlags;
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
        public static Comment ToComment(explik_comments entity)
        {
            if (entity == null)
                return null;

            Comment comment = new Comment();
            comment.Id = entity.Id;
            comment.CreatedBy = entity.CreatedBy;
            comment.CreatedOn = entity.CreatedOn;
            comment.PageId = (int)entity.PageId;
            comment.Rating = (int)entity.Rating;
            comment.ControlledBy = entity.ControlledBy;
            comment.IsControlled = entity.IsControlled;
            comment.IsRejected = entity.IsRejected;
            comment.Text = entity.Text;

            return comment;
        }

        /// <summary>
        /// Intentionally doesn't populate the User.Password property (as this is only ever stored).
        /// </summary>
        public static Alert ToAlert(explik_alerts entity)
        {
            if (entity == null)
                return null;

            Alert alert = new Alert();
            alert.Id = entity.Id;
            alert.PageId = (int)entity.PageId;
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
        public static Competition ToCompetition(explik_competition entity)
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

        public static CompetitionPage ToCompetitionPage(explik_competitionpage entity)
        {
            if (entity == null)
                return null;

            CompetitionPage competitionPage = new CompetitionPage();
            competitionPage.Id = entity.Id;
            competitionPage.PageId = entity.PageId;
            competitionPage.NbRating = (int)entity.NbRating;
            competitionPage.TotalRating = (int)entity.TotalRating;
            competitionPage.UserName = entity.UserName;
            competitionPage.Ranking = (int)entity.Ranking;

            return competitionPage;
        }

        public static IEnumerable<PageContent> ToPageContentList(IEnumerable<explik_pagecontent> entities, explik_pages pageEntity)
		{
			List<PageContent> list = new List<PageContent>();
			foreach (explik_pagecontent entity in entities)
			{
				PageContent pageContent = ToPageContent(entity, pageEntity);
				list.Add(pageContent);
			}

			return list;
		}

		public static IEnumerable<Page> ToPageList(IEnumerable<explik_pages> entities)
		{
			List<Page> list = new List<Page>();
			foreach (explik_pages entity in entities)
			{
				Page page = ToPage(entity);
				list.Add(page);
			}

			return list;
		}

		public static IEnumerable<User> ToUserList(List<explik_users> entities)
		{
			List<User> list = new List<User>();
			foreach (explik_users entity in entities)
			{
				User user = ToUser(entity);
				list.Add(user);
			}

			return list;
		}

        public static IEnumerable<Comment> ToCommentList(List<explik_comments> entities)
        {
            List<Comment> list = new List<Comment>();
            foreach (explik_comments entity in entities)
            {
                Comment comment = ToComment(entity);
                list.Add(comment);
            }

            return list;
        }

        public static IEnumerable<Alert> ToAlertList(List<explik_alerts> entities)
        {
            List<Alert> list = new List<Alert>();
            foreach (explik_alerts entity in entities)
            {
                Alert alert = ToAlert(entity);
                list.Add(alert);
            }

            return list;
        }

        public static IEnumerable<Competition> ToCompetitionList(List<explik_competition> entities)
        {
            List<Competition> list = new List<Competition>();
            foreach (explik_competition entity in entities)
            {
                Competition competition = ToCompetition(entity);
                list.Add(competition);
            }

            return list;
        }

        public static IEnumerable<CompetitionPage> ToCompetitionPageList(List<explik_competitionpage> entities)
        {
            List<CompetitionPage> list = new List<CompetitionPage>();
            foreach (explik_competitionpage entity in entities)
            {
                CompetitionPage competition = ToCompetitionPage(entity);
                list.Add(competition);
            }

            return list;
        }

        public static Course ToCourse(explik_course entity)
        {
            if (entity == null)
                return null;

            Course course = new Course();
            course.Id = entity.Id;
            course.Title = entity.Title;
            course.CreatedBy = entity.CreatedBy;

            return course;
        }

        public static CoursePage ToCoursePage(explik_coursepage entity)
        {
            if (entity == null)
                return null;

            CoursePage coursePage = new CoursePage();
            coursePage.Id = entity.Id;
            coursePage.PageId = entity.PageId;
            coursePage.CourseId = entity.CourseId;
            coursePage.Order = (int)entity.Order;

            return coursePage;
        }

        public static IEnumerable<CoursePage> ToCoursePageList(List<explik_coursepage> entities)
        {
            List<CoursePage> list = new List<CoursePage>();
            foreach (explik_coursepage entity in entities)
            {
                CoursePage course = ToCoursePage(entity);
                list.Add(course);
            }

            return list;
        }

        public static IEnumerable<Course> ToCourseList(List<explik_course> entities)
        {
            List<Course> list = new List<Course>();
            foreach (explik_course entity in entities)
            {
                Course course = ToCourse(entity);
                list.Add(course);
            }

            return list;
        }
    }
}

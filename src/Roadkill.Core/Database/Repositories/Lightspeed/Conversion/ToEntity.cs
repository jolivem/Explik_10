using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Roadkill.Core.Database;
using Roadkill.Core.Database.Repositories.Entities;

namespace Roadkill.Core.Database
{
	/// <summary>
	/// Maps a Roadkill domain objects (or list of) to a Lightspeed entity classes to the.
	/// These methods deliberatly don't return a new <see cref="Entity"/> as that causes its 
	/// state to be marked as New, which can have side effects.
	/// </summary>
	public class ToEntity
	{
		public static void FromUser(User user, explik_users entity)
		{
			entity.ActivationKey = user.ActivationKey;
			entity.Email = user.Email;
			entity.Firstname = user.Firstname;
			entity.IsActivated = user.IsActivated;
            entity.Contribution = (int)user.Contribution;
            entity.DisplayFlags = (int)user.DisplayFlags;
			entity.IsAdmin = user.IsAdmin;
            entity.IsEditor = user.IsEditor;
            entity.IsController = user.IsController;
		    entity.AttachmentsPath = user.AttachmentsPath;
            entity.Lastname = user.Lastname;
			entity.Password = user.Password;
			entity.PasswordResetKey = user.PasswordResetKey;
			entity.Salt = user.Salt;
			entity.Username = user.Username;
		}

        public static void FromComment(Comment comment, explik_comments entity)
        {
            entity.CreatedBy = comment.CreatedBy;
            entity.CreatedOn = comment.CreatedOn;
            entity.Id = comment.Id.ToString();
            entity.PageId = comment.PageId;
            entity.Rating = comment.Rating;
            entity.ControlledBy = comment.ControlledBy;
            entity.IsControlled = comment.IsControlled;
            entity.IsRejected = comment.IsRejected;
            entity.Text = comment.Text;
        }

        public static void FromAlert(Alert alert, explik_alerts entity)
        {
            entity.Id = alert.Id.ToString();
            entity.PageId = alert.PageId;
            entity.CommentId = alert.CommentId.ToString();
            entity.CreatedBy = alert.CreatedBy;
            entity.CreatedOn = alert.CreatedOn;
            entity.Ilk = alert.Ilk;
        }

        public static void FromCompetition(Competition competition, explik_competition entity)
        {
            //entity.Id = competition.Id;
            entity.PublicationStart = competition.PublicationStart;
            entity.PublicationStop = competition.PublicationStop;
            entity.RatingStart = competition.RatingStart;
            entity.RatingStop = competition.RatingStop;
            entity.Status = competition.Status;
            entity.PageTag = competition.PageTag;
            entity.PageId = competition.PageId;
        }

        public static void FromCompetitionPage(CompetitionPage competitionpage, explik_competitionpage entity)
        {
            entity.Id = competitionpage.Id;
            entity.CompetitionId = competitionpage.CompetitionId;
            entity.PageId = competitionpage.PageId;
            entity.NbRating = (int)competitionpage.NbRating;
            entity.TotalRating = (int)competitionpage.TotalRating;
            entity.UserName = competitionpage.UserName;
            entity.Ranking = competitionpage.Ranking;
        }

         public static void FromPage(Page page, explik_pages entity)
		{
            entity.NbRating = (int)page.NbRating;
            entity.NbView = (int)page.NbView;
            entity.TotalRating = (int)page.TotalRating;
            entity.CreatedBy = page.CreatedBy;
            entity.CreatedOn = page.CreatedOn;
            entity.IsLocked = page.IsLocked;
            entity.IsVideo = page.IsVideo;
            entity.IsSubmitted = page.IsSubmitted;
            entity.IsControlled = page.IsControlled;
            entity.IsRejected = page.IsRejected;
            entity.IsCopied = page.IsCopied;
            entity.PublishedOn = page.PublishedOn;
			entity.ControlledBy = page.ControlledBy;
			entity.Tags = page.Tags;
            entity.Summary = page.Summary;
            entity.Title = page.Title;
            entity.FilePath = page.FilePath;
            entity.VideoUrl = page.VideoUrl;
            entity.Pseudonym = page.Pseudonym;
            entity.ControllerRating = (int)page.ControllerRating;
            entity.CompetitionId = page.CompetitionId;
        }

        public static void FromPageContent(PageContent pageContent, explik_pagecontent entity)
		{
			entity.EditedOn = pageContent.EditedOn;
			entity.ControlledBy = pageContent.ControlledBy;
			entity.Text = pageContent.Text;
			entity.VersionNumber = pageContent.VersionNumber;
		}

        public static void FromCourse(Course course, explik_course entity)
        {
            //entity.Id = competition.Id;
            entity.Id = course.Id;
            entity.Title = course.Title;
            entity.CreatedBy = course.CreatedBy;
        }

        public static void FromCoursePage(CoursePage coursepage, explik_coursepage entity)
        {
            entity.CourseId = coursepage.Id;
            entity.PageId = coursepage.PageId;
            entity.Order = coursepage.Order;
        }

    }
}

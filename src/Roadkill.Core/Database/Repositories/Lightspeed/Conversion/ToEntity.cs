using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	/// <summary>
	/// Maps a Roadkill domain objects (or list of) to a Lightspeed entity classes to the.
	/// These methods deliberatly don't return a new <see cref="Entity"/> as that causes its 
	/// state to be marked as New, which can have side effects.
	/// </summary>
	public class ToEntity
	{
		public static void FromUser(User user, UserEntity entity)
		{
			entity.ActivationKey = user.ActivationKey;
			entity.Email = user.Email;
			entity.Firstname = user.Firstname;
			entity.IsActivated = user.IsActivated;
            entity.Contribution = user.Contribution;
            entity.DisplayFlags = user.DisplayFlags;
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

        public static void FromComment(Comment comment, CommentEntity entity)
        {
            entity.CreatedBy = comment.CreatedBy;
            entity.CreatedOn = comment.CreatedOn;
            entity.Id = comment.Id;
            entity.PageId = comment.PageId;
            entity.Rating = comment.Rating;
            entity.ControlledBy = comment.ControlledBy;
            entity.IsControlled = comment.IsControlled;
            entity.IsRejected = comment.IsRejected;
            entity.Text = comment.Text;
        }

        public static void FromAlert(Alert alert, AlertEntity entity)
        {
            entity.Id = alert.Id;
            entity.PageId = alert.PageId;
            entity.CommentId = alert.CommentId;
            entity.CreatedBy = alert.CreatedBy;
            entity.CreatedOn = alert.CreatedOn;
            entity.Ilk = alert.Ilk;
        }

        public static void FromCompetition(Competition competition, CompetitionEntity entity)
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

        public static void FromCompetitionPage(CompetitionPage competitionpage, CompetitionPageEntity entity)
        {
            entity.Id = competitionpage.Id;
            entity.CompetitionId = competitionpage.CompetitionId;
            entity.PageId = competitionpage.PageId;
            entity.NbRating = competitionpage.NbRating;
            entity.TotalRating = competitionpage.TotalRating;
            entity.UserName = competitionpage.UserName;
            entity.Ranking = competitionpage.Ranking;
        }

         public static void FromPage(Page page, PageEntity entity)
		{
            entity.NbRating = page.NbRating;
            entity.NbView = page.NbView;
            entity.TotalRating = page.TotalRating;
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
            entity.ControllerRating = page.ControllerRating;
            entity.CompetitionId = page.CompetitionId;
        }

        public static void FromPageContent(PageContent pageContent, PageContentEntity entity)
		{
			entity.EditedOn = pageContent.EditedOn;
			entity.ControlledBy = pageContent.ControlledBy;
			entity.Text = pageContent.Text;
			entity.VersionNumber = pageContent.VersionNumber;
		}

        public static void FromCourse(Course course, CourseEntity entity)
        {
            //entity.Id = competition.Id;
            entity.Id = course.Id;
            entity.Title = course.Title;
            entity.CreatedBy = course.CreatedBy;
        }

        public static void FromCoursePage(CoursePage coursepage, CoursePageEntity entity)
        {
            entity.CourseId = coursepage.Id;
            entity.PageId = coursepage.PageId;
            entity.Order = coursepage.Order;
        }

    }
}

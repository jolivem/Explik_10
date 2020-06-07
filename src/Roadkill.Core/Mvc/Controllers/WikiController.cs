using System;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Roadkill.Core.Configuration;
using Roadkill.Core.Services;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Security;
using Roadkill.Core.Localization;

namespace Roadkill.Core.Mvc.Controllers
{
	/// <summary>
	/// Provides functionality for the /wiki/{id}/{title} route, which all pages are displayed via.
	/// </summary>
	[OptionalAuthorization]
	public class WikiController : ControllerBase
	{
        public PageService _pageService { get; private set; }
        public CompetitionService _competitionService { get; private set; }

        public WikiController(ApplicationSettings settings, UserServiceBase userManager, PageService pageService,
            CompetitionService competitionService, IUserContext context, SettingsService settingsService)
			: base(settings, userManager, context, settingsService) 
		{
            _pageService = pageService;
            _competitionService = competitionService;
        }

		/// <summary>
		/// Displays the wiki page using the provided id.
		/// </summary>
		/// <param name="id">The page id</param>
		/// <param name="title">This parameter is passed in, but never used in queries.</param>
		/// <returns>A <see cref="PageViewModel"/> to the Index view.</returns>
		/// <remarks>This action adds a "Last-Modified" header using the page's last modified date, if no user is currently logged in.</remarks>
		/// <exception cref="HttpNotFoundResult">Thrown if the page with the id cannot be found.</exception>
		[BrowserCache]
		public ActionResult Index(int? id, string title)
		{
			if (id == null || id < 1)
				return RedirectToAction("Index", "Home");

			PageViewModel model = _pageService.GetById(id.Value, true);

            // If it is my page, don't display all the stuff !!!
            if (model.CreatedBy == Context.CurrentUsername)
		    {
                model.CompetitionInfo = "";
                model.ModificationsEnable = true;

                if (model.CompetitionId != -1)
                {
                    CompetitionViewModel competition = _competitionService.GetById(model.CompetitionId);
                    if (competition.Status == CompetitionViewModel.Statuses.PublicationOngoing)
                    {
                        model.ModificationsEnable = true;
                        model.CompetitionInfo = SiteStrings.MyPage_IsInCompetitionPublicationOngoing;
                    }
                    if (model.IsPublished &&
                        competition.Status == CompetitionViewModel.Statuses.PauseBeforeRating ||
                        competition.Status == CompetitionViewModel.Statuses.RatingOngoing)
                    {
                        model.ModificationsEnable = false;
                        model.CompetitionInfo = SiteStrings.MyPage_IsInCompetitionRatingOngoing;
                    }
                    if (model.IsPublished &&
                        competition.Status == CompetitionViewModel.Statuses.Achieved ||
                        competition.Status == CompetitionViewModel.Statuses.PauseBeforeAchieved)
                    {
                        model.ModificationsEnable = false;
                        model.CompetitionInfo = SiteStrings.MyPage_IsInPastCompetition;
                    }
                }

                return View("MyPage", model);
		    }

            // TODO one request for optimization
            ViewBag.userrating = _pageService.GetPageRatingFromUser(model.Id, Context.CurrentUsername);
            ViewBag.usercomment = _pageService.GetPageCommentFromUser(model.Id, Context.CurrentUsername);
            ViewBag.currentuser = Context.CurrentUsername;
            string ip = _pageService.GetUserIp();
            ViewBag.useralert = _pageService.GetPageAlertFromUser(model.Id, ip);

            ViewBag.CourseId = _pageService.FindCourseByPage(model.Id);

            // handle courses
            CourseViewModel course = _pageService.FindCourseByPage(model.Id);
            if (course != null)
            {
                ViewBag.CourseTitle = course.EncodedTitle;
                ViewBag.CourseId = course.CourseId;
            }
            else
            {
                ViewBag.CourseId = -1;
            }

            _pageService.IncrementNbView(model.Id);

			if (model == null)
				throw new HttpException(404, string.Format("The page with id '{0}' could not be found", id));

            // when a user alerts, it i taken into account by Pages/PageAlert

            return View(model);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
  //      public ActionResult PageToolbar(int? id) //MJO is it used ?
		//{
		//	if (id == null || id < 1)
		//		return Content("");

		//	PageViewModel model = _pageService.GetById(id.Value);

		//	if (model == null)
		//		return Content(string.Format("The page with id '{0}' could not be found", id));

		//	return PartialView(model);
		//}


        /// <summary>
        /// 404 not found page - configured in the web.config
        /// </summary>
        public ActionResult NotFound()
		{
			return View("404");
		}

		/// <summary>
		/// 500 internal error - configured in the web.config
		/// </summary>
		public ActionResult ServerError()
		{
			return View("500");
		}
	}
}

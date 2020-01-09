using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Roadkill.Core.Configuration;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Database;

using Roadkill.Core.Attachments;
using Roadkill.Core.Localization;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Mvc.Controllers
{
    /// <summary>
    /// Provides all page related functionality, including editing and viewing pages.
    /// </summary>
    [HandleError]
    [OptionalAuthorization]
    public class CompetitionsController : ControllerBase
    {

        private IRepository _repository;
        private ICompetitionService _competitionService;
        private PageService _pageService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="userManager"></param>
        /// <param name="settingsService"></param>
        /// <param name="context"></param>
        /// <param name="competitionService"></param>
        /// <param name="repository"></param>
        public CompetitionsController(ApplicationSettings settings, UserServiceBase userManager, SettingsService settingsService, IUserContext context, 
            ICompetitionService competitionService, PageService pageService, IRepository repository)
            : base(settings, userManager, context, settingsService)
        {
            _repository = repository;
            _competitionService = competitionService;
            _pageService = pageService;
        }

        /// <summary>
        /// Edit a competition for modifications
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminRequired]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                CompetitionViewModel model = _competitionService.GetById((int)id);
                if (model != null)
                {
                    // already done model.StatusString = CompetitionViewModel.StatusToString(model.Status);
                    return View("Edit", model);
                }
            }

            return RedirectToAction("New");
        }

        /// <summary>
        /// Edit a new Competition
        /// </summary>
        /// <returns></returns>
        [AdminRequired]
        public ActionResult New()
        {
            CompetitionViewModel model = new CompetitionViewModel();

            return View("Edit", model);
        }

        /// <summary>
        /// List all competitions
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            if (Context.IsAdmin)
            {
                return RedirectToAction("ListForAdmin");
            }
            else
            {
                // only achieved competitions and possibility to see results
                List<CompetitionViewModel> models = _competitionService.GetCompetitions(false);
                return View(models);
            }
        }

        /// <summary>
        /// List and odify competitions (admin only)
        /// </summary>
        /// <returns></returns>
        [AdminRequired]
        public ActionResult ListForAdmin()
        {
            // all competitions and possibility to edit
            List<CompetitionViewModel> models = _competitionService.GetCompetitions(true);
            return View("ListForAdmin", models);
        }

        /// <summary>
        /// Results from CompetitionPages
        /// </summary>
        /// <param name="id">competition id</param>
        /// <returns></returns>
        public ActionResult Result(int id)
        {
            CompetitionViewModel competition = _competitionService.GetById(id);
            if (competition != null)
            {
                // get info about the competition
                ViewBag.competitionTitle = competition.PageTitle;
                ViewBag.CompetitionContent = "";
                PageViewModel page = _pageService.GetById(competition.PageId, true);
                if (page != null)
                {
                    ViewBag.CompetitionContent = page.ContentAsHtml;
                }

                // get list of competition Pages
                List<PageViewModel> model = _competitionService.GetCompetitionPages(id);

                return View(model);
            }

            return RedirectToAction("List");
        }

        /// <summary>
        /// Display the competition page and a button to participate
        /// </summary>
        /// <returns></returns>
        public ActionResult Participate(int id)
        {
            CompetitionViewModel competition = _competitionService.GetById(id);
            if (competition != null && competition.PageId != -1)
            {
                PageViewModel model = _pageService.GetById(competition.PageId);
                return View(model);
            }
            return RedirectToAction("List");
        }

        /// <summary>
        /// Display the competition page and a button to participate
        /// </summary>
        /// <returns></returns>
        public ActionResult ParticipateConfirmed()
        {
            // redirect to the creation of a page
            return RedirectToAction("New", "pages");
        }
        /// <summary>
        /// List the competition pages in order to rate them
        /// </summary>
        /// <returns></returns>
        public ActionResult ListPagesForRating(int id)
        {
            CompetitionViewModel competition = _competitionService.GetById(id);
            List<PageAndUserRatingViewModel> model = _pageService.FindPagesByCompetition(id, Context.CurrentUsername);
            if (model != null)
            {
                ViewBag.competitionId = id;
                ViewBag.competitionTitle = competition.PageTitle;
                return View(model);
            }
            return RedirectToAction("List");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="competitionId"></param>
        /// <returns></returns>
        public ActionResult ViewForRating(int? id, int competitionId)
        {
            if (id == null || id < 1)
                return RedirectToAction("Index", "Home");

            PageViewModel model = _pageService.GetById(id.Value, true);

            if (model.CreatedBy == Context.CurrentUsername)
            {
                return View("MyPage", model);
            }

            // TODO one request for optimization
            ViewBag.userrating = _pageService.GetPageRatingFromUser(model.Id, Context.CurrentUsername);
            ViewBag.currentuser = Context.CurrentUsername;
            ViewBag.competitionId = competitionId;
            _pageService.IncrementNbView(id.Value);


            // when a user alerts, it i taken into account by Pages/PageAlert

            return View(model);
        }

        #region HttpPost

        [HttpPost]
        [AdminRequired]
        public ActionResult Edit(CompetitionViewModel model)
        {
            model.Status = CompetitionViewModel.StatusStringToEnum(model.StatusString);

            _competitionService.UpdateCompetition(model);

            return RedirectToAction("List");
        }

        [ValidateInput(false)]
        [AdminRequired]
        [HttpPost]
        public ActionResult New(CompetitionViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            model.Status = CompetitionViewModel.StatusStringToEnum(model.StatusString);
            _competitionService.AddCompetition(model);

            return RedirectToAction("List");
        }

        //[HttpPost]
        //public ActionResult Achieve(int id)
        //{
        //    _competitionService.Achieve(id);
        //    return RedirectToAction("List");
        //}


        #endregion


    }
}


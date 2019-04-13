using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web.Mvc;
using Roadkill.Core.Diff;
using Roadkill.Core.Converters;
using Roadkill.Core.Configuration;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using System.Web;
using Roadkill.Core.Text;
using Roadkill.Core.Extensions;
using Roadkill.Core.Database;
using System.IO;

using Roadkill.Core.Attachments;
using Roadkill.Core.Email;
using Roadkill.Core.Localization;

namespace Roadkill.Core.Mvc.Controllers
{
	/// <summary>
	/// Provides all page related functionality, including editing and viewing pages.
	/// </summary>
	[HandleError]
	[OptionalAuthorization]
	public class PagesController : ControllerBase
	{
		private SettingsService _settingsService;
		private IPageService _pageService;
		private SearchService _searchService;
		private PageHistoryService _historyService;
        private AttachmentPathUtil _attachmentPathUtil;
        //private UserServiceBase _userServiceBase;
        private IRepository _repository;
        private PublishPageEmail _publishPageEmail;
        private RejectPageEmail _rejectPageEmail;

        public const string AlertLanguage = "language";
        public const string AlertPublicity = "publicity";
        public const string AlertRespect="respect";
        public const string AlertControversial="controversial";
        public const string AlertOther = "other";

        public PagesController(ApplicationSettings settings, UserServiceBase userManager,
			SettingsService settingsService, IPageService pageService, SearchService searchService,
            PageHistoryService historyService, IUserContext context, IRepository repository,
            PublishPageEmail publishPageEmail, RejectPageEmail rejectPageEmail)
			: base(settings, userManager, context, settingsService)
		{
			_settingsService = settingsService;
			_pageService = pageService;
			_searchService = searchService;
			_historyService = historyService;
            _attachmentPathUtil = new AttachmentPathUtil(settings);
		    //_userServiceBase = userManager;
		    _repository = repository;
            _publishPageEmail = publishPageEmail;
            _rejectPageEmail = rejectPageEmail;

        }

		/// <summary>
		/// Displays a list of all page titles and ids in Roadkill.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
		[BrowserCache]
        [ControllerRequired]
        public ActionResult AllPages()
		{
			return View(_pageService.AllPages());
		}

        /// <summary>
        /// Displays a list of all page titles and ids in Roadkill.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
        [BrowserCache]
        [ControllerRequired]
        public ActionResult AllNewPages()
        {
            var all = _pageService.AllNewPages();
            return View(all);
        }

        /// <summary>
        /// Displays a list of all alerted page titles and ids in Roadkill.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
        //[BrowserCache]
        //[ControllerRequired]
        //public ActionResult AllPagesWithAlerts()
        //{
        //    var all = _pageService.AllPagesWithAlerts();
        //    return View(all);
        //}

        /// <summary>
        /// Displays a list of current user page titles and ids in Roadkill.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
        [EditorRequired]
        [BrowserCache]
        public ActionResult MyPages(string id, bool? encoded)
        {
            // after submit, id = null --> leads to an exception
            // when changing the user, the id is the older one
            ViewBag.IsUserAdmin = Context.IsAdmin;
            string currentUser = Context.CurrentUsername;
            if (id == Context.CurrentUsername)
            {

                // Usernames are base64 encoded by roadkill (to cater for usernames like domain\john).
                // However the URL also supports humanly-readable format, e.g. /ByUser/chris
                if (encoded == true)
                {
                    id = id.FromBase64();
                }

                ViewData["Username"] = id;

                return View(_pageService.MyPages(id));
            }
            else
            {
                return View(_pageService.MyPages(currentUser));
                //return View(); // TODO check what is the result --> exception
            }
        }

        /// <summary>
        /// Displays all tags (categories if you prefer that term) in Roadkill.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TagViewModel}"/> as the model.</returns>
        [BrowserCache]
		public ActionResult AllTags()
		{
			return View(_pageService.AllTags().OrderBy(x => x.Name));
		}

		/// <summary>
		/// Returns all tags in the system as a JSON string.
		/// </summary>
		/// <param name="term">The jQuery UI autocomplete filter passed in, e.g. when "ho" is typed for homepage.</param>
		/// <returns>A string array of tags.</returns>
		/// <remarks>This action requires editor rights.</remarks>
		[EditorRequired]
		public ActionResult AllTagsAsJson(string term = "")
		{
			IEnumerable<TagViewModel> tags = _pageService.AllTags();
			if (!string.IsNullOrEmpty(term))
				tags = tags.Where(x => x.Name.StartsWith(term, StringComparison.InvariantCultureIgnoreCase));

			IEnumerable<string> tagsJson = tags.Select(t => t.Name).ToList();
			return Json(tagsJson, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Displays all pages for a particular user.
		/// </summary>
		/// <param name="id">The username</param>
		/// <param name="encoded">Whether the username paramter is Base64 encoded.</param>
		/// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
		public ActionResult ByUser(string id, bool? encoded)
		{
			// Usernames are base64 encoded by roadkill (to cater for usernames like domain\john).
			// However the URL also supports humanly-readable format, e.g. /ByUser/chris
			if (encoded == true)
			{
				id = id.FromBase64();
			}

			ViewData["Username"] = id;

			return View(_pageService.AllPagesCreatedBy(id));
		}

        /// <summary>
        /// Deletes a wiki page.
        /// </summary>
        /// <param name="id">The id of the page to delete.</param>
        /// <returns>Redirects to AllPages action.</returns>
        /// <remarks>This action requires admin rights.</remarks>
        [AdminRequired]
        [ControllerRequired]
        public ActionResult Delete(int id)
        {
            _pageService.DeletePage(id);

            return RedirectToAction("MyPages");
        }


        /// <summary>
        /// Draft is an Ajax request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EditorRequired]
        public ActionResult SetDraftAndEdit(int id)
        {
            _pageService.SetDraft(id);
            //PageViewModel model = _pageService.GetById(id);
            return RedirectToAction("Edit", new { id });
        }

        /// <summary>
        /// Draft is an Ajax request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [EditorRequired]
        public ActionResult SetDraft(int id)
        {
            _pageService.SetDraft(id);

            // return sucess to ajax request
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes a wiki page.
        /// </summary>
        /// <param name="id">The id of the page to delete.</param>
        /// <returns>Redirects to AllPages action.</returns>
        /// <remarks>This action requires admin rights.</remarks>
        [EditorRequired]
        public ActionResult Submit(int id, string view)
        {
            if (Context.IsAdmin)
            {
                // submit is a validate for the admin
                _pageService.ValidatePage(id, Context.CurrentUsername, 0);
            }
            else
            {
                _repository.SubmitPage(id);
            }

            if (view.ToLower() == "mypages")
            {
                return RedirectToAction("MyPages");
            }

            //if (view == "MyPage")
            return RedirectToAction("Index", "Wiki", new { id });
        }

        /// <summary>
        /// Deletes a wiki page.
        /// </summary>
        /// <param name="id">The id of the page to validate.</param>
        /// <returns>Redirects to AllPages action.</returns>
        /// <remarks>This action requires admin rights.</remarks>
        [ControllerRequired]
        public ActionResult ControlPage(int id)
        {
            PageViewModel model = _pageService.GetById(id, true);

            if (model != null)
            {
                if (model.IsLocked && !Context.IsAdmin)
                    return new HttpStatusCodeResult(403, string.Format("The page '{0}' can only be edited by administrators.", model.Title));

                model.AllTags = _pageService.AllTags().ToList();

                UserActivity userActivity = _pageService.GetUserActivity(model.CreatedBy);
                model.SetUserActivity(userActivity);

                ViewBag.AlertLanguage = AlertLanguage;
                ViewBag.AlertPublicity = AlertPublicity;
                ViewBag.AlertRespect = AlertRespect;
                ViewBag.AlertControversial = AlertControversial;
                ViewBag.AlertOther = AlertOther;

                return View("ControlPage", model);
            }
            else
            {
                //MJO TODO return RedirectToAction("New");
            }

            return RedirectToAction("ControlPage");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="svalidated"></param>
        /// <param name="srating"></param>
        /// <param name="RawTags"></param>
        /// <param name="rejecttype">"language", "publicity", "respect", "controversial" or "other"</param>
        /// <param name="ControllerRating"></param>
        /// <returns></returns>
        [ControllerRequired]
        [HttpPost]
        public ActionResult ControlPage(int id, string svalidated, string RawTags, string rejecttype, string ControllerRating)
        {
            PageViewModel model = _pageService.GetById(id);
            User user = _repository.GetUserByUsername(model.CreatedBy);

            if (svalidated == "true")
            {
                // control should be made by front-end
                int controlRating = Int32.Parse(ControllerRating);
                if (controlRating > 5)
                {
                    controlRating = 5;
                }
                if (controlRating < 0)
                {
                    controlRating = 0;
                }

                _pageService.ValidatePage(id, Context.CurrentUsername, controlRating, RawTags);

                PageEmailInfo info = new PageEmailInfo(user, model, null);
                _publishPageEmail.Send(info);
            }

            if (svalidated == "false")
            {
                // updating index is useless
                _pageService.RejectPage(id);

                // send an email
                PageEmailInfo info = new PageEmailInfo(user, model, rejecttype);
                _rejectPageEmail.Send(info);
            }

            return RedirectToAction("AllNewPages");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ControllerRequired]
        public ActionResult ReControlPage(int id)
        {
            PageViewModel model = _pageService.GetById(id, true);

            if (model != null)
            {
                if (model.IsLocked && !Context.IsAdmin)
                    return new HttpStatusCodeResult(403, string.Format("The page '{0}' can only be edited by administrators.", model.Title));

                model.AllTags = _pageService.AllTags().ToList();

                ViewBag.AlertLanguage = AlertLanguage;
                ViewBag.AlertPublicity = AlertPublicity;
                ViewBag.AlertRespect = AlertRespect;
                ViewBag.AlertControversial = AlertControversial;
                ViewBag.AlertOther = AlertOther;

                return View("ReControlPage", model);
            }
            else
            {
                //MJO TODO return RedirectToAction("New");
            }

            return RedirectToAction("ReControlPage"); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rejecttype"></param>
        /// <returns></returns>
        [HttpPost]
        [ControllerRequired]
        public ActionResult ReControlPage(int id, string rejecttype)
        {
            // POST here means that the page as been rejected

            // Send an email
            PageViewModel model = _pageService.GetById(id);
            User user = _repository.GetUserByUsername(model.CreatedBy);
            PageEmailInfo info = new PageEmailInfo(user, model, rejecttype);
            _rejectPageEmail.Send(info);

            // delete alerts of the page
            _repository.DeletPageAlerts(id);

            _pageService.RejectPage(id);
            return RedirectToAction("ListAlerts", "Alerts");
        }

        /// <summary>
        /// Deletes a wiki page.
        /// </summary>
        /// <param name="id">The id of the page to reject.</param>
        /// <returns>Redirects to AllPages action.</returns>
        /// <remarks>This action requires admin rights.</remarks>
        [ControllerRequired]
        public ActionResult DeletPageAlerts(int pageId)
        {
            _repository.DeletPageAlerts(pageId);
            return RedirectToAction("AllNewPages");
        }


        /// <summary>
        /// Displays the edit View for the page provided in the id.
        /// </summary>
        /// <param name="id">The ID of the page to edit.</param>
        /// <returns>An filled <see cref="PageViewModel"/> as the model. If the page id cannot be found, the action
        /// redirects to the New page.</returns>
        /// <remarks>This action requires editor rights.</remarks>
        [EditorRequired]
        public ActionResult Edit(int id)
        {

            PageViewModel model = _pageService.GetById(id, true);

            ViewBag.userpath = Context.AttachmentsPath;

            if (model != null)
            {
                if (model.IsLocked && !Context.IsAdmin)
                    return new HttpStatusCodeResult(403, string.Format("The page '{0}' can only be edited by administrators.", model.Title));

                model.AllTags = _pageService.AllTags().ToList();

                return View("Edit", model);
            }
            else
            {
                return RedirectToAction("New");
            }
        }

		/// <summary>
		/// Saves all POST'd data for a page edit to the database.
		/// </summary>
		/// <param name="model">A filled <see cref="PageViewModel"/> containing the new data.</param>
		/// <returns>Redirects to /Wiki/{id} using the passed in <see cref="PageViewModel.Id"/>.</returns>
		/// <remarks>This action requires editor rights.</remarks>
		[EditorRequired]
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(PageViewModel model)
		{
			if (!ModelState.IsValid)
				return View("Edit", model);

            _pageService.UpdatePage(model);

			return RedirectToAction("Index", "Wiki", new { id = model.Id });
		}

		/// <summary>
		/// This action is for JSON calls only. Displays a HTML preview for the provided 
		/// wiki markup/markdown. This action is POST only.
		/// </summary>
		/// <param name="id">The wiki markup.</param>
		/// <returns>The markup as rendered as HTML.</returns>
		/// <remarks>This action requires editor rights.</remarks>
		[ValidateInput(false)]
		[EditorRequired]
		[HttpPost]
		public ActionResult GetPreview(string id)
		{
            PageHtml pagehtml = "";

			if (!string.IsNullOrEmpty(id))
			{
				MarkupConverter converter = _pageService.GetMarkupConverter();
				pagehtml = converter.ToHtml(id);
			}

			return JavaScript(pagehtml.Html);
		}

		/// <summary>
		/// Lists the history of edits for a page.
		/// </summary>
		/// <param name="id">The ID of the page.</param>
		/// <returns>An <see cref="IList{PageHistoryViewModel}"/> as the model.</returns>
		[BrowserCache]
		public ActionResult History(int id)
		{
			ViewData["PageId"] = id;
			return View(_historyService.GetHistory(id).ToList());
		}

		/// <summary>
		/// Displays the Edit view in new page mode.
		/// </summary>
		/// <returns>An empty <see cref="PageViewModel"/> as the model.</returns>
		/// <remarks>This action requires editor rights.</remarks>
		[EditorRequired]
		public ActionResult New(string title = "", string tags = "")
		{
			PageViewModel model = new PageViewModel()
			{
				Title = title,
				RawTags = tags,
			};

			model.AllTags = _pageService.AllTags().ToList();

			return View("Edit", model);
		}

        /// <summary>
        /// Saves a new page using the provided <see cref="PageViewModel"/> object to the database.
        /// </summary>
        /// <param name="model">The page details to save.</param>
        /// <returns>Redirects to /Wiki/{id} using the newly created page's ID.</returns>
        /// <remarks>This action requires editor rights.</remarks>
        [EditorRequired]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(PageViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            model = _pageService.AddPage(model);

            return RedirectToAction("Index", "Wiki", new { id = model.Id });
        }

        /// <summary>
        /// Reverts a page to the version specified, creating a new version in the process.
        /// </summary>
        /// <param name="versionId">The Guid ID of the version to revert to.</param>
        /// <param name="pageId">The id of the page</param>
        /// <returns>Redirects to the History action using the pageId parameter.</returns>
        /// <remarks>This action requires editor rights.</remarks>
        [EditorRequired]
		public ActionResult Revert(Guid versionId, int pageId)
		{
			// Check if the page is locked to admin edits only before reverting.
			PageViewModel page = _pageService.GetById(pageId);
			if (page == null || (page.IsLocked && !Context.IsAdmin))
				return RedirectToAction("Index", "Home");

			_historyService.RevertTo(versionId, Context);

			return RedirectToAction("History", new { id = pageId });
		}

		/// <summary>
		/// Returns all pages for the given tag.
		/// </summary>
		/// <param name="id">The tag name</param>
		/// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
		public ActionResult Tag(string id)
		{
            MarkupConverter converter = _pageService.GetMarkupConverter();
            GalleryViewModel galleryModel = new GalleryViewModel(converter);
            galleryModel.listPages = _pageService.FindByTag(id).ToList();

            galleryModel.Title = string.Format(SiteStrings.Pages_ForTag, HttpUtility.UrlDecode(id));
            //SiteStrings.Gallery_Last_Publications;

            return View("../Home/Index", galleryModel);

   //         id = HttpUtility.UrlDecode(id);
			//ViewData["Tagname"] = id;

			//return View(_pageService.FindByTag(id));
		}

		/// <summary>
		/// Gets a particular version of a page.
		/// </summary>
		/// <param name="id">The Guid ID for the version.</param>
		/// <returns>A <see cref="PageViewModel"/> as the model, which contains the HTML diff
		/// output inside the <see cref="PageViewModel.Content"/> property.</returns>
		public ActionResult Version(Guid id)
		{
			MarkupConverter converter = _pageService.GetMarkupConverter();
			IList<PageViewModel> bothVersions = _historyService.CompareVersions(id).ToList();
			string diffHtml = "";

			if (bothVersions[1] != null)
			{
				string oldVersion = converter.ToHtml(bothVersions[1].Content).Html;
				string newVersion = converter.ToHtml(bothVersions[0].Content).Html;
				HtmlDiff diff = new HtmlDiff(oldVersion, newVersion);
				diffHtml = diff.Build();
			}
			else
			{
				diffHtml = converter.ToHtml(bothVersions[0].Content).Html;
			}

			PageViewModel model = bothVersions[0];
			model.Content = diffHtml;
			return View(model);
		}


        /// <summary>
        /// Displays the edit View for the page provided in the id.
        /// </summary>
        /// <param name="id">The ID of the page to edit.</param>
        /// <returns>An filled <see cref="PageViewModel"/> as the model. If the page id cannot be found, the action
        /// redirects to the New page.</returns>
        /// <remarks>This action requires editor rights.</remarks>
        [EditorRequired]
        public ActionResult Rate(int id)
        {
            RateViewModel model = new RateViewModel(_pageService.GetCurrentContent(id).Page);

            if (model != null)
            {
                var view = View("Error", (Object)model);
                return view;
            }
            else
            {
                return RedirectToAction("New");
            }
        }

        /// <summary>
        /// Save an alert for the page
        /// </summary>
        /// <param name="id">The ID of the page to edit.</param>
        /// <returns>An filled <see cref="PageViewModel"/> as the model. If the page id cannot be found, the action
        /// redirects to the New page.</returns>
        /// <remarks>This action requires editor rights.</remarks>
        [HttpPost]
        public ActionResult PageAlert(int id, string alerttype)
        {
            string ip = _pageService.GetUserIp();
            Alert alert = new Alert(id, ip, alerttype);
            _pageService.AddAlert(alert);
            return RedirectToAction("Index", "Wiki", new { id });
        }

        [HttpPost]
        public ActionResult PageRemoveAlert(int id)
        {
            string ip = _pageService.GetUserIp();
            _repository.DeletPageAlertsByUser(id, ip);
            return RedirectToAction("Index", "Wiki", new { id });
        }

        /// <summary>
        /// Comment Alert, not page alert !!!
        /// </summary>
        /// <param name="commenGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommentAlert(Guid commenGuid)
        {
            Alert alert = new Alert(commenGuid, Context.CurrentUsername, "");
            _pageService.AddAlert(alert);
            return Content("Alert taken into account", MediaTypeNames.Text.Plain);
        }


        /// <summary>
        /// Saves all POST'd data for a page edit to the database.
        /// </summary>
        /// <param name="model">A filled <see cref="PageViewModel"/> containing the new data.</param>
        /// <returns>Redirects to /Wiki/{id} using the passed in <see cref="PageViewModel.Id"/>.</returns>
        /// <remarks>This action requires editor rights.</remarks>
        [EditorRequired]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Rate(PageViewModel model)
        {
            _pageService.UpdatePage(model);
            return RedirectToAction("Index", "Wiki", new { id = model.Id });
        }

        [EditorRequired]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult  PageRating(int id, string rating)
        {
            _pageService.SetPageRatingForUser( id, Context.CurrentUsername, Int32.Parse(rating));

            // TODO redirect strange because it is an ajax post
            return RedirectToAction("Index", "Wiki");
        }

        [EditorRequired]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PageAddComment(int id, string commenttext)
        {
            _pageService.SetPageCommentForUser(id, Context.CurrentUsername, commenttext);

            // TODO redirect strange because it is an ajax post
            return RedirectToAction("Index", "Wiki");
        }

        [EditorRequired]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PageRemoveComment(int id)
        {
            _pageService.SetPageCommentForUser(id, Context.CurrentUsername, "");

            // TODO redirect strange because it is an ajax post
            return RedirectToAction("Index", "Wiki");
        }
    }
}


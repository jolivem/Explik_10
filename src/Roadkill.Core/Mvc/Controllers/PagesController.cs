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
        private UserServiceBase _userServiceBase;
        private IRepository _repository;


		public PagesController(ApplicationSettings settings, UserServiceBase userManager,
			SettingsService settingsService, IPageService pageService, SearchService searchService,
            PageHistoryService historyService, IUserContext context, IRepository repository)
			: base(settings, userManager, context, settingsService)
		{
			_settingsService = settingsService;
			_pageService = pageService;
			_searchService = searchService;
			_historyService = historyService;
            _attachmentPathUtil = new AttachmentPathUtil(settings);
		    _userServiceBase = userManager;
		    _repository = repository;

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
	    [BrowserCache]
	    [ControllerRequired]
	    public ActionResult AllPagesWithAlerts()
	    {
	        var all = _pageService.AllPagesWithAlerts();
	        return View(all);
	    }

	    /// <summary>
        /// Displays a list of current user page titles and ids in Roadkill.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
        [BrowserCache]
        public ActionResult MyPages(string id, bool? encoded)
        {
            //TODO removeid, not needed !!!
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
                return View(); // TODO check what is the result --> exception
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
        //[AdminRequired]
        [ControllerRequired]
        public ActionResult Delete(int id)
        {
            _pageService.DeletePage(id);

            return RedirectToAction("MyPages");
        }

        [EditorRequired]
        public ActionResult Draft(int id)
        {
            _repository.SetDraft(id);

            return RedirectToAction("MyPages");
        }

        /// <summary>
        /// Deletes a wiki page.
        /// </summary>
        /// <param name="id">The id of the page to delete.</param>
        /// <returns>Redirects to AllPages action.</returns>
        /// <remarks>This action requires admin rights.</remarks>
        [EditorRequired]
        public ActionResult Submit(int id)
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

            return RedirectToAction("MyPages");
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
        /// <param name="scanvas"></param>
        /// <returns></returns>
        [ControllerRequired]
        [HttpPost]
        public ActionResult ControlPage(int id, string svalidated, string srating, string RawTags, string scanvas)
        {
            if (svalidated == "true")
            {
                _pageService.ValidatePage(id, Context.CurrentUsername, Int32.Parse(srating), RawTags);

                SaveCanvas(id, scanvas);
            }

            if (svalidated == "false")
            {
                _repository.RejectPage(id);
                
            }

            return RedirectToAction("AllNewPages");
        }

        /// <summary>
        /// Deletes a wiki page.
        /// </summary>
        /// <param name="id">The id of the page to reject.</param>
        /// <returns>Redirects to AllPages action.</returns>
        /// <remarks>This action requires admin rights.</remarks>
        //[ControllerRequired]
        //public ActionResult Reject(int id)
        //{
        //    _repository.RejectPage(id);
        //    return RedirectToAction("AllNewPages");
        //}

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

            // use viewBag because it is not a page data but a user data
            //ViewBag.userrating = _pageService.GetPageRatingFromUser(model.Id, Context.CurrentUsername);
            
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
            //TODO validate the page automatically, nonono use validate in a specific window, need possibility to use draft

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
			id = HttpUtility.UrlDecode(id);
			ViewData["Tagname"] = id;

			return View(_pageService.FindByTag(id));
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
        /// 
        /// </summary>
        /// <param name="id">The ID of the page to edit.</param>
        /// <returns>An filled <see cref="PageViewModel"/> as the model. If the page id cannot be found, the action
        /// redirects to the New page.</returns>
        /// <remarks>This action requires editor rights.</remarks>
        //[EditorRequired] //TODO controller required
        //public ActionResult Valid(int id)
        //{
        //    PageViewModel model = _pageService.GetById(id, true);

        //    if (model != null)
        //    {
        //        if (model.IsLocked && !Context.IsAdmin)
        //            return new HttpStatusCodeResult(403, string.Format("The page '{0}' can only be edited by administrators.", model.Title));

        //        model.AllTags = _pageService.AllTags().ToList();

        //        return View("Edit", model);
        //    }
        //    else
        //    {
        //        return RedirectToAction("New");
        //    }
        //}
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public void SaveCanvas(int id, string image)
        {
            string physicalPath = _attachmentPathUtil.ConvertUrlPathToPhysicalPath(_pageService.GetCurrentContent((int)id).Page.FilePath);

            if (physicalPath != null)
            {
                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                string physicalFilePath = Path.Combine(physicalPath, "page_" + id + ".jpg");
                using (FileStream fs = new FileStream(physicalFilePath, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        byte[] data = Convert.FromBase64String(image); //convert from base64
                        bw.Write(data);
                        bw.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Save an alert for the page
        /// </summary>
        /// <param name="id">The ID of the page to edit.</param>
        /// <returns>An filled <see cref="PageViewModel"/> as the model. If the page id cannot be found, the action
        /// redirects to the New page.</returns>
        /// <remarks>This action requires editor rights.</remarks>
        public ActionResult PageAlert(int id)
        {
            Alert alert = new Alert(id, Context.CurrentUsername);
            _pageService.AddAlert(alert);
            return Content("Alert taken into account", MediaTypeNames.Text.Plain);
        }

        /// <summary>
        /// Comment Alert, not page alert !!!
        /// </summary>
        /// <param name="commenGuid"></param>
        /// <returns></returns>
        public ActionResult CommentAlert(Guid commenGuid)
        {
            Alert alert = new Alert(commenGuid, Context.CurrentUsername);
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
            //if (!ModelState.IsValid)
            //    return View("Rate", model); //TODO

            _pageService.UpdatePage(model);

            return RedirectToAction("Index", "Wiki", new { id = model.Id });
        }

        [EditorRequired]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PageRating(int id, string rating)
        {

            //TODO if not editor, redirect to login
            //if (!ModelState.IsValid)
            //    return View("Rate", model); //TODO

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

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Validate(int id)
        //{
        //    //if (!ModelState.IsValid)
        //    //    return View("Rate", model); //TODO check if hack the address !!!!

        //    // update controller
        //    // update date
        //    string sRating = Request.Form["rating"];
        //    int iRating = Int32.Parse(sRating);
        //    _pageService.ValidatePage(id, Context.CurrentUsername, iRating); //TODO check now or UTCNow
        //    //_pageService.UpdatePage(id); // update tags TODO
        //    //TODO send a mail

        //    return RedirectToAction("AllNewPages", "Pages");
        //}
    }
}


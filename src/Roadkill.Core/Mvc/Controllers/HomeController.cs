using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Roadkill.Core.Converters;
using Roadkill.Core.Localization;
using Roadkill.Core.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.UI;

using Roadkill.Core.Attachments;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Text;

using Page = Roadkill.Core.Database.Page;

namespace Roadkill.Core.Mvc.Controllers
{
	/// <summary>
	/// Provides functionality that is common through the site.
	/// </summary>
	[OptionalAuthorization]
	public class HomeController : ControllerBase
	{
	    private PageService _pageService;
		private SearchService _searchService;
		private MarkupConverter _markupConverter;
        
        private ApplicationSettings _applicationSettings ;

		public HomeController(ApplicationSettings settings, UserServiceBase userManager, MarkupConverter markupConverter,
			PageService pageService, SearchService searchService, IUserContext context, SettingsService settingsService)
			: base(settings, userManager, context, settingsService) 
		{
			_markupConverter = markupConverter;
			_searchService = searchService;
			_pageService = pageService;
            //_attachmentPathUtil = new AttachmentPathUtil(settings);
		    _applicationSettings = settings;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [BrowserCache]
        public ActionResult Index()
        {
            PageViewModel model = null;
            model = _pageService.FindPageWithTag("__home");
            return View(model);
        }

        [BrowserCache]
        public ActionResult LastPublications()
        {
            GalleryViewModel galleryModel = new GalleryViewModel(_markupConverter);

            galleryModel.listPages = (List<PageViewModel>)_pageService.PagesMostRecent(30);

            galleryModel.Title = SiteStrings.Google_Title;

            //find Explik introduction
            ViewBag.ExplikIntroduction = "";
            PageViewModel page = _pageService.FindPageWithTag("___intro");
            if (page != null)
            {
                ViewBag.ExplikIntroduction = page.ContentAsHtml;
            }

            return View( galleryModel);
        }

        /// <summary>
        /// Searches the lucene index using the search string provided.
        /// </summary>
        public ActionResult Search(string q)
		{
			ViewData["search"] = q;

			List<SearchResultViewModel> results = _searchService.Search(q).ToList();
			return View(results);
		}

		/// <summary>
		/// Returns Javascript 'constants' for the site.
		/// </summary>
		/// <param name="version">This is sent by the views to ensure new versions of Roadkill have this JS file cleared from the cache.</param>
		[CacheContentType(Duration = 86400 * 30, ContentType = "application/javascript")] // 30 days
		[AllowAnonymous]
		public ActionResult GlobalJsVars(string version)
		{
			return View();
		}

		/// <summary>
		/// Displays the left side menu view, including new page/settings if logged in.
		/// </summary>
		[AllowAnonymous]
		public ActionResult NavMenu()
		{
			return Content(_pageService.GetMenu(Context));
		}

		/// <summary>
		/// Displays the a Bootstrap-styled left side menu view, including new page/settings if logged in.
		/// </summary>
		[AllowAnonymous]
		public ActionResult BootstrapNavMenu()
		{
			return Content(_pageService.GetBootStrapNavMenu(Context));
		}
		
		/// <summary>
		/// Legacy action - use NavMenu().
		/// </summary>
		/// <returns></returns>
		[Obsolete]
		[AllowAnonymous]
		public ActionResult LeftMenu()
		{
			return Content(_pageService.GetMenu(Context));
		}

        /// <summary>
        /// Displays the wiki page using the provided id, without title and with a "return" link
        /// </summary>
        /// <param name="tag">The page id</param>
        [BrowserCache]
        public ActionResult About(string tag)
        {
            if (tag == null)
                return RedirectToAction("Index", "Home");

            PageViewModel model = null;

            // id should be a kind of enum
            model = _pageService.FindPageWithTag(tag);
 
            if (model != null)
            {
                return View("About", model);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
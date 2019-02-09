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


        [BrowserCache]
        public ActionResult Index()
        {
            PageViewModel pmodel = _pageService.FindHomePage();
            var zaza = pmodel;

            GalleryViewModel galleryModel = new GalleryViewModel(_markupConverter);

            galleryModel.listMostRecent = (List<PageViewModel>)_pageService.PagesMostRecent(10);
            foreach (PageViewModel model in galleryModel.listMostRecent)
            {
                model.FilePath = _applicationSettings.AttachmentsUrlPath + "/" + model.FilePath + "/";
            }

            //galleryModel.listMostViewed = (List<PageViewModel>)_pageService.PagesMostViewed(10);
            //foreach (PageViewModel model in galleryModel.listMostViewed)
            //{
            //    model.FilePath = _applicationSettings.AttachmentsUrlPath + "/" + model.FilePath + "/";
            //}

            //galleryModel.listBestRated = (List<PageViewModel>)_pageService.PagesBestRated(10);
            //foreach (PageViewModel model in galleryModel.listBestRated)
            //{
            //    model.FilePath = _applicationSettings.AttachmentsUrlPath + "/" + model.FilePath + "/";
            //}

            return View("Index", galleryModel);
            // display a galery of pages

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
        /// <param name="id">The page id</param>
        /// <param name="title">This parameter is passed in, but never used in queries.</param>
        [BrowserCache]
        public ActionResult About(int? id, string title)
        {
            if (id == null || id < 1)
                return RedirectToAction("Index", "Home");

            PageViewModel model = null;

            // id should be a kind of enum
            switch (id)
            {
                case 1: // Contact
                    model = _pageService.GetById(1, true);
                    break;
                case 2: // About
                    model = _pageService.GetById(2, true);
                    break;
                case 3: // Privacy
                    model = _pageService.GetById(3, true);
                    break;
                case 4: // Avertissements
                    model = _pageService.GetById(4, true);
                    break;
                case 5: // First key
                    model = _pageService.GetById(5, true);
                    break;
            }

            if (model != null)
            {
                return View("About", model);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
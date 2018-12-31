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
		/// Display the homepage/mainpage. If no page has been tagged with the 'homepage' tag,
		/// then a dummy PageViewModel is put in its place.
		/// </summary>
		//[BrowserCache]
		//public ActionResult Index()
		//{
		//    if (Context.IsController)
		//    {
  //              return RedirectToAction("AllNewPages", "Pages");
		//    }
  //          return RedirectToAction("Index");

		//	//return View(model);
		//    //return null;
  //      }


        [BrowserCache]
        public ActionResult Index()
        {
            PageViewModel pmodel = _pageService.FindHomePage();
            var zaza = pmodel;

            GalleryViewModel model = new GalleryViewModel();
            var toto = _pageService.MyPages(Context.CurrentUsername);
            var titi = toto;
            model.listMostRecent = (List<Page>)_pageService.PagesMostRecent(10);
            foreach (Page page in model.listMostRecent)
            {
                page.FilePath = _applicationSettings.AttachmentsUrlPath + "/" + page.FilePath + "/";
            }
            
            model.listMostViewed = (List<Page>)_pageService.PagesMostViewed(10);
            foreach (Page page in model.listMostViewed)
            {
                page.FilePath = _applicationSettings.AttachmentsUrlPath + "/" + page.FilePath + "/";
            }

            model.listBestRated = (List<Page>)_pageService.PagesBestRated(5);
            foreach (Page page in model.listBestRated)
            {
                page.FilePath = _applicationSettings.AttachmentsUrlPath + "/" + page.FilePath + "/";
            }

            return View("Index", model);
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
	}
}
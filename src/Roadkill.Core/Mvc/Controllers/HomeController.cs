﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Roadkill.Core.Converters;
using Roadkill.Core.Localization;
using Roadkill.Core.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
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

		public HomeController(ApplicationSettings settings, UserServiceBase userManager, MarkupConverter markupConverter,
			PageService pageService, SearchService searchService, IUserContext context, SettingsService settingsService)
			: base(settings, userManager, context, settingsService) 
		{
			_markupConverter = markupConverter;
			_searchService = searchService;
			_pageService = pageService;
		}

		/// <summary>
		/// Display the homepage/mainpage. If no page has been tagged with the 'homepage' tag,
		/// then a dummy PageViewModel is put in its place.
		/// </summary>
		[BrowserCache]
		public ActionResult Index()
		{
			// Get the first locked homepage
            //PageViewModel model = _pageService.FindHomePage();

            //if (model == null)
            //{
            //    model = new PageViewModel();
            //    model.Title = SiteStrings.NoMainPage_Title;
            //    model.Content = SiteStrings.NoMainPage_Label;
            //    model.ContentAsHtml = _markupConverter.ToHtml(SiteStrings.NoMainPage_Label).Html;
            //    model.CreatedBy = "";
            //    model.CreatedOn = DateTime.UtcNow;
            //    model.RawTags = "homepage";
            //    model.ModifiedOn = DateTime.UtcNow;
            //    model.ControlledBy = "";
            //    model.NbRating = 0;
            //    model.NbView = 0;
            //    model.IsVideo = false;
            //    model.IsSubmitted = false;
            //    model.IsControlled = false;
            //    model.IsRejected = false;
            //    model.TotalRating = 0;
            //    model.VideoUrl = "";
            //}

            return RedirectToAction("Gallery");

			//return View(model);
		    //return null;
        }


        [BrowserCache]
        public ActionResult Gallery()
        {
            PageViewModel pmodel = _pageService.FindHomePage();
            var zaza = pmodel;

            GalleryViewModel model = new GalleryViewModel();
            var toto = _pageService.MyPages(Context.CurrentUsername);
            var titi = toto;
            model.listMostRecent = (List<Page>)_pageService.PagesMostRecent(5);
            model.listMostViewed = (List<Page>)_pageService.PagesMostViewed(5);
            model.listBestRated = (List<Page>)_pageService.PagesBestRated(5);

            return View("Gallery", model);
            // display a galery of pages

		}

		/// <summary>
		/// Searches the lucene index using the search string provided.
		/// </summary>
		public ActionResult Search(string q)
		{
			ViewData["search"] = q;

			List<SearchResultViewModel> results = _searchService.Search(q).ToList();
		    foreach (var result in results)
		    {
		        Page page = _pageService.FindById(result.Id);
		        if (page != null)
		        {
		            result.NbView = page.NbView;
		            result.Rating = page.TotalRating / page.NbRating;
		            //Image = page.FilePath + "thumb.png"; //TODO
		            result.Canvas = "/Assets/Images/RaspberryPiBoard.png";
		        }
		    }


 //string image;
            
 //           image = b == 0 ? "/Assets/Images/RaspberryPiBoard.png" 
 //               builder.AppendLine("<div class='col-xs-4 caption'  style='padding-left:0'>");
 //               {
 //                   builder.AppendLine(string.Format("<a href='/wiki/{0}'>", page.Id));
 //                   {
 //                       builder.AppendLine(string.Format("<img class='img-responsive' src='{0}' alt='Lights' height='120'>", image));
 //                       builder.AppendLine("</a>");
 //                   }
 //                   builder.AppendLine("</div>");
 //               }
 //               builder.AppendLine("<div class='col-xs-8'  style='padding-left:0'>");
 //               {
 //                   builder.AppendLine(string.Format("<p class='block-with-text'>{0}<br>", page.Title)); //TODO 1 line only
 //                   builder.AppendLine(string.Format("<p class='block-with-text'><small>{0}</small><br></p>", page.Summary));
 //                   builder.AppendLine(string.Format("<p><small>{1}  Views: {0}</small></p>", page.NbView, EncodePageRating(page)));
 //                   builder.AppendLine("</div>");
 //               }
 //               builder.AppendLine("</div>");

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
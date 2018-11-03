using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Roadkill.Core.Localization;
using Roadkill.Core.Database;
using Roadkill.Core.Text;
using Roadkill.Core.Converters;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace Roadkill.Core.Mvc.ViewModels
{
	/// <summary>
	/// Provides summary data for a page.
	/// </summary>
	[CustomValidation(typeof(RateViewModel), "VerifyRawTags")] // TODO MJO
	public class RateViewModel
	{
		/// <summary>
		/// The page's unique id.
		/// </summary>
		public int Id { get; set; }
		
		/// <summary>
		/// The user who created the page.
		/// </summary>
		public string CreatedBy { get; set; }

		/// <summary>
		/// The page title.
		/// </summary>
		[Required(ErrorMessageResourceType=typeof(SiteStrings), ErrorMessageResourceName="Page_Validation_Title")]
		public string Title { get; set; }

		/// <summary>
		/// The page title, encoded so it is a safe search-engine friendly url.
		/// </summary>
		public string EncodedTitle
		{
			get
			{
				return RateViewModel.EncodePageTitle(Title);
			}
		}
		
		public RateViewModel()
		{
		}

		public RateViewModel(Page page)
		{
			if (page == null)
				throw new ArgumentNullException("page");

			Id = page.Id;
			Title = page.Title;
            CreatedBy = page.CreatedBy;
		}

        public RateViewModel(PageContent pageContent, MarkupConverter converter)
		{
			if (pageContent == null)
				throw new ArgumentNullException("pageContent");

			if (pageContent.Page == null)
				throw new ArgumentNullException("pageContent.Page");

			if (converter == null)
				throw new ArgumentNullException("converter");

			Id = pageContent.Page.Id;
			Title = pageContent.Page.Title;
		}

		// TODO: tests
		/// <summary>
		/// Removes all bad characters (ones which cannot be used in a URL for a page) from a page title.
		/// </summary>
		public static string EncodePageTitle(string title)
		{
			if (string.IsNullOrEmpty(title))
				return title;

			// Search engine friendly slug routine with help from http://www.intrepidstudios.com/blog/2009/2/10/function-to-generate-a-url-friendly-string.aspx

			// remove invalid characters
			title = Regex.Replace(title, @"[^\w\d\s-]", "");  // this is unicode safe, but may need to revert back to 'a-zA-Z0-9', need to check spec

			// convert multiple spaces/hyphens into one space       
			title = Regex.Replace(title, @"[\s-]+", " ").Trim();

			// If it's over 30 chars, take the first 30.
			title = title.Substring(0, title.Length <= 75 ? title.Length : 75).Trim();

			// hyphenate spaces
			title = Regex.Replace(title, @"\s", "-");

			return title;
		}
	}
}

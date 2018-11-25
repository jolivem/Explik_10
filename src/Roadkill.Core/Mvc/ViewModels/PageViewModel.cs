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

using Roadkill.Core.Services;

namespace Roadkill.Core.Mvc.ViewModels
{
    //public class Comments
    //{
    //    public string CreateBy;
    //    public DateTime CreatedOn;
    //    public string Comment;
    //    public int Rating; //from 1 to 5
    //}
    
    
	/// <summary>
	/// Provides summary data for a page.
	/// </summary>
	[CustomValidation(typeof(PageViewModel), "VerifyRawTags")]
	public class PageViewModel
	{
		private static string[] _tagBlackList = 
		{
			"#", ";", "/", "?", ":", "@", "&", "=", "{", "}", "|", "\\", "^", "[", "]", "`"		
		};

		private List<string> _tags;
		private string _rawTags;
		private string _content;

		/// <summary>
		/// The page's unique id.
		/// </summary>
		public int Id { get; set; }
		
		/// <summary>
		/// The text content for the page.
		/// </summary>
		public string Content
		{
			get { return _content; }
			set
			{
				// Ensure the content isn't null for lucene's benefit
				_content = value;
				if (_content == null)
					_content = "";
			}
		}

		/// <summary>
		/// The content after it has been transformed into HTML by the current wiki markup converter. This property 
		/// is only set when the PageContent object is passed into the constructor, and is empty unless explicitly 
		/// set by the caller.
		/// </summary>
		public string ContentAsHtml { get; set; }

		/// <summary>
		/// The user who created the page.
		/// </summary>
		public string CreatedBy { get; set; }

		/// <summary>
		/// The date the page was created.
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Returns true if no Id exists for the page.
		/// </summary>
		public bool IsNew
		{
			get
			{
				return Id == 0;
			}
		}

	    /// <summary>
	    /// Used for Controlling pages
	    /// </summary>
        private UserActivity _userActivity;

        /// <summary>
		/// The user who last modified the page.
		/// </summary>
		public string ControlledBy { get; set; }

		/// <summary>
		/// The date the page was last modified on.
		/// </summary>
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// Displays ModifiedOn in IS8601 format, plus the timezone offset included for timeago
		/// </summary>
		public string ModifiedOnWithOffset
		{
			get
			{
				// EditedOn (ModifiedOn in the domain) is stored in UTC time, so just add a Z to indicate this.
				return string.Format("{0}Z", ModifiedOn.ToString("s"));
			}
		}
		
		/// <summary>
		/// Gets the tags for the page as a list.
		/// </summary>
		public IEnumerable<string> Tags
		{
			get { return _tags; }
		}

		/// <summary>
		/// Sets or gets the tags for the page - these should be in comma separated format.
		/// </summary>
		public string RawTags
		{
			get 
			{ 
				return _rawTags; 
			}
			set
			{
				_rawTags = value;
				ParseRawTags();
			}
		}

		/// <summary>
		/// The page title before any update.
		/// </summary>
		public string PreviousTitle { get; set; }

		/// <summary>
		/// The page title.
		/// </summary>
		[Required(ErrorMessageResourceType=typeof(SiteStrings), ErrorMessageResourceName="Page_Validation_Title")]
		public string Title { get; set; }

		/// <summary>
        /// The page summary.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
		/// The page title, encoded so it is a safe search-engine friendly url.
		/// </summary>
		public string EncodedTitle
		{
			get
			{
				return PageViewModel.EncodePageTitle(Title);
			}
		}
		
		/// <summary>
        /// The global rating of the page.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
		/// The current version number for the page.
		/// </summary>
		public int VersionNumber { get; set; }

		/// <summary>
		/// Whether the page has been locked so that only admins can edit it.
		/// </summary>
		public bool IsLocked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long NbView;

        /// <summary>
        /// 
        /// </summary>
        public long NbRating;

        /// <summary>
        /// 
        /// </summary>
        public string RejectReason { get; set; }

        public IEnumerable<string> RejectReasonAvailable
        {
            get
            {
                return new string[] { "Unpolite", "TBC1", "TBC2" };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long TotalRating;

		/// <summary>
		/// Determines if the summary object can be cached on the browser and in the object cache. 
		/// This is true by default, but plugins that run on a page can mark a page as not cacheable.
		/// </summary>
		public bool IsCacheable { get; set; }

		/// <summary>
		/// Any additional head tag HTML generated by the text plugins.
		/// </summary>
		public string PluginHeadHtml { get; set; }

		/// <summary>
		/// Any additional footer HTML generated by the text plugins.
		/// </summary>
		public string PluginFooterHtml { get; set; }

		/// <summary>
		///  Any additional HTML generated by the text plugins that sits before the #container element.
		/// </summary>
		public string PluginPreContainer { get; set; }
        
		/// <summary>
        ///  
        /// </summary>
        //public string RatingStars { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string PluginComments { get; set; }

        /// <summary>
        ///  Any additional HTML generated by the text plugins that sits before the #container element.
        /// </summary>
        public string PluginPostContainer { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public List<Comment> AllComments { get; set; }

        public bool IsVideo { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsControlled { get; set; }
        public bool IsRejected { get; set; }

        public string VideoUrl { get; set; }

        /// <summary>
        /// Retrieves all tags for all pages in the system. This is empty unless filled by the controller.
        /// </summary>
        [XmlIgnore]
		public List<TagViewModel> AllTags { get; set; }

		public PageViewModel()
		{
			_tags = new List<string>();
			IsCacheable = true;
            //RatingStars = "";
			PluginHeadHtml = "";
			PluginFooterHtml = "";
            PluginComments = "";
			PluginPreContainer = "";
			PluginPostContainer = "";
            PluginComments = "";
            AllTags = new List<TagViewModel>();
            AllComments = new List<Comment>();
		}

		public PageViewModel(Page page)
		{
			if (page == null)
				throw new ArgumentNullException("page");

			Id = page.Id;
			Title = page.Title;
			PreviousTitle = page.Title;
			CreatedBy = page.CreatedBy;
			CreatedOn = page.CreatedOn;
            IsLocked = page.IsLocked;
            IsVideo = page.IsVideo;
            IsSubmitted = page.IsSubmitted;
            IsControlled = page.IsControlled;
            IsRejected = page.IsRejected;
            ControlledBy = page.ControlledBy;
			ModifiedOn = page.ModifiedOn;
			RawTags = page.Tags;
		    Summary = page.Summary;
		    VideoUrl = page.VideoUrl;

			CreatedOn = DateTime.SpecifyKind(CreatedOn, DateTimeKind.Utc);
			ModifiedOn = DateTime.SpecifyKind(ModifiedOn, DateTimeKind.Utc);
			AllTags = new List<TagViewModel>();
            //AllComments = page.GetAllComments();

            NbView = page.NbView;
            NbRating = page.NbRating;
            TotalRating = page.TotalRating;
		}
       /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetCommentsHtml()
        {

            StringBuilder builder = new StringBuilder();
            if (AllComments != null)
            {
                if (AllComments.Count == 0)
                {
                    return builder.AppendLine("TODO be the first to comment").ToString();
                }

                builder.AppendLine("<div>");
                foreach (var comment in AllComments)
                {
                    builder.AppendLine("<div class='row'>");
                    {

                        // first the user on the left
                        builder.AppendLine("<div class='col-sm-2'>");
                        {
                            builder.AppendLine("<p>By " + comment.CreatedBy + "</p>");
                            builder.AppendLine("<p><small>On " + comment.CreatedOn + "</small></p>");

                            builder.AppendLine("</div>");
                        }
                        // then the comment text

                        builder.AppendLine("<div class='col-sm-10'>");
                        {
                            builder.AppendLine(EncodePageRating16(comment.Rating, "active "));

                            builder.AppendLine("<p>");
                            builder.AppendLine(comment.Text);
                            builder.AppendLine("</div>");
                        }
                        builder.AppendLine("</div>");

                    }
                    //builder.AppendLine("<hr>");
                }//foreach
                builder.AppendLine("</div>");
            }
            return builder.ToString();
        }

        /// <summary>
                /// 
                /// </summary>
                /// <param name="rating"></param>
                /// <returns></returns>
        public static string EncodePageRating16(int rating, string active)
        {
            StringBuilder builder = new StringBuilder();
            string formatStr = "<span class='rating16 stars16 {2}star16-{0}' value='{1}'></span>";

            for (double i = .5; i <= 5.0; i = i + .5)
            {
                if (i <= (double)rating)
                {
                    builder.AppendFormat(formatStr, (i * 2) % 2 == 1 ? "left_on" : "right_on", i, active);
                }
                else
                {
                    builder.AppendFormat(formatStr, (i * 2) % 2 == 1 ? "left_off" : "right_off", i, active);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public string EncodePageRating32(double rating)
        {
            string passive = "passive ";
            StringBuilder builder = new StringBuilder();
            string formatStr = "<span class='rating32 stars32 {2}star32-{0}' value='{1}'></span>";

            for (double d = .5; d <= 5.0; d = d + .5)
            {
                if (d <= rating)
                {
                    builder.AppendFormat(formatStr, (d * 2) % 2 == 1 ? "left_on" : "right_on", d, passive);
                }
                else
                {
                    builder.AppendFormat(formatStr, (d * 2) % 2 == 1 ? "left_off" : "right_off", d, passive);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// PageViewModel
        /// </summary>
        /// <param name="pageContent"></param>
        /// <param name="converter"></param>
		public PageViewModel(PageContent pageContent, MarkupConverter converter)
		{
			if (pageContent == null)
				throw new ArgumentNullException("pageContent");

			if (pageContent.Page == null)
				throw new ArgumentNullException("pageContent.Page");

			if (converter == null)
				throw new ArgumentNullException("converter");

			Id = pageContent.Page.Id;
			Title = pageContent.Page.Title;
			PreviousTitle = pageContent.Page.Title;
			CreatedBy = pageContent.Page.CreatedBy;
			CreatedOn = pageContent.Page.CreatedOn;
            IsLocked = pageContent.Page.IsLocked;
            IsVideo = pageContent.Page.IsVideo;
            IsRejected = pageContent.Page.IsRejected;
            IsSubmitted = pageContent.Page.IsSubmitted;
            IsControlled = pageContent.Page.IsControlled;
            ControlledBy = pageContent.Page.ControlledBy;
			ModifiedOn = pageContent.Page.ModifiedOn;
			RawTags = pageContent.Page.Tags;
			Content = pageContent.Text;
			VersionNumber = pageContent.VersionNumber;
            PluginComments = ""; //TODO can be remove
            //PluginComments = GetCommentsHtml();
            ControlledBy = pageContent.Page.ControlledBy;
            NbView = pageContent.Page.NbView;
            NbRating = pageContent.Page.NbRating;
            TotalRating = pageContent.Page.TotalRating;
            Summary = pageContent.Page.Summary;
            VideoUrl = pageContent.Page.VideoUrl;

            PageHtml pageHtml = converter.ToHtml(pageContent.Text);
			ContentAsHtml = pageHtml.Html;
			IsCacheable = pageHtml.IsCacheable;
			PluginHeadHtml = pageHtml.HeadHtml;
			PluginFooterHtml = pageHtml.FooterHtml;
			PluginPreContainer = pageHtml.PreContainerHtml;
			PluginPostContainer = pageHtml.PostContainerHtml;

            CreatedOn = DateTime.SpecifyKind(CreatedOn, DateTimeKind.Utc);
			ModifiedOn = DateTime.SpecifyKind(ModifiedOn, DateTimeKind.Utc);
			AllTags = new List<TagViewModel>();
            //RatingStars = EncodePageRating32(4, "active ");
		}

		/// <summary>
		/// Joins the parsed tags with a comma.
		/// </summary>
		public string CommaDelimitedTags()
		{
			return string.Join(",", _tags);
		}

		/// <summary>
		/// Formats the <see cref="AllTags"/> to insert inside an array initializer like [];
		/// </summary>
		public string JavascriptArrayForAllTags()
		{
			IEnumerable<string> allTags = AllTags.OrderBy(x => x.Name).Select(t => t.Name);
			return "\"" +string.Join("\", \"", allTags) + "\"";
		}

		/// <summary>
		/// Joins the tags with a space.
		/// </summary>
		public string SpaceDelimitedTags()
		{
			return string.Join(" ", _tags);
		}

		private void ParseRawTags()
		{
			_tags = ParseTags(_rawTags).ToList();
		}

		/// <summary>
		/// Takes a string of tags: "tagone,tagtwo,tag3 " and returns a list.
		/// </summary>
		/// <param name="tags"></param>
		/// <returns></returns>
		public static IEnumerable<string> ParseTags(string tags)
		{
			List<string> tagList = new List<string>();
			char delimiter = ',';

			if (!string.IsNullOrEmpty(tags))
			{
				// For the legacy tag seperator format
				if (tags.IndexOf(";") != -1)
					delimiter = ';';

				if (tags.IndexOf(delimiter) != -1)
				{
					string[] parts = tags.Split(delimiter);
					foreach (string item in parts)
					{
						if (item != "," && !string.IsNullOrWhiteSpace(item))
							tagList.Add(item.Trim());
					}
				}
				else
				{
					tagList.Add(tags.TrimEnd());
				}
			}

			return tagList;
		}

		/// <summary>
		/// Returns false if the tag contains ; / ? : @ & = { } | \ ^ [ ] `		
		/// </summary>
		/// <param name="tag">The tag</param>
		/// <returns></returns>
		public static ValidationResult VerifyRawTags(PageViewModel pageViewModel, ValidationContext context)
		{
			if (string.IsNullOrEmpty(pageViewModel.RawTags))
				return ValidationResult.Success;

			if (_tagBlackList.Any(x => pageViewModel.RawTags.Contains(x)))
			{
				return new ValidationResult("Invalid characters in tag"); // doesn't need to be localized as there's a javascript check
			}
			else
			{
				return ValidationResult.Success;
			}
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

        public void SetUserActivity(UserActivity activity)
        {
            _userActivity = new UserActivity()
            {
                OldestPageDate = activity.OldestPageDate,
                GlobalRating = activity.GlobalRating,
                NbPublications = activity.NbPublications,
            };
        }

        public UserActivity GetUserActivity()
        {
            return _userActivity;
        }
        public string EncodeRating()
        {
            string active = "active ";
            StringBuilder builder = new StringBuilder();

            string[] titles = { SiteStrings.Rating_level1, SiteStrings.Rating_level2, SiteStrings.Rating_level3, SiteStrings.Rating_level4, SiteStrings.Rating_level5 };

            for (int i = 1; i <= 5; i++)
            {
                string formatStr = "<span class='rating32 stars32 {2}star32-{0}' title='{3}' value='{1}'></span>";
                if (i <= Rating)
                {
                    builder.AppendFormat(formatStr, "left_on", i, active, titles[i - 1]);
                    builder.AppendFormat(formatStr, "right_on", i, active, titles[i - 1]);
                }
                else
                {
                    builder.AppendFormat(formatStr, "left_off", i, active, titles[i - 1]);
                    builder.AppendFormat(formatStr, "right_off", i, active, titles[i - 1]);
                }
            }

            //for (double i = .5; i <= 5.0; i = i + .5)
            //{
            //    int rounded = (int)i;
            //    if (i <= rating)
            //    {
            //        builder.AppendFormat(formatStr, (i * 2) % 2 == 1 ? "left_on" : "right_on", i, active);
            //    }
            //    else
            //    {
            //        builder.AppendFormat(formatStr, (i * 2) % 2 == 1 ? "left_off" : "right_off", i, active);
            //    }
            //}
            //builder.AppendFormat("rating={0}", rating);
            return builder.ToString();
        }
    }
}

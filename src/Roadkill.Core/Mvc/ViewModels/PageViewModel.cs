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
        public DateTime PublishedOn { get; set; }

        /// <summary>
        /// Displays PublishedOn in IS8601 format, plus the timezone offset included for timeago
        /// </summary>
        public string ModifiedOnWithOffset
        {
            get
            {
                // EditedOn (PublishedOn in the domain) is stored in UTC time, so just add a Z to indicate this.
                return string.Format("{0}Z", PublishedOn.ToString("s"));
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
        [Required(ErrorMessageResourceType = typeof(SiteStrings), ErrorMessageResourceName = "Page_Validation_Title")]
        public string Title { get; set; }

        /// <summary>
        /// The page summary.
        /// </summary>
        //public string Summary { get; set; } see content page
        public string ContentSummary { get; set; }

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
        public string FilePath { get; set; }

        /// <summary>
        ///  
        /// </summary>
        //public string CurrentUserComment { get; set; }

        /// <summary>
        ///  Any additional HTML generated by the text plugins that sits before the #container element.
        /// </summary>
        public string PluginPostContainer { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public List<Comment> AllComments { get; set; }

        //public bool IsVideo { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsControlled { get; set; }
        public bool IsRejected { get; set; }
        //public bool IsCopied { get; set; }

        public string VideoUrl { get; set; }
        public string Pseudonym { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPublished
        {
            get
            {
                return IsControlled && !IsRejected;
            }
        }
        
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
            PluginFooterHtml = "TODO_______";
            //CurrentUserComment = "";
            PluginPreContainer = "";
            PluginPostContainer = "";
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
            //IsVideo = page.IsVideo;
            IsSubmitted = page.IsSubmitted;
            IsControlled = page.IsControlled;
            IsRejected = page.IsRejected;
            //IsCopied = page.IsCopied;
            ControlledBy = page.ControlledBy;
            PublishedOn = page.PublishedOn;
            RawTags = page.Tags;
            //Summary = page.Summary;
            VideoUrl = page.VideoUrl;
            Pseudonym = page.Pseudonym;
            FilePath = page.FilePath;

            CreatedOn = DateTime.SpecifyKind(CreatedOn, DateTimeKind.Utc);
            PublishedOn = DateTime.SpecifyKind(PublishedOn, DateTimeKind.Utc);
            AllTags = new List<TagViewModel>();
            //CurrentUserComment = page.model.CurrentUserComment =
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
                builder.AppendLine("<h4>USE ENCODECOMMENTHTML()</h4>");

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string EncodeCommentsHtml()
        {

            StringBuilder builder = new StringBuilder();
            if (AllComments != null && AllComments.Count != 0)
            {
                //builder.AppendLine("<p>&nbsp;</p>");
                builder.AppendLine("<hr size=2 align=left width='100 %'/>");
                builder.AppendLine("<div class='row col-sm-8'>");

                //builder.AppendLine("<div id='container-comments' class='container'>");
                builder.AppendLine("<h4>" + SiteStrings.Page_Comments_Title + "</h4>");

                builder.AppendLine("<table>");
                {
                    //builder.AppendLine("<div>");
                    foreach (var comment in AllComments)
                    {
                        //builder.AppendLine("<hr size=2 align=left width='90%'/>");
                        builder.AppendLine("<tr>");
                        {

                            // first the user on the left
                            builder.AppendLine("<td style='padding:2px; width:80px; border-top-width:1px; border-top-style:solid; border-color:#dddddd; padding-top:10px' align='center' valign='top'>");
                            {
                                string src = "/Assets/Images/letters/letter-" + CreatedBy.First().ToString().ToUpper() + ".jpg";

                                builder.AppendLine("<img src='" + src + "' border='0' alt='loading' style='width:20px;height:20px;'/>");
                                builder.AppendLine("<br />");
                                builder.AppendLine("<small>" + comment.CreatedBy + "</small>");
                                builder.AppendLine("</td>");
                            }

                            // then the comment text
                            builder.AppendLine("<td width='100%' style='padding:2px; border-top-width:1px; border-top-style:solid; border-color:#dddddd; padding-top:10px; padding-left:10px;'>");
                            {
                                if (comment.Rating > 0)
                                {
                                    // another table for alignment
                                    builder.AppendLine("<table><tr><td><span style='display:inline-flex;'>");
                                    builder.AppendLine(EncodeCommentRating(comment.Rating));
                                    builder.AppendLine("</span></td><td>");
                                    builder.AppendLine("<span style='padding-left:10px;color:green;'><small>" + SiteStrings.Comment_Info_PublishedOn + comment.CreatedOn.ToString("dd/MM/yyyy") + "</small></span>");
                                    builder.AppendLine("</td></tr></table>");
                                }
                                else
                                {
                                    builder.AppendLine("<div style='color:green;'><small>" + SiteStrings.Comment_Info_PublishedOn + comment.CreatedOn.ToString("dd/MM/yyyy") + "</small></div>");
                                }
                                builder.AppendLine("<small>" + comment.Text + "</small>");
                                builder.AppendLine("</td>");
                            }
                            builder.AppendLine("</tr>");
                        }
                        //builder.AppendLine("<hr>");
                    }//foreach
                    builder.AppendLine("</table>");
                }
                builder.AppendLine("</div>");
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        public static string EncodeCommentRating(int rating)
        {
            StringBuilder builder = new StringBuilder();
            string active = "passive";
            string formatStr = "<span class='rating16 stars16 {2} star16-{0}' value='{1}'></span>";

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
        /// Global rating for the page. The one at the right of the title
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public string EncodePageAverageRating()
        {
            double rating = .0;
            if (NbRating > 0)
            {
                rating = (double)TotalRating / (double)NbRating;
            }
 
            StringBuilder builder = new StringBuilder();
            builder.Append("<span style='display:inline-flex;'>");
            string formatStr = "<span class='rating-average stars star-{0}' value='{1}'></span>";

            for (double d = .5; d <= 5.0; d = d + .5)
            {
                if (d <= rating)
                {
                    builder.AppendFormat(formatStr, (d * 2) % 2 == 1 ? "left_on" : "right_on", d);
                }
                else
                {
                    builder.AppendFormat(formatStr, (d * 2) % 2 == 1 ? "left_off" : "right_off", d);
                }
            }
            builder.Append("</span>");
            if (NbRating > 0)
            {
                builder.Append("<span style='padding-left:10px;color:green;'>"+ rating.ToString("0.0") + "/5</span>");
            }
            return builder.ToString();
        }

        
        public string GetRatingValue(double rating)
        {
            return String.Format("{0:0.#}", rating);
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
            //IsVideo = pageContent.Page.IsVideo;
            IsRejected = pageContent.Page.IsRejected;
            //IsCopied = pageContent.Page.IsCopied;
            IsSubmitted = pageContent.Page.IsSubmitted;
            IsControlled = pageContent.Page.IsControlled;
            ControlledBy = pageContent.Page.ControlledBy;
            PublishedOn = pageContent.Page.PublishedOn;
            RawTags = pageContent.Page.Tags;
            Content = pageContent.Text;
            VersionNumber = pageContent.VersionNumber;
            //CurrentUserComment = pageContent.Page.cu;
            FilePath = pageContent.Page.FilePath;
            ControlledBy = pageContent.Page.ControlledBy;
            NbView = pageContent.Page.NbView;
            NbRating = pageContent.Page.NbRating;
            TotalRating = pageContent.Page.TotalRating;
            //Summary = pageContent.Page.Summary;
            VideoUrl = pageContent.Page.VideoUrl;
            Pseudonym = pageContent.Page.Pseudonym;

            PageHtml pageHtml = converter.ToHtml(pageContent.Text);
            ContentAsHtml = pageHtml.Html;
            IsCacheable = pageHtml.IsCacheable;
            PluginHeadHtml = pageHtml.HeadHtml;
            PluginFooterHtml = pageHtml.FooterHtml;
            PluginPreContainer = pageHtml.PreContainerHtml;
            PluginPostContainer = pageHtml.PostContainerHtml;

            CreatedOn = DateTime.SpecifyKind(CreatedOn, DateTimeKind.Utc);
            PublishedOn = DateTime.SpecifyKind(PublishedOn, DateTimeKind.Utc);
            AllTags = new List<TagViewModel>();
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
            return "\"" + string.Join("\", \"", allTags) + "\"";
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

        public string EncodeControlRating()
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


        /// <summary>
        /// uses 24 pixels stars
        /// </summary>
        /// <returns></returns>
        public string EncodeUserRating()
        {
            string active = "active ";
            StringBuilder builder = new StringBuilder();

            string[] titles = { SiteStrings.Rating_level1, SiteStrings.Rating_level2, SiteStrings.Rating_level3, SiteStrings.Rating_level4, SiteStrings.Rating_level5 };


            // The state of stars is initiate when page is ready, in setRating__()
            for (int i = 1; i <= 5; i++)
            {
                string formatStr = "<span class='rating stars {2}star-{0}' title='{3}' value='{1}'></span>";
                builder.AppendFormat(formatStr, "left_off", i, active, titles[i - 1]);
                builder.AppendFormat(formatStr, "right_off", i, active, titles[i - 1]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// EncodeRateInfo
        /// </summary>
        /// <returns></returns>
        public string EncodeRateInfo(int userRating)
        {
            // value for rating 0 is '.' to avoid height change
            string[] titles = { "&nbsp;", SiteStrings.Rating_level1, SiteStrings.Rating_level2, SiteStrings.Rating_level3, SiteStrings.Rating_level4, SiteStrings.Rating_level5 };
            return titles[userRating];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPortraitImage()
        {
            string first = CreatedBy.First().ToString().ToUpper();
            return "~/Assets/Images/letters/letter-" + first + ".jpg";
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDraft
        {
            get
            {
                if (!IsSubmitted && !IsControlled && !IsRejected)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EditBtnDisable
        {
            get
            {
                if (IsDraft)
                {
                    return "";
                }

                return "disabled";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DraftBtnDisable
        {
            get
            {
                if (!IsDraft)
                {
                    return "confirm";
                }

                return "disabled";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SubmitBtnDisable
        {
            get
            {
                if (IsDraft)
                {
                    return "confirm";
                }

                return "disabled";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EditBtnHidden
        {
            get
            {
                if (IsDraft)
                {
                    return ""; // means display
                }

                return "display:none;";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DraftBtnHidden
        {
            get
            {
                if (IsDraft)
                {
                    return "display:none;";
                    
                }
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SubmitBtnHidden
        {
            get
            {
                if (IsDraft)
                {
                    return ""; // means display
                }

                return "display:none;";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            get
            {
                if(IsPublished)
                {
                    return @SiteStrings.Page_Info_Published; 
                }
                if (IsSubmitted)
                {
                    return @SiteStrings.Page_Info_Submitted;
                }
                if (IsRejected)
                {
                    return @SiteStrings.Page_Info_Rejected;
                }
                return @SiteStrings.Page_Info_Draft;
            }
        }
    }
}

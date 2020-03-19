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
using System.Web.Mvc;

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
        /// The controller rating of the page.
        /// </summary>
        public long ControllerRating { get; set; }

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
        //public string RejectReason { get; set; }

        public IEnumerable<string> RejectReasonAvailable
        {
            get
            {
                return new string[] { "Unpolite", "TBC1", "TBC2" }; //TODO
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

        /// <summary>
        /// true if the page is participating to the current competition
        /// </summary>
        public bool IsInCompetition { get; set; }

        /// <summary>
        /// id of the competition if the page is participating or has participated to a competition
        /// </summary>
        public int CompetitionId { get; set; }

        /// <summary>
        /// false if page involved in the current competition and competition on rating or achieved
        /// </summary>
        public bool ModificationsEnable { get; set; }

        /// <summary>
        /// Message to display if the pge is in a competition
        /// </summary>
        public string CompetitionInfo { get; set; }

        /// <summary>
        /// Not used
        /// </summary>
        public string VideoUrl { get; set; }

        // Not used
        public string Pseudonym { get; set; }

        /// <summary>
        /// true if the page has been controlled
        /// </summary>
        public bool IsPublished
        {
            get
            {
                return IsControlled && !IsRejected;
            }
        }

        /// <summary>
        /// Ranking of the page if participated to a competition
        /// </summary>
        public int Ranking { get; set; }

        /// <summary>
        /// Hits of a user, 
        /// Array of 3 values: number of gold medals, then silver, then bronze
        /// </summary>
        public int[] UserHits;
        
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
            CompetitionId = -1;
            IsInCompetition = false;
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
            IsSubmitted = page.IsSubmitted;
            IsControlled = page.IsControlled;
            IsRejected = page.IsRejected;
            ControlledBy = page.ControlledBy;
            PublishedOn = page.PublishedOn;
            RawTags = page.Tags;
            ControllerRating = page.ControllerRating;
            VideoUrl = page.VideoUrl;
            Pseudonym = page.Pseudonym;
            FilePath = page.FilePath;

            CreatedOn = DateTime.SpecifyKind(CreatedOn, DateTimeKind.Utc);
            PublishedOn = DateTime.SpecifyKind(PublishedOn, DateTimeKind.Utc);
            AllTags = new List<TagViewModel>();

            NbView = page.NbView;
            NbRating = page.NbRating;
            TotalRating = page.TotalRating;
            CompetitionId = page.CompetitionId;
            IsInCompetition = CompetitionId != -1 ? true : false;
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
            IsRejected = pageContent.Page.IsRejected;
            IsSubmitted = pageContent.Page.IsSubmitted;
            IsControlled = pageContent.Page.IsControlled;
            ControlledBy = pageContent.Page.ControlledBy;
            PublishedOn = pageContent.Page.PublishedOn;
            RawTags = pageContent.Page.Tags;
            Content = pageContent.Text;
            VersionNumber = pageContent.VersionNumber;
            FilePath = pageContent.Page.FilePath;
            ControlledBy = pageContent.Page.ControlledBy;
            NbView = pageContent.Page.NbView;
            NbRating = pageContent.Page.NbRating;
            TotalRating = pageContent.Page.TotalRating;
            ControllerRating = pageContent.Page.ControllerRating;
            CompetitionId = pageContent.Page.CompetitionId;
            IsInCompetition = CompetitionId != -1 ? true : false;
            VideoUrl = pageContent.Page.VideoUrl;
            Pseudonym = pageContent.Page.Pseudonym;

            ContentSummary = GetContentSummary(converter);
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
                builder.AppendLine("<hr size=4 align=left width='100 %' style='border-top-color:#4f8bdf;'/>");
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
        /// 
        /// </summary>
        /// <param name="ranking"></param>
        /// <returns></returns>
        public static string EncodeImageRanking(int ranking)
        {
            StringBuilder builder = new StringBuilder();
            switch (ranking)
            {
                case 1:
                    builder.Append("<img src='/Assets/Images/2_gold.png' style='float:left;width:32px;height:32px;margin-right:10px;'/>");
                    break;
                case 2:
                    builder.Append("<img src='/Assets/Images/2_silver.png' style='float:left;width:32px;height:32px;margin-right:10px;'/>");
                    break;
                case 3:
                    builder.Append("<img src='/Assets/Images/2_bronze.png' style='float:left;width:32px;height:32px;margin-right:10px;'/>");
                    break;
                default:
                    return "";
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hits"></param>
        /// <returns></returns>
        public static string EncodeUserHits(int[] hits)
        {
            StringBuilder builder = new StringBuilder();
            if (hits != null && hits.Length == 3)
            {
                // gold
                builder.Append(BuildHits("2_gold", hits[0]));

                // silver
                builder.Append(BuildHits("2_silver", hits[1]));

                // bronze
                builder.Append(BuildHits("2_bronze", hits[2]));

            }

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private static StringBuilder BuildHits(string image, int number)
        {
            StringBuilder result = new StringBuilder();
            string hitFormat = ("<img src='/Assets/Images/{0}.png' style='width:24px;height:24px;margin-right:4px;'/>");
            // gold
            if (number > 0)
            {
                for (int i = 0; i < number; i++ )
                {
                    result.AppendFormat(hitFormat, image);
                }
            }
            return result;
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
                builder.Append("<span style='padding-left:10px;color:green;'> ("+ NbRating + ")</span>");
            }
            return builder.ToString();
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

        /// <summary>
        /// 
        /// </summary>
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
        /// <param name="model"></param>
        /// <param name="markupConverter"></param>
        /// <returns></returns>
        //public static string GetContentSummary(PageViewModel model, MarkupConverter markupConverter)
        //{
        //    // Turn the contents into HTML, then strip the tags for the mini summary. This needs some works
        //    string modelHtml = model.Content;
        //    Regex _removeTagsRegex = new Regex("<(.|\n)*?>");
        //    modelHtml = markupConverter.ToHtml(modelHtml);
        //    modelHtml = _removeTagsRegex.Replace(modelHtml, "");

        //    if (modelHtml.Length > 150)
        //        modelHtml = modelHtml.Substring(0, 149);

        //    if (model.Content.Contains("youtube") && modelHtml.Length <= 3) // 2 is for "\n"
        //    {
        //        modelHtml = "Vidéo. " + modelHtml; //TODO english traduction
        //    }
        //    return modelHtml;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="markupConverter"></param>
        /// <returns></returns>
        public string GetContentSummary(MarkupConverter markupConverter)
        {
            // Turn the contents into HTML, then strip the tags for the mini summary. This needs some works
            string modelHtml = Content;
            Regex _removeTagsRegex = new Regex("<(.|\n)*?>");
            modelHtml = markupConverter.ToHtml(modelHtml);
            modelHtml = ReplaceImgInHtml(modelHtml);
            modelHtml = ReplaceYoutubeInHtml(modelHtml);
            modelHtml = ReplacePdfInHtml(modelHtml);

            //TODO find youtube and image 
            modelHtml = _removeTagsRegex.Replace(modelHtml, "");

            if (modelHtml.Length > 150)
                modelHtml = modelHtml.Substring(0, 149);

            return modelHtml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string ReplaceImgInHtml( string html)
        {
            bool found;
            string newHtml = html;
            do
            {
                found = false;
                // find image tag
                int index1 = newHtml.IndexOf("<img src=");
                if (index1 >= 0)
                {
                    int index2 = newHtml.IndexOf("\">", index1);
                    if (index2 > index1)
                    {
                        found = true;
                        string sub1 = newHtml.Substring(0, index1);
                        string sub2 = newHtml.Substring(index2 + 2);
                        newHtml = sub1 + "[image] " + sub2;
                    }
                }
            } while (found);

            return newHtml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string ReplaceYoutubeInHtml(string html)
        {
            bool found;
            string newHtml = html;
            do
            {
                found = false;
                // find youtube tag
                int index1 = newHtml.IndexOf("<code>youtube");
                if (index1 >= 0)
                {
                    int index2 = newHtml.IndexOf("</code>", index1);
                    if (index2 > index1)
                    {
                        found = true;
                        string sub1 = newHtml.Substring(0, index1);
                        string sub2 = newHtml.Substring(index2 + 7);
                        newHtml = sub1 + "[video]" + sub2;
                    }
                }

            } while (found);

            return newHtml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string ReplacePdfInHtml(string html)
        {
            bool found;
            string newHtml = html;
            do
            {
                found = false;
                // find youtube tag
                int index1 = newHtml.IndexOf("<code>pdf");
                if (index1 >= 0)
                {
                    int index2 = newHtml.IndexOf("</code>", index1);
                    if (index2 > index1)
                    {
                        found = true;
                        string sub1 = newHtml.Substring(0, index1);
                        string sub2 = newHtml.Substring(index2 + 7);
                        newHtml = sub1 + "[pdf]" + sub2;
                    }
                }

            } while (found);

            return newHtml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string EncodePageRating(PageViewModel model)
        {
            double rating;
            if (model.NbRating == 0)
            {
                rating = .0;
            }
            else
            {
                //long nbController = model.TotalRating / 3;
                rating = ((double)model.TotalRating / (double)model.NbRating); //TODO take into account Controller rating
            }

            return EncodePageRating(rating);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        public static string EncodePageRating(double rating)
        {
            string active = "passive";
            StringBuilder builder = new StringBuilder();
            string formatStr = "<span class='rating16 stars16 {2} star16-{0}' value='{1}'></span>";

            for (double i = .5; i <= 5.0; i = i + .5)
            {
                if (i <= rating)
                {
                    builder.AppendFormat(formatStr, (i * 2) % 2 == 1 ? "left_on" : "right_on", i, active);
                }
                else
                {
                    builder.AppendFormat(formatStr, (i * 2) % 2 == 1 ? "left_off" : "right_off", i, active);
                }
            }
            //builder.AppendFormat("rating={0}", rating);
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string EncodeTags(PageViewModel page)
        {
            StringBuilder builder = new StringBuilder();
            string formatStr = "<span class='searchresult-tags'>{0}&nbsp;</span>";
            bool atLeastOne = false;
            foreach (string tag in page.Tags)
            {
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    builder.AppendFormat(formatStr, tag);
                }
                atLeastOne = true;
            }

            // add carriage return if necessary
            if (atLeastOne)
            {
                builder.Append("<br />");
            }
            return builder.ToString();
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
        public string Status
        {
            get
            {
                if(IsPublished)
                {
                    return "<span style='color:green;'>" + @SiteStrings.Page_Info_Published + "</span>";
                }
                if (IsSubmitted)
                {
                    return @SiteStrings.Page_Info_Submitted;
                }
                if (IsRejected)
                {
                    return "<span style='color:red;'>" + @SiteStrings.Page_Info_Rejected + "</span>";
                }
                return @SiteStrings.Page_Info_Draft;
            }
        }
    }
}

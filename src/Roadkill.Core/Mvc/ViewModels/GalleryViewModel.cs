using System;
using System.Collections.Generic;
using System.Text;

using Roadkill.Core.Database;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Mvc.ViewModels
{
    /// <summary>
    /// Provides summary data for a page.
    /// </summary>
    public class GalleryViewModel
    {
        public List<PageViewModel> listMostRecent;
        //List<Thumbnail> listRecommended;
        public List<PageViewModel> listBestRated;
        //public List<PageViewModel> listMostViewed;
        MarkupConverter markupConverter;

        public GalleryViewModel(MarkupConverter converter)
        {
            listMostRecent = new List<PageViewModel>();
            //listRecommended = new List<Thumbnail>();
            listBestRated = new List<PageViewModel>();
            //listMostViewed = new List<PageViewModel>();
            markupConverter = converter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetCanvas(PageViewModel model)
        {
            return model.FilePath + "page_" + model.Id + ".png";
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="page"></param>
        ///// <param name="b"></param>
        ///// <returns></returns>
        //public string EncodeThumbNail(Page page, int b) //TODO remove b
        //{
        //    //image = b == 0 ? "/Assets/Images/RaspberryPiBoard.png" : "/Assets/Images/spectrogram_zoomed.png";
        //    StringBuilder builder = new StringBuilder();
        //    string canvas = page.FilePath + "page_" + page.Id + ".png";

        //    builder.AppendLine("<table style='width:100%'>");
        //    {
        //        builder.AppendLine("<tr>");

        //        builder.AppendLine("<td class='thumb-crop'>");
        //        {
        //            builder.AppendLine(string.Format("<a href='/wiki/{0}'>", page.Id));
        //            {
        //                builder.AppendLine(string.Format("<img class='img-responsive' src='{0}' alt='Lights'>", canvas));
        //                builder.AppendLine("</a>");
        //            }
        //            builder.AppendLine("</td>");
        //        }
        //        builder.AppendLine("<td>");
        //        {
        //            builder.AppendLine(string.Format("<p class='block-with-text'>{0}<br>", page.Title)); //TODO 1 line only
        //            builder.AppendLine(string.Format("<p class='block-with-text'><small>{0}</small><br></p>", page.Summary));
        //            builder.AppendLine(string.Format("<p><small>{1}  Views: {0}</small></p>", page.NbView, EncodePageRating(page)));
        //            builder.AppendLine("</td");
        //        }
        //        builder.AppendLine("</tr>");
        //        builder.AppendLine("</table>");
        //    }
        //    return builder.ToString();
        //}
        
        //public string EncodeThumbNail_with_rows(Page page, int b) //TODO remove b
        //{

        //    //image = b == 0 ? "/Assets/Images/RaspberryPiBoard.png" : "/Assets/Images/spectrogram_zoomed.png";
        //    StringBuilder builder = new StringBuilder();
        //    string canvas = page.FilePath + "page_" + page.Id + ".png";

        //    builder.AppendLine("<div class='row' style='margin-left:0'>");
        //    {
        //        builder.AppendLine("<div class='col-xs-4 thumb-crop'  style='padding-left:0'>");
        //        {
        //            builder.AppendLine(string.Format("<a href='/wiki/{0}'>", page.Id));
        //            {
        //                builder.AppendLine(string.Format("<img class='img-responsive' src='{0}' alt='Lights'>", canvas));
        //                builder.AppendLine("</a>");
        //            }
        //            builder.AppendLine("</div>");
        //        }
        //        builder.AppendLine("<div class='col-xs-8'  style='padding-left:0'>");
        //        {
        //            builder.AppendLine(string.Format("<p class='block-with-text'>{0}<br>", page.Title)); //TODO 1 line only
        //            builder.AppendLine(string.Format("<p class='block-with-text'><small>{0}</small><br></p>", page.Summary));
        //            builder.AppendLine(string.Format("<p><small>{1}  Views: {0}</small></p>", page.NbView, EncodePageRating(page)));
        //            builder.AppendLine("</div>");
        //        }
        //        builder.AppendLine("</div>");
        //    }
        //    return builder.ToString();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public string EncodePageRating(PageViewModel model)
        {
            string active = "passive";
            StringBuilder builder = new StringBuilder();
            string formatStr = "<span class='rating16 stars16 {2} star16-{0}' value='{1}'></span>";

            double rating;
            if (model.NbRating == 0)
            {
                rating = .0;
            }
            else
            {
                long nbController = model.TotalRating / 3;
                rating = (model.TotalRating / model.NbRating); //TODO take into account Controller rating
            }
            //rating = Math.Round(rating, 1);

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
        /// <returns></returns>
        public string EncodeGallery()
        {
            StringBuilder builder = new StringBuilder();

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public string EncodeListPages(List<PageViewModel> models)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<table>");
            {
                foreach (PageViewModel model in models) /* TODO Min or Max */
                {
                    builder.AppendLine("<tr>");
                    {
                        builder.AppendLine("<td class='searchresult-td'>");
                        {
                            builder.AppendLine("<div class='canvas-td'>");
                            builder.AppendLine("<img src='" + GetCanvas(model) + "'>");
                            builder.AppendLine("</div>");
                            builder.AppendLine("</td>");
                        }
                        builder.AppendLine("<td class='searchresult-td'>");
                        {
                            builder.AppendLine("<span class='searchresult-title'><a href='/Wiki/" + model.Id + "/"+ model.Title + "'>"+model.Title+"</a></span><br />");
                            builder.AppendLine("<span class='searchresult-summary'>"+ GetContentSummary( model) + "...</span><br/>");
                            builder.AppendLine("<span>");
                            {
                                builder.AppendLine("<span class='searchresult-date'>" + model.CreatedBy + " - " + model.PublishedOn.ToString("dd MMM yyyy") + " - " + model.NbView + " views</span>");
                                builder.AppendLine("<span style='display: inline-block;margin-bottom:-2px;'>" + EncodePageRating(model) + "</span>");
                                builder.AppendLine("</span>");
                            }
                            builder.AppendLine("</td>");
                        }
                        builder.AppendLine("</tr>");
                    }
                }
                builder.AppendLine("</table>");
            }

            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetContentSummary(PageViewModel model)
        {
            // Turn the contents into HTML, then strip the tags for the mini summary. This needs some works
            string modelHtml = model.Content;
            Regex _removeTagsRegex = new Regex("<(.|\n)*?>");
            modelHtml = markupConverter.ToHtml(modelHtml);
            modelHtml = _removeTagsRegex.Replace(modelHtml, "");

            if (modelHtml.Length > 150)
                modelHtml = modelHtml.Substring(0, 149);

            return modelHtml;
        }
    }
}
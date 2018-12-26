using System;
using System.Collections.Generic;
using System.Text;

using Roadkill.Core.Database;
using System.Web.Mvc;

namespace Roadkill.Core.Mvc.ViewModels
{
    /// <summary>
    /// Provides summary data for a page.
    /// </summary>
    public class GalleryViewModel
    {
        public List<Page> listMostRecent;
        //List<Thumbnail> listRecommended;
        public List<Page> listBestRated;
        public List<Page> listMostViewed;

        public GalleryViewModel()
        {
            listMostRecent = new List<Page>();
            //listRecommended = new List<Thumbnail>();
            listBestRated = new List<Page>();
            listMostViewed = new List<Page>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetCanvas(Page page)
        {
            return page.FilePath + "page_" + page.Id + ".png";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public string EncodeThumbNail(Page page, int b) //TODO remove b
        {
            //image = b == 0 ? "/Assets/Images/RaspberryPiBoard.png" : "/Assets/Images/spectrogram_zoomed.png";
            StringBuilder builder = new StringBuilder();
            string canvas = page.FilePath + "page_" + page.Id + ".png";

            builder.AppendLine("<table style='width:100%'>");
            {
                builder.AppendLine("<tr>");

                builder.AppendLine("<td class='thumb-crop'>");
                {
                    builder.AppendLine(string.Format("<a href='/wiki/{0}'>", page.Id));
                    {
                        builder.AppendLine(string.Format("<img class='img-responsive' src='{0}' alt='Lights'>", canvas));
                        builder.AppendLine("</a>");
                    }
                    builder.AppendLine("</td>");
                }
                builder.AppendLine("<td>");
                {
                    builder.AppendLine(string.Format("<p class='block-with-text'>{0}<br>", page.Title)); //TODO 1 line only
                    builder.AppendLine(string.Format("<p class='block-with-text'><small>{0}</small><br></p>", page.Summary));
                    builder.AppendLine(string.Format("<p><small>{1}  Views: {0}</small></p>", page.NbView, EncodePageRating(page)));
                    builder.AppendLine("</td");
                }
                builder.AppendLine("</tr>");
                builder.AppendLine("</table>");
            }
            return builder.ToString();
        }
        
        public string EncodeThumbNail_with_rows(Page page, int b) //TODO remove b
        {

            //image = b == 0 ? "/Assets/Images/RaspberryPiBoard.png" : "/Assets/Images/spectrogram_zoomed.png";
            StringBuilder builder = new StringBuilder();
            string canvas = page.FilePath + "page_" + page.Id + ".png";

            builder.AppendLine("<div class='row' style='margin-left:0'>");
            {
                builder.AppendLine("<div class='col-xs-4 thumb-crop'  style='padding-left:0'>");
                {
                    builder.AppendLine(string.Format("<a href='/wiki/{0}'>", page.Id));
                    {
                        builder.AppendLine(string.Format("<img class='img-responsive' src='{0}' alt='Lights'>", canvas));
                        builder.AppendLine("</a>");
                    }
                    builder.AppendLine("</div>");
                }
                builder.AppendLine("<div class='col-xs-8'  style='padding-left:0'>");
                {
                    builder.AppendLine(string.Format("<p class='block-with-text'>{0}<br>", page.Title)); //TODO 1 line only
                    builder.AppendLine(string.Format("<p class='block-with-text'><small>{0}</small><br></p>", page.Summary));
                    builder.AppendLine(string.Format("<p><small>{1}  Views: {0}</small></p>", page.NbView, EncodePageRating(page)));
                    builder.AppendLine("</div>");
                }
                builder.AppendLine("</div>");
            }
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public string EncodePageRating(Page page)
        {
            string active = "passive";
            StringBuilder builder = new StringBuilder();
            string formatStr = "<span class='rating16 stars16 {2} star16-{0}' value='{1}'></span>";

            double rating;
            if (page.TotalRating == 0)
            {
                rating = page.ControllerRating;
            }
            else
            {
                long nbController = page.TotalRating / 3;
                rating = (page.TotalRating / page.NbRating); //TODO take into account Controller rating
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
        public string EncodeListPages(string title,List<Page> pages)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<h4>"+ title + "</h4>");
            builder.AppendLine("<table>");
            {
                foreach (Page page in pages) /* TODO Min or Max */
                {
                    builder.AppendLine("<tr>");
                    {
                        builder.AppendLine("<td style='padding:2px; width:80px; border-bottom-width:1px; border-bottom-style:solid; color:#888888;'>");
                        {
                            builder.AppendLine("<img src='" + GetCanvas(page) + "' style='float: left;'>");
                            builder.AppendLine("</td>");
                        }
                        builder.AppendLine("<td style='padding:2px; border-bottom-width:1px; border-bottom-style:solid; color:#888888;'>");
                        {
                            builder.AppendLine("<span class='searchresult-title'><a href='/Wiki/" + page.Id + "/"+ page.Title + "'>"+page.Title+"</a></span><br />");
                            builder.AppendLine("<span class='searchresult-summary'>"+page.Summary+"...</span><br/>");
                            builder.AppendLine("<span>");
                            {
                                builder.AppendLine("<span class='searchresult-date'>" + page.CreatedBy + " - " + page.PublishedOn.ToString("dd MMM yyyy") + " - " + page.NbView + " views</span>");
                                builder.AppendLine("<span style='display: inline-block;margin-bottom:-2px;'>" + EncodePageRating(page) + "</span>");
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
    }
}
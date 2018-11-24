using System;
using System.Collections.Generic;
using System.Text;

using Roadkill.Core.Database;

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


        public string EncodeThumbNail(Page page, int b) //TODO remove b
        {
            string image;
            
            image = b == 0 ? "/Assets/Images/RaspberryPiBoard.png" : "/Assets/Images/spectrogram_zoomed.png";

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<div class='thumbnail row' style='margin-left:0'>");
            {

                //builder.AppendLine("<div>");
                //{
                //    builder.AppendLine(string.Format("<a href='/wiki/{0}'>", page.Id));
                //    {
                //        builder.AppendLine(string.Format("<img class='img-responsive' src='{0}' alt='Lights' style='float:left;width:112px;height:42px;'>", image));

                //        builder.AppendLine(string.Format("<p><strong>{0}</strong></p>", page.Title));
                //        builder.AppendLine(string.Format("<p>{0}</p>", page.Summary));
                //        builder.AppendLine(string.Format("<p>{1}  Views: {0}</p>", page.NbView, EncodePageRating(rating, "active")));
                //        builder.AppendLine("</a>");
                //    }
                //    builder.AppendLine("</div>");
                //}

                builder.AppendLine("<div class='col-xs-4 caption'  style='padding-left:0'>");
                {
                    builder.AppendLine(string.Format("<a href='/wiki/{0}'>", page.Id));
                    {
                        builder.AppendLine(string.Format("<img class='img-responsive' src='{0}' alt='Lights' height='120'>", image));
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

        private static string EncodePageRating(Page page)
        {
            string active = "active ";
            StringBuilder builder = new StringBuilder();
            string formatStr = "<span class='rating stars {2}star-{0}' value='{1}'></span>";

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
       
        public string EncodeRating(float rating)
        {
            return "";
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Text;

using Roadkill.Core.Database;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Roadkill.Core.Converters;
using Roadkill.Core.Localization;

namespace Roadkill.Core.Mvc.ViewModels
{
    /// <summary>
    /// Provides summary data for a page.
    /// </summary>
    public class GalleryViewModel
    {
        public List<PageViewModel> listPages;
        MarkupConverter markupConverter;
        public string Title;

        public GalleryViewModel(MarkupConverter converter)
        {
            listPages = new List<PageViewModel>();
            markupConverter = converter;
        }

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
        //public string EncodeListPages(List<PageViewModel> models)
        //{
        //    StringBuilder builder = new StringBuilder();

        //    builder.AppendLine("<table>");
        //    {
        //        foreach (PageViewModel model in models) /* TODO Min or Max */
        //        {
        //            builder.AppendLine("<tr>");
        //            {
        //                builder.AppendLine("<td class='searchresult-td' valign='top'>");
        //                {
        //                }
        //                builder.AppendLine("<td class='searchresult-td'>");
        //                {
        //                    builder.AppendLine("<span class='searchresult-title'><a href='/Wiki/" + model.Id + "/"+ model.Title + "'>"+model.Title+"</a></span><br />");
        //                    builder.AppendLine("<span class='searchresult-summary'>"+ GetContentSummary( model) + "...</span><br/>");
        //                    builder.AppendLine("<span>");
        //                    {
        //                        builder.AppendLine("<span class='searchresult-date'>" + model.CreatedBy + " - " + model.PublishedOn.ToString("dd MMM yyyy") + " - " + model.NbView + " "+ @SiteStrings.Page_Info_NbView + "</span>");
        //                        builder.AppendLine("<span style='display: inline-block;margin-bottom:-2px;'>" + EncodePageRating(model) + "</span>");
        //                        builder.AppendLine("</span>");
        //                    }
        //                    builder.AppendLine("</td>");
        //                }
        //                builder.AppendLine("</tr>");
        //            }
        //        }
        //        builder.AppendLine("</table>");
        //    }

        //    return builder.ToString();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetContentSummary(PageViewModel model)
        {
            // Turn the contents into HTML, then strip the tags for the mini summary. This needs some works
            string modelHtml = model.Content;
            Regex _removeTagsRegex = new Regex("<(.|\n)*?>");
            modelHtml = markupConverter.ToHtml(modelHtml);
            modelHtml = _removeTagsRegex.Replace(modelHtml, "");

            if (modelHtml.Length > 150)
                modelHtml = modelHtml.Substring(0, 149);

            if (model.Content.Contains("youtube") && modelHtml.Length <= 3) // 2 is for "\n"
            {
                modelHtml = "Vidéo. " + modelHtml; //TODO english traduction
            }
            return modelHtml;
        }

        public string EncodeTags(PageViewModel page)
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
    }
}
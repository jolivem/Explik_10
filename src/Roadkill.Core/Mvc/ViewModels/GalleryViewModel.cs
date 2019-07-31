using System;
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
        //MarkupConverter markupConverter;
        public string Title;

        public GalleryViewModel(MarkupConverter converter)
        {
            listPages = new List<PageViewModel>();
            //markupConverter = converter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public string GetContentSummary(PageViewModel model)
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
    }
}
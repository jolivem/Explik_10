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
    }
}
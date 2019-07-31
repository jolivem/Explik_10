
using Roadkill.Core.Converters;
using Roadkill.Core.Database;
using System.Text.RegularExpressions;

namespace Roadkill.Core.Mvc.ViewModels
{
    /// <summary>
    /// Provides summary data for a page.
    /// </summary>
    public class PageAndUserRatingViewModel : PageViewModel
    {

        // the rating of the current user
        public int UserRating;
        public PageAndUserRatingViewModel( PageContent content, MarkupConverter converter, int rating) : base(content, converter)
        {
            UserRating = rating;
        }

    }
}
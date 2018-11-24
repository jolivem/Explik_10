using System;
using System.Text;

using Roadkill.Core.Localization;

namespace Roadkill.Core.Mvc.ViewModels
{
    public class CommentViewModel
    {
        public string Text;
        public int Rating;
        public int PageId;
        public string PageTitle;
        public string CreatedBy;
        public DateTime CreatedOn;

        public string EncodeRating()
        {
            string active = "active ";
            StringBuilder builder = new StringBuilder();

            string[] titles = { SiteStrings.Rating_level1, SiteStrings.Rating_level2, SiteStrings.Rating_level3, SiteStrings.Rating_level4, SiteStrings.Rating_level5 };

            for (int i = 1; i <= 5; i ++)
            {
                string formatStr = "<span class='rating32 stars32 {2}star32-{0}' title='{3}' value='{1}'></span>";
                if (i <= Rating)
                {
                    builder.AppendFormat(formatStr, "left_on", i, active, titles[i-1]);
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
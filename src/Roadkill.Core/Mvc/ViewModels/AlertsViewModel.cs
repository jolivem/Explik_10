using Roadkill.Core.Database;
using System;
using System.Collections.Generic;

namespace Roadkill.Core.Mvc.ViewModels
{
    public class AlertsViewModel
    {
        public Dictionary<int, PageAlertsInfo> pageList;
        //public Dictionary<int, Info> commentList;

        /// <summary>
        /// 
        /// </summary>
        public AlertsViewModel()
        {
            pageList = new Dictionary<int, PageAlertsInfo>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="alerts"></param>
        //internal void Fill(IEnumerable<Alert> alerts)
        //{
        //    foreach(Alert alert in alerts)
        //    {
        //        // if it is a comment
        //        if (alert.CommentId != Guid.Empty)
        //        {

        //        }
        //        else // it is a page
        //        {
        //            if (pageList.ContainsKey(alert.PageId))
        //            {
        //                pageList[alert.PageId] = new Info( pageList[alert.PageId] + ", " + alert.Ilk;
        //            }
        //            else
        //            {
        //                pageList.Add(alert.PageId, alert.Ilk);
        //            }
        //        }
        //    }
        //}

        internal void Add(PageAlertsInfo info)
        {
            if (pageList.ContainsKey(info.PageId))
            {
                // add the ilk
                pageList[info.PageId].Ilks = pageList[info.PageId].Ilks + ", " + info.Ilks;
            }
            else
            {
                pageList.Add(info.PageId, info);
            }
        }
    }
}

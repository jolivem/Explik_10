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

        internal void Add(PageAlertsInfo info)
        {
            if (pageList.ContainsKey(info.PageId))
            {
                // add the ilk
                pageList[info.PageId].Number++;
            }
            else
            {
                pageList.Add(info.PageId, info);
                pageList[info.PageId].Number=1;
            }
        }
    }
}

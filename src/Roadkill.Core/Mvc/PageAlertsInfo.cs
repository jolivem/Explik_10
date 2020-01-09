
namespace Roadkill.Core.Mvc
{
    public class PageAlertsInfo
    {
        public int PageId;
        public string PageTitle;
        public int Number;
        public PageAlertsInfo(int pageId, string pageTitle)
        {
            PageTitle = pageTitle;
            PageId = pageId;
            Number = 1;
        }
        //public void AddIlk( string ilk)
        //{
        //    Ilks = Ilks + ", " + ilk;
        //}
    }
}


namespace Roadkill.Core.Mvc
{
    public class PageAlertsInfo
    {
        public int PageId;
        public string PageTitle;
        public string Ilks;
        public PageAlertsInfo(int pageId, string ilk, string pageTitle)
        {
            PageTitle = pageTitle;
            PageId = pageId;
            Ilks = ilk;
        }
        public void AddIlk( string ilk)
        {
            Ilks = Ilks + ", " + ilk;
        }
    }
}

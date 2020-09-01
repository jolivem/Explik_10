using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Email
{
    public class PageEmailInfo
    {
        public User User;
        public PageViewModel Page;

        public PageEmailInfo(User user, PageViewModel page)
        {
            User = user;
            //RejectType = rejectType;
            Page = page;
        }
    }
}

using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Email
{
    public class PageEmailInfo
    {
        public User User;
        public string RejectType; // key word
        public string RejectReason; // clear text
        public PageViewModel Page;

        public PageEmailInfo(User user, PageViewModel page, string rejectType)
        {
            User = user;
            RejectType = rejectType;
            Page = page;
        }
    }
}

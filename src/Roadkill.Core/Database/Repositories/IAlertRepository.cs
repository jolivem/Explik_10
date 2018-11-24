using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Database
{
    public interface IAlertRepository
    {
        void DeleteAlert(Guid alertId);
        void DeletPageAlerts(int pageId);
        void DeletCommentAlerts(Guid commentId);
        IEnumerable<Alert> FindAlertsByPage(int pageId);
        IEnumerable<Alert> FindAlertsByComment(Guid commentGuid);
        void AddAlert(Alert alert);
    }
}

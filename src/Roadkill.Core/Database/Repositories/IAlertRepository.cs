using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Database
{
    public interface IAlertRepository
    {
        void DeleteAlert(Guid alertId);
        IEnumerable<Alert> FindAllAlertByPage(int pageId);
        IEnumerable<Alert> FindAllAlertByComment(Guid guid);
        void AddAlert(Alert alert);
    }
}

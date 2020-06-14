using System;
using System.Collections.Generic;
using System.Linq;


namespace Roadkill.Core.Database
{
    public interface IAlertRepository
    {
        void DeleteAlert(int alertId);
        void DeletePageAlerts(int pageId);
        IEnumerable<Alert> FindAlertsByPage(int pageId);
        Alert FindAlertByPageAndUser(int pageId, string usernamme);
        void DeletPageAlertsByUser(int pageId, string username);
        void AddAlert(Alert alert);
        IEnumerable<Alert> GetAlerts();
    }
}

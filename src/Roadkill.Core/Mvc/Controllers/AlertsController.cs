using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Roadkill.Core.Configuration;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Database;

namespace Roadkill.Core.Mvc.Controllers
{
    /// <summary>
    /// Provides all page related functionality, including editing and viewing pages.
    /// </summary>
    [HandleError]
    [OptionalAuthorization]
    public class AlertsController : ControllerBase
    {
        private IRepository _repository;
        private IPageService _pageService;

        public AlertsController(ApplicationSettings settings, UserServiceBase userManager,
            SettingsService settingsService, IUserContext context, IRepository repository, IPageService pageService)
            : base(settings, userManager, context, settingsService)
        {
            _repository = repository;
            _pageService = pageService;
        }

        /// <summary>
        /// Displays a list of all page titles and ids in Roadkill.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
        [ControllerRequired]
        public ActionResult ListAlerts()
        {
            AlertsViewModel model = new AlertsViewModel();
            var alerts = _repository.GetAlerts();
            if (alerts != null)
            {
                foreach (Alert alert in alerts)
                {
                    if (alert.CommentId == Guid.Empty)
                    {
                        Page page = _repository.GetPageById(alert.PageId);
                        if (page != null)
                        {
                            // check that page is still in publish state
                            if (!page.IsControlled || page.IsRejected)
                            {
                                // remove alerts of this page
                                _repository.DeletePageAlerts(page.Id);
                            }
                            else
                            {
                                PageAlertsInfo info = new PageAlertsInfo(alert.PageId, alert.Ilk, page.Title);
                                model.Add(info);
                            }
                        }
                    }
                    else
                    {
                        //TODO alerts for comments
                    }
                }
            }
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteForPage(int id)
        {
            _repository.DeletePageAlerts(id);

            return RedirectToAction("ListAlerts", "Alerts");
        }
    }
}

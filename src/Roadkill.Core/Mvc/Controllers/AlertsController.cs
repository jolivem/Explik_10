using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Roadkill.Core.Configuration;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Database;

using Roadkill.Core.Attachments;

namespace Roadkill.Core.Mvc.Controllers
{
    /// <summary>
    /// Provides all page related functionality, including editing and viewing pages.
    /// </summary>
    [HandleError]
    [OptionalAuthorization]
    public class AlertsController : ControllerBase
    {
        private SettingsService _settingsService;
        private AttachmentPathUtil _attachmentPathUtil;
        private UserServiceBase _userServiceBase;
        private IRepository _repository;


        public AlertsController(ApplicationSettings settings, UserServiceBase userManager,
            SettingsService settingsService,
             IUserContext context, IRepository repository)
            : base(settings, userManager, context, settingsService)
        {
            _settingsService = settingsService;
            _attachmentPathUtil = new AttachmentPathUtil(settings);
            _userServiceBase = userManager;
            _repository = repository;

        }

        /// <summary>
        /// Displays a list of all page titles and ids in Roadkill.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
        [ControllerRequired]
        public ActionResult AllNewComments()
        {
            List<CommentViewModel> commentsModel = new List<CommentViewModel>();
            var comments = _repository.FindCommentsToControl();
            foreach (Comment comment in comments)
            {
                commentsModel.Add(new CommentViewModel(comment));
            }

            return View(commentsModel);
        }

        public ActionResult Validate(string id)
        {
            Guid guid;
            if (Guid.TryParse(id, out guid))
            {
                _repository.ValidateComment(guid);
            }
            return RedirectToAction("AllNewComments", "Comments");
        }

        public ActionResult Reject(string id)
        {
            Guid guid;
            if (Guid.TryParse(id, out guid))
            {
                _repository.RejectComment(guid);
            }

            return RedirectToAction("AllNewComments", "Comments");
        }
    }
}

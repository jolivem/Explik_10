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
    public class CommentsController : ControllerBase
    {

        private IRepository _repository;

        public CommentsController(ApplicationSettings settings, UserServiceBase userManager,
            SettingsService settingsService, IUserContext context, IRepository repository)
            : base(settings, userManager, context, settingsService)
        {
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
            foreach( Comment comment in comments)
            {
                commentsModel.Add(new CommentViewModel(comment));
            }

            return View(commentsModel);
        }

        public ActionResult Validate(int id)
        {
            _repository.ValidateComment(id);
            return RedirectToAction("AllNewComments", "Comments");
        }

        public ActionResult Reject(int id)
        {
            _repository.RejectComment(id);
            return RedirectToAction("AllNewComments", "Comments");
        }
    }
}


using System;
using System.Web.Mvc;

using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Security;
using Roadkill.Core.Services;

namespace Roadkill.Core.Mvc.Controllers
{
    public class CommentController : ControllerBase
    {
        private PageService _pageService;

        public CommentController(ApplicationSettings settings, UserServiceBase userManager,
            SettingsService settingsService, PageService pageService, SearchService searchService,
            PageHistoryService historyService, IUserContext context)
            : base(settings, userManager, context, settingsService)
        {
            _pageService = pageService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of the page</param>
        /// <returns></returns>
        /// TODO add a filter to oblige login
        public ActionResult CommentPage(int id) //TODO open in a new tab
        {
            var titi = Context.CurrentUsername;

            var page = _pageService.GetById(id);
            if (page != null && Context.CurrentUsername != "")
            {
                CommentViewModel model = new CommentViewModel();
                model.PageId = id;
                model.PageTitle = page.Title;
                model.CreatedBy = Context.CurrentUsername;
                model.Text = "";
                model.Rating = 0; // 0 is undefined

                return View("EditComment", model);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommentPage(int id, string srating, string comment) //TODO open in a new tab
        {
            try
            {
                int iRating = Int32.Parse(srating);
                Comment _comment = new Comment(id, Context.CurrentUsername, iRating, comment);
                _pageService.AddComment(_comment);
                return View("Error"); // TODO come back to the page and diplay a toast

            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}

/// <summary>
/// Displays the edit View for the page provided in the id.
/// </summary>
/// <param name="id">The ID of the page to edit.</param>
/// <returns>An filled <see cref="PageViewModel"/> as the model. If the page id cannot be found, the action
/// redirects to the New page.</returns>
/// <remarks>This action requires editor rights.</remarks>
//[HttpPost]
//public ActionResult AddComment(int id, string text, int rating)
//{
//    Comment comment = new Comment( id, Context.CurrentUsername, rating, text);
//    var titi = Request.QueryString["commentarea"];
//    var toto = titi;

//    return Content("tout va bien", MediaTypeNames.Text.Plain);
//}

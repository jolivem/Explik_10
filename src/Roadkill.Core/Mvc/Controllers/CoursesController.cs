using System.Collections.Generic;
using System.Web.Mvc;
using Roadkill.Core.Configuration;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Extensions;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;
using System.Linq;

namespace Roadkill.Core.Mvc.Controllers
{
    /// <summary>
    /// Provides all page related functionality, including editing and viewing pages.
    /// </summary>
    [HandleError]
    [OptionalAuthorization]
    public class CoursesController : ControllerBase
    {

        private ICourseService _courseService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="userManager"></param>
        /// <param name="settingsService"></param>
        /// <param name="context"></param>
        /// <param name="courseService"></param>
        /// <param name="repository"></param>
        public CoursesController(ApplicationSettings settings, UserServiceBase userManager, SettingsService settingsService, IUserContext context, 
            ICourseService courseService)
            : base(settings, userManager, context, settingsService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Edit a course for modifications
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EditorRequired]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                CourseViewModel model = _courseService.GetByIdWithPages((int)id, false);
                if (model != null)
                {
                    model.PreviousTitle = model.Title; // in order to see if it has changed
                    return View("Edit", model);
                }
            }

            return RedirectToAction("New");
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [EditorRequired]
        public ActionResult DeleteCourse(int id)
        {
            _courseService.DeleteCourse(id);
            return Json("success", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// List the pages of a course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id, string title)
        {
            CourseViewModel model = _courseService.GetByIdWithPages(id, true);
            if (model != null)
            {
                model.PreviousTitle = model.Title; // in order to see if it has changed
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// View my course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EditorRequired]
        public ActionResult MyCourse(int id)
        {
            CourseViewModel model = _courseService.GetByIdWithPages((int)id, false);
            if (model != null)
            {
                model.PreviousTitle = model.Title; // in order to see if it has changed
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Edit a new Course
        /// </summary>
        /// <returns></returns>
        [EditorRequired]
        public ActionResult New()
        {
            CourseViewModel model = new CourseViewModel();

            return View(model);
        }


        /// <summary>
        /// Displays a list of current user page titles and ids in Roadkill.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
        [EditorRequired]
        [BrowserCache]
        public ActionResult MyCourses(string id, bool? encoded)
        {
            // Don't load CoursePages here, only Titles !

            // after submit, id = null --> leads to an exception
            // when changing the user, the id is the older one
            string currentUser = Context.CurrentUsername;
            List<CourseViewModel> models;
            if (id == Context.CurrentUsername)
            {

                // Usernames are base64 encoded by roadkill (to cater for usernames like domain\john).
                // However the URL also supports humanly-readable format, e.g. /ByUser/chris
                if (encoded == true)
                {
                    id = id.FromBase64();
                }

                ViewData["Username"] = id;

                models = _courseService.MyCourses(id);
                return View(models);
            }
            else
            {
                models = _courseService.MyCourses(currentUser).ToList();
            }

            return View(models);
        }

        /// <summary>
        /// Displays a list of current user page titles and ids in Roadkill.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> as the model.</returns>
        [EditorRequired]
        [BrowserCache]
        public ActionResult SelectPages(int id, string title)
        {
            if (id == -1)
            {
                return null;
            }

            CourseSelectionViewModel model;
            model = _courseService.GetByIdWithAllUserPages(id, Context.CurrentUsername);
            return View(model);
        }

        #region HttpPost

        [HttpPost]
        [EditorRequired]
        public ActionResult Edit(CourseViewModel model)
        {
            _courseService.UpdateCourseOrder(model);
            return RedirectToAction("MyCourses");
        }

        [HttpPost]
        [EditorRequired]
        public ActionResult SelectPages(CourseSelectionViewModel model)
        {
            _courseService.UpdateCourseSelection(model);
            return RedirectToAction("edit", "courses", new { id = model.CourseId });
        }

        /// <summary>
        /// New courses = list of pages
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [EditorRequired]
        [HttpPost]
        public ActionResult New(CourseViewModel model)
        {
            //TODO check if title is empty
            if (!ModelState.IsValid)
                return View("New", model);

            int id = _courseService.AddCourse(model.Title, Context.CurrentUsername);

            return RedirectToAction("MyCourses", new { id = model.CourseId });
        }

        #endregion
    }
}


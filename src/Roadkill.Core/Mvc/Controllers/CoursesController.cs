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
                CourseViewModel model = _courseService.GetByIdWithPages((int)id);
                if (model != null)
                {
                    return View("Edit", model);
                }
            }

            return RedirectToAction("New");
        }

        /// <summary>
        /// Edit a new Course
        /// </summary>
        /// <returns></returns>
        [EditorRequired]
        public ActionResult New()
        {
            CourseViewModel model = new CourseViewModel();

            return View("Edit", model);
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

                models = _courseService.MyCourses(id).ToList();
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
                // it is a new course, create
                id = _courseService.AddCourse(title, Context.CurrentUsername);
            }
            
            CourseViewModel models;
            models = _courseService.GetByIdWithAllUserPages(id, Context.CurrentUsername);
            return View(models);
        }

        #region HttpPost

        [HttpPost]
        [EditorRequired]
        public ActionResult Edit(CourseViewModel model)
        {
            //model.Status = CourseViewModel.StatusStringToEnum(model.StatusString);

            _courseService.UpdateCourse(model);

            return RedirectToAction("List");
        }

        /// <summary>
        /// New courses = list of pages
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[ValidateInput(false)]
        //[EditorRequired]
        //[HttpPost]
        //public ActionResult New(CourseViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View("Edit", model);

        //    _courseService.AddCourse(model);

        //    return RedirectToAction("List");
        //}

        #endregion
    }
}


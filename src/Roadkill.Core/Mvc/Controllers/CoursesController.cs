using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Roadkill.Core.Configuration;
using Roadkill.Core.Services;
using Roadkill.Core.Security;
using Roadkill.Core.Mvc.Attributes;
using Roadkill.Core.Database;

using Roadkill.Core.Attachments;
using Roadkill.Core.Localization;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Mvc.Controllers
{
    /// <summary>
    /// Provides all page related functionality, including editing and viewing pages.
    /// </summary>
    [HandleError]
    [OptionalAuthorization]
    public class CoursesController : ControllerBase
    {

        private IRepository _repository;
        private ICourseService _courseService;
        //private PageService _pageService;

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
            ICourseService courseService, PageService pageService, IRepository repository)
            : base(settings, userManager, context, settingsService)
        {
            _repository = repository;
            _courseService = courseService;
            _pageService = pageService;
        }

        /// <summary>
        /// Edit a course for modifications
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminRequired]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                CourseViewModel model = _courseService.GetById((int)id);
                if (model != null)
                {
                    // already done model.StatusString = CourseViewModel.StatusToString(model.Status);
                    return View("Edit", model);
                }
            }

            return RedirectToAction("New");
        }

        /// <summary>
        /// Edit a new Course
        /// </summary>
        /// <returns></returns>
        [AdminRequired]
        public ActionResult New()
        {
            CourseViewModel model = new CourseViewModel();

            return View("Edit", model);
        }

        /// <summary>
        /// List all courses
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            if (Context.IsAdmin)
            {
                return RedirectToAction("ListForAdmin");
            }
            else
            {
                // only achieved courses and possibility to see results
                List<CourseViewModel> models = _courseService.GetCourses();
                return View(models);
            }
        }

        /// <summary>
        /// Results from CoursePages
        /// </summary>
        /// <param name="id">course id</param>
        /// <returns></returns>
        public ActionResult Result(int id)
        {
            CourseViewModel course = _courseService.GetById(id);
            if (course != null)
            {
                // get info about the course
                ViewBag.courseTitle = course.PageTitle;
                ViewBag.CourseContent = "";
                PageViewModel page = _pageService.GetById(course.PageId, true);
                if (page != null)
                {
                    ViewBag.CourseContent = page.ContentAsHtml;
                }

                // get list of course Pages
                List<PageViewModel> model = _courseService.GetCoursePages(id);

                return View(model);
            }

            return RedirectToAction("List");
        }

        /// <summary>
        /// Results from CoursePages
        /// </summary>
        /// <param name="id">course id</param>
        /// <returns></returns>
        [AdminRequired]
        public ActionResult ListPagesForAdmin(int id)
        {
            try
            {
                CourseViewModel course = _courseService.GetById(id);
                IEnumerable<PageViewModel> model = _pageService.FindPagesByCourseId(id);
                if (model != null)
                {
                    ViewBag.courseId = id;
                    ViewBag.courseTitle = course.PageTitle;
                    return View(model);
                }
                return RedirectToAction("List");
            }
            catch( Exception ex)
            {
                var toto = ex;
            }
            return null;
        }

        #region HttpPost

        [HttpPost]
        [AdminRequired]
        public ActionResult Edit(CourseViewModel model)
        {
            model.Status = CourseViewModel.StatusStringToEnum(model.StatusString);

            _courseService.UpdateCourse(model);

            return RedirectToAction("List");
        }

        [ValidateInput(false)]
        [AdminRequired]
        [HttpPost]
        public ActionResult New(CourseViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            model.Status = CourseViewModel.StatusStringToEnum(model.StatusString);
            _courseService.AddCourse(model);

            return RedirectToAction("List");
        }

        //[HttpPost]
        //public ActionResult Achieve(int id)
        //{
        //    _courseService.Achieve(id);
        //    return RedirectToAction("List");
        //}


        #endregion


    }
}


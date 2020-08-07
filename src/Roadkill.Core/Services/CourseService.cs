using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Services
{
    public class CourseService : ServiceBase, ICourseService
    {

        public CourseService(ApplicationSettings settings, IRepository repository)
            : base(settings, repository)
        {
        }

        /// <summary>
        /// Add a course
        /// </summary>
        /// <param name="title"></param>
        /// <param name="createdBy"></param>
        /// <returns>the Id of the new course</returns>
        public int AddCourse(string title, string createdBy)
        {
            var course = new Course(title, createdBy);
            return Repository.AddCourse(course);
        }

        /// <summary>
        /// Get Courses and pages included in the course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public CourseViewModel GetByIdWithPages(int courseId)
        {
            try
            {
                // Get the course
                Course course = Repository.GetCourseById(courseId);
                if (course == null)
                {
                    return null;
                }

                CourseViewModel model = new CourseViewModel(course);

                // Add coursePages
                IEnumerable<CoursePage> coursePages = Repository.GetCoursePages(courseId).ToList();
                model.CoursePagesModels = (from coursePage in coursePages
                                         select new CoursePageViewModel(coursePage, 
                                         Repository.GetPageById(coursePage.PageId))).ToList();

                // sort by order
                if (model != null && model.CoursePagesModels != null)
                {
                    // if has already been ordered
                    if (model.CoursePagesModels.Exists(cp => cp.Order != 0))
                    {
                        model.CoursePagesModels.Sort(new ComparerByOrder());
                    }
                    else
                    {
                        // order by page id
                        model.CoursePagesModels.Sort(new ComparerByPageId());
                    }
                }

                return model;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, $"An exception occurred while getting competitions id = {courseId}");
            }
        }

        /// <summary>
        /// Get Courses and pages of the user
        /// Used to select pages inside the course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public CourseSelectionViewModel GetByIdWithAllUserPages(int courseId, string username)
        {
            try
            {
                // Get the course
                Course course = Repository.GetCourseById(courseId);
                if (course == null)
                {
                    return null;
                }

                // Course -> CourseViewModel
                CourseSelectionViewModel model = new CourseSelectionViewModel()
                {
                    CourseId = courseId,
                    CreatedBy = course.CreatedBy,
                    Title = course.Title,
                };

                // Get coursePages of the course
                //IEnumerable<CoursePage> coursePages = Repository.GetCoursePages(courseId).ToList();

                // get all the user pages
                IEnumerable<Page> pages = Repository.MyPages(username);
                IEnumerable<PageViewModel> pageModels = from page in pages
                                            select new PageViewModel(page);

                // Add information from coursePage and from page
                // title, selected or not, ....
                foreach (Page page in pages)
                {
                    // get list of courses containing the page
                    List<Course> courses = Repository.FindCoursesByPageId(page.Id).ToList();
                    //CoursePage coursePage = coursePages.SingleOrDefault(x => x.PageId == page.Id);
                    model.Pages.Add(
                        new PageWithCoursesViewModel(page, courses, courses.Exists(c => c.Id == courseId)));
                }

                return model;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, $"An exception occurred while getting competitions id = {courseId}");
            }
        }

        /// <summary>
        /// Retrieves a list of all the user courses.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{PageViewModel}"/> of the pages.</returns>
        /// <exception cref="DatabaseException">An databaseerror occurred while retrieving the list.</exception>
        public List<CourseViewModel> MyCourses(string userId)
        {
            try
            {
                List<CourseViewModel> courseModels;

                IEnumerable<Course> courses = Repository.GetCoursesByUser(userId).OrderByDescending(p => p.Id);
                courseModels = (from course in courses
                               select new CourseViewModel(course)).ToList();

                // don't fill CoursePageViewModel here

                // but fill nb page foreach course
                foreach (var model in courseModels)
                {
                    model.NbPages = Repository.GetCoursePages(model.CourseId).Count();
                }

                return courseModels;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving my courses from the database");
            }
        }

        public void UpdateCourseSelection(CourseSelectionViewModel model)
        {
            if (model != null)
            {
                // remove all coursepages from course
                Repository.DeleteCoursePages(model.CourseId);

                // add selected course pages in the course
                foreach( PageWithCoursesViewModel page in model.Pages)
                {
                    if (page.Selected)
                    {
                        CoursePage coursePage = new CoursePage()
                        {
                            CourseId = model.CourseId,
                            PageId = page.Page.Id,
                            Order = 0, // not ordered yet
                        };
                        Repository.AddCoursePage(coursePage);
                    }
                }
            }
        }

        public void UpdateCourseOrder(CourseViewModel model)
        {
            if (model != null)
            {
                if (model.Title != model.PreviousTitle)
                {
                    Repository.UpdateCourseTitle(model.CourseId, model.Title);
                }

                foreach (CoursePageViewModel coursePageModel in model.CoursePagesModels)
                {
                    Repository.UpdateCoursePageOrder(coursePageModel.Id, coursePageModel.Order);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCourse(int id)
        {
            // delete pages of the course
            Repository.DeleteCoursePages(id);

            // delete the course
            Repository.DeleteCourse(id);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    internal class ComparerByOrder : IComparer<CoursePageViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0 if equal ; 1 if x > y ; -1 if y > x</returns>
        public int Compare(CoursePageViewModel x, CoursePageViewModel y)
        {
            return x.Order.CompareTo(y.Order);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ComparerByPageId : IComparer<CoursePageViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0 if equal ; 1 if x > y ; -1 if y > x</returns>
        public int Compare(CoursePageViewModel x, CoursePageViewModel y)
        {
            return x.Page.Id.CompareTo(y.Page.Id);
        }
    }

}

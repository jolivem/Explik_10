using System;
using System.Collections.Generic;
using System.Linq;
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
        public CourseViewModel GetByIdWithAllUserPages(int courseId, string username)
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
                CourseViewModel model = new CourseViewModel(course);

                // Get coursePages of the course
                IEnumerable<CoursePage> coursePages = Repository.GetCoursePages(courseId).ToList();

                // get all the user pages
                IEnumerable<Page> pages = Repository.MyPages(username);
                IEnumerable<PageViewModel> pageModels = from page in pages
                                            select new PageViewModel(page);

                // Add information from coursePage and from page
                foreach( Page page in pages)
                {
                    CoursePage coursePage = coursePages.SingleOrDefault(x => x.PageId == page.Id);
                        model.CoursePagesModels.Add(
                            new CoursePageViewModel(coursePage, page));
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
        public IEnumerable<CourseViewModel> MyCourses(string userId)
        {
            try
            {
                IEnumerable<CourseViewModel> courseModels;

                IEnumerable<Course> courses = Repository.GetCoursesByUser(userId).OrderByDescending(p => p.Id);
                courseModels = from course in courses
                               select new CourseViewModel(course);

                // don't fill CoursePageViewModel here

                return courseModels;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex, "An error occurred while retrieving my courses from the database");
            }
        }

        public List<CoursePageViewModel> GetCoursePages(int id)
        {
            throw new NotImplementedException();
        }

        public List<CourseViewModel> GetCourses(bool forAdmin = false)
        {
            throw new NotImplementedException();
        }

        public List<CourseViewModel> GetCourses()
        {
            throw new NotImplementedException();
        }

        public void UpdateCourse(CourseViewModel course)
        {
            throw new NotImplementedException();
        }


    }
}

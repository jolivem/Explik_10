using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Database.Repositories
{
    public interface ICourseRepository
    {
        /// <summary>
        /// Create new Course
        /// </summary>
        /// <param name="Course"></param>
        void AddCourse(Course Course);

        /// <summary>
        /// Update the dates of the Course, or th state
        /// </summary>
        /// <param name="Course"></param>
        void UpdateCourse(Course Course);

        /// <summary>
        /// Update the pageId of the page tag of the Course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="pageId"></param>
        void UpdateCoursePageId(int courseId, int pageId);

        /// <summary>
        /// Get all compeitions for display
        /// </summary>
        /// <returns></returns>
        IEnumerable<Course> GetCoursesByUser(string createdBy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Course GetCourseById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Course GetCourseByPage(string tag);

        /// <summary>
        /// Get all pages registered for a given Course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<CoursePage> GetCoursePages(int courseId);

        /// <summary>
        /// Delete a given Course
        /// </summary>
        /// <param name="id"></param>
        void DeleteCourse(int id);

        /// <summary>
        /// Add a page in a given Course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="page"></param>
        //void ArchiveCoursePage(int courseId, Page page);
    }
}

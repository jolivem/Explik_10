using Roadkill.Core.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Services
{
    public interface ICourseService
    {
        CourseViewModel GetByIdWithPages(int id);

        List<CoursePageViewModel> GetCoursePages(int id);

        int AddCourse(string title, string createdBy);

        void UpdateCourseSelection(CourseViewModel course);

        void UpdateCourseOrder(CourseViewModel course);

        List<CourseViewModel> GetCourses();

        // get the list of courses without included pages
        IEnumerable<CourseViewModel> MyCourses(string userId);

        // Get all pages of the user and tick the one selected in the course of the user
        // used to slect pages to be included in the course
        CourseViewModel GetByIdWithAllUserPages(int courseId, string username);

        void DeleteCourse(int id);

    }
}

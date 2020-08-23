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
        CourseViewModel GetByIdWithPages(int id, bool onlyControlled=false);

        int AddCourse(string title, string createdBy);

        void UpdateCourseSelection(CourseSelectionViewModel course);

        void UpdateCourseOrder(CourseViewModel course);

        // get the list of courses without included pages
        List<CourseViewModel> MyCourses(string userId);

        // Get all pages of the user and tick the one selected in the course of the user
        // used to slect pages to be included in the course
        CourseSelectionViewModel GetByIdWithAllUserPages(int courseId, string username);

        void DeleteCourse(int id);

    }
}

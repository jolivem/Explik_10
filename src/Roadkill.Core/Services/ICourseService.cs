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
        CourseViewModel GetById(int id);

        List<PageViewModel> GetCoursePages(int id);

        void AddCourse(CourseViewModel model, bool debug = false);

        void UpdateCourse(CourseViewModel course);

        List<CourseViewModel> GetCourses();

        //Course GetCourseByStatus(CourseViewModel.Statuses status);

        //void Achieve(int courseId);
    }
}

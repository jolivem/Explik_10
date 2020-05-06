using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Services
{
    public class CourseService : ICourseService
    {
        public void AddCourse(CourseViewModel model, bool debug = false)
        {
            throw new NotImplementedException();
        }

        public CourseViewModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<PageViewModel> GetCoursePages(int id)
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

        CourseViewModel ICourseService.GetById(int id)
        {
            throw new NotImplementedException();
        }

        List<PageViewModel> ICourseService.GetCoursePages(int id)
        {
            throw new NotImplementedException();
        }
    }
}

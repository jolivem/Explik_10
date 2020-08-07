using Roadkill.Core.Database;
using System.Collections.Generic;

namespace Roadkill.Core.Mvc.ViewModels
{

    /// <summary>
    /// One course with its pages model for views
    /// </summary>
    public class PageWithCoursesViewModel
    {
        public PageViewModel Page { get; set; }
        public List<Course> Courses { get; set; }
        public bool Selected { get; set; } // used only in order to select the pages 

        /// <summary>
        /// 
        /// </summary>
        public PageWithCoursesViewModel()
        {
            Page = new PageViewModel();
            Courses = new List<Course>();
            Selected = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="courses">the course to be displayed</param>
        /// <param name="coursePage">A potential course page, may be nul</param>
        /// <param name="page">a page (maybe no selected)</param>
        public PageWithCoursesViewModel(Page page, List<Course> courses, bool selected)
        {
            Page = new PageViewModel(page);
            Courses = courses;
            Selected = selected;
        }
    }
}

using Roadkill.Core.Database;
using Roadkill.Core.Localization;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Roadkill.Core.Mvc.ViewModels
{

    /// <summary>
    /// 
    /// </summary>
    public class CourseViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CourseViewModel()
        {
            Title = "";
            CreatedBy = "";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        public CourseViewModel(Course course)
        {
            Id = course.Id;
            Title = course.Title;
            CreatedBy = course.CreatedBy;
        }

    }
}

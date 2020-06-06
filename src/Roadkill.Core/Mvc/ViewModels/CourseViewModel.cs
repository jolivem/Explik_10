using Roadkill.Core.Database;
using Roadkill.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Roadkill.Core.Mvc.ViewModels
{

    /// <summary>
    /// 
    /// </summary>
    public class CourseViewModel
    {

        public int CourseId { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public string PreviousTitle { get; set; }
        public List<CoursePageViewModel> CoursePagesModels { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public CourseViewModel()
        {
            CourseId = -1;
            Title = "";
            CreatedBy = "";
            CoursePagesModels = new List<CoursePageViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        public CourseViewModel(Course course)
        {
            CourseId = course.Id;
            Title = course.Title;
            CreatedBy = course.CreatedBy;
            CoursePagesModels = new List<CoursePageViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPortraitImage()
        {
            string first = CreatedBy.First().ToString().ToUpper();
            return "~/Assets/Images/letters/letter-" + first + ".jpg";
        }


        /// <summary>
        /// 
        /// </summary>
        public string EncodedTitle
        {
            get
            {
                return PageViewModel.EncodePageTitle(Title);
            }
        }
    }
}

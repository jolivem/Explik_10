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
    public class CourseSelectionViewModel
    {

        public int CourseId { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        //public string PreviousTitle { get; set; }
        public List<PageWithCoursesViewModel> Pages { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public CourseSelectionViewModel()
        {
            CourseId = -1;
            Title = "";
            CreatedBy = "";
            Pages = new List<PageWithCoursesViewModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        public CourseSelectionViewModel(Course course)
        {
            CourseId = course.Id;
            Title = course.Title;
            CreatedBy = course.CreatedBy;
            Pages = new List<PageWithCoursesViewModel>();
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

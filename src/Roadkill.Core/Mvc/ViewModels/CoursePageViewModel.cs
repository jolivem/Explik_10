﻿using Roadkill.Core.Database;
using Roadkill.Core.Localization;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Roadkill.Core.Mvc.ViewModels
{

    /// <summary>
    /// One course with its pages model for views
    /// </summary>
    public class CoursePageViewModel
    {
        public int Id { get; set; } // coursePageId ... not sure it is useful
        public int CourseId { get; set; }
        public PageViewModel Page { get; set; }      
        public int Order { get; set; }
        public bool Selected { get; set; } // used only in order to select the pages 

        /// <summary>
        /// 
        /// </summary>
        public CoursePageViewModel()
        {
            CourseId = 0;
            Page = new PageViewModel();
            Order = 0;
            Selected = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        public CoursePageViewModel(CoursePage coursePage, Page page)
        {
            if (coursePage == null)
            {
                Order = -1;
                Selected = false;
                CourseId = -1;
            }
            else
            {
                Order = coursePage.Order;
                Selected = true;
                Id = coursePage.Id;
                CourseId = coursePage.CourseId;
            }
            Page = new PageViewModel(page);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseId">the courseId to be displayed</param>
        /// <param name="coursePage">A potential course page, may be nul</param>
        /// <param name="page">a page (maybe no selected)</param>
        public CoursePageViewModel(int courseId, CoursePage coursePage, Page page)
        {
            if (coursePage == null)
            {
                Order = -1;
                Selected = false;
                CourseId = -1;
            }
            else
            {
                if (courseId == coursePage.CourseId)
                {
                    Selected = true;
                    Order = coursePage.Order;
                }
                
                Id = coursePage.Id;
                CourseId = coursePage.CourseId;
            }
            Page = new PageViewModel(page);
        }
    }
}

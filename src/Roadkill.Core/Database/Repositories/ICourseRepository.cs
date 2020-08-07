﻿using System;
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
        int AddCourse(Course Course);

        /// <summary>
        /// Update the dates of the Course, or th state
        /// </summary>
        /// <param name="Course"></param>
        void UpdateCourse(Course Course);

        /// <summary>
        /// Update the title of a given course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        void UpdateCourseTitle(int id, string title);

        /// <summary>
        /// Create new Course page
        /// </summary>
        /// <param name="Course"></param>
        int AddCoursePage(CoursePage coursePage);

        /// <summary>
        /// Update the pageId of the page tag of the Course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="pageId"></param>
        void UpdateCoursePageOrder(int coursePageId, int order);

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
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<Page> GetPagesByCourseId(int courseId);

        /// <summary>
        /// Get all pages registered for a given Course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<CoursePage> GetCoursePages(int courseId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of the course page</param>
        /// <returns></returns>
        CoursePage GetCoursePageById(int id);

        /// <summary>
        /// Delete a given Course
        /// </summary>
        /// <param name="id"></param>
        void DeleteCourse(int id);

        /// <summary>
        /// Delete a given Course page
        /// </summary>
        /// <param name="id"></param>
        void DeleteCoursePage(int coursePageId);

        /// <summary>
        /// Delet all coursePages of a course
        /// </summary>
        /// <param name="courseId"></param>
        void DeleteCoursePages(int courseId);

        /// Delete all courses and course pages
        /// </summary>
        void DeleteCourses();

        /// <summary>
        /// Delete the course pages associated to a given page id
        /// </summary>
        /// <param name="pageId"></param>
        void DeleteCoursePagesforPageId(int pageId);

        /// <summary>
        /// Find a course containing the page
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        IEnumerable<Course> FindCoursesByPageId(int pageId);
    }
}

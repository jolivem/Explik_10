using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roadkill.Core.Database.Repositories.Entities;
using Roadkill.Core.Database;

namespace Roadkill.UT.Repository
{
    /// <summary>
    /// Description résumée pour UTEntity
    /// </summary>
    [TestClass]
    public class UTConversion
    {

        [TestMethod]
        public void T001_FromTo_Course()
        {
            explik_course expliCourse1 = new explik_course()
            {
                Id = 1,
                Title = "title",
                CreatedBy = "user"
            };
            explik_course expliCourse10 = new explik_course()
            {
                Id = 10,
                Title = "title10",
                CreatedBy = "user10"
            };

            // Arrange Course
            Course course = new Course();
            course = FromEntity.ToCourse(expliCourse1);
            explik_course expliCourse2 = new explik_course();
            ToEntity.FromCourse(course, expliCourse2);

            Assert.IsTrue(expliCourse1.Id == expliCourse2.Id);
            Assert.IsTrue(expliCourse1.Title == expliCourse2.Title);
            Assert.IsTrue(expliCourse1.CreatedBy == expliCourse2.CreatedBy);

            // Test Course list


        }

        [TestMethod]
        public void T003_To_CourseList()
        {
            explik_course expliCourse1 = new explik_course()
            {
                Id = 1,
                Title = "title1",
                CreatedBy = "user1"
            };
            explik_course expliCourse2 = new explik_course()
            {
                Id = 2,
                Title = "title2",
                CreatedBy = "user2"
            };

            // Arrange Course list

            List<explik_course> explikCourseList1 = new List<explik_course> { expliCourse1, expliCourse2 };

            List<Course> courseList = new List<Course>();
            courseList = FromEntity.ToCourseList(explikCourseList1).ToList();

            // Tests

            Assert.IsTrue(courseList[0].Id == expliCourse1.Id);
            Assert.IsTrue(courseList[0].Title == expliCourse1.Title);
            Assert.IsTrue(courseList[0].CreatedBy == expliCourse1.CreatedBy);

            Assert.IsTrue(courseList[1].Id == expliCourse2.Id);
            Assert.IsTrue(courseList[1].Title == expliCourse2.Title);
            Assert.IsTrue(courseList[1].CreatedBy == expliCourse2.CreatedBy);
        }

        [TestMethod]
        public void T002_FromTo_CoursePage()
        {
            explik_coursepage explikCoursePage1 = new explik_coursepage()
            {
                Id = 1,
                CourseId = 2,
                PageId = 3,
                Order = 4
            };

            //explik_coursepage coursePage2 = new explik_coursepage()
            //{
            //    Id = 2,
            //    CourseId = 1,
            //    PageId = 2,
            //};

            CoursePage coursePage = new CoursePage();
            coursePage = FromEntity.ToCoursePage(explikCoursePage1);
            explik_coursepage explikCoursePage2 = new explik_coursepage();
            ToEntity.FromCoursePage(coursePage, explikCoursePage2);

            Assert.IsTrue(explikCoursePage1.Id == explikCoursePage2.Id);
            Assert.IsTrue(explikCoursePage2.CourseId == explikCoursePage2.CourseId);
            Assert.IsTrue(explikCoursePage2.Order == explikCoursePage2.Order);
        }

        [TestMethod]
        public void T004_To_CoursePageList()
        {
            explik_coursepage explikCoursePage1 = new explik_coursepage()
            {
                Id = 10,
                CourseId = 12,
                PageId = 13,
                Order = 14
            };

            explik_coursepage explikCoursePage2 = new explik_coursepage()
            {
                Id = 21,
                CourseId = 22,
                PageId = 23,
                Order = 24
            };

            // Arrange Course page list

            List<explik_coursepage> explikCoursePageList1 = new List<explik_coursepage> { explikCoursePage1, explikCoursePage2 };

            List<CoursePage> coursePageList = new List<CoursePage>();
            coursePageList = FromEntity.ToCoursePageList(explikCoursePageList1).ToList();

            // Tests

            Assert.IsTrue(coursePageList[0].Id == explikCoursePage1.Id);
            Assert.IsTrue(coursePageList[0].CourseId == explikCoursePage1.CourseId);
            Assert.IsTrue(coursePageList[0].PageId == explikCoursePage1.PageId);
            Assert.IsTrue(coursePageList[0].Order == explikCoursePage1.Order);

            Assert.IsTrue(coursePageList[1].Id == explikCoursePage2.Id);
            Assert.IsTrue(coursePageList[1].CourseId == explikCoursePage2.CourseId);
            Assert.IsTrue(coursePageList[1].PageId == explikCoursePage2.PageId);
            Assert.IsTrue(coursePageList[1].Order == explikCoursePage2.Order);

        }
    }
}

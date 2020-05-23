using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Database.LightSpeed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roadkill.UT.Repository
{
    [TestClass]
    public class UTCourseRepository
    {

        protected IRepository Repository;
        protected ApplicationSettings ApplicationSettings;
        private string connectionString = @"server=localhost;user id=Admin;password=Admin;database=explik;";
        protected virtual DataStoreType DataStoreType { get { return null; } }

        [TestInitialize]
        public void SetUp()
        {
            // Process before each test
            ApplicationSettings = new ApplicationSettings() { ConnectionString = connectionString, DataStoreType = DataStoreType };
            Repository = new LightSpeedRepository(ApplicationSettings);
            Repository.DeleteCourses();
        }


        [TestMethod]
        public void AddCourse()
        {
            Course course1 = new Course("title", "user");
            int id = Repository.AddCourse(course1);

            var course2 = Repository.GetCourseById(id);

            Assert.IsTrue(course1.Title == course2.Title);
            Assert.IsTrue(course1.CreatedBy == course2.CreatedBy);
        }

        [TestMethod]
        public void UpdateCourse()
        {
            Course course1 = new Course("title", "user");
            int id = Repository.AddCourse(course1);

            course1.Title = "title1";
            course1.CreatedBy = "user2";
            Repository.UpdateCourse(course1);

            var course2 = Repository.GetCourseById(id);

            Assert.IsTrue(course1.Title == course2.Title);
            Assert.IsTrue(course1.CreatedBy == course2.CreatedBy);
        }

        [TestMethod]
        public void GetCoursesByUser()
        {
            Course course1 = new Course("title11", "user1");
            Repository.AddCourse(course1);
            Course course2 = new Course("title12", "user1");
            Repository.AddCourse(course2);
            Course course3 = new Course("title21", "user2");
            Repository.AddCourse(course3);

            List<Course> list = Repository.GetCoursesByUser("user1").ToList();
            Assert.IsTrue(list.Count == 2);
            Assert.IsTrue(list[0].Title == course1.Title);
            Assert.IsTrue(list[0].CreatedBy == course1.CreatedBy);
            Assert.IsTrue(list[1].Title == course2.Title);
            Assert.IsTrue(list[1].CreatedBy == course2.CreatedBy);

            list = Repository.GetCoursesByUser("user2").ToList();
            Assert.IsTrue(list.Count == 1);
            Assert.IsTrue(list[0].Title == course3.Title);
            Assert.IsTrue(list[0].CreatedBy == course3.CreatedBy);
        }

        [TestMethod]
        public void AddCoursePage()
        {
            CoursePage coursePage1 = new CoursePage()
            {
                CourseId = 1,
                PageId = 2,
                Order = 3,
            };

            int id = Repository.AddCoursePage(coursePage1);

            var coursePage2 = Repository.GetCoursePageById(id);

            Assert.IsTrue(coursePage1.CourseId == coursePage2.CourseId);
            Assert.IsTrue(coursePage1.PageId == coursePage2.PageId);
            Assert.IsTrue(coursePage1.Order == coursePage2.Order);
        }

        [TestMethod]
        public void UpdateCoursePageOrder()
        {
            CoursePage coursePage1 = new CoursePage()
            {
                CourseId = 1,
                PageId = 2,
                Order = 3,
            };

            int id = Repository.AddCoursePage(coursePage1);
            Repository.UpdateCoursePageOrder(coursePage1.Id, 5);
            var coursePage2 = Repository.GetCoursePageById(id);

            Assert.IsTrue(coursePage1.CourseId == coursePage2.CourseId);
            Assert.IsTrue(coursePage1.PageId == coursePage2.PageId);
            Assert.IsTrue(5 == coursePage2.Order);

        }

        [TestMethod]
        public void GetPagesByCourseId()
        {
            Repository.DeleteAllPages();

            Page page1 = new Page() { Title = "title1", CreatedBy = "user1", };
            PageContent pageContent1 = Repository.AddNewPage(page1, "text", "admin", DateTime.Now);
            Page page2 = new Page() { Title = "title2", CreatedBy = "user1", };
            PageContent pageContent2 = Repository.AddNewPage(page2, "text", "admin", DateTime.Now);
            Page page3 = new Page() { Title = "title3", CreatedBy = "user1", };
            PageContent pageContent3 = Repository.AddNewPage(page3, "text", "admin", DateTime.Now);
            Page page4 = new Page() { Title = "title4", CreatedBy = "user2", };
            PageContent pageContent4 = Repository.AddNewPage(page4, "text", "admin", DateTime.Now);

            Course course1 = new Course("titleCourse1", "user1");
            int id1 = Repository.AddCourse(course1);
            CoursePage coursePage11 = new CoursePage() { CourseId = id1, PageId = pageContent1.Page.Id, Order = 1, };
            Repository.AddCoursePage(coursePage11);
            CoursePage coursePage12 = new CoursePage() { CourseId = id1, PageId = pageContent2.Page.Id, Order = 2, };
            Repository.AddCoursePage(coursePage12);
            CoursePage coursePage13 = new CoursePage() { CourseId = id1, PageId = pageContent3.Page.Id, Order = 3, };
            Repository.AddCoursePage(coursePage13);


            Course course2 = new Course("titleCourse2", "user2");
            int id2 = Repository.AddCourse(course2);
            CoursePage coursePage21 = new CoursePage() { CourseId = id2, PageId = pageContent4.Page.Id, Order = 1, };
            Repository.AddCoursePage(coursePage21);


            var list1 = Repository.GetPagesByCourseId(id1).ToList();
            Assert.IsTrue(list1.Count == 3);
            Assert.IsTrue(list1[0].Id == pageContent1.Page.Id);
            Assert.IsTrue(list1[1].Id == pageContent2.Page.Id);
            Assert.IsTrue(list1[2].Id == pageContent3.Page.Id);

            var list2 = Repository.GetPagesByCourseId(id2).ToList();
            Assert.IsTrue(list2.Count == 1);
            Assert.IsTrue(list2[0].Id == pageContent4.Page.Id);

        }

        //[TestMethod]
        //Course GetCourseByPage()
        //{
        //    Course GetCourseByPage(string tag);
        //}

        [TestMethod]
        public void GetCoursePages()
        {

            Course course1 = new Course("titleCourse1", "user1");
            int id1 = Repository.AddCourse(course1);
            CoursePage coursePage11 = new CoursePage() { CourseId = id1, PageId = 11, Order = 1, };
            Repository.AddCoursePage(coursePage11);
            CoursePage coursePage12 = new CoursePage() { CourseId = id1, PageId = 12, Order = 2, };
            Repository.AddCoursePage(coursePage12);
            CoursePage coursePage13 = new CoursePage() { CourseId = id1, PageId = 13, Order = 3, };
            Repository.AddCoursePage(coursePage13);


            Course course2 = new Course("titleCourse2", "user2");
            int id2 = Repository.AddCourse(course2);
            CoursePage coursePage21 = new CoursePage() { CourseId = id2, PageId = 21, Order = 1, };
            Repository.AddCoursePage(coursePage21);


            var list1 = Repository.GetCoursePages(id1).ToList();
            Assert.IsTrue(list1.Count == 3);
            Assert.IsTrue(list1[0].PageId == coursePage11.PageId);
            Assert.IsTrue(list1[0].Order == coursePage11.Order);
            Assert.IsTrue(list1[1].PageId == coursePage12.PageId);
            Assert.IsTrue(list1[1].Order == coursePage12.Order);
            Assert.IsTrue(list1[2].PageId == coursePage13.PageId);
            Assert.IsTrue(list1[2].Order == coursePage13.Order);

            var list2 = Repository.GetCoursePages(id2).ToList();
            Assert.IsTrue(list2.Count == 1);
            Assert.IsTrue(list2[0].PageId == coursePage21.PageId);
            Assert.IsTrue(list2[0].Order == coursePage21.Order);
        }

        [TestMethod]
        public void DeleteCourse()
        {
            Course course1 = new Course("titleCourse1", "user1");
            int id1 = Repository.AddCourse(course1);

            Repository.DeleteCourse(id1);
            Course course = Repository.GetCourseById(id1);
            Assert.IsTrue(course == null);
        }

        [TestMethod]
        public void DeleteCoursePage()
        {
            CoursePage course1 = new CoursePage() { CourseId = 1, PageId = 2, Order = 3 };
            int id1 = Repository.AddCoursePage(course1);

            Repository.DeleteCoursePage(id1);
            CoursePage course2 = Repository.GetCoursePageById(id1);
            Assert.IsTrue(course2 == null);
        }

    }
}

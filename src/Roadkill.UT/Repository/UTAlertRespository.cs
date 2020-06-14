using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.UT.Repository
{
    [TestClass]
    public class UTAlertRespository
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
            //Repository.DeleteAlert();
        }


        [TestMethod]
        public void AddAlert()
        {
            Alert alert = new Alert(18, "user", "type");
            Repository.AddAlert(alert);

            var alert2 = Repository.FindAlertsByPage(18);

            Assert.IsTrue(alert2!=null);
            //Assert.IsTrue(course1.CreatedBy == course2.CreatedBy);
        }
    }
}


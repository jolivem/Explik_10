using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.UT.Services
{
    [TestClass]
    public class UTSearchService
    {
        protected IRepository Repository;
        protected ApplicationSettings ApplicationSettings;

        private string connectionString = @"server=localhost;user id=Admin;password=Admin;database=explik;";
        protected virtual DataStoreType DataStoreType { get { return null; } }

        [TestInitialize]
        public void SetUp()
        {
            ApplicationSettings = new ApplicationSettings() { ConnectionString = connectionString, DataStoreType = DataStoreType };

            Repository = new LightSpeedRepository(ApplicationSettings);
        }

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    SearchService 
        //}
    }
}

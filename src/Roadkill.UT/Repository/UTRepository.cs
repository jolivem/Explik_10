using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Database.LightSpeed;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Roadkill.UT.Repository
{
	[TestClass]
	public class RepositoryTests
	{
		protected IRepository Repository;
		protected ApplicationSettings ApplicationSettings;

		protected string ConnectionString { get; set; }
		protected virtual DataStoreType DataStoreType { get { return null; } }


		[TestInitialize]
		public void SetUp()
		{
            ConnectionString = "tuti";
            ApplicationSettings = new ApplicationSettings() { ConnectionString = ConnectionString, DataStoreType = DataStoreType };

			Repository = new LightSpeedRepository(ApplicationSettings);
		}

        //protected abstract IRepository GetRepository();

        [TestMethod]
        public void T001_TestConnection()
        {
            //bool test = Repository.TestConnection(@"server=uu;user id=Admin;password=Admin;persistsecurityinfo=True;database=explik");
            bool test = Repository.TestConnection(@"server=localhost;user id=Admin;password=Admin;database=explik;port=3306");
            Assert.IsTrue(test);
        }

        //[TestMethod]
        //public void T001_TestConnection()
        //{
        //    var list = Repository.AllPages();
        //    Assert.IsTrue(list != null);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Database.LightSpeed;

namespace Roadkill.UT.Repository
{
	[TestClass]
	public class UserRepositoryTests
	{
		protected User _adminUser;
		protected User _editor;
		protected User _inactiveUser;

        protected IRepository Repository;
        protected ApplicationSettings ApplicationSettings;

        private string connectionString = @"server=localhost;user id=Admin;password=Admin;database=explik;";
        protected virtual DataStoreType DataStoreType { get { return null; } }

        [TestInitialize]
		public void SetUp()
		{
            ApplicationSettings = new ApplicationSettings() { ConnectionString = connectionString, DataStoreType = DataStoreType };

            Repository = new LightSpeedRepository(ApplicationSettings);

            Repository.DeleteAllUsers();

            _adminUser = NewUser("admin@localhost", "admin", true, true);
			_adminUser = Repository.SaveOrUpdateUser(_adminUser);

			_editor = NewUser("editor1@localhost", "editor1", false, true);
			_editor = Repository.SaveOrUpdateUser(_editor);

			_inactiveUser = NewUser("editor2@localhost", "editor2", false, true, false);
			_inactiveUser = Repository.SaveOrUpdateUser(_inactiveUser);
		}

		protected User NewUser(string email, string username, bool isAdmin, bool isEditor, bool isActive = true)
		{
            var user = new User()
            {
                Email = email,
                Username = username,
                Salt = "123",
                IsActivated = isActive,
                IsAdmin = isAdmin,
                IsEditor = isEditor,
                ActivationKey = Guid.NewGuid().ToString(),
                Firstname = "Firstname",
                Lastname = "Lastname",
                PasswordResetKey = Guid.NewGuid().ToString()
            };
            user.SetPassword("Password");
            return user;

		}


        [TestMethod]
        public void AddUser()
        {
            var user = NewUser("uusseerr@localhost", "uusseerr", false, true);
            Repository.SaveOrUpdateUser(user);
        }

		[TestMethod]
		public void GetAdminById()
		{
			// Arrange
			User expectedUser = _adminUser;

			// Act
			User noUser = Repository.GetAdminById(_editor.Id);
			User actualUser = Repository.GetAdminById(expectedUser.Id);

			// Assert
			Assert.IsTrue(noUser == null);

			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void GetUserByActivationKey_With_InactiveUser()
		{
			// Arrange
			User expectedUser = _inactiveUser;

			// Act
			User noUser = Repository.GetUserByActivationKey("badkey");
			User actualUser = Repository.GetUserByActivationKey(expectedUser.ActivationKey);

			// Assert
			Assert.IsTrue(noUser == null);

			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void GetUserByActivationKey_With_ActiveUser()
		{
			// Arrange
			User expectedUser = _adminUser;

			// Act
			User actualUser = Repository.GetUserByActivationKey(expectedUser.ActivationKey);

			// Assert
			Assert.IsTrue(actualUser == null);
		}

		[TestMethod]
		public void GetEditorById()
		{
			// Arrange
			User expectedUser = _editor;

			// Act
			User noUser = Repository.GetEditorById(Guid.Empty);
			User actualUser = Repository.GetEditorById(_editor.Id);
			User adminUser = Repository.GetEditorById(_adminUser.Id);

			// Assert
			Assert.IsTrue(noUser == null);
			Assert.IsTrue(adminUser != null);

			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void GetUserByEmail()
		{
			// Arrange
			User expectedUser = _editor;

			// Act
			User noUser = Repository.GetUserByEmail("invalid@email.com");
			User actualUser = Repository.GetUserByEmail(_editor.Email);

			// Assert
			Assert.IsTrue(noUser == null);

			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void GetUserByEmail_With_Inactive_User_And_No_Flag_Set_Should_Return_User()
		{
			// Arrange
			User expectedUser = _inactiveUser;

			// Act
			User actualUser = Repository.GetUserByEmail(_inactiveUser.Email);

			// Assert
			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void GetUserByEmail_With_Inactive_User_And_Active_Only_Flag_Should_Return_Null()
		{
			// Arrange

			// Act
			User actualUser = Repository.GetUserByEmail(_inactiveUser.Email, true);

			// Assert
			Assert.IsTrue(actualUser == null);
		}

		[TestMethod]
		public void GetUserById()
		{
			// Arrange
			User expectedUser = _editor;

			// Act
			User noUser = Repository.GetUserById(Guid.Empty);
			User actualUser = Repository.GetUserById(_editor.Id);

			// Assert
			Assert.IsTrue(noUser == null);

			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void GetUserById_Should_Return_Null_When_User_Is_InActive_And_Active_Flag_Is_True()
		{
			// Arrange
			User expectedUser = null;

			// Act
			User actualUser = Repository.GetUserById(_inactiveUser.Id, true);

			// Assert
			Assert.IsTrue(actualUser == (expectedUser));
		}

		[TestMethod]
		public void GetUserById_Should_Return_User_When_User_Is_InActive_And_Flag_Is_Not_Set()
		{
			// Arrange
			User expectedUser = _inactiveUser;

			// Act
			User actualUser = Repository.GetUserById(_inactiveUser.Id);

			// Assert
			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void GetUserById_Should_Return_Null_For_Active_User_When_Flag_Is_False()
		{
			// Arrange
			User expectedUser = _inactiveUser;

			// Act
			User noUser = Repository.GetUserById(_editor.Id, false);

			// Assert
			Assert.IsTrue(noUser == null);
		}

		[TestMethod]
		public void GetUserByPasswordResetKey()
		{
			// Arrange
			User expectedUser = _editor;

			// Act
			User noUser = Repository.GetUserByUsername("badkey");
			User actualUser = Repository.GetUserByPasswordResetKey(_editor.PasswordResetKey);

			// Assert
			Assert.IsTrue(noUser == null);

			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void GetUserByUsername()
		{
			// Arrange
			User expectedUser = _editor;

			// Act
			User noUser = Repository.GetUserByUsername("nobody");
			User actualUser = Repository.GetUserByUsername(_editor.Username);

			// Assert
			Assert.IsTrue(noUser == null);

			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void GetUserByUsernameOrEmail()
		{
			// Arrange
			User expectedUser = _editor;

			// Act
			User noUser = Repository.GetUserByUsernameOrEmail("nobody", "nobody@nobody.com");
			User emailUser = Repository.GetUserByUsernameOrEmail("nousername", _editor.Email);
			User actualUser = Repository.GetUserByUsernameOrEmail(_editor.Username, "doesntexist@email.com");

			// Assert
			Assert.IsTrue(noUser == null);
			Assert.IsTrue(emailUser != null);

			Assert.IsTrue(actualUser.Id == (expectedUser.Id));
			Assert.IsTrue(actualUser.ActivationKey == (expectedUser.ActivationKey));
			Assert.IsTrue(actualUser.Email == (expectedUser.Email));
			Assert.IsTrue(actualUser.Firstname == (expectedUser.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (expectedUser.IsActivated));
			Assert.IsTrue(actualUser.IsAdmin == (expectedUser.IsAdmin));
			Assert.IsTrue(actualUser.IsEditor == (expectedUser.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (expectedUser.Lastname));
			Assert.IsTrue(actualUser.Password == (expectedUser.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (expectedUser.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (expectedUser.Salt));
		}

		[TestMethod]
		public void FindAllEditors()
		{
			// Arrange


			// Act
			List<User> allEditors = Repository.FindAllEditors().ToList();

			// Assert
			Assert.IsTrue(allEditors.Count == (3)); // includes the admin
		}

		[TestMethod]
		public void FindAllAdmins()
		{
			// Arrange


			// Act
			List<User> allEditors = Repository.FindAllAdmins().ToList();

			// Assert
			Assert.IsTrue(allEditors.Count == (1));
		}

		[TestMethod]
		public void DeleteUser()
		{
			// Arrange
			User user = Repository.GetUserByUsername("admin");
			Guid id = user.Id;

			// Act
			Repository.DeleteUser(user);

			// Assert
			Assert.IsTrue(Repository.GetUserById(user.Id) == null);
		}

		[TestMethod]
		public void DeleteAllUsers()
		{
			// Arrange


			// Act
			Repository.DeleteAllUsers();

			// Assert
			Assert.IsTrue(Repository.FindAllAdmins().Count() == (0));
			Assert.IsTrue(Repository.FindAllEditors().Count() == (0));
		}

		[TestMethod]
		public void SaveOrUpdateUser()
		{
			// Arrange
			User user = _adminUser;
			user.ActivationKey = "2key";
			user.Email = "2email@email.com";
			user.Firstname = "2firstname";
			user.IsActivated = true;
			user.IsEditor = true;
			user.Lastname = "2lastname";
			//user.Password = "2password";
			user.PasswordResetKey = "2passwordkey";
			user.Salt = "2salt";
			user.Username = "2username";

			// Act
			Repository.SaveOrUpdateUser(user);
			User actualUser = Repository.GetUserById(user.Id);

			// Assert
			Assert.IsTrue(actualUser.Id == (user.Id));
			Assert.IsTrue(actualUser.ActivationKey == (user.ActivationKey));
			Assert.IsTrue(actualUser.Firstname == (user.Firstname));
			Assert.IsTrue(actualUser.IsActivated == (user.IsActivated));
			Assert.IsTrue(actualUser.IsEditor == (user.IsEditor));
			Assert.IsTrue(actualUser.Lastname == (user.Lastname));
			Assert.IsTrue(actualUser.Password == (user.Password));
			Assert.IsTrue(actualUser.PasswordResetKey == (user.PasswordResetKey));
			Assert.IsTrue(actualUser.Salt == (user.Salt));
			Assert.IsTrue(actualUser.Username == (user.Username));
		}
	}
}

using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.Security;

namespace RegisterUser.UserBoundedContext
{
    [TestClass]
    public class RegisterUser
    {
        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // Create the test database
            var script = File.ReadAllText(
                @"C:\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\RegisterUser\Sql\CreateTestDatabase.sql");

            ScriptRunner.Run(ConnectionManager.GetConnection("Master").ConnectionString, script);
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        public void Register_User_Tests()
        {
            // Insert
            var commandAggregate = new RegisterUserCommandAggregate(new RegisterUserInputDto
            {
                Username = "user1",
                Email = "user1@example.com",
                Password = "P@$$1",
                ConfirmPassword = "P@$$1"
            });

            commandAggregate.Save();

            var userId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new GetUserByUserNameQueryAggregate();

            var userDto = queryAggregate.Get(username: "user1");

            Assert.AreEqual(userId, userDto.UserId);

            Assert.AreEqual("user1", userDto.Username);

            Assert.AreEqual("user1@example.com", userDto.Email);

            // The salt is generated dynamically at insert
            Assert.AreEqual(Password.ToHashBase64("P@$$1", userDto.PasswordSalt), userDto.PasswordHash); // Login successful

        }
    }
}

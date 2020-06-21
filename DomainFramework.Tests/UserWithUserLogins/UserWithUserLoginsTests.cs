using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UserWithUserLogins.UserBoundedContext
{
    [TestClass]
    public class UserWithUserLogins
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\UserWithUserLogins\Sql\CreateTestDatabase.sql");

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
        public void Save_User_Tests()
        {
            // Insert
            var createUserCommandAggregate = new CreateUserCommandAggregate(new CreateUserInputDto
            {
                UserName = "user",
                Email = "user@example.com",
                UserLogins = new List<UserLoginInputDto>
                {
                    new UserLoginInputDto
                    {
                        Provider = "provider1",
                        UserKey = "userKey1"
                    },
                    new UserLoginInputDto
                    {
                        Provider = "provider2",
                        UserKey = "userKey2"
                    }
                }
            });

            createUserCommandAggregate.Save();

            var userId = createUserCommandAggregate.RootEntity.Id;

            // Read
            var getUserByUserLoginQueryAggregate = new GetUserByUserLoginQueryAggregate();

            var userDto = getUserByUserLoginQueryAggregate.Get(provider: "provider2", userKey: "userKey2");

            Assert.AreEqual(userId, userDto.UserId);

            Assert.AreEqual("user", userDto.UserName);

            Assert.AreEqual("user@example.com", userDto.Email);

            Assert.AreEqual(2, userDto.UserLogins.Count());

            var userLoginDto = userDto.UserLogins.ElementAt(0);

            Assert.AreEqual("provider1", userLoginDto.Provider);

            Assert.AreEqual("userKey1", userLoginDto.UserKey);

            userLoginDto = userDto.UserLogins.ElementAt(1);

            Assert.AreEqual("provider2", userLoginDto.Provider);

            Assert.AreEqual("userKey2", userLoginDto.UserKey);

            // Add a user login
            var addUserLoginCommandAggregate = new AddUserLoginCommandAggregate(new UserAddUserLoginInputDto
            {
                UserId = userDto.UserId,
                UserLogins = new List<UserLoginInputDto>
                {
                    new UserLoginInputDto
                    {
                        Provider = "provider3",
                        UserKey = "userKey3"
                    }
                }
            });

            addUserLoginCommandAggregate.Save();

            // Read the user using the added user login
            getUserByUserLoginQueryAggregate = new GetUserByUserLoginQueryAggregate();

            userDto = getUserByUserLoginQueryAggregate.Get(provider: "provider3", userKey: "userKey3");

            Assert.AreEqual(userId, userDto.UserId);

            Assert.AreEqual("user", userDto.UserName);

            Assert.AreEqual("user@example.com", userDto.Email);

            Assert.AreEqual(3, userDto.UserLogins.Count());

            userLoginDto = userDto.UserLogins.ElementAt(0);

            Assert.AreEqual("provider1", userLoginDto.Provider);

            Assert.AreEqual("userKey1", userLoginDto.UserKey);

            userLoginDto = userDto.UserLogins.ElementAt(1);

            Assert.AreEqual("provider2", userLoginDto.Provider);

            Assert.AreEqual("userKey2", userLoginDto.UserKey);

            userLoginDto = userDto.UserLogins.ElementAt(2);

            Assert.AreEqual("provider3", userLoginDto.Provider);

            Assert.AreEqual("userKey3", userLoginDto.UserKey);

            // Read the user by the normalized email
            var getUserByNormalizedEmailQueryAggregate = new GetUserByNormalizedEmailQueryAggregate();

            userDto = getUserByNormalizedEmailQueryAggregate.Get(email: "user@example.com");

            Assert.AreEqual(userId, userDto.UserId);

            Assert.AreEqual("user", userDto.UserName);

            Assert.AreEqual("user@example.com", userDto.Email);

            Assert.AreEqual(3, userDto.UserLogins.Count());

            userLoginDto = userDto.UserLogins.ElementAt(0);

            Assert.AreEqual("provider1", userLoginDto.Provider);

            Assert.AreEqual("userKey1", userLoginDto.UserKey);

            userLoginDto = userDto.UserLogins.ElementAt(1);

            Assert.AreEqual("provider2", userLoginDto.Provider);

            Assert.AreEqual("userKey2", userLoginDto.UserKey);

            userLoginDto = userDto.UserLogins.ElementAt(2);

            Assert.AreEqual("provider3", userLoginDto.Provider);

            Assert.AreEqual("userKey3", userLoginDto.UserKey);

            //// Delete
            //var deleteAggregate = new DeleteUserCommandAggregate(new DeleteUserInputDto
            //{
            //    Id = userId.Value
            //});

            //deleteAggregate.Save();

            //userDto = queryAggregate.Get(userId);

            //Assert.IsNull(userDto);
        }
    }
}

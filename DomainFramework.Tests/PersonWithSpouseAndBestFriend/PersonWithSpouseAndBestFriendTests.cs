using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.Security;

namespace PersonWithSpouseAndBestFriend.PersonBoundedContext
{
    [TestClass]
    public class PersonWithSpouseAndBestFriend
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
                @"C:\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\PersonWithSpouseAndBestFriend\Sql\CreateTestDatabase.sql");

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
        public void Person_With_Spouse_And_Best_Friend_Tests()
        {
            // Insert
            var commandAggregate = new SavePersonCommandAggregate(new SavePersonInputDto
            {
                Name = "Fiona",
                Gender = 'F',
                MarriedTo = new SavePersonInputDto
                {
                    Name = "Shrek",
                    Gender = 'M',

                },
                BestFriendOf = new SavePersonInputDto
                {
                    Name = "Donkey",
                    Gender = 'M',
                }
            });

            commandAggregate.Save();

            var personId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new PersonQueryAggregate();

            var fionaDto = queryAggregate.Get(personId);

            Assert.AreEqual(personId, fionaDto.PersonId);

            Assert.AreEqual("Fiona", fionaDto.Name);

            Assert.AreEqual("Shrek", fionaDto.MarriedTo.Name);

            Assert.AreEqual("Donkey", fionaDto.BestFriendOf.Name);

            // Test the bi-directionality of the MarriedTo association
            queryAggregate = new PersonQueryAggregate();

            var shrekDto = queryAggregate.Get(fionaDto.MarriedTo.PersonId);

            Assert.AreEqual("Shrek", shrekDto.Name);

            Assert.AreEqual("Fiona", shrekDto.MarriedTo.Name);

            // Test the bi-directionality of the BestFriendOf association
            queryAggregate = new PersonQueryAggregate();

            var donkeyDto = queryAggregate.Get(fionaDto.BestFriendOf.PersonId);

            Assert.AreEqual("Donkey", donkeyDto.Name);

            Assert.AreEqual("Fiona", donkeyDto.BestFriendOf.Name);

        }
    }
}

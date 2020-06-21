using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.Security;

namespace PersonWithSpouse.PersonBoundedContext
{
    [TestClass]
    public class PersonWithSpouse
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\PersonWithSpouse\Sql\CreateTestDatabase.sql");

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
        public void Person_With_Spouse_Tests()
        {
            // Insert
            var commandAggregate = new SavePersonCommandAggregate(new PersonInputDto
            {
                Name = "Fiona",
                Gender = 'F',
                MarriedTo = new PersonInputDto
                {
                    Name = "Shrek",
                    Gender = 'M'
                }
            });

            commandAggregate.Save();

            var personId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new GetPersonByIdQueryAggregate();

            var personDto = queryAggregate.Get(personId);

            Assert.AreEqual(personId, personDto.PersonId);

            Assert.AreEqual("Fiona", personDto.Name);

            Assert.AreEqual("Shrek", personDto.MarriedTo.Name);

            // Test the bi-directionality of the association
            queryAggregate = new GetPersonByIdQueryAggregate();

            personDto = queryAggregate.Get(personDto.MarriedTo.PersonId);

            Assert.AreEqual("Shrek", personDto.Name);

            Assert.AreEqual("Fiona", personDto.MarriedTo.Name);

        }
    }
}

using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.Security;

namespace PersonWithSpouseAndDependants.PersonBoundedContext
{
    [TestClass]
    public class PersonWithSpouseAndDependants
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\PersonWithSpouseAndDependants\Sql\CreateTestDatabase.sql");

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
        public void Person_With_Spouse_And_Dependants_Tests()
        {
            // Insert
            var commandAggregate = new SavePersonCommandAggregate(new SavePersonInputDto
            {
                Name = "Fiona",
                Spouse = new SavePersonInputDto
                {
                    Name = "Shrek"
                },
                Dependants = new List<SavePersonInputDto>
                {
                    new SavePersonInputDto
                    {
                        Name = "Fionita"
                    },
                    new SavePersonInputDto
                    {
                        Name = "Shrekito"
                    }
                }
            });

            commandAggregate.Save();

            var personId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new PersonQueryAggregate();

            var personDto = queryAggregate.Get(personId);

            Assert.AreEqual(personId, personDto.PersonId);

            Assert.AreEqual("Fiona", personDto.Name);

            Assert.AreEqual("Shrek", personDto.Spouse.Name);

            Assert.AreEqual(2, personDto.Dependants.Count());

            var dependant = personDto.Dependants.First();

            Assert.AreEqual("Fionita", dependant.Name);

            dependant = personDto.Dependants.Last();

            Assert.AreEqual("Shrekito", dependant.Name);

            // Test the bi-directionality of the association
            queryAggregate = new PersonQueryAggregate();

            personDto = queryAggregate.Get(personDto.Spouse.PersonId);

            Assert.AreEqual("Shrek", personDto.Name);

            Assert.AreEqual("Fiona", personDto.Spouse.Name);

        }
    }
}

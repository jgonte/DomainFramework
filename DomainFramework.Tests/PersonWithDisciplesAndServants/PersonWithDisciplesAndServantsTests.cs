using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.Security;

namespace PersonWithDisciplesAndServants.PersonBoundedContext
{
    [TestClass]
    public class PersonWithDisciplesAndServants
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\PersonWithDisciplesAndServants\Sql\CreateTestDatabase.sql");

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
        public void Person_With_Disciples_And_Servants_Tests()
        {
            // Insert
            var commandAggregate = new SavePersonCommandAggregate(new PersonInputDto
            {
                Name = "Master Shifu",
                Gender = 'M',
                Disciples = new List<PersonInputDto>
                {
                    new PersonInputDto
                    {
                        Name = "Tigress",
                        Gender = 'F'
                    },
                    new PersonInputDto
                    {
                        Name = "Po",
                        Gender = 'M'
                    },
                    new PersonInputDto
                    {
                        Name = "Mantis",
                        Gender = 'M'
                    }
                },
                Servants = new List<PersonInputDto>
                {
                    new PersonInputDto
                    {
                        Name = "Fiona",
                        Gender = 'F'
                    },
                    new PersonInputDto
                    {
                        Name = "Shrek",
                        Gender = 'M'
                    },
                    new PersonInputDto
                    {
                        Name = "Donkey",
                        Gender = 'M'
                    }
                }
            });

            commandAggregate.Save();

            var personId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new GetPersonByIdQueryAggregate();

            var personDto = queryAggregate.Get(personId);

            Assert.AreEqual(personId, personDto.PersonId);

            Assert.AreEqual("Master Shifu", personDto.Name);

            var disciple = personDto.Disciples.ElementAt(0);

            Assert.AreEqual("Tigress", disciple.Name);

            disciple = personDto.Disciples.ElementAt(1);

            Assert.AreEqual("Po", disciple.Name);

            disciple = personDto.Disciples.ElementAt(2);

            Assert.AreEqual("Mantis", disciple.Name);

            var servant = personDto.Servants.ElementAt(0);

            Assert.AreEqual("Fiona", servant.Name);

            servant = personDto.Servants.ElementAt(1);

            Assert.AreEqual("Shrek", servant.Name);

            servant = personDto.Servants.ElementAt(2);

            Assert.AreEqual("Donkey", servant.Name);

        }
    }
}

using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.Security;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    [TestClass]
    public class OrganizationPersonWithCommonEntities
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\OrganizationPersonWithCommonEntities\Sql\CreateTestDatabase.sql");

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
        public void Organization_Person_With_Common_Entities_Tests()
        {
            // Person
            // Insert
            var personCommandAggregate = new SavePersonCommandAggregate(new SavePersonInputDto
            {
                Name = "Fiona",
                Phones = new List<SavePhoneInputDto>
                {
                    new SavePhoneInputDto
                    {
                        Number = "3051112233"
                    },
                    new SavePhoneInputDto
                    {
                       Number = "3051112234"
                    }
                },
                Address = new SaveAddressInputDto
                {
                    Street = "123 Main St"
                }
            });

            personCommandAggregate.Save();

            var personId = personCommandAggregate.RootEntity.Id;

            // Read
            var personQueryAggregate = new PersonQueryAggregate();

            var personDto = personQueryAggregate.Get(personId);

            Assert.AreEqual(personId, personDto.PersonId);

            Assert.AreEqual("Fiona", personDto.Name);

            var phone = personDto.Phones.ElementAt(0);

            Assert.AreEqual("3051112233", phone.Number);

            phone = personDto.Phones.ElementAt(1);

            Assert.AreEqual("3051112234", phone.Number);

            Assert.AreEqual("123 Main St", personDto.Address.Street);

            // Organization
            // Insert
            var organizationCommandAggregate = new SaveOrganizationCommandAggregate(new SaveOrganizationInputDto
            {
                Name = "My Company Inc.",
                Phones = new List<SavePhoneInputDto>
                {
                    new SavePhoneInputDto
                    {
                        Number = "3051112200"
                    },
                    new SavePhoneInputDto
                    {
                       Number = "3051112201"
                    }
                },
                Address = new SaveAddressInputDto
                {
                    Street = "345 Main St"
                }
            });

            organizationCommandAggregate.Save();

            var organizationId = organizationCommandAggregate.RootEntity.Id;

            // Read
            var organizationQueryAggregate = new OrganizationQueryAggregate();

            var organizationDto = organizationQueryAggregate.Get(organizationId);

            Assert.AreEqual(organizationId, organizationDto.OrganizationId);

            Assert.AreEqual("My Company Inc.", organizationDto.Name);

            phone = organizationDto.Phones.ElementAt(0);

            Assert.AreEqual("3051112200", phone.Number);

            phone = organizationDto.Phones.ElementAt(1);

            Assert.AreEqual("3051112201", phone.Number);

            Assert.AreEqual("345 Main St", organizationDto.Address.Street);

        }
    }
}

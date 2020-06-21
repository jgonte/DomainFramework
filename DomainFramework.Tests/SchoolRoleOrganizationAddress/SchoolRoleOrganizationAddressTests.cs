using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.IO;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    [TestClass]
    public class SchoolRoleOrganizationAddressTests
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\SchoolRoleOrganizationAddress\Sql\CreateTestDatabase.sql");

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
        public void School_Organization_Address_Tests()
        {
            // Configure the user
            var commandAggregate = new SaveSchoolCommandAggregate(new SaveSchoolInputDto
            {
                Organization = new OrganizationInputDto
                {
                    Name = "School1",
                    OrganizationRole = new OrganizationRoleInputDto
                    {
                    },
                    Address = new AddressInputDto
                    {
                        Street = "Street1"
                    },
                    Phone = new PhoneInputDto
                    {
                        Number = "3051112233"
                    }
                },
                IsCharter = true
            });

            commandAggregate.Save();

            var schoolId = commandAggregate.RootEntity.Id;

            var queryAggregate = new GetByIdSchoolQueryAggregate();

            var school = queryAggregate.Get(schoolId);

            Assert.IsTrue(school.IsCharter);

            Assert.AreEqual(1, school.Organization.OrganizationId);

            Assert.AreEqual("School1", school.Organization.Name);

            Assert.AreEqual(1, school.Organization.Address.AddressId);

            Assert.AreEqual("Street1", school.Organization.Address.Street);

            Assert.AreEqual("3051112233", school.Organization.Phone.Number);

        }
    }
}

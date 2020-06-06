using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    [TestClass]
    public class EmployeeWithDependants
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\EmployeeWithDependants\Sql\CreateTestDatabase.sql");

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
        public void EmployeeWithDependants_Tests()
        {
            // Configure the user
            var commandAggregate = new SaveEmployeeCommandAggregate(new SaveEmployeeInputDto
            {
                Name = "Yana",
                HireDate = new DateTime(2019, 9, 8),
                CellPhone = new PhoneNumberInputDto
                {
                    AreaCode = "305",
                    Exchange = "678",
                    Number = "9900"
                },
                Dependants = new List<PersonInputDto>
                {
                    new PersonInputDto
                    {
                        Name = "Sarah",
                        CellPhone = new PhoneNumberInputDto
                        {
                            AreaCode= "305",
                            Exchange = "555",
                            Number = "7890"
                        }
                    },
                    new PersonInputDto
                    {
                        Name = "Mark",
                        CellPhone = new PhoneNumberInputDto
                        {
                            AreaCode= "786",
                            Exchange = "777",
                            Number = "1234"
                        }
                    }
                }
            });

            commandAggregate.Save();

            var employeeId = commandAggregate.RootEntity.Id;

            var queryAggregate = new GetByIdEmployeeQueryAggregate();

            var employee = queryAggregate.Get(employeeId);

            Assert.AreEqual("Yana", employee.Name);

            Assert.AreEqual(new DateTime(2019, 9, 8), employee.HireDate);

            var cellPhone = employee.CellPhone;

            Assert.AreEqual("305", cellPhone.AreaCode);

            Assert.AreEqual("678", cellPhone.Exchange);

            Assert.AreEqual("9900", cellPhone.Number);

            var dependants = employee.Dependants;

            Assert.AreEqual(2, dependants.Count());

            var dependant = dependants.ElementAt(0);

            Assert.AreEqual("Sarah", dependant.Name);

            cellPhone = dependant.CellPhone;

            Assert.AreEqual("305", cellPhone.AreaCode);

            Assert.AreEqual("555", cellPhone.Exchange);

            Assert.AreEqual("7890", cellPhone.Number);
        }

        [TestMethod]
        public async Task EmployeeWithDependants_Async_Tests()
        {
            // Configure the user
            var commandAggregate = new SaveEmployeeCommandAggregate(new SaveEmployeeInputDto
            {
                Name = "Yana",
                HireDate = new DateTime(2019, 9, 8),
                CellPhone = new PhoneNumberInputDto
                {
                    AreaCode = "305",
                    Exchange = "678",
                    Number = "9900"
                },
                Dependants = new List<PersonInputDto>
                {
                    new PersonInputDto
                    {
                        Name = "Sarah",
                        CellPhone = new PhoneNumberInputDto
                        {
                            AreaCode= "305",
                            Exchange = "555",
                            Number = "7890"
                        }
                    },
                    new PersonInputDto
                    {
                        Name = "Mark",
                        CellPhone = new PhoneNumberInputDto
                        {
                            AreaCode= "786",
                            Exchange = "777",
                            Number = "1234"
                        }
                    }
                }
            });

            await commandAggregate.SaveAsync();

            var employeeId = commandAggregate.RootEntity.Id;

            var queryAggregate = new GetByIdEmployeeQueryAggregate();

            var employee = await queryAggregate.GetAsync(employeeId);

            Assert.AreEqual("Yana", employee.Name);

            Assert.AreEqual(new DateTime(2019, 9, 8), employee.HireDate);

            var cellPhone = employee.CellPhone;

            Assert.AreEqual("305", cellPhone.AreaCode);

            Assert.AreEqual("678", cellPhone.Exchange);

            Assert.AreEqual("9900", cellPhone.Number);

            var dependants = employee.Dependants;

            Assert.AreEqual(2, dependants.Count());

            var dependant = dependants.ElementAt(0);

            Assert.AreEqual("Sarah", dependant.Name);

            cellPhone = dependant.CellPhone;

            Assert.AreEqual("305", cellPhone.AreaCode);

            Assert.AreEqual("555", cellPhone.Exchange);

            Assert.AreEqual("7890", cellPhone.Number);

            dependant = dependants.ElementAt(1);

            Assert.AreEqual("Mark", dependant.Name);

            cellPhone = dependant.CellPhone;

            Assert.AreEqual("786", cellPhone.AreaCode);

            Assert.AreEqual("777", cellPhone.Exchange);

            Assert.AreEqual("1234", cellPhone.Number);

        }
    }
}

using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.IO;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    [TestClass]
    public class EmployeeWithSpouseTests
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\EmployeeWithSpouse\Sql\CreateTestDatabase.sql"
            );

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
        public void EmployeeWithSpouse_Save_Tests()
        {
            // Insert
            var commandAggregate = new SaveEmployeeCommandAggregate(new SaveEmployeeInputDto
            {
                Name = "George",
                CellPhone = new PhoneNumberInputDto
                {
                    AreaCode = "305",
                    Exchange = "234",
                    Number = "1234"
                },
                HireDate = new DateTime(2015, 3, 27)
            });

            commandAggregate.Save();

            var employeeId = commandAggregate.RootEntity.Id;

            // Read
            var employeeQueryAggregate = new EmployeeQueryAggregate();

            var employeeDto = employeeQueryAggregate.Get(employeeId);

            Assert.AreEqual(employeeId, employeeDto.Id);

            Assert.AreEqual("George", employeeDto.Name);

            var cellPhone = employeeDto.CellPhone;

            Assert.AreEqual("305", cellPhone.AreaCode);

            Assert.AreEqual("234", cellPhone.Exchange);

            Assert.AreEqual("1234", cellPhone.Number);

            Assert.AreEqual(new DateTime(2015, 3, 27), employeeDto.HireDate);

            Assert.IsNull(employeeDto.Spouse);

            // Update
            commandAggregate = new SaveEmployeeCommandAggregate(new SaveEmployeeInputDto
            {
                EmployeeId = employeeId,
                Name = "Jorge",
                CellPhone = new PhoneNumberInputDto
                {
                    AreaCode = "786",
                    Exchange = "567",
                    Number = "5678"
                },
                HireDate = new DateTime(2016, 4, 28),
                Spouse = new PersonInputDto
                {
                    Name = "Yana",
                    CellPhone = new PhoneNumberInputDto
                    {
                        AreaCode = "305",
                        Exchange = "890",
                        Number = "8900"
                    }
                }
            });

            commandAggregate.Save();

            // Read
            employeeQueryAggregate = new EmployeeQueryAggregate();

            employeeDto = employeeQueryAggregate.Get(employeeId);

            Assert.AreEqual(employeeId, employeeDto.Id);

            Assert.AreEqual("Jorge", employeeDto.Name);

            cellPhone = employeeDto.CellPhone;

            Assert.AreEqual("786", cellPhone.AreaCode);

            Assert.AreEqual("567", cellPhone.Exchange);

            Assert.AreEqual("5678", cellPhone.Number);

            Assert.AreEqual(new DateTime(2016, 4, 28), employeeDto.HireDate);

            var spouseDto = employeeDto.Spouse;

            Assert.AreEqual("Yana", spouseDto.Name);

            cellPhone = spouseDto.CellPhone;

            Assert.AreEqual("305", cellPhone.AreaCode);

            Assert.AreEqual("890", cellPhone.Exchange);

            Assert.AreEqual("8900", cellPhone.Number);

            // Read polymorphic
            //var personQueryAggregate = new PersonQueryAggregate();

            //employeeDto = (EmployeeOutputDto)personQueryAggregate.Get(employeeId);

            //Assert.AreEqual(employeeId, employeeDto.Id);

            //Assert.AreEqual("Jorge", employeeDto.Name);

            //cellPhone = employeeDto.CellPhone;

            //Assert.AreEqual("786", cellPhone.AreaCode);

            //Assert.AreEqual("567", cellPhone.Exchange);

            //Assert.AreEqual("5678", cellPhone.Number);

            //Assert.AreEqual(new DateTime(2016, 4, 28), employeeDto.HireDate);

            //spouseDto = employeeDto.Spouse;

            //Assert.AreEqual("Yana", spouseDto.Name);

            //cellPhone = spouseDto.CellPhone;

            //Assert.AreEqual("305", cellPhone.AreaCode);

            //Assert.AreEqual("890", cellPhone.Exchange);

            //Assert.AreEqual("8900", cellPhone.Number);

            // Delete
            //var deleteAggregate = new DeleteEmployeeCommandAggregate(new DeleteEmployeeInputDto
            //{
            //    Id = countryId.Value
            //});

            //deleteAggregate.Save();

            //countryDto = queryAggregate.Get(countryId);

            //Assert.IsNull(countryDto);
        }
    }
}

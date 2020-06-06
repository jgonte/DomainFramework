using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.IO;

namespace EmployeePerson.EmployeeBoundedContext
{
    [TestClass]
    public class EmployeePersonTests
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\EmployeePerson\Sql\CreateTestDatabase.sql"
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
        public void Save_Employee_Only_Tests()
        {
            // Insert
            var commandAggregate = new SaveEmployeeCommandAggregate(new SaveEmployeeInputDto
            {
                Name = "Pablito",
                Gender = 'M',
                HireDate = new DateTime(1978, 3, 27),
                CellPhone = new PhoneNumberInputDto
                {
                    AreaCode = "305",
                    Exchange = "111",
                    Number = "1234"
                }
            });

            commandAggregate.Save();

            var employeeId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new EmployeeQueryAggregate();

            var employeeDto = queryAggregate.Get(employeeId);

            Assert.AreEqual(employeeId, employeeDto.Id);

            Assert.AreEqual("Pablito", employeeDto.Name);

            Assert.AreEqual('M', employeeDto.Gender);

            Assert.AreEqual(new DateTime(1978, 3, 27), employeeDto.HireDate);

            Assert.AreEqual("305", employeeDto.CellPhone.AreaCode);

            Assert.AreEqual("111", employeeDto.CellPhone.Exchange);

            Assert.AreEqual("1234", employeeDto.CellPhone.Number);

            // Update
            commandAggregate = new SaveEmployeeCommandAggregate(new SaveEmployeeInputDto
            {
                EmployeeId = employeeId,
                Name = "Camila",
                Gender = 'F',
                HireDate = new DateTime(1988, 4, 28),
                CellPhone = new PhoneNumberInputDto
                {
                    AreaCode = "786",
                    Exchange = "222",
                    Number = "5678"
                }
            });

            commandAggregate.Save();

            // Read
            queryAggregate = new EmployeeQueryAggregate();

            employeeDto = queryAggregate.Get(employeeId);

            Assert.AreEqual(employeeId, employeeDto.Id);

            Assert.AreEqual("Camila", employeeDto.Name);

            Assert.AreEqual('F', employeeDto.Gender);

            Assert.AreEqual(new DateTime(1988, 4, 28), employeeDto.HireDate);

            Assert.AreEqual("786", employeeDto.CellPhone.AreaCode);

            Assert.AreEqual("222", employeeDto.CellPhone.Exchange);

            Assert.AreEqual("5678", employeeDto.CellPhone.Number);


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

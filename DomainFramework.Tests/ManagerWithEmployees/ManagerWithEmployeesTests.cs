using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    [TestClass]
    public class ManagerWithEmployees
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\ManagerWithEmployees\Sql\CreateTestDatabase.sql");

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
        public void Save_Manager_Only_Tests()
        {
            // Insert
            var commandAggregate = new SaveManagerCommandAggregate(new SaveManagerInputDto
            {
                Department = "IT",
                Name = "Eli"
            });

            commandAggregate.Save();

            var managerId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new ManagerQueryAggregate();

            var managerDto = queryAggregate.Get(managerId);

            Assert.AreEqual(managerId, managerDto.Id);

            Assert.AreEqual("IT", managerDto.Department);

            Assert.AreEqual("Eli", managerDto.Name);

            Assert.IsNull(managerDto.Employees);

            // Update
            commandAggregate = new SaveManagerCommandAggregate(new SaveManagerInputDto
            {
                Id = managerId,
                Department = "Information Technology",
                Name = "Eliahu"
            });

            commandAggregate.Save();

            // Read
            queryAggregate = new ManagerQueryAggregate();

            managerDto = queryAggregate.Get(managerId);

            Assert.AreEqual(managerId, managerDto.Id);

            Assert.AreEqual("Information Technology", managerDto.Department);

            Assert.AreEqual("Eliahu", managerDto.Name);

            Assert.IsNull(managerDto.Employees);

            // Delete
            //var deleteAggregate = new DeleteManagerCommandAggregate(new DeleteManagerInputDto
            //{
            //    Id = managerId.Value
            //});

            //deleteAggregate.Save();

            //managerDto = queryAggregate.Get(managerId);

            //Assert.IsNull(managerDto);
        }

        [TestMethod]
        public void Save_Manager_With_Employees_Tests()
        {
            // Insert
            var commandAggregate = new SaveManagerCommandAggregate(new SaveManagerInputDto
            {
                Department = "IT",
                Name = "Eli",
                Employees = new List<EmployeeInputDto>
                {
                    new EmployeeInputDto
                    {
                        Name = "Yana"
                    },
                    new EmployeeInputDto
                    {
                        Name = "Mark"
                    },
                    new EmployeeInputDto
                    {
                        Name = "Sarah"
                    }
                }
            });

            commandAggregate.Save();

            var managerId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new ManagerQueryAggregate();

            var managerDto = queryAggregate.Get(managerId);

            Assert.AreEqual(managerId, managerDto.Id);

            Assert.AreEqual("IT", managerDto.Department);

            Assert.AreEqual("Eli", managerDto.Name);

            Assert.AreEqual(3, managerDto.Employees.Count());

            var employeeDto = managerDto.Employees.ElementAt(0);

            Assert.AreEqual("Yana", employeeDto.Name);

            Assert.AreEqual(managerDto.Id, employeeDto.SupervisorId);

            employeeDto = managerDto.Employees.ElementAt(1);

            Assert.AreEqual("Mark", employeeDto.Name);

            Assert.AreEqual(managerDto.Id, employeeDto.SupervisorId);

            employeeDto = managerDto.Employees.ElementAt(2);

            Assert.AreEqual("Sarah", employeeDto.Name);

            Assert.AreEqual(managerDto.Id, employeeDto.SupervisorId);

            // Update
            commandAggregate = new SaveManagerCommandAggregate(new SaveManagerInputDto
            {
                Id = managerId,
                Department = "Information Technology",
                Name = "Eliahu",
                Employees = new List<EmployeeInputDto>
                {
                    new EmployeeInputDto
                    {
                        Name = "Jorge"
                    },
                    new EmployeeInputDto
                    {
                        Name = "Moshe"
                    },
                    new EmployeeInputDto
                    {
                        Name = "Daphni"
                    }
                }
            });

            commandAggregate.Save();

            // Read
            queryAggregate = new ManagerQueryAggregate();

            managerDto = queryAggregate.Get(managerId);

            Assert.AreEqual(managerId, managerDto.Id);

            Assert.AreEqual("Information Technology", managerDto.Department);

            Assert.AreEqual("Eliahu", managerDto.Name);

            employeeDto = managerDto.Employees.ElementAt(0);

            Assert.AreEqual("Jorge", employeeDto.Name);

            Assert.AreEqual(managerDto.Id, employeeDto.SupervisorId);

            employeeDto = managerDto.Employees.ElementAt(1);

            Assert.AreEqual("Moshe", employeeDto.Name);

            Assert.AreEqual(managerDto.Id, employeeDto.SupervisorId);

            employeeDto = managerDto.Employees.ElementAt(2);

            Assert.AreEqual("Daphni", employeeDto.Name);

            Assert.AreEqual(managerDto.Id, employeeDto.SupervisorId);

            // Delete
            //var deleteAggregate = new DeleteManagerCommandAggregate(new DeleteManagerInputDto
            //{
            //    Id = managerId.Value
            //});

            //deleteAggregate.Save();

            //managerDto = queryAggregate.Get(managerId);

            //Assert.IsNull(managerDto);
        }
    }
}

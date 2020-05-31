using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.IO;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    [TestClass]
    public class ExecutiveEmployeePersonCustomerTests
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\ExecutiveEmployeePersonCustomer\Sql\CreateTestDatabase.sql"
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
        public void Save_Executive_Only_Tests()
        {
            // Insert
            var commandAggregate = new SaveExecutiveCommandAggregate(new SaveExecutiveInputDto
            {
                Name = "Bill",
                HireDate = new DateTime(1978, 3, 27),
                Bonus = 1000000.00m,
                Asset = new AssetInputDto
                {
                    Number = 123
                }
            });

            commandAggregate.Save();

            var executiveId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new GetExecutiveByIdQueryAggregate();

            var executiveDto = queryAggregate.Get(executiveId);

            Assert.AreEqual(executiveId, executiveDto.Id);

            Assert.AreEqual("Bill", executiveDto.Name);

            Assert.AreEqual(new DateTime(1978, 3, 27), executiveDto.HireDate);

            Assert.AreEqual(1000000.00m, executiveDto.Bonus);

            Assert.AreEqual(123, executiveDto.Asset.Number);

            // Update
            commandAggregate = new SaveExecutiveCommandAggregate(new SaveExecutiveInputDto
            {
                ExecutiveId = executiveId,
                Name = "William",
                HireDate = new DateTime(1988, 4, 28),
                Bonus = 2000000.00m,
                Asset = new AssetInputDto
                {
                    Number = 345
                }
            });

            commandAggregate.Save();

            // Read
            queryAggregate = new GetExecutiveByIdQueryAggregate();

            executiveDto = queryAggregate.Get(executiveId);

            Assert.AreEqual(executiveId, executiveDto.Id);

            Assert.AreEqual("William", executiveDto.Name);

            Assert.AreEqual(new DateTime(1988, 4, 28), executiveDto.HireDate);

            Assert.AreEqual(2000000.00m, executiveDto.Bonus);

            Assert.AreEqual(345, executiveDto.Asset.Number);

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

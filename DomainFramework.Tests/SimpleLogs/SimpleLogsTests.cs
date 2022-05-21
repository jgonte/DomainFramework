using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    [TestClass]
    public class SimpleLogsTests
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
                @"C:\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\SimpleLogs\Sql\CreateTestDatabase.sql");

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
        public void Simple_Logs_Test()
        {
            DateTime? dateTime = null;

            // Insert 5 logs
            for (int i = 0; i < 5; ++i)
            {
                var insertLogCommandAggregate = new SimpleLogCommandAggregate(new SimpleLogInputDto
                {
                    MessageType = "W",
                    Message = $"Log waning {i + 1}"
                });

                insertLogCommandAggregate.Save();

                if (i == 2) // Get the time after the inserted 3rd so we can delete the last two later
                {
                    Thread.Sleep(200);

                    dateTime = DateTime.Now;
                }
            }
            
            // Read them
            var getSimpleLogsQueryAggregate = new GetSimpleLogsQueryAggregate();

            var (count, logs) = getSimpleLogsQueryAggregate.Get(queryParameters: null);

            Assert.AreEqual(5, count);

            // Delete the first 3 logs
            var deleteAggregate = new DeleteOlderLogsCommandAggregate(
                new DeleteOlderLogsInputDto
                {
                    When = dateTime.Value
                }
            );

            deleteAggregate.Save();

            // Read them
            getSimpleLogsQueryAggregate = new GetSimpleLogsQueryAggregate();

            (count, logs) = getSimpleLogsQueryAggregate.Get(queryParameters: null);

            Assert.AreEqual(2, count);
        }
    }
}

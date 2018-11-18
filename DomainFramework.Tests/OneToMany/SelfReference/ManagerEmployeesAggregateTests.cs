using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    [TestClass]
    public class ManagerEmployeesAggregateTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkOneToManySelfReferenceTest.ConnectionString";

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static async Task MyClassInitialize(TestContext testContext)
        {
            // Test script executor (create database)
            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection("Master"),
@"
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'DomainFrameworkOneToManySelfReferenceTest'
)
BEGIN
    DROP DATABASE DomainFrameworkOneToManySelfReferenceTest
END
GO

CREATE DATABASE DomainFrameworkOneToManySelfReferenceTest
GO

USE DomainFrameworkOneToManySelfReferenceTest
GO

CREATE TABLE DomainFrameworkOneToManySelfReferenceTest..Person(
    [PersonId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL,
    [ManagerId] INT
)

ALTER TABLE DomainFrameworkOneToManySelfReferenceTest..Person
ADD CONSTRAINT Person_PK PRIMARY KEY (PersonId)
GO

ALTER TABLE DomainFrameworkOneToManySelfReferenceTest..Person
ADD CONSTRAINT Manager_Employee_FK FOREIGN KEY (ManagerId) REFERENCES DomainFrameworkOneToManySelfReferenceTest..Person(PersonId);
GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
 @"
CREATE PROCEDURE [p_Person_Create]
    @firstName VARCHAR(50),
    @managerId INT
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [PersonId] INT NOT NULL
    );

    INSERT INTO Person
    (
        [FirstName],
        [ManagerId]
    )
    OUTPUT
        INSERTED.[PersonId]
    INTO @outputData
    VALUES
    (
        @firstName,
        @managerId
    );

    SELECT
        [PersonId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Person_Get]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [PersonId] AS Id,
        [FirstName],
        [ManagerId]
    FROM [Person]
        WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [p_Person_GetEmployees]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        s.[PersonId] AS Id,
        s.[FirstName],
        s.[ManagerId]
    FROM [Person] s
    INNER JOIN [Person] p
        ON s.[ManagerId] = p.[PersonId]
    WHERE p.[PersonId] = @personId

END;
GO

CREATE PROCEDURE [p_Person_Update]
    @personId INT,
    @firstName VARCHAR(50),
    @managerId INT
AS
BEGIN

    UPDATE Person
    SET
        [FirstName] = @firstName, 
        [ManagerId] = @managerId
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [p_Person_Delete]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [Person]
        WHERE [PersonId] = @personId

END;
GO

",
                        "^GO");
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
        public async Task Manager_Employee_Aggregate_Async_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<PersonEntity3>(() => new PersonCommandRepository4());

            // Add two employees under Dafni
            var manager1Entity = new PersonEntity3
            {
                FirstName = "Dafni"
            };

            var commandAggregate = new ManagerEmployeesCommandAggregate(context, manager1Entity);

            commandAggregate.AddEmployee(new PersonEntity3
            {
                FirstName = "Jorge"
            });

            commandAggregate.AddEmployee(new PersonEntity3
            {
                FirstName = "Nurit"
            });

            await commandAggregate.SaveAsync();

            // Add two employees under Moshe
            var manager2Entity = new PersonEntity3
            {
                FirstName = "Moshe"
            };

            commandAggregate = new ManagerEmployeesCommandAggregate(context, manager2Entity);

            commandAggregate.AddEmployee(new PersonEntity3
            {
                FirstName = "Yoel"
            });

            commandAggregate.AddEmployee(new PersonEntity3
            {
                FirstName = "Vadim"
            });

            await commandAggregate.SaveAsync();

            // Add two employees under Mark
            var manager3Entity = new PersonEntity3
            {
                FirstName = "Mark"
            };

            commandAggregate = new ManagerEmployeesCommandAggregate(context, manager3Entity);

            commandAggregate.AddEmployee(manager1Entity);

            commandAggregate.AddEmployee(manager2Entity);

            await commandAggregate.SaveAsync();

            var rootManagerId = manager3Entity.Id;

            // Read

            context.RegisterQueryRepository<PersonEntity3>(new PersonQueryRepository4());

            var queryAggregate = new ManagerEmployeesQueryAggregate(context);

            await queryAggregate.LoadAsync(rootManagerId);

            var personEntity = queryAggregate.RootEntity;

            Assert.AreEqual(rootManagerId, personEntity.Id);

            Assert.AreEqual("Mark", personEntity.FirstName);

            Assert.AreEqual(2, queryAggregate.Employees.Count());

            var firstManagerEntity = queryAggregate.Employees.ElementAt(0);

            Assert.AreEqual("Dafni", firstManagerEntity.FirstName);

            var firstManagerQueryAggregate = new ManagerEmployeesQueryAggregate(context)
            {
                RootEntity = firstManagerEntity
            };

            await firstManagerQueryAggregate.LoadLinksAsync();

            Assert.AreEqual(2, firstManagerQueryAggregate.Employees.Count());

            Assert.AreEqual("Jorge", firstManagerQueryAggregate.Employees.ElementAt(0).FirstName);

            Assert.AreEqual("Nurit", firstManagerQueryAggregate.Employees.ElementAt(1).FirstName);

            var secondManagerEntity = queryAggregate.Employees.ElementAt(1);

            Assert.AreEqual("Moshe", secondManagerEntity.FirstName);

            var secondManagerQueryAggregate = new ManagerEmployeesQueryAggregate(context)
            {
                RootEntity = secondManagerEntity
            };

            await secondManagerQueryAggregate.LoadLinksAsync();

            Assert.AreEqual(2, secondManagerQueryAggregate.Employees.Count());

            Assert.AreEqual("Yoel", secondManagerQueryAggregate.Employees.ElementAt(0).FirstName);

            Assert.AreEqual("Vadim", secondManagerQueryAggregate.Employees.ElementAt(1).FirstName);
        }
    }
}

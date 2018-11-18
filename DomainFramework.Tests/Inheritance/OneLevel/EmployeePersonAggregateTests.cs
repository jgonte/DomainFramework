﻿using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    [TestClass]
    public class EmployeePersonAggregateTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkInheritanceOneLevelTest.ConnectionString";

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
    WHERE Name = N'DomainFrameworkInheritanceOneLevelTest'
)
BEGIN
    DROP DATABASE DomainFrameworkInheritanceOneLevelTest
END
GO

CREATE DATABASE DomainFrameworkInheritanceOneLevelTest
GO

USE DomainFrameworkInheritanceOneLevelTest
GO

CREATE TABLE DomainFrameworkInheritanceOneLevelTest..Person(
    [PersonId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL
)

ALTER TABLE DomainFrameworkInheritanceOneLevelTest..Person
ADD CONSTRAINT Person_PK PRIMARY KEY (PersonId)

CREATE TABLE DomainFrameworkInheritanceOneLevelTest..Employee(
    [EmployeeId] INT NOT NULL,
    [Salary] MONEY NOT NULL
)

ALTER TABLE DomainFrameworkInheritanceOneLevelTest..Employee
ADD CONSTRAINT Employee_PK PRIMARY KEY (EmployeeId)
GO

ALTER TABLE DomainFrameworkInheritanceOneLevelTest..Employee
ADD CONSTRAINT Employee_Person_FK FOREIGN KEY (EmployeeId) REFERENCES DomainFrameworkInheritanceOneLevelTest..Person(PersonId)
ON DELETE CASCADE;

GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
@"
CREATE PROCEDURE [p_Person_Create]
    @firstName VARCHAR(50)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [PersonId] INT NOT NULL
    );

    INSERT INTO Person
    (
        [FirstName]
    )
    OUTPUT
        INSERTED.[PersonId]
    INTO @outputData
    VALUES
    (
        @firstName
    );

    SELECT
        [PersonId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Employee_Create]
    @employeeId INT,
    @salary MONEY
AS
BEGIN
    INSERT INTO Employee
    (
        [EmployeeId],
        [Salary]
    )
    VALUES
    (
        @employeeId,
        @salary
    );
END;
GO

CREATE PROCEDURE [p_Employee_Get]
    @employeeId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        e.EmployeeId AS Id,
        e.[Salary]
    FROM [Employee] e
    WHERE e.[EmployeeId] = @employeeId

END;
GO

CREATE PROCEDURE [p_Person_Get]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        p.[FirstName]
    FROM [Person] p
    WHERE p.PersonId  = @personId

END;
GO

CREATE PROCEDURE [p_Employee_Update]
    @employeeId INT,
    @salary MONEY
AS
BEGIN

    UPDATE Employee
    SET
        [Salary] = @salary
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [p_Person_Update]
    @personId INT,
    @firstName VARCHAR(50)
AS
BEGIN

    UPDATE Person
    SET
        [FirstName] = @firstName
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [p_Employee_Delete]
    @employeeId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [Employee]
    WHERE [EmployeeId] = @employeeId

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
        public async Task Employee_Aggregate_Inherits_From_Person_Save_Async_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<PersonEntity>(() => new PersonCommandRepository());

            context.RegisterCommandRepositoryFactory<EmployeeEntity>(() => new EmployeeCommandRepository());

            // Insert

            var commandAggregate = new EmployeePersonCommandAggregate(context, firstName: "Johnny", salary: 65000);

            await commandAggregate.SaveAsync();

            var employeeEntity = commandAggregate.RootEntity;

            Assert.IsNotNull(employeeEntity.Id);

            var employeeId = employeeEntity.Id;

            // Read

            context.RegisterQueryRepository<EmployeeEntity>(new EmployeeQueryRepository());

            context.RegisterQueryRepository<PersonEntity>(new PersonQueryRepository());

            var queryAggregate = new EmployeePersonQueryAggregate(context);

            queryAggregate.Load(employeeId);

            employeeEntity = queryAggregate.RootEntity;

            Assert.AreEqual(employeeId, employeeEntity.Id);

            Assert.AreEqual(65000, employeeEntity.Salary);

            Assert.AreEqual("Johnny", queryAggregate.Person.FirstName);

            // Update

            employeeEntity = commandAggregate.RootEntity;

            employeeEntity.Salary = 75000;

            commandAggregate.Person.FirstName = "John";

            await commandAggregate.SaveAsync();

            // Read after update

            queryAggregate.Load(employeeId);

            employeeEntity = queryAggregate.RootEntity;

            Assert.AreEqual(employeeId, employeeEntity.Id);

            Assert.AreEqual(75000, employeeEntity.Salary);

            Assert.AreEqual("John", queryAggregate.Person.FirstName);

            // Delete
            await commandAggregate.DeleteAsync();

            queryAggregate.Load(employeeId);

            Assert.IsNull(queryAggregate.RootEntity);
        }

        [TestMethod]
        public void Employee_Aggregate_Inherits_From_Person_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<PersonEntity>(() => new PersonCommandRepository());

            context.RegisterCommandRepositoryFactory<EmployeeEntity>(() => new EmployeeCommandRepository());

            // Insert

            var commandAggregate = new EmployeePersonCommandAggregate(context, firstName: "Jack", salary: 75000);

            commandAggregate.Save();

            var employeeEntity = commandAggregate.RootEntity;

            Assert.IsNotNull(employeeEntity.Id);

            var employeeId = employeeEntity.Id;

            // Read

            context.RegisterQueryRepository<EmployeeEntity>(new EmployeeQueryRepository());

            context.RegisterQueryRepository<PersonEntity>(new PersonQueryRepository());

            var queryAggregate = new EmployeePersonQueryAggregate(context);

            queryAggregate.Load(employeeId);

            employeeEntity = queryAggregate.RootEntity;

            Assert.AreEqual(employeeId, employeeEntity.Id);

            Assert.AreEqual(75000, employeeEntity.Salary);

            Assert.AreEqual("Jack", queryAggregate.Person.FirstName);

            // Update

            employeeEntity = commandAggregate.RootEntity;

            employeeEntity.Salary = 80000;

            commandAggregate.Person.FirstName = "Jacob";

            commandAggregate.Save();

            // Read after update

            queryAggregate.Load(employeeId);

            employeeEntity = queryAggregate.RootEntity;

            Assert.AreEqual(employeeId, employeeEntity.Id);

            Assert.AreEqual(80000, employeeEntity.Salary);

            Assert.AreEqual("Jacob", queryAggregate.Person.FirstName);

            // Delete

            commandAggregate.Delete();

            queryAggregate.Load(employeeId);

            Assert.IsNull(queryAggregate.RootEntity);
        }
    }
}

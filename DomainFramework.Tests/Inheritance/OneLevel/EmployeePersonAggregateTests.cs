using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using DataAccess;
using System.Linq;
using DomainFramework.DataAccess;

namespace DomainFramework.Tests.OneLevelInheritance
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
            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection("master"),
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

CREATE TABLE DomainFrameworkInheritanceOneLevelTest..Employee(
    [EmployeeId] INT NOT NULL IDENTITY,
    [Title] VARCHAR(50)
)

ALTER TABLE DomainFrameworkInheritanceOneLevelTest..Employee
ADD CONSTRAINT Employee_PK PRIMARY KEY (EmployeeId)
GO

CREATE TABLE DomainFrameworkInheritanceOneLevelTest..Person(
    [PersonId] INT NOT NULL IDENTITY,
    [Index] INT NOT NULL,
    [EmployeeId] INT  NOT NULL
)

ALTER TABLE DomainFrameworkInheritanceOneLevelTest..Person
ADD CONSTRAINT Person_PK PRIMARY KEY (PersonId)

ALTER TABLE DomainFrameworkInheritanceOneLevelTest..Person
ADD CONSTRAINT Person_Employee_FK FOREIGN KEY (EmployeeId) REFERENCES DomainFrameworkInheritanceOneLevelTest..Employee(EmployeeId)
ON DELETE CASCADE;

GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
            @"
CREATE PROCEDURE [p_Employee_Create]
    @title VARCHAR(50)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [EmployeeId] INT NOT NULL
    );

    INSERT INTO Employee
    (
        [Title]
    )
    OUTPUT
        INSERTED.[EmployeeId]
    INTO @outputData
    VALUES
    (
        @title
    );

    SELECT
        [EmployeeId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Person_Create]
    @index INT,
    @bookId INT
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [PersonId] INT NOT NULL
    );

    INSERT INTO Person
    (
        [Index],
        [EmployeeId]
    )
    OUTPUT
        INSERTED.[PersonId]
    INTO @outputData
    VALUES
    (
        @index,
        @bookId
    );

    SELECT
        [PersonId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Employee_Get]
    @bookId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Title]
    FROM [Employee]
        WHERE [EmployeeId] = @bookId

END;
GO

CREATE PROCEDURE [p_Person_Get]
    @pageId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Index],
        [EmployeeId]
    FROM [Person]
        WHERE [PersonId] = @pageId

END;
GO

CREATE PROCEDURE [p_Employee_GetPersons]
    @bookId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [PersonId],
        [Index],
        [EmployeeId]
    FROM [Person]
        WHERE [EmployeeId] = @bookId

END;
GO

CREATE PROCEDURE [p_Employee_Update]
    @bookId INT,
    @title VARCHAR(50)
AS
BEGIN

    UPDATE Employee
    SET
        [Title] = @title
    WHERE [EmployeeId] = @bookId;

END;
GO

CREATE PROCEDURE [p_Person_Update]
    @pageId INT,
    @index INT,
    @bookId INT
AS
BEGIN

    UPDATE Person
    SET
        [Index] = @index,
        [EmployeeId] = @bookId
    WHERE [PersonId] = @pageId;

END;
GO

CREATE PROCEDURE [p_Employee_Delete]
    @bookId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [Employee]
        WHERE [EmployeeId] = @bookId

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
        public async Task Employee_Aggregate_Inherits_From_Person_Tests()
        {
            var personEntity = new PersonEntity
            {
                FirstName = "John"
            };

            var employeeEntity = new EmployeeEntity
            {
                Salary = 65000
            };

            var context = new RepositoryContext("SqlServerTest.DomainFrameworkInheritanceOneLevelTest.ConnectionString");

            context.RegisterCommandRepository<PersonEntity>(new PersonCommandRepository());

            context.RegisterCommandRepository<EmployeeEntity>(new EmployeeCommandRepository());

            // Insert

            var employeeCommandAggregate = new EmployeePersonCommandAggregate(context, employeeEntity);

            employeeCommandAggregate.SetPerson(personEntity);

            await employeeCommandAggregate.SaveAsync();

            Assert.AreEqual(1, employeeEntity.Id);

            //var pages = employeeCommandAggregate.Persons;

            //Assert.AreEqual(3, pages.Count());

            //var page = pages.ElementAt(0);

            //Assert.AreEqual(1, page.Id);

            //Assert.AreEqual(1, page.Data.Index);

            //Assert.AreEqual(2, page.EmployeeId);

            //page = pages.ElementAt(1);

            //Assert.AreEqual(2, page.Id);

            //Assert.AreEqual(2, page.Data.Index);

            //Assert.AreEqual(2, page.EmployeeId);

            //page = pages.ElementAt(2);

            //Assert.AreEqual(3, page.Id);

            //Assert.AreEqual(3, page.Data.Index);

            //Assert.AreEqual(2, page.EmployeeId);

            //// Read

            //context.RegisterQueryRepository<EmployeeEntity>(new EmployeeQueryRepository());

            //context.RegisterQueryRepository<PersonEntity>(new PersonQueryRepository());

            //var bookQueryAggregate = new EmployeePersonsQueryAggregate(context, employeeEntity);

            //bookQueryAggregate.Load(2);

            //employeeEntity = bookQueryAggregate.RootEntity;

            //Assert.AreEqual(2, employeeEntity.Id);

            //Assert.AreEqual("Programming C#", employeeEntity.Data.Title);

            //Assert.AreEqual(3, bookQueryAggregate.Persons.Count());

            //page = bookQueryAggregate.Persons.ElementAt(0);

            //Assert.AreEqual(1, page.Id);

            //Assert.AreEqual(1, page.Data.Index);

            //Assert.AreEqual(2, page.EmployeeId);

            //page = bookQueryAggregate.Persons.ElementAt(1);

            //Assert.AreEqual(2, page.Id);

            //Assert.AreEqual(2, page.Data.Index);

            //Assert.AreEqual(2, page.EmployeeId);

            //page = bookQueryAggregate.Persons.ElementAt(2);

            //Assert.AreEqual(3, page.Id);

            //Assert.AreEqual(3, page.Data.Index);

            //Assert.AreEqual(2, page.EmployeeId);

            //// Update

            //employeeEntity = employeeCommandAggregate.RootEntity;

            //employeeEntity.Data.Title = "Programming C# 2nd Ed.";

            //pages = employeeCommandAggregate.Persons;

            //page = pages.ElementAt(0);

            //page.Data.Index = 10;

            //page = pages.ElementAt(1);

            //page.Data.Index = 20;

            //page = pages.ElementAt(2);

            //page.Data.Index = 30;

            //employeeCommandAggregate.Save();

            //// Read after update

            //bookQueryAggregate.Load(2);

            //employeeEntity = bookQueryAggregate.RootEntity;

            //Assert.AreEqual(2, employeeEntity.Id);

            //Assert.AreEqual("Programming C# 2nd Ed.", employeeEntity.Data.Title);

            //Assert.AreEqual(3, bookQueryAggregate.Persons.Count());

            //page = bookQueryAggregate.Persons.ElementAt(0);

            //Assert.AreEqual(1, page.Id);

            //Assert.AreEqual(10, page.Data.Index);

            //Assert.AreEqual(2, page.EmployeeId);

            //page = bookQueryAggregate.Persons.ElementAt(1);

            //Assert.AreEqual(2, page.Id);

            //Assert.AreEqual(20, page.Data.Index);

            //Assert.AreEqual(2, page.EmployeeId);

            //page = bookQueryAggregate.Persons.ElementAt(2);

            //Assert.AreEqual(3, page.Id);

            //Assert.AreEqual(30, page.Data.Index);

            //Assert.AreEqual(2, page.EmployeeId);

            //// Delete

            //employeeCommandAggregate.Delete();

            //bookQueryAggregate.Load(2);

            //Assert.IsNull(bookQueryAggregate.RootEntity);
        }
    }
}

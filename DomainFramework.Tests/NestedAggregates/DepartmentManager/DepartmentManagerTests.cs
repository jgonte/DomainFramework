using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    [TestClass]
    public class DepartmentManagerTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkNestedAggregatesTest.ConnectionString";

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
    WHERE Name = N'DomainFrameworkNestedAggregatesTest'
)
BEGIN
    DROP DATABASE DomainFrameworkNestedAggregatesTest
END
GO

CREATE DATABASE DomainFrameworkNestedAggregatesTest
GO

USE DomainFrameworkNestedAggregatesTest
GO

CREATE TABLE DomainFrameworkNestedAggregatesTest..Department(
    [DepartmentId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL
)

ALTER TABLE DomainFrameworkNestedAggregatesTest..Department
ADD CONSTRAINT Department_PK PRIMARY KEY (DepartmentId)
GO

CREATE TABLE DomainFrameworkNestedAggregatesTest..Person(
    [PersonId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL
)

ALTER TABLE DomainFrameworkNestedAggregatesTest..Person
ADD CONSTRAINT Person_PK PRIMARY KEY (PersonId)
GO

CREATE TABLE DomainFrameworkNestedAggregatesTest..EmployeeRole(
    [PersonId] INT NOT NULL,
    [Salary] MONEY NOT NULL,
    [DepartmentId] INT NOT NULL
)

ALTER TABLE DomainFrameworkNestedAggregatesTest..EmployeeRole
ADD CONSTRAINT EmployeeRole_PK PRIMARY KEY (PersonId)
GO

ALTER TABLE DomainFrameworkNestedAggregatesTest..EmployeeRole
ADD CONSTRAINT EmployeeRole_Person_FK FOREIGN KEY (PersonId) REFERENCES DomainFrameworkNestedAggregatesTest..Person(PersonId);
GO

CREATE TABLE DomainFrameworkNestedAggregatesTest..DepartmentManagerRole(
    [EmployeeRoleId] INT NOT NULL,
    [ManagesDepartmentId] INT NOT NULL
)

ALTER TABLE DomainFrameworkNestedAggregatesTest..DepartmentManagerRole
ADD CONSTRAINT DepartmentManagerRole_PK PRIMARY KEY (EmployeeRoleId, ManagesDepartmentId)
GO

ALTER TABLE DomainFrameworkNestedAggregatesTest..DepartmentManagerRole
ADD CONSTRAINT DepartmentManagerRole_EmployeeRole_FK FOREIGN KEY (EmployeeRoleId) REFERENCES DomainFrameworkNestedAggregatesTest..EmployeeRole(PersonId);
GO

ALTER TABLE DomainFrameworkNestedAggregatesTest..DepartmentManagerRole
ADD CONSTRAINT DepartmentManagerRole_ManagesDepartment_FK FOREIGN KEY (ManagesDepartmentId) REFERENCES DomainFrameworkNestedAggregatesTest..Department(DepartmentId);
GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
 @"
CREATE PROCEDURE [p_Department_Create]
    @name VARCHAR(50)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [DepartmentId] INT NOT NULL
    );

    INSERT INTO Department
    (
        [Name]
    )
    OUTPUT
        INSERTED.[DepartmentId]
    INTO @outputData
    VALUES
    (
        @name
    );

    SELECT
        [DepartmentId]
    FROM @outputData;

END;
GO

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

CREATE PROCEDURE [p_EmployeeRole_Create]
    @personId INT,
    @salary MONEY,
    @departmentId INT
AS
BEGIN
    INSERT INTO EmployeeRole
    (
        [PersonId],
        [Salary],
        [DepartmentId]
    )
    VALUES
    (
        @personId,
        @salary,
        @departmentId
    );
END;
GO

CREATE PROCEDURE [p_DepartmentManagerRole_Create]
    @employeeRoleId INT,
    @managesDepartmentId INT
AS
BEGIN
    INSERT INTO DepartmentManagerRole
    (
        [EmployeeRoleId],
        [ManagesDepartmentId]
    )
    VALUES
    (
        @employeeRoleId,
        @managesDepartmentId
    );
END;
GO

CREATE PROCEDURE [p_Department_Get]
    @countryCode INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Name]
    FROM [Department]
        WHERE [DepartmentId] = @countryCode;

END;
GO

CREATE PROCEDURE [p_CapitalCity_Get]
    @capitalCityId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Name],
        [DepartmentId]
    FROM [CapitalCity]
        WHERE [CapitalCityId] = @capitalCityId

END;
GO

CREATE PROCEDURE [p_Department_GetCapitalCity]
    @countryCode INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [CapitalCityId],
        [Name],
        [DepartmentId]
    FROM [CapitalCity]
        WHERE [DepartmentId] = @countryCode

END;
GO

CREATE PROCEDURE [p_Department_Update]
    @countryCode INT,
    @name VARCHAR(50)
AS
BEGIN

    UPDATE Department
    SET
        [Name] = @name
    WHERE [DepartmentId] = @countryCode;

END;
GO

CREATE PROCEDURE [p_Department_Delete]
    @countryCode INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [Department]
        WHERE [DepartmentId] = @countryCode

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
        public void Setup_Department_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<DepartmentEntity>(() => new DepartmentCommandRepository());

            context.RegisterCommandRepositoryFactory<PersonEntity>(() => new PersonCommandRepository());

            context.RegisterCommandRepositoryFactory<EmployeeRoleEntity>(() => new EmployeeRoleCommandRepository());

            context.RegisterCommandRepositoryFactory<DepartmentManagerRoleEntity>(() => new DepartmentManagerRoleCommandRepository());

            // Configure the department
            var departmentCommandAggregate = new DepartmentCommandAggregate(context, new DepartmentDto
            {
                Name = "InformationTechnology",
                Employees = new Employee1Dto[]
                {
                    new Employee1Dto
                    {
                        FirstName = "Sam",
                        Salary = 60000
                    },
                    new Employee1Dto
                    {
                        FirstName = "Kevin",
                        Salary = 65000
                    },
                    new Employee1Dto
                    {
                        FirstName = "John",
                        Salary = 70000,
                        IsManager = true
                    }
                }
            });

            // Save
            departmentCommandAggregate.Save();
        }

        [TestMethod]
        public async Task Setup_Department_Async_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<DepartmentEntity>(() => new DepartmentCommandRepository());

            context.RegisterCommandRepositoryFactory<PersonEntity>(() => new PersonCommandRepository());

            context.RegisterCommandRepositoryFactory<EmployeeRoleEntity>(() => new EmployeeRoleCommandRepository());

            context.RegisterCommandRepositoryFactory<DepartmentManagerRoleEntity>(() => new DepartmentManagerRoleCommandRepository());

            // Configure the department
            var departmentCommandAggregate = new DepartmentCommandAggregate(context, new DepartmentDto
            {
                Name = "InformationTechnology",
                Employees = new Employee1Dto[]
                {
                    new Employee1Dto
                    {
                        FirstName = "Sam",
                        Salary = 60000
                    },
                    new Employee1Dto
                    {
                        FirstName = "Kevin",
                        Salary = 65000
                    },
                    new Employee1Dto
                    {
                        FirstName = "John",
                        Salary = 70000,
                        IsManager = true
                    }
                }
            });

            // Save
            await departmentCommandAggregate.SaveAsync();

            //var getDepartmentsQueryAggregate = new GetDepartmentsQueryAggregate();
        }
    }
}

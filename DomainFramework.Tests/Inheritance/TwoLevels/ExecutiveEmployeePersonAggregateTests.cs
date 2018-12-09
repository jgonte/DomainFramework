using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    [TestClass]
    public class ExecutiveEmployeePersonAggregateTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkInheritanceTwoLevelsTest.ConnectionString";

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
    WHERE Name = N'DomainFrameworkInheritanceTwoLevelsTest'
)
BEGIN
    DROP DATABASE DomainFrameworkInheritanceTwoLevelsTest
END
GO

CREATE DATABASE DomainFrameworkInheritanceTwoLevelsTest
GO

USE DomainFrameworkInheritanceTwoLevelsTest
GO

CREATE TABLE DomainFrameworkInheritanceTwoLevelsTest..Person(
    [PersonId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL
)

ALTER TABLE DomainFrameworkInheritanceTwoLevelsTest..Person
ADD CONSTRAINT Person_PK PRIMARY KEY (PersonId)

CREATE TABLE DomainFrameworkInheritanceTwoLevelsTest..Employee(
    [EmployeeId] INT NOT NULL,
    [Salary] MONEY NOT NULL
)

ALTER TABLE DomainFrameworkInheritanceTwoLevelsTest..Employee
ADD CONSTRAINT Employee_PK PRIMARY KEY (EmployeeId)
GO

ALTER TABLE DomainFrameworkInheritanceTwoLevelsTest..Employee
ADD CONSTRAINT Employee_Person_FK FOREIGN KEY (EmployeeId) REFERENCES DomainFrameworkInheritanceTwoLevelsTest..Person(PersonId)
ON DELETE CASCADE;
GO

CREATE TABLE DomainFrameworkInheritanceTwoLevelsTest..Executive(
    [ExecutiveId] INT NOT NULL,
    [Bonus] MONEY NOT NULL
)

ALTER TABLE DomainFrameworkInheritanceTwoLevelsTest..Executive
ADD CONSTRAINT Executive_PK PRIMARY KEY (ExecutiveId)
GO

ALTER TABLE DomainFrameworkInheritanceTwoLevelsTest..Executive
ADD CONSTRAINT Executive_Employee_FK FOREIGN KEY (ExecutiveId) REFERENCES DomainFrameworkInheritanceTwoLevelsTest..Employee(EmployeeId)
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

CREATE PROCEDURE [p_Executive_Create]
    @executiveId INT,
    @bonus MONEY
AS
BEGIN
    INSERT INTO Executive
    (
        [ExecutiveId],
        [Bonus]
    )
    VALUES
    (
        @executiveId,
        @bonus
    );
END;
GO

CREATE PROCEDURE [p_Executive_Get]
    @executiveId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        e.ExecutiveId AS Id,
        e.[Bonus]
    FROM [Executive] e
    WHERE e.[ExecutiveId] = @executiveId

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

CREATE PROCEDURE [p_Executive_Update]
    @executiveId INT,
    @bonus MONEY
AS
BEGIN

    UPDATE Executive
    SET
        [Bonus] = @bonus
    WHERE [ExecutiveId] = @executiveId;

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

CREATE PROCEDURE [p_Executive_Delete]
    @executiveId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [Executive]
    WHERE [ExecutiveId] = @executiveId

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
        public async Task Executive_Employee_Aggregate_Inherits_From_Person_Save_Async_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<PersonEntity>(() => new PersonCommandRepository());

            context.RegisterCommandRepositoryFactory<EmployeeEntity>(() => new EmployeeCommandRepository());

            context.RegisterCommandRepositoryFactory<ExecutiveEntity>(() => new ExecutiveCommandRepository());

            // Insert

            var saveAggregate = new SaveExecutiveEmployeePersonCommandAggregate(context, firstName: "Scottish", salary: 175000, bonus: 900000);

            await saveAggregate.SaveAsync();

            var executiveEntity = saveAggregate.RootEntity;

            Assert.IsNotNull(executiveEntity.Id);

            var executiveId = executiveEntity.Id;

            // Read

            context.RegisterQueryRepository<ExecutiveEntity>(new ExecutiveQueryRepository());

            context.RegisterQueryRepository<EmployeeEntity>(new EmployeeQueryRepository());

            context.RegisterQueryRepository<PersonEntity>(new PersonQueryRepository());

            var queryAggregate = new ExecutiveEmployeePersonQueryAggregate(context);

            await queryAggregate.LoadAsync(executiveId);

            executiveEntity = queryAggregate.RootEntity;

            Assert.AreEqual(executiveId, executiveEntity.Id);

            Assert.AreEqual(900000, executiveEntity.Bonus);

            Assert.AreEqual(175000, queryAggregate.Employee.Salary);

            Assert.AreEqual("Scottish", queryAggregate.Person.FirstName);

            // Update

            saveAggregate.RootEntity.Bonus = 1000000;

            saveAggregate.Employee.Salary = 180000;

            saveAggregate.Person.FirstName = "Scott";

            await saveAggregate.SaveAsync();

            // Read after update

            await queryAggregate.LoadAsync(executiveId);

            executiveEntity = queryAggregate.RootEntity;

            Assert.AreEqual(executiveId, executiveEntity.Id);

            Assert.AreEqual(1000000, executiveEntity.Bonus);

            Assert.AreEqual(180000, queryAggregate.Employee.Salary);

            Assert.AreEqual("Scott", queryAggregate.Person.FirstName);

            // Delete

            var deleteAggregate = new DeleteExecutiveEmployeePersonCommandAggregate(context, executiveId);

            await deleteAggregate.SaveAsync();

            queryAggregate.Load(executiveId);

            Assert.IsNull(queryAggregate.RootEntity);
        }

        [TestMethod]
        public void Executive_Employee_Aggregate_Inherits_From_Person_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<PersonEntity>(() => new PersonCommandRepository());

            context.RegisterCommandRepositoryFactory<EmployeeEntity>(() => new EmployeeCommandRepository());

            context.RegisterCommandRepositoryFactory<ExecutiveEntity>(() => new ExecutiveCommandRepository());

            // Insert

            var saveAggregate = new SaveExecutiveEmployeePersonCommandAggregate(context, firstName: "Allan", salary: 200000, bonus: 1000000);

            saveAggregate.Save();

            var executiveEntity = saveAggregate.RootEntity;

            Assert.IsNotNull(executiveEntity.Id);

            var executiveId = executiveEntity.Id;

            // Read

            context.RegisterQueryRepository<ExecutiveEntity>(new ExecutiveQueryRepository());

            context.RegisterQueryRepository<EmployeeEntity>(new EmployeeQueryRepository());

            context.RegisterQueryRepository<PersonEntity>(new PersonQueryRepository());

            var queryAggregate = new ExecutiveEmployeePersonQueryAggregate(context);

            queryAggregate.Load(executiveId);

            executiveEntity = queryAggregate.RootEntity;

            Assert.AreEqual(executiveId, executiveEntity.Id);

            Assert.AreEqual(1000000, executiveEntity.Bonus);

            Assert.AreEqual(200000, queryAggregate.Employee.Salary);

            Assert.AreEqual("Allan", queryAggregate.Person.FirstName);

            // Update

            saveAggregate.RootEntity.Bonus = 1100000;

            saveAggregate.Employee.Salary = 190000;

            saveAggregate.Person.FirstName = "Alejandro";

            saveAggregate.Save();

            // Read after update

            queryAggregate.Load(executiveId);

            executiveEntity = queryAggregate.RootEntity;

            Assert.AreEqual(executiveId, executiveEntity.Id);

            Assert.AreEqual(1100000, executiveEntity.Bonus);

            Assert.AreEqual(190000, queryAggregate.Employee.Salary);

            Assert.AreEqual("Alejandro", queryAggregate.Person.FirstName);

            // Delete

            var deleteAggregate = new DeleteExecutiveEmployeePersonCommandAggregate(context, executiveId);

            deleteAggregate.Save();

            queryAggregate.Load(executiveId);

            Assert.IsNull(queryAggregate.RootEntity);
        }
    }
}

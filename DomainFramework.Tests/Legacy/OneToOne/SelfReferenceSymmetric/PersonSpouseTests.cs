using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DomainFramework.Tests.OneToOne.SelfReference
{
    [TestClass]
    public class PersonSpouseTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkOneToOneSelfReferenceTest.ConnectionString";

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
    WHERE Name = N'DomainFrameworkOneToOneSelfReferenceTest'
)
BEGIN
    DROP DATABASE DomainFrameworkOneToOneSelfReferenceTest
END
GO

CREATE DATABASE DomainFrameworkOneToOneSelfReferenceTest
GO

USE DomainFrameworkOneToOneSelfReferenceTest
GO

CREATE TABLE DomainFrameworkOneToOneSelfReferenceTest..Person(
    [PersonId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL,
    [SpouseId] INT
)

ALTER TABLE DomainFrameworkOneToOneSelfReferenceTest..Person
ADD CONSTRAINT Person_PK PRIMARY KEY (PersonId)
GO

ALTER TABLE DomainFrameworkOneToOneSelfReferenceTest..Person
ADD CONSTRAINT Person_Spouse_FK FOREIGN KEY (SpouseId) REFERENCES DomainFrameworkOneToOneSelfReferenceTest..Person(PersonId);

GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
 @"
CREATE PROCEDURE [p_Person_Create]
    @firstName VARCHAR(50),
    @spouseId INT
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [PersonId] INT NOT NULL
    );

    INSERT INTO Person
    (
        [FirstName],
        [SpouseId]
    )
    OUTPUT
        INSERTED.[PersonId]
    INTO @outputData
    VALUES
    (
        @firstName,
        @spouseId
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
        [SpouseId]
    FROM [Person]
        WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [p_Person_GetSpouse]
    @personId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        s.[PersonId] AS Id,
        s.[FirstName],
        s.[SpouseId]
    FROM [Person] s
    INNER JOIN [Person] p
        ON s.[SpouseId] = p.[PersonId]
    WHERE p.[PersonId] = @personId

END;
GO

CREATE PROCEDURE [p_Person_Update]
    @personId INT,
    @firstName VARCHAR(50),
    @spouseId INT
AS
BEGIN

    UPDATE Person
    SET
        [FirstName] = @firstName, 
        [SpouseId] = @spouseId
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
        public void Person_Aggregate_With_Spouse_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<PersonEntity2>(() => new PersonCommandRepository2());

            // Insert
            var commandAggregate = new PersonSpouseCommandAggregate(context, 
                new PersonSpouseDto
                {
                    FirstName = "Jorge",
                    SpouseName = "Yana"
                });

            commandAggregate.Save();

            var entity = commandAggregate.RootEntity;

            Assert.IsNotNull(entity.Id);

            var id = entity.Id;

            var spouse = commandAggregate.Spouse;

            Assert.IsNotNull(spouse.Id);

            // Read

            context.RegisterQueryRepository<PersonEntity2>(new PersonQueryRepository2());

            var queryAggregate = new PersonSpouseQueryAggregate(context);

            queryAggregate.Get(id);

            entity = queryAggregate.RootEntity;

            Assert.IsNotNull(entity.Id);

            Assert.AreEqual("Jorge", entity.FirstName);

            spouse = queryAggregate.Spouse;

            Assert.IsNotNull(spouse.Id);

            Assert.AreEqual(spouse.Id, entity.SpouseId);

            Assert.AreEqual("Yana", spouse.FirstName);

            Assert.AreEqual(entity.Id, spouse.SpouseId);

            // Update

            entity = commandAggregate.RootEntity;

            entity.FirstName = "Mark";

            spouse = commandAggregate.Spouse;

            spouse.FirstName = "Katia";

            commandAggregate.Save();

            // Read after update

            queryAggregate.Get(id);

            entity = queryAggregate.RootEntity;

            Assert.IsNotNull(entity.Id);

            Assert.AreEqual("Mark", entity.FirstName);

            spouse = queryAggregate.Spouse;

            Assert.IsNotNull(spouse.Id);

            Assert.AreEqual(spouse.Id, entity.SpouseId);

            Assert.AreEqual("Katia", spouse.FirstName);

            Assert.AreEqual(entity.Id, spouse.SpouseId);

            // Delete

            //commandAggregate.Delete();

            //queryAggregate.Get(entity.Id);

            //Assert.IsNull(queryAggregate.RootEntity);
        }
    }
}

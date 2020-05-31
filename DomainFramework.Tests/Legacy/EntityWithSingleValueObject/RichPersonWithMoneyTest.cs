using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EntityWithValueObjects
{
    [TestClass]
    public class RichPersonWithMoneyTest
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkOneEntityWithOneValueObjectTest.ConnectionString";

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
    WHERE Name = N'DomainFrameworkOneEntityWithOneValueObjectTest'
)
BEGIN
    DROP DATABASE DomainFrameworkOneEntityWithOneValueObjectTest
END
GO

CREATE DATABASE DomainFrameworkOneEntityWithOneValueObjectTest
GO

USE DomainFrameworkOneEntityWithOneValueObjectTest
GO

CREATE TABLE DomainFrameworkOneEntityWithOneValueObjectTest..RichPerson(
    [RichPersonId] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL,
    [MoneyAmount] NUMERIC(12, 2)
)

ALTER TABLE DomainFrameworkOneEntityWithOneValueObjectTest..RichPerson
ADD CONSTRAINT RichPerson_PK PRIMARY KEY (RichPersonId)
GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
 @"
CREATE PROCEDURE [p_RichPerson_Create]
    @firstName VARCHAR(50),
    @moneyAmount NUMERIC(12, 2)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [RichPersonId] INT NOT NULL
    );

    INSERT INTO RichPerson
    (
        [FirstName],
        [MoneyAmount]
    )
    OUTPUT
        INSERTED.[RichPersonId]
    INTO @outputData
    VALUES
    (
        @firstName,
        @moneyAmount
    );

    SELECT
        [RichPersonId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_RichPerson_Get]
    @richPersonId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [FirstName],
        [MoneyAmount]
    FROM [RichPerson]
        WHERE [RichPersonId] = @richPersonId;

END;
GO

CREATE PROCEDURE [p_RichPerson_Update]
    @richPersonId INT,
    @firstName VARCHAR(50),
    @moneyAmount NUMERIC(12, 2)
AS
BEGIN

    UPDATE RichPerson
    SET
        [FirstName] = @firstName, 
        [MoneyAmount] = @moneyAmount
    WHERE [RichPersonId] = @richPersonId;

END;
GO

CREATE PROCEDURE [p_RichPerson_Delete]
    @richPersonId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [RichPerson]
        WHERE [RichPersonId] = @richPersonId

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
        public void Rich_Person_With_Money_Test()
        {
            var richPersonEntity = new RichPersonEntity
            {
                FirstName = "Bill",
                Capital = new Money(100000000)
            };

            // Insert
            var commandRepository = new RichPersonCommandRepository
            {
                ConnectionName = connectionName
            };

            commandRepository.Save(richPersonEntity, user: null, unitOfWork: null, selector: null);

            var id = richPersonEntity.Id;

            // Read
            var queryRepository = new RichPersonQueryRepository
            {
                ConnectionName = connectionName
            };

            richPersonEntity = queryRepository.GetById(id);

            Assert.AreEqual(id, richPersonEntity.Id);

            Assert.AreEqual("Bill", richPersonEntity.FirstName);

            Assert.AreEqual(new Money(100000000), richPersonEntity.Capital);

            // Update
            richPersonEntity.FirstName = "William";

            richPersonEntity.Capital = richPersonEntity.Capital.Add(new Money(100000000));

            commandRepository.Save(richPersonEntity, user: null, unitOfWork: null, selector: null);

            // Read changes
            richPersonEntity = queryRepository.GetById(id);

            Assert.AreEqual(id, richPersonEntity.Id);

            Assert.AreEqual("William", richPersonEntity.FirstName);

            Assert.AreEqual(new Money(200000000), richPersonEntity.Capital);
        }
    }
}

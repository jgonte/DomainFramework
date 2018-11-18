using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    [TestClass]
    public class CountryCapitalTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkOneToOneTest.ConnectionString";

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
    WHERE Name = N'DomainFrameworkOneToOneTest'
)
BEGIN
    DROP DATABASE DomainFrameworkOneToOneTest
END
GO

CREATE DATABASE DomainFrameworkOneToOneTest
GO

USE DomainFrameworkOneToOneTest
GO

CREATE TABLE DomainFrameworkOneToOneTest..Country(
    [CountryCode] CHAR(2) NOT NULL,
    [Name] VARCHAR(50) NOT NULL
)

ALTER TABLE DomainFrameworkOneToOneTest..Country
ADD CONSTRAINT Country_PK PRIMARY KEY (CountryCode)
GO

CREATE TABLE DomainFrameworkOneToOneTest..CapitalCity(
    [CapitalCityId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CountryCode] CHAR(2) NOT NULL
)

ALTER TABLE DomainFrameworkOneToOneTest..CapitalCity
ADD CONSTRAINT CapitalCity_PK PRIMARY KEY (CapitalCityId)

ALTER TABLE DomainFrameworkOneToOneTest..CapitalCity
ADD CONSTRAINT CapitalCity_Country_FK FOREIGN KEY (CountryCode) REFERENCES DomainFrameworkOneToOneTest..Country(CountryCode)
ON DELETE CASCADE;

GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
 @"
CREATE PROCEDURE [p_Country_Create]
    @countryCode CHAR(2),
    @name VARCHAR(50)
AS
BEGIN

    INSERT INTO Country
    (
        [CountryCode],
        [Name]
    )
    VALUES
    (
        @countryCode,
        @name
    );
END;
GO

CREATE PROCEDURE [p_CapitalCity_Create]
    @name VARCHAR(50),
    @countryCode CHAR(2)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [CapitalCityId] INT NOT NULL
    );

    INSERT INTO CapitalCity
    (
        [Name],
        [CountryCode]
    )
    OUTPUT
        INSERTED.[CapitalCityId]
    INTO @outputData
    VALUES
    (
        @name,
        @countryCode
    );

    SELECT
        [CapitalCityId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Country_Get]
    @countryCode CHAR(2)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Name]
    FROM [Country]
        WHERE [CountryCode] = @countryCode;

END;
GO

CREATE PROCEDURE [p_CapitalCity_Get]
    @capitalCityId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Name],
        [CountryCode]
    FROM [CapitalCity]
        WHERE [CapitalCityId] = @capitalCityId

END;
GO

CREATE PROCEDURE [p_Country_GetCapitalCity]
    @countryCode CHAR(2)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [CapitalCityId],
        [Name],
        [CountryCode]
    FROM [CapitalCity]
        WHERE [CountryCode] = @countryCode

END;
GO

CREATE PROCEDURE [p_Country_Update]
    @countryCode CHAR(2),
    @name VARCHAR(50)
AS
BEGIN

    UPDATE Country
    SET
        [Name] = @name
    WHERE [CountryCode] = @countryCode;

END;
GO

CREATE PROCEDURE [p_CapitalCity_Update]
    @capitalCityId INT,
    @name VARCHAR(50),
    @countryCode CHAR(2)
AS
BEGIN

    UPDATE CapitalCity
    SET
        [Name] = @name,
        [CountryCode] = @countryCode
    WHERE [CapitalCityId] = @capitalCityId;

END;
GO

CREATE PROCEDURE [p_Country_Delete]
    @countryCode CHAR(2)
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [Country]
        WHERE [CountryCode] = @countryCode

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
        public void Country_Aggregate_With_CapitalCity_Country_Only_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<CountryEntity>(() => new CountryCommandRepository());

            context.RegisterCommandRepositoryFactory<CapitalCityEntity>(() => new CapitalCityCommandRepository());

            // Insert
            var commandAggregate = new CountryCapitalCityCommandAggregate(context,
                new CountryWithCapitalCityDto
                {
                    CountryCode = "il",
                    Name = "ISRAEL",
                    CapitalCity = null
                });

            commandAggregate.Save();

            var countryEntity = commandAggregate.RootEntity;

            Assert.AreEqual("ISRAEL", countryEntity.Data.Name);

            Assert.AreEqual("il", countryEntity.Id);

            // Read

            context.RegisterQueryRepository<CountryEntity>(new CountryQueryRepository());

            context.RegisterQueryRepository<CapitalCityEntity>(new CapitalCityQueryRepository());

            var queryAggregate = new CountryCapitalCityQueryAggregate(context);

            queryAggregate.Load("il");

            countryEntity = queryAggregate.RootEntity;

            Assert.AreEqual("il", countryEntity.Id);

            Assert.AreEqual("ISRAEL", countryEntity.Data.Name);

            // Update

            countryEntity = commandAggregate.RootEntity;

            countryEntity.Data.Name = "Israel";

            commandAggregate.Save();

            // Read after update

            queryAggregate.Load("il");

            countryEntity = queryAggregate.RootEntity;

            Assert.AreEqual("il", countryEntity.Id);

            Assert.AreEqual("Israel", countryEntity.Data.Name);

            // Delete

            commandAggregate.Delete();

            queryAggregate.Load("il");

            Assert.IsNull(queryAggregate.RootEntity);
        }

        [TestMethod]
        public void Country_Aggregate_With_CapitalCity_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<CountryEntity>(() => new CountryCommandRepository());

            context.RegisterCommandRepositoryFactory<CapitalCityEntity>(() => new CapitalCityCommandRepository());

            // Insert
            var commandAggregate = new CountryCapitalCityCommandAggregate(context, new CountryWithCapitalCityDto
            {
                CountryCode = "us",
                Name = "United States",
                CapitalCity = new CapitalCityDto
                {
                    Name = "Washington"
                }
            });

            commandAggregate.Save();

            var countryEntity = commandAggregate.RootEntity;

            Assert.AreEqual("us", countryEntity.Id);

            var capitalCity = commandAggregate.CapitalCity;

            Assert.AreEqual(1, capitalCity.Id);

            Assert.AreEqual("Washington", capitalCity.Data.Name);

            Assert.AreEqual("us", capitalCity.CountryCode);

            // Read
            context.RegisterQueryRepository<CountryEntity>(new CountryQueryRepository());

            context.RegisterQueryRepository<CapitalCityEntity>(new CapitalCityQueryRepository());

            var queryAggregate = new CountryCapitalCityQueryAggregate(context);

            queryAggregate.Load("us");

            countryEntity = queryAggregate.RootEntity;

            Assert.AreEqual("us", countryEntity.Id);

            Assert.AreEqual("United States", countryEntity.Data.Name);

            capitalCity = queryAggregate.CapitalCity;

            Assert.AreEqual(1, capitalCity.Id);

            Assert.AreEqual("Washington", capitalCity.Data.Name);

            Assert.AreEqual("us", capitalCity.CountryCode);

            // Update

            countryEntity = commandAggregate.RootEntity;

            countryEntity.Data.Name = "United States of America";

            capitalCity = commandAggregate.CapitalCity;

            capitalCity.Data.Name = "Washington, DC.";

            commandAggregate.Save();

            // Read after update

            queryAggregate.Load("us");

            countryEntity = queryAggregate.RootEntity;

            Assert.AreEqual("us", countryEntity.Id);

            Assert.AreEqual("United States of America", countryEntity.Data.Name);

            Assert.AreEqual("Washington, DC.", capitalCity.Data.Name);

            Assert.AreEqual("us", capitalCity.CountryCode);

            // Delete

            commandAggregate.Delete();

            queryAggregate.Load("us");

            Assert.IsNull(queryAggregate.RootEntity);
        }
    }
}

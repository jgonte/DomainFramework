using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests.PersonWithSpouseAndDependants
{
    [TestClass]
    public class PersonWithSpouseAndDependantsTests
    {
        static readonly string connectionName = "SqlServerTest.PersonWithSpouseAndDependants.ConnectionString";

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
@"USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'PersonWithSpouseAndDependants'
)
BEGIN
    DROP DATABASE [PersonWithSpouseAndDependants]
END
GO

CREATE DATABASE [PersonWithSpouseAndDependants]
GO

USE [PersonWithSpouseAndDependants]
GO

CREATE TABLE [PersonWithSpouseAndDependants]..[Person]
(
    [PersonId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME,
    [MarriedPersonId] INT,
    [ProviderPersonId] INT
    CONSTRAINT Person_PK PRIMARY KEY ([PersonId])
);
GO

ALTER TABLE [PersonWithSpouseAndDependants]..[Person]
    ADD CONSTRAINT Person_Spouse_FK FOREIGN KEY ([MarriedPersonId])
        REFERENCES [PersonWithSpouseAndDependants]..[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Spouse_FK_IX]
    ON [PersonWithSpouseAndDependants]..[Person]
    (
        [MarriedPersonId]
    );
GO

ALTER TABLE [PersonWithSpouseAndDependants]..[Person]
    ADD CONSTRAINT Person_Dependants_FK FOREIGN KEY ([ProviderPersonId])
        REFERENCES [PersonWithSpouseAndDependants]..[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Dependants_FK_IX]
    ON [PersonWithSpouseAndDependants]..[Person]
    (
        [ProviderPersonId]
    );
GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
@"CREATE PROCEDURE .[pPerson_DeleteSpouse]
    @marriedPersonId INT
AS
BEGIN
    DELETE FROM [PersonWithSpouseAndDependants]..[Person]
    WHERE [MarriedPersonId] = @marriedPersonId;

END;
GO

CREATE PROCEDURE .[pPerson_DeleteDependants]
    @providerPersonId INT
AS
BEGIN
    DELETE FROM [PersonWithSpouseAndDependants]..[Person]
    WHERE [ProviderPersonId] = @providerPersonId;

END;
GO

CREATE PROCEDURE .[pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [PersonWithSpouseAndDependants]..[Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE .[pPerson_Insert]
    @name VARCHAR(50),
    @createdBy INT,
    @marriedPersonId INT,
    @providerPersonId INT
AS
BEGIN
    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [PersonWithSpouseAndDependants]..[Person]
    (
        [Name],
        [CreatedBy],
        [MarriedPersonId],
        [ProviderPersonId]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @createdBy,
        @marriedPersonId,
        @providerPersonId
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE .[pPerson_Update]
    @personId INT,
    @name VARCHAR(50),
    @lastUpdatedBy INT,
    @marriedPersonId INT,
    @providerPersonId INT
AS
BEGIN
    UPDATE [PersonWithSpouseAndDependants]..[Person]
    SET
        [Name] = @name,
        [LastUpdatedBy] = @lastUpdatedBy,
        [MarriedPersonId] = @marriedPersonId,
        [ProviderPersonId] = @providerPersonId,
        [LastUpdatedWhen] = GETDATE()
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE .[pPerson_Get]
AS
BEGIN
    SELECT
        p.[PersonId] AS ""Id"",
        p.[Name] AS ""Name"",
        p.[MarriedPersonId] AS ""MarriedPersonId"",
        p.[ProviderPersonId] AS ""ProviderPersonId""
    FROM [PersonWithSpouseAndDependants]..[Person] p;

END;
GO

CREATE PROCEDURE .[pPerson_GetById]
    @personId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS ""Id"",
        p.[Name] AS ""Name"",
        p.[MarriedPersonId] AS ""MarriedPersonId"",
        p.[ProviderPersonId] AS ""ProviderPersonId""
    FROM [PersonWithSpouseAndDependants]..[Person] p
    WHERE p.[PersonId] = @personId;

END;
GO

CREATE PROCEDURE .[pPerson_GetDependants_ForPerson]
    @providerPersonId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS ""Id"",
        p.[Name] AS ""Name"",
        p.[MarriedPersonId] AS ""MarriedPersonId"",
        p.[ProviderPersonId] AS ""ProviderPersonId""
    FROM [PersonWithSpouseAndDependants]..[Person] p
    WHERE p.[ProviderPersonId] = @providerPersonId;

END;
GO

CREATE PROCEDURE .[pPerson_GetSpouse_ForPerson]
    @marriedPersonId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS ""Id"",
        p.[Name] AS ""Name"",
        p.[MarriedPersonId] AS ""MarriedPersonId"",
        p.[ProviderPersonId] AS ""ProviderPersonId""
    FROM [PersonWithSpouseAndDependants]..[Person] p
    WHERE p.[MarriedPersonId] = @marriedPersonId;

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
        public void Person_With_Spouse_And_Dependants_Tests()
        {
            // Configure the user
            var commandAggregate = new SavePersonCommandAggregate(new SavePersonInputDto
            {
                Name = "Jorge",
                Spouse = new SavePersonInputDto
                {
                    Name = "Yana"
                },
                Dependants = new List<SavePersonInputDto>
                {
                    new SavePersonInputDto
                    {
                        Name = "Mark"
                    },
                    new SavePersonInputDto
                    {
                        Name = "Sarah"
                    },
                    new SavePersonInputDto
                    {
                        Name = "Katia"
                    }
                }
            });
            
            commandAggregate.Save();
        }
    }
}

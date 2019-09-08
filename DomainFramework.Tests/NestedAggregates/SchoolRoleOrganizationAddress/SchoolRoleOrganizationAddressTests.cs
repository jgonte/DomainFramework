using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    [TestClass]
    public class SchoolRoleOrganizationAddressTests
    {
        static readonly string connectionName = "SqlServerTest.SchoolRoleOrganizationAddress.ConnectionString";

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
    WHERE Name = N'SchoolRoleOrganizationAddress'
)
BEGIN
    DROP DATABASE [SchoolRoleOrganizationAddress]
END
GO

CREATE DATABASE [SchoolRoleOrganizationAddress]
GO

USE [SchoolRoleOrganizationAddress]
GO

CREATE TABLE [SchoolRoleOrganizationAddress]..[Address]
(
    [AddressId] INT NOT NULL IDENTITY,
    [Street] VARCHAR(50) NOT NULL
    CONSTRAINT Address_PK PRIMARY KEY ([AddressId])
);
GO

CREATE TABLE [SchoolRoleOrganizationAddress]..[Organization]
(
    [OrganizationId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME,
    [Phone_Number] VARCHAR(10) NOT NULL,
    [AddressId] INT NOT NULL
    CONSTRAINT Organization_PK PRIMARY KEY ([OrganizationId])
);
GO

CREATE TABLE [SchoolRoleOrganizationAddress]..[Role]
(
    [RoleId] INT NOT NULL IDENTITY,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME
    CONSTRAINT Role_PK PRIMARY KEY ([RoleId])
);
GO

CREATE TABLE [SchoolRoleOrganizationAddress]..[School]
(
    [SchoolId] INT NOT NULL,
    [IsCharter] BIT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME
    CONSTRAINT School_PK PRIMARY KEY ([SchoolId])
);
GO

CREATE TABLE [SchoolRoleOrganizationAddress]..[OrganizationRole]
(
    [OrganizationId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME
    CONSTRAINT OrganizationRole_PK PRIMARY KEY ([OrganizationId], [RoleId])
);
GO

ALTER TABLE [SchoolRoleOrganizationAddress]..[School]
    ADD CONSTRAINT School_Role_IFK FOREIGN KEY ([SchoolId])
        REFERENCES [SchoolRoleOrganizationAddress]..[Role] ([RoleId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [School_Role_IFK_IX]
    ON [SchoolRoleOrganizationAddress]..[School]
    (
        [SchoolId]
    );
GO

ALTER TABLE [SchoolRoleOrganizationAddress]..[Organization]
    ADD CONSTRAINT Address_Organizations_FK FOREIGN KEY ([AddressId])
        REFERENCES [SchoolRoleOrganizationAddress]..[Address] ([AddressId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Address_Organizations_FK_IX]
    ON [SchoolRoleOrganizationAddress]..[Organization]
    (
        [AddressId]
    );
GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
@"CREATE PROCEDURE .[pAddress_DeleteRole]
    @organizationId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[OrganizationRole]
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE .[pAddress_DeleteOrganization]
    @roleId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[OrganizationRole]
    WHERE [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pAddress_DeleteOrganizations]
    @addressId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[Organization]
    WHERE [AddressId] = @addressId;

END;
GO

CREATE PROCEDURE .[pAddress_Delete]
    @addressId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[Address]
    WHERE [AddressId] = @addressId;

END;
GO

CREATE PROCEDURE .[pAddress_Insert]
    @street VARCHAR(50)
AS
BEGIN
    DECLARE @addressOutputData TABLE
    (
        [AddressId] INT
    );

    INSERT INTO [SchoolRoleOrganizationAddress]..[Address]
    (
        [Street]
    )
    OUTPUT
        INSERTED.[AddressId]
        INTO @addressOutputData
    VALUES
    (
        @street
    );

    SELECT
        [AddressId]
    FROM @addressOutputData;

END;
GO

CREATE PROCEDURE .[pAddress_Update]
    @addressId INT,
    @street VARCHAR(50)
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress]..[Address]
    SET
        [Street] = @street
    WHERE [AddressId] = @addressId;

END;
GO

CREATE PROCEDURE .[pAddress_Get]
AS
BEGIN
    SELECT
        a.[AddressId] AS ""Id"",
        a.[Street] AS ""Street""
    FROM [SchoolRoleOrganizationAddress]..[Address] a;

END;
GO

CREATE PROCEDURE .[pAddress_GetAddress_ForOrganization]
    @organizationId INT
AS
BEGIN
    SELECT
        a.[AddressId] AS ""Id"",
        a.[Street] AS ""Street""
    FROM [SchoolRoleOrganizationAddress]..[Address] a
    INNER JOIN [SchoolRoleOrganizationAddress]..[Organization] o
        ON a.[AddressId] = o.[AddressId]
    WHERE o.[OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE .[pAddress_GetById]
    @addressId INT
AS
BEGIN
    SELECT
        a.[AddressId] AS ""Id"",
        a.[Street] AS ""Street""
    FROM [SchoolRoleOrganizationAddress]..[Address] a
    WHERE a.[AddressId] = @addressId;

END;
GO

CREATE PROCEDURE .[pOrganization_DeleteRole]
    @organizationId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[OrganizationRole]
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE .[pOrganization_Delete]
    @organizationId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[Organization]
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE .[pOrganization_Insert]
    @name VARCHAR(50),
    @createdBy INT,
    @phone_Number VARCHAR(10),
    @addressId INT
AS
BEGIN
    DECLARE @organizationOutputData TABLE
    (
        [OrganizationId] INT
    );

    INSERT INTO [SchoolRoleOrganizationAddress]..[Organization]
    (
        [Name],
        [CreatedBy],
        [Phone_Number],
        [AddressId]
    )
    OUTPUT
        INSERTED.[OrganizationId]
        INTO @organizationOutputData
    VALUES
    (
        @name,
        @createdBy,
        @phone_Number,
        @addressId
    );

    SELECT
        [OrganizationId]
    FROM @organizationOutputData;

END;
GO

CREATE PROCEDURE .[pOrganization_Update]
    @organizationId INT,
    @name VARCHAR(50),
    @lastUpdatedBy INT,
    @phone_Number VARCHAR(10),
    @addressId INT
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress]..[Organization]
    SET
        [Name] = @name,
        [LastUpdatedBy] = @lastUpdatedBy,
        [Phone_Number] = @phone_Number,
        [AddressId] = @addressId,
        [LastUpdatedWhen] = GETDATE()
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE .[pOrganization_Get]
AS
BEGIN
    SELECT
        o.[OrganizationId] AS ""Id"",
        o.[Name] AS ""Name"",
        o.[Phone_Number] AS ""Phone.Number"",
        o.[AddressId] AS ""AddressId""
    FROM [SchoolRoleOrganizationAddress]..[Organization] o;

END;
GO

CREATE PROCEDURE .[pOrganization_GetById]
    @organizationId INT
AS
BEGIN
    SELECT
        o.[OrganizationId] AS ""Id"",
        o.[Name] AS ""Name"",
        o.[Phone_Number] AS ""Phone.Number"",
        o.[AddressId] AS ""AddressId""
    FROM [SchoolRoleOrganizationAddress]..[Organization] o
    WHERE o.[OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE .[pOrganization_GetOrganization_ForOrganizationRole]
    @roleId INT
AS
BEGIN
    SELECT
        o.[OrganizationId] AS ""Id"",
        o.[Name] AS ""Name"",
        o.[Phone_Number] AS ""Phone.Number"",
        o.[AddressId] AS ""AddressId""
    FROM [SchoolRoleOrganizationAddress]..[Organization] o
    INNER JOIN [SchoolRoleOrganizationAddress]..[OrganizationRole] r
        ON o.[OrganizationId] = r.[OrganizationId]
    WHERE r.[RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pOrganizationRole_DeleteRole]
    @organizationId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[OrganizationRole]
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE .[pOrganizationRole_DeleteOrganization]
    @roleId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[OrganizationRole]
    WHERE [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pOrganizationRole_Delete]
    @organizationId INT,
    @roleId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[OrganizationRole]
    WHERE [OrganizationId] = @organizationId
    AND [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pOrganizationRole_Insert]
    @organizationId INT,
    @roleId INT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [SchoolRoleOrganizationAddress]..[OrganizationRole]
    (
        [OrganizationId],
        [RoleId],
        [CreatedBy]
    )
    VALUES
    (
        @organizationId,
        @roleId,
        @createdBy
    );

END;
GO

CREATE PROCEDURE .[pOrganizationRole_Update]
    @organizationId INT,
    @roleId INT,
    @lastUpdatedBy INT
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress]..[OrganizationRole]
    SET
        [LastUpdatedBy] = @lastUpdatedBy,
        [LastUpdatedWhen] = GETDATE()
    WHERE [OrganizationId] = @organizationId
    AND [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pOrganizationRole_Get]
AS
BEGIN
    SELECT
        o.[OrganizationId] AS ""Id.OrganizationId"",
        o.[RoleId] AS ""Id.RoleId""
    FROM [SchoolRoleOrganizationAddress]..[OrganizationRole] o;

END;
GO

CREATE PROCEDURE .[pOrganizationRole_GetById]
    @organizationId INT,
    @roleId INT
AS
BEGIN
    SELECT
        o.[OrganizationId] AS ""Id.OrganizationId"",
        o.[RoleId] AS ""Id.RoleId""
    FROM [SchoolRoleOrganizationAddress]..[OrganizationRole] o
    WHERE o.[OrganizationId] = @organizationId
    AND o.[RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pRole_DeleteOrganization]
    @roleId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[OrganizationRole]
    WHERE [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pRole_Delete]
    @roleId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[Role]
    WHERE [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pRole_Insert]
    @createdBy INT
AS
BEGIN
    DECLARE @roleOutputData TABLE
    (
        [RoleId] INT
    );

    INSERT INTO [SchoolRoleOrganizationAddress]..[Role]
    (
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[RoleId]
        INTO @roleOutputData
    VALUES
    (
        @createdBy
    );

    SELECT
        [RoleId]
    FROM @roleOutputData;

END;
GO

CREATE PROCEDURE .[pRole_Update]
    @roleId INT,
    @lastUpdatedBy INT
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress]..[Role]
    SET
        [LastUpdatedBy] = @lastUpdatedBy,
        [LastUpdatedWhen] = GETDATE()
    WHERE [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pRole_Get]
AS
BEGIN
    SELECT
        _q_.[Id] AS ""Id"",
        _q_.[IsCharter] AS ""IsCharter"",
        _q_.[_EntityType_] AS ""_EntityType_""
    FROM 
    (
        SELECT
            s.[SchoolId] AS ""Id"",
            s.[IsCharter] AS ""IsCharter"",
            1 AS ""_EntityType_""
        FROM [SchoolRoleOrganizationAddress]..[School] s
        INNER JOIN [SchoolRoleOrganizationAddress]..[Role] r
            ON s.[SchoolId] = r.[RoleId]
        UNION ALL
        (
            SELECT
                r.[RoleId] AS ""Id"",
                NULL AS ""IsCharter"",
                2 AS ""_EntityType_""
            FROM [SchoolRoleOrganizationAddress]..[Role] r
            LEFT OUTER JOIN [SchoolRoleOrganizationAddress]..[School] s
                ON s.[SchoolId] = r.[RoleId]
            WHERE s.[SchoolId] IS NULL
        )
    ) _q_;

END;
GO

CREATE PROCEDURE .[pRole_GetById]
    @roleId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS ""Id"",
        _q_.[IsCharter] AS ""IsCharter"",
        _q_.[_EntityType_] AS ""_EntityType_""
    FROM 
    (
        SELECT
            s.[SchoolId] AS ""Id"",
            s.[IsCharter] AS ""IsCharter"",
            1 AS ""_EntityType_""
        FROM [SchoolRoleOrganizationAddress]..[School] s
        INNER JOIN [SchoolRoleOrganizationAddress]..[Role] r
            ON s.[SchoolId] = r.[RoleId]
        UNION ALL
        (
            SELECT
                r.[RoleId] AS ""Id"",
                NULL AS ""IsCharter"",
                2 AS ""_EntityType_""
            FROM [SchoolRoleOrganizationAddress]..[Role] r
            LEFT OUTER JOIN [SchoolRoleOrganizationAddress]..[School] s
                ON s.[SchoolId] = r.[RoleId]
            WHERE s.[SchoolId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @roleId;

END;
GO

CREATE PROCEDURE .[pSchool_DeleteOrganization]
    @roleId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[OrganizationRole]
    WHERE [RoleId] = @roleId;

END;
GO

CREATE PROCEDURE .[pSchool_DeleteRole]
    @organizationId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[OrganizationRole]
    WHERE [OrganizationId] = @organizationId;

END;
GO

CREATE PROCEDURE .[pSchool_Delete]
    @schoolId INT
AS
BEGIN
    DELETE FROM [SchoolRoleOrganizationAddress]..[School]
    WHERE [SchoolId] = @schoolId;

END;
GO

CREATE PROCEDURE .[pSchool_Insert]
    @isCharter BIT,
    @createdBy INT
AS
BEGIN
    DECLARE @roleId INT;

    DECLARE @roleOutputData TABLE
    (
        [RoleId] INT
    );

    INSERT INTO [SchoolRoleOrganizationAddress]..[Role]
    (
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[RoleId]
        INTO @roleOutputData
    VALUES
    (
        @createdBy
    );

    SELECT
        @roleId = [RoleId]
    FROM @roleOutputData;

    INSERT INTO [SchoolRoleOrganizationAddress]..[School]
    (
        [SchoolId],
        [IsCharter],
        [CreatedBy]
    )
    VALUES
    (
        @roleId,
        @isCharter,
        @createdBy
    );

    SELECT
        [RoleId]
    FROM @roleOutputData;

END;
GO

CREATE PROCEDURE .[pSchool_Update]
    @schoolId INT,
    @isCharter BIT,
    @lastUpdatedBy INT
AS
BEGIN
    UPDATE [SchoolRoleOrganizationAddress]..[Role]
    SET
        [LastUpdatedBy] = @lastUpdatedBy,
        [LastUpdatedWhen] = GETDATE()
    WHERE [RoleId] = @schoolId;

    UPDATE [SchoolRoleOrganizationAddress]..[School]
    SET
        [IsCharter] = @isCharter,
        [LastUpdatedBy] = @lastUpdatedBy,
        [LastUpdatedWhen] = GETDATE()
    WHERE [SchoolId] = @schoolId;

END;
GO

CREATE PROCEDURE .[pSchool_Get]
AS
BEGIN
    SELECT
        s.[SchoolId] AS ""Id"",
        s.[IsCharter] AS ""IsCharter""
    FROM [SchoolRoleOrganizationAddress]..[School] s
    INNER JOIN [SchoolRoleOrganizationAddress]..[Role] r
        ON s.[SchoolId] = r.[RoleId];

END;
GO

CREATE PROCEDURE .[pSchool_GetById]
    @schoolId INT
AS
BEGIN
    SELECT
        s.[SchoolId] AS ""Id"",
        s.[IsCharter] AS ""IsCharter""
    FROM [SchoolRoleOrganizationAddress]..[School] s
    INNER JOIN [SchoolRoleOrganizationAddress]..[Role] r
        ON s.[SchoolId] = r.[RoleId]
    WHERE s.[SchoolId] = @schoolId;

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
        public void School_Organization_Address_Tests()
        {
            // Configure the user
            var commandAggregate = new SaveSchoolCommandAggregate(new SaveSchoolInputDto
            {
                Organization = new OrganizationInputDto
                {
                    Name = "School1",
                    Address = new AddressInputDto
                    {
                        Street = "Street1"
                    },
                    Phone = new PhoneInputDto
                    {
                        Number = "3051112233"
                    }
                },
                IsCharter = true
            });

            commandAggregate.Save();

            var schoolId = commandAggregate.RootEntity.Id;

            var queryAggregate = new GetByIdSchoolQueryAggregate();

            var school = queryAggregate.Get(schoolId);

            Assert.IsTrue(school.IsCharter);

            Assert.AreEqual(1, school.Organization.Id);

            Assert.AreEqual("School1", school.Organization.Name);

            Assert.AreEqual(1, school.Organization.Address.Id);

            Assert.AreEqual("Street1", school.Organization.Address.Street);

            Assert.AreEqual("3051112233", school.Organization.Phone.Number);

        }
    }
}

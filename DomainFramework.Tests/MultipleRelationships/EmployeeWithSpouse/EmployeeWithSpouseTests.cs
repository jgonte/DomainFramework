using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Validation;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    [TestClass]
    public class EmployeeWithSpouseTests
    {
        static readonly string connectionName = "SqlServerTest.EmployeeWithSpouse.ConnectionString";

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
    WHERE Name = N'EmployeeWithSpouse'
)
BEGIN
    DROP DATABASE [EmployeeWithSpouse]
END
GO

CREATE DATABASE [EmployeeWithSpouse]
GO

USE [EmployeeWithSpouse]
GO

CREATE TABLE [EmployeeWithSpouse]..[Employee]
(
    [EmployeeId] INT NOT NULL,
    [HireDate] DATETIME NOT NULL
    CONSTRAINT Employee_PK PRIMARY KEY ([EmployeeId])
);
GO

CREATE TABLE [EmployeeWithSpouse]..[Person]
(
    [PersonId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME,
    [CellPhone_AreaCode] VARCHAR(3),
    [CellPhone_Exchange] VARCHAR(3),
    [CellPhone_Number] VARCHAR(4),
    [MarriedToPersonId] INT
    CONSTRAINT Person_PK PRIMARY KEY ([PersonId])
);
GO

ALTER TABLE [EmployeeWithSpouse]..[Employee]
    ADD CONSTRAINT Employee_Person_IFK FOREIGN KEY ([EmployeeId])
        REFERENCES [EmployeeWithSpouse]..[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Employee_Person_IFK_IX]
    ON [EmployeeWithSpouse]..[Employee]
    (
        [EmployeeId]
    );
GO

ALTER TABLE [EmployeeWithSpouse]..[Person]
    ADD CONSTRAINT Person_Spouse_FK FOREIGN KEY ([MarriedToPersonId])
        REFERENCES [EmployeeWithSpouse]..[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Spouse_FK_IX]
    ON [EmployeeWithSpouse]..[Person]
    (
        [MarriedToPersonId]
    );
GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
@"CREATE PROCEDURE [pPerson_DeleteSpouse]
    @marriedToPersonId INT
AS
BEGIN
    DELETE FROM [EmployeeWithSpouse]..[Person]
    WHERE [MarriedToPersonId] = @marriedToPersonId;

END;
GO

CREATE PROCEDURE [pEmployee_Delete]
    @employeeId INT
AS
BEGIN
    DELETE FROM [EmployeeWithSpouse]..[Employee]
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [pEmployee_Insert]
    @hireDate DATETIME,
    @name VARCHAR(50),
    @createdBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4),
    @marriedToPersonId INT
AS
BEGIN
    DECLARE @personId INT;

    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [EmployeeWithSpouse]..[Person]
    (
        [Name],
        [CreatedBy],
        [CellPhone_AreaCode],
        [CellPhone_Exchange],
        [CellPhone_Number],
        [MarriedToPersonId]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @createdBy,
        @cellPhone_AreaCode,
        @cellPhone_Exchange,
        @cellPhone_Number,
        @marriedToPersonId
    );

    SELECT
        @personId = [PersonId]
    FROM @personOutputData;

    INSERT INTO [EmployeeWithSpouse]..[Employee]
    (
        [EmployeeId],
        [HireDate]
    )
    VALUES
    (
        @personId,
        @hireDate
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [pEmployee_Update]
    @employeeId INT,
    @hireDate DATETIME,
    @name VARCHAR(50),
    @lastUpdatedBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4),
    @marriedToPersonId INT
AS
BEGIN
    UPDATE [EmployeeWithSpouse]..[Person]
    SET
        [Name] = @name,
        [LastUpdatedBy] = @lastUpdatedBy,
        [CellPhone_AreaCode] = @cellPhone_AreaCode,
        [CellPhone_Exchange] = @cellPhone_Exchange,
        [CellPhone_Number] = @cellPhone_Number,
        [MarriedToPersonId] = @marriedToPersonId,
        [LastUpdatedWhen] = GETDATE()
    WHERE [PersonId] = @employeeId;

    UPDATE [EmployeeWithSpouse]..[Employee]
    SET
        [HireDate] = @hireDate
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [pEmployee_Get]
AS
BEGIN
    SELECT
        e.[EmployeeId] AS ""Id"",
        p.[Name] AS ""Name"",
        p.[CellPhone_AreaCode] AS ""CellPhone.AreaCode"",
        p.[CellPhone_Exchange] AS ""CellPhone.Exchange"",
        p.[CellPhone_Number] AS ""CellPhone.Number"",
        p.[MarriedToPersonId] AS ""MarriedToPersonId"",
        e.[HireDate] AS ""HireDate""
    FROM [EmployeeWithSpouse]..[Employee] e
    INNER JOIN [EmployeeWithSpouse]..[Person] p
        ON e.[EmployeeId] = p.[PersonId];

END;
GO

CREATE PROCEDURE [pEmployee_GetById]
    @employeeId INT
AS
BEGIN
    SELECT
        e.[EmployeeId] AS ""Id"",
        p.[Name] AS ""Name"",
        p.[CellPhone_AreaCode] AS ""CellPhone.AreaCode"",
        p.[CellPhone_Exchange] AS ""CellPhone.Exchange"",
        p.[CellPhone_Number] AS ""CellPhone.Number"",
        p.[MarriedToPersonId] AS ""MarriedToPersonId"",
        e.[HireDate] AS ""HireDate""
    FROM [EmployeeWithSpouse]..[Employee] e
    INNER JOIN [EmployeeWithSpouse]..[Person] p
        ON e.[EmployeeId] = p.[PersonId]
    WHERE e.[EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [EmployeeWithSpouse]..[Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [pPerson_Insert]
    @name VARCHAR(50),
    @createdBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4),
    @marriedToPersonId INT
AS
BEGIN
    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [EmployeeWithSpouse]..[Person]
    (
        [Name],
        [CreatedBy],
        [CellPhone_AreaCode],
        [CellPhone_Exchange],
        [CellPhone_Number],
        [MarriedToPersonId]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @createdBy,
        @cellPhone_AreaCode,
        @cellPhone_Exchange,
        @cellPhone_Number,
        @marriedToPersonId
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [pPerson_Update]
    @personId INT,
    @name VARCHAR(50),
    @lastUpdatedBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4),
    @marriedToPersonId INT
AS
BEGIN
    UPDATE [EmployeeWithSpouse]..[Person]
    SET
        [Name] = @name,
        [LastUpdatedBy] = @lastUpdatedBy,
        [CellPhone_AreaCode] = @cellPhone_AreaCode,
        [CellPhone_Exchange] = @cellPhone_Exchange,
        [CellPhone_Number] = @cellPhone_Number,
        [MarriedToPersonId] = @marriedToPersonId,
        [LastUpdatedWhen] = GETDATE()
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [pPerson_Get]
AS
BEGIN
    SELECT
        _q_.[Id] AS ""Id"",
        _q_.[Name] AS ""Name"",
        _q_.[CellPhone.AreaCode] AS ""CellPhone.AreaCode"",
        _q_.[CellPhone.Exchange] AS ""CellPhone.Exchange"",
        _q_.[CellPhone.Number] AS ""CellPhone.Number"",
        _q_.[MarriedToPersonId] AS ""MarriedToPersonId"",
        _q_.[HireDate] AS ""HireDate"",
        _q_.[_EntityType_] AS ""_EntityType_""
    FROM 
    (
        SELECT
            e.[EmployeeId] AS ""Id"",
            p.[Name] AS ""Name"",
            p.[CellPhone_AreaCode] AS ""CellPhone.AreaCode"",
            p.[CellPhone_Exchange] AS ""CellPhone.Exchange"",
            p.[CellPhone_Number] AS ""CellPhone.Number"",
            p.[MarriedToPersonId] AS ""MarriedToPersonId"",
            e.[HireDate] AS ""HireDate"",
            1 AS ""_EntityType_""
        FROM [EmployeeWithSpouse]..[Employee] e
        INNER JOIN [EmployeeWithSpouse]..[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS ""Id"",
                p.[Name] AS ""Name"",
                p.[CellPhone_AreaCode] AS ""CellPhone.AreaCode"",
                p.[CellPhone_Exchange] AS ""CellPhone.Exchange"",
                p.[CellPhone_Number] AS ""CellPhone.Number"",
                p.[MarriedToPersonId] AS ""MarriedToPersonId"",
                NULL AS ""HireDate"",
                2 AS ""_EntityType_""
            FROM [EmployeeWithSpouse]..[Person] p
            LEFT OUTER JOIN [EmployeeWithSpouse]..[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
        )
    ) _q_;

END;
GO

CREATE PROCEDURE [pPerson_GetById]
    @personId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS ""Id"",
        _q_.[Name] AS ""Name"",
        _q_.[CellPhone.AreaCode] AS ""CellPhone.AreaCode"",
        _q_.[CellPhone.Exchange] AS ""CellPhone.Exchange"",
        _q_.[CellPhone.Number] AS ""CellPhone.Number"",
        _q_.[MarriedToPersonId] AS ""MarriedToPersonId"",
        _q_.[HireDate] AS ""HireDate"",
        _q_.[_EntityType_] AS ""_EntityType_""
    FROM 
    (
        SELECT
            e.[EmployeeId] AS ""Id"",
            p.[Name] AS ""Name"",
            p.[CellPhone_AreaCode] AS ""CellPhone.AreaCode"",
            p.[CellPhone_Exchange] AS ""CellPhone.Exchange"",
            p.[CellPhone_Number] AS ""CellPhone.Number"",
            p.[MarriedToPersonId] AS ""MarriedToPersonId"",
            e.[HireDate] AS ""HireDate"",
            1 AS ""_EntityType_""
        FROM [EmployeeWithSpouse]..[Employee] e
        INNER JOIN [EmployeeWithSpouse]..[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS ""Id"",
                p.[Name] AS ""Name"",
                p.[CellPhone_AreaCode] AS ""CellPhone.AreaCode"",
                p.[CellPhone_Exchange] AS ""CellPhone.Exchange"",
                p.[CellPhone_Number] AS ""CellPhone.Number"",
                p.[MarriedToPersonId] AS ""MarriedToPersonId"",
                NULL AS ""HireDate"",
                2 AS ""_EntityType_""
            FROM [EmployeeWithSpouse]..[Person] p
            LEFT OUTER JOIN [EmployeeWithSpouse]..[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @personId;

END;
GO

CREATE PROCEDURE [pPerson_GetSpouse_ForPerson]
    @marriedToPersonId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS ""Id"",
        _q_.[Name] AS ""Name"",
        _q_.[CellPhone.AreaCode] AS ""CellPhone.AreaCode"",
        _q_.[CellPhone.Exchange] AS ""CellPhone.Exchange"",
        _q_.[CellPhone.Number] AS ""CellPhone.Number"",
        _q_.[MarriedToPersonId] AS ""MarriedToPersonId"",
        _q_.[HireDate] AS ""HireDate"",
        _q_.[_EntityType_] AS ""_EntityType_""
    FROM 
    (
        SELECT
            e.[EmployeeId] AS ""Id"",
            p.[Name] AS ""Name"",
            p.[CellPhone_AreaCode] AS ""CellPhone.AreaCode"",
            p.[CellPhone_Exchange] AS ""CellPhone.Exchange"",
            p.[CellPhone_Number] AS ""CellPhone.Number"",
            p.[MarriedToPersonId] AS ""MarriedToPersonId"",
            e.[HireDate] AS ""HireDate"",
            1 AS ""_EntityType_""
        FROM [EmployeeWithSpouse]..[Employee] e
        INNER JOIN [EmployeeWithSpouse]..[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS ""Id"",
                p.[Name] AS ""Name"",
                p.[CellPhone_AreaCode] AS ""CellPhone.AreaCode"",
                p.[CellPhone_Exchange] AS ""CellPhone.Exchange"",
                p.[CellPhone_Number] AS ""CellPhone.Number"",
                p.[MarriedToPersonId] AS ""MarriedToPersonId"",
                NULL AS ""HireDate"",
                2 AS ""_EntityType_""
            FROM [EmployeeWithSpouse]..[Person] p
            LEFT OUTER JOIN [EmployeeWithSpouse]..[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
        )
    ) _q_
    WHERE _q_.[MarriedToPersonId] = @marriedToPersonId;

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
        public void Employee_With_Spouse_Tests()
        {
            var inputDto = new SaveEmployeeInputDto
            {
                Name = "Jorge",
                HireDate = new System.DateTime(2015, 7, 18),
                CellPhone = new SavePhoneNumberInputDto
                {
                    AreaCode = "786",
                    Exchange = "233",
                    Number = "1111"
                },
                Spouse = new PersonInputDto
                {
                    Name = "Yana",
                    CellPhone = new PhoneNumberInputDto
                    {
                        AreaCode = "954",
                        Exchange = "122",
                        Number = "8888"
                    }
                }
            };

            var result = new ValidationResult();

            inputDto.Validate(result);

            Assert.IsTrue(result.IsValid);

            var commandAggregate = new SaveEmployeeCommandAggregate(inputDto);

            commandAggregate.Save();

            var employeeId = commandAggregate.RootEntity.Id;

            var queryAggregate = new EmployeeQueryAggregate();

            var employee = queryAggregate.Get(employeeId);

            Assert.AreEqual("Jorge", employee.Name);

            Assert.AreEqual(new System.DateTime(2015, 7, 18), employee.HireDate);

            var cellPhone = employee.CellPhone;

            Assert.AreEqual("786", cellPhone.AreaCode);

            Assert.AreEqual("233", cellPhone.Exchange);

            Assert.AreEqual("1111", cellPhone.Number);

            var spouse = employee.Spouse;

            Assert.AreEqual("Yana", spouse.Name);

            cellPhone = spouse.CellPhone;

            Assert.AreEqual("954", cellPhone.AreaCode);

            Assert.AreEqual("122", cellPhone.Exchange);

            Assert.AreEqual("8888", cellPhone.Number);
        }

        [TestMethod]
        public void Employee_With_Spouse_No_Cell_Phones_Tests()
        {
            var inputDto = new SaveEmployeeInputDto
            {
                Name = "Jorge",
                HireDate = new System.DateTime(2015, 7, 18),
                //CellPhone = new SavePhoneNumberInputDto
                //{
                //    AreaCode = "786",
                //    Exchange = "233",
                //    Number = "1111"
                //},
                Spouse = new PersonInputDto
                {
                    Name = "Yana",
                    //CellPhone = new PhoneNumberInputDto
                    //{
                    //    AreaCode = "954",
                    //    Exchange = "122",
                    //    Number = "8888"
                    //}
                }
            };

            var result = new ValidationResult();

            inputDto.Validate(result);

            Assert.IsTrue(result.IsValid);

            var commandAggregate = new SaveEmployeeCommandAggregate(inputDto);

            commandAggregate.Save();

            var employeeId = commandAggregate.RootEntity.Id;

            var queryAggregate = new EmployeeQueryAggregate();

            var employee = queryAggregate.Get(employeeId);

            Assert.AreEqual("Jorge", employee.Name);

            Assert.AreEqual(new System.DateTime(2015, 7, 18), employee.HireDate);

            Assert.IsNull(employee.CellPhone);

            var spouse = employee.Spouse;

            Assert.AreEqual("Yana", spouse.Name);

            Assert.IsNull(spouse.CellPhone);
        }

        [TestMethod]
        public void Employee_With_No_Spouse_No_Cell_Phone_Tests()
        {
            var inputDto = new SaveEmployeeInputDto
            {
                Name = "Jorge",
                HireDate = new System.DateTime(2015, 7, 18),
                //CellPhone = new SavePhoneNumberInputDto
                //{
                //    AreaCode = "786",
                //    Exchange = "233",
                //    Number = "1111"
                //},
                //Spouse = new PersonInputDto
                //{
                //    Name = "Yana",
                //    CellPhone = new PhoneNumberInputDto
                //    {
                //        AreaCode = "954",
                //        Exchange = "122",
                //        Number = "8888"
                //    }
                //}
            };

            var result = new ValidationResult();

            inputDto.Validate(result);

            Assert.IsTrue(result.IsValid);

            var commandAggregate = new SaveEmployeeCommandAggregate(inputDto);

            commandAggregate.Save();

            var employeeId = commandAggregate.RootEntity.Id;

            var queryAggregate = new EmployeeQueryAggregate();

            var employee = queryAggregate.Get(employeeId);

            Assert.AreEqual("Jorge", employee.Name);

            Assert.AreEqual(new System.DateTime(2015, 7, 18), employee.HireDate);

            Assert.IsNull(employee.CellPhone);

            Assert.IsNull(employee.Spouse);
        }
    }
}

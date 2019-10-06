
USE master
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

CREATE SCHEMA [EmployeeBoundedContext];
GO

CREATE TABLE [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee]
(
    [EmployeeId] INT NOT NULL,
    [HireDate] DATETIME NOT NULL
    CONSTRAINT Employee_PK PRIMARY KEY ([EmployeeId])
);
GO

CREATE TABLE [EmployeeWithSpouse].[EmployeeBoundedContext].[Person]
(
    [PersonId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [CellPhone_AreaCode] VARCHAR(3),
    [CellPhone_Exchange] VARCHAR(3),
    [CellPhone_Number] VARCHAR(4),
    [MarriedToPersonId] INT
    CONSTRAINT Person_PK PRIMARY KEY ([PersonId])
);
GO

ALTER TABLE [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee]
    ADD CONSTRAINT Employee_Person_IFK FOREIGN KEY ([EmployeeId])
        REFERENCES [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Employee_Person_IFK_IX]
    ON [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee]
    (
        [EmployeeId]
    );
GO

ALTER TABLE [EmployeeWithSpouse].[EmployeeBoundedContext].[Person]
    ADD CONSTRAINT Person_Spouse_FK FOREIGN KEY ([MarriedToPersonId])
        REFERENCES [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Spouse_FK_IX]
    ON [EmployeeWithSpouse].[EmployeeBoundedContext].[Person]
    (
        [MarriedToPersonId]
    );
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_DeleteSpouse]
    @marriedToPersonId INT
AS
BEGIN
    DELETE FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Person]
    WHERE [MarriedToPersonId] = @marriedToPersonId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_Delete]
    @employeeId INT
AS
BEGIN
    DELETE FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee]
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_Insert]
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

    INSERT INTO [EmployeeWithSpouse].[EmployeeBoundedContext].[Person]
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

    INSERT INTO [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee]
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

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_Update]
    @employeeId INT,
    @hireDate DATETIME,
    @name VARCHAR(50),
    @updatedBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4),
    @marriedToPersonId INT
AS
BEGIN
    UPDATE [EmployeeWithSpouse].[EmployeeBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [CellPhone_AreaCode] = @cellPhone_AreaCode,
        [CellPhone_Exchange] = @cellPhone_Exchange,
        [CellPhone_Number] = @cellPhone_Number,
        [MarriedToPersonId] = @marriedToPersonId,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @employeeId;

    UPDATE [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee]
    SET
        [HireDate] = @hireDate
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_Get]
AS
BEGIN
    SELECT
        e.[EmployeeId] AS "Id",
        p.[Name] AS "Name",
        p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
        p.[CellPhone_Exchange] AS "CellPhone.Exchange",
        p.[CellPhone_Number] AS "CellPhone.Number",
        p.[MarriedToPersonId] AS "MarriedToPersonId",
        e.[HireDate] AS "HireDate"
    FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee] e
    INNER JOIN [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId];

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_GetById]
    @employeeId INT
AS
BEGIN
    SELECT
        e.[EmployeeId] AS "Id",
        p.[Name] AS "Name",
        p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
        p.[CellPhone_Exchange] AS "CellPhone.Exchange",
        p.[CellPhone_Number] AS "CellPhone.Number",
        p.[MarriedToPersonId] AS "MarriedToPersonId",
        e.[HireDate] AS "HireDate"
    FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee] e
    INNER JOIN [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId]
    WHERE e.[EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Insert]
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

    INSERT INTO [EmployeeWithSpouse].[EmployeeBoundedContext].[Person]
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

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Update]
    @personId INT,
    @name VARCHAR(50),
    @updatedBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4),
    @marriedToPersonId INT
AS
BEGIN
    UPDATE [EmployeeWithSpouse].[EmployeeBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [CellPhone_AreaCode] = @cellPhone_AreaCode,
        [CellPhone_Exchange] = @cellPhone_Exchange,
        [CellPhone_Number] = @cellPhone_Number,
        [MarriedToPersonId] = @marriedToPersonId,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Get]
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[CellPhone.AreaCode] AS "CellPhone.AreaCode",
        _q_.[CellPhone.Exchange] AS "CellPhone.Exchange",
        _q_.[CellPhone.Number] AS "CellPhone.Number",
        _q_.[MarriedToPersonId] AS "MarriedToPersonId",
        _q_.[HireDate] AS "HireDate",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
            p.[CellPhone_Exchange] AS "CellPhone.Exchange",
            p.[CellPhone_Number] AS "CellPhone.Number",
            p.[MarriedToPersonId] AS "MarriedToPersonId",
            e.[HireDate] AS "HireDate",
            1 AS "_EntityType_"
        FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee] e
        INNER JOIN [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
                p.[CellPhone_Exchange] AS "CellPhone.Exchange",
                p.[CellPhone_Number] AS "CellPhone.Number",
                p.[MarriedToPersonId] AS "MarriedToPersonId",
                NULL AS "HireDate",
                2 AS "_EntityType_"
            FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] p
            LEFT OUTER JOIN [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
        )
    ) _q_;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_GetById]
    @personId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[CellPhone.AreaCode] AS "CellPhone.AreaCode",
        _q_.[CellPhone.Exchange] AS "CellPhone.Exchange",
        _q_.[CellPhone.Number] AS "CellPhone.Number",
        _q_.[MarriedToPersonId] AS "MarriedToPersonId",
        _q_.[HireDate] AS "HireDate",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
            p.[CellPhone_Exchange] AS "CellPhone.Exchange",
            p.[CellPhone_Number] AS "CellPhone.Number",
            p.[MarriedToPersonId] AS "MarriedToPersonId",
            e.[HireDate] AS "HireDate",
            1 AS "_EntityType_"
        FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee] e
        INNER JOIN [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
                p.[CellPhone_Exchange] AS "CellPhone.Exchange",
                p.[CellPhone_Number] AS "CellPhone.Number",
                p.[MarriedToPersonId] AS "MarriedToPersonId",
                NULL AS "HireDate",
                2 AS "_EntityType_"
            FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] p
            LEFT OUTER JOIN [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @personId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_GetSpouse_ForPerson]
    @marriedToPersonId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[CellPhone.AreaCode] AS "CellPhone.AreaCode",
        _q_.[CellPhone.Exchange] AS "CellPhone.Exchange",
        _q_.[CellPhone.Number] AS "CellPhone.Number",
        _q_.[MarriedToPersonId] AS "MarriedToPersonId",
        _q_.[HireDate] AS "HireDate",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
            p.[CellPhone_Exchange] AS "CellPhone.Exchange",
            p.[CellPhone_Number] AS "CellPhone.Number",
            p.[MarriedToPersonId] AS "MarriedToPersonId",
            e.[HireDate] AS "HireDate",
            1 AS "_EntityType_"
        FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee] e
        INNER JOIN [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
                p.[CellPhone_Exchange] AS "CellPhone.Exchange",
                p.[CellPhone_Number] AS "CellPhone.Number",
                p.[MarriedToPersonId] AS "MarriedToPersonId",
                NULL AS "HireDate",
                2 AS "_EntityType_"
            FROM [EmployeeWithSpouse].[EmployeeBoundedContext].[Person] p
            LEFT OUTER JOIN [EmployeeWithSpouse].[EmployeeBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
        )
    ) _q_
    WHERE _q_.[MarriedToPersonId] = @marriedToPersonId;

END;
GO


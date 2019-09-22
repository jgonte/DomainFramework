
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'EmployeeWithDependants'
)
BEGIN
    DROP DATABASE [EmployeeWithDependants]
END
GO

CREATE DATABASE [EmployeeWithDependants]
GO

USE [EmployeeWithDependants]
GO

CREATE SCHEMA [EmployeeBoundedContext];
GO

CREATE TABLE [EmployeeWithDependants].[EmployeeBoundedContext].[Employee]
(
    [EmployeeId] INT NOT NULL,
    [HireDate] DATETIME NOT NULL
    CONSTRAINT Employee_PK PRIMARY KEY ([EmployeeId])
);
GO

CREATE TABLE [EmployeeWithDependants].[EmployeeBoundedContext].[Person]
(
    [PersonId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME,
    [CellPhone_AreaCode] VARCHAR(3) NOT NULL,
    [CellPhone_Exchange] VARCHAR(3) NOT NULL,
    [CellPhone_Number] VARCHAR(4) NOT NULL,
    [ProviderEmployeeId] INT
    CONSTRAINT Person_PK PRIMARY KEY ([PersonId])
);
GO

ALTER TABLE [EmployeeWithDependants].[EmployeeBoundedContext].[Employee]
    ADD CONSTRAINT Employee_Person_IFK FOREIGN KEY ([EmployeeId])
        REFERENCES [EmployeeWithDependants].[EmployeeBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Employee_Person_IFK_IX]
    ON [EmployeeWithDependants].[EmployeeBoundedContext].[Employee]
    (
        [EmployeeId]
    );
GO

ALTER TABLE [EmployeeWithDependants].[EmployeeBoundedContext].[Person]
    ADD CONSTRAINT Employee_Dependants_FK FOREIGN KEY ([ProviderEmployeeId])
        REFERENCES [EmployeeWithDependants].[EmployeeBoundedContext].[Employee] ([EmployeeId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Employee_Dependants_FK_IX]
    ON [EmployeeWithDependants].[EmployeeBoundedContext].[Person]
    (
        [ProviderEmployeeId]
    );
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_DeleteDependants]
    @providerEmployeeId INT
AS
BEGIN
    DELETE FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Person]
    WHERE [ProviderEmployeeId] = @providerEmployeeId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_Delete]
    @employeeId INT
AS
BEGIN
    DELETE FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Employee]
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
    @providerEmployeeId INT
AS
BEGIN
    DECLARE @personId INT;

    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [EmployeeWithDependants].[EmployeeBoundedContext].[Person]
    (
        [Name],
        [CreatedBy],
        [CellPhone_AreaCode],
        [CellPhone_Exchange],
        [CellPhone_Number],
        [ProviderEmployeeId]
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
        @providerEmployeeId
    );

    SELECT
        @personId = [PersonId]
    FROM @personOutputData;

    INSERT INTO [EmployeeWithDependants].[EmployeeBoundedContext].[Employee]
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
    @lastUpdatedBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4),
    @providerEmployeeId INT
AS
BEGIN
    UPDATE [EmployeeWithDependants].[EmployeeBoundedContext].[Person]
    SET
        [Name] = @name,
        [LastUpdatedBy] = @lastUpdatedBy,
        [CellPhone_AreaCode] = @cellPhone_AreaCode,
        [CellPhone_Exchange] = @cellPhone_Exchange,
        [CellPhone_Number] = @cellPhone_Number,
        [ProviderEmployeeId] = @providerEmployeeId,
        [LastUpdatedWhen] = GETDATE()
    WHERE [PersonId] = @employeeId;

    UPDATE [EmployeeWithDependants].[EmployeeBoundedContext].[Employee]
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
        p.[ProviderEmployeeId] AS "ProviderEmployeeId",
        e.[HireDate] AS "HireDate"
    FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Employee] e
    INNER JOIN [EmployeeWithDependants].[EmployeeBoundedContext].[Person] p
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
        p.[ProviderEmployeeId] AS "ProviderEmployeeId",
        e.[HireDate] AS "HireDate"
    FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Employee] e
    INNER JOIN [EmployeeWithDependants].[EmployeeBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId]
    WHERE e.[EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Insert]
    @name VARCHAR(50),
    @createdBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4),
    @providerEmployeeId INT
AS
BEGIN
    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [EmployeeWithDependants].[EmployeeBoundedContext].[Person]
    (
        [Name],
        [CreatedBy],
        [CellPhone_AreaCode],
        [CellPhone_Exchange],
        [CellPhone_Number],
        [ProviderEmployeeId]
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
        @providerEmployeeId
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Update]
    @personId INT,
    @name VARCHAR(50),
    @lastUpdatedBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4),
    @providerEmployeeId INT
AS
BEGIN
    UPDATE [EmployeeWithDependants].[EmployeeBoundedContext].[Person]
    SET
        [Name] = @name,
        [LastUpdatedBy] = @lastUpdatedBy,
        [CellPhone_AreaCode] = @cellPhone_AreaCode,
        [CellPhone_Exchange] = @cellPhone_Exchange,
        [CellPhone_Number] = @cellPhone_Number,
        [ProviderEmployeeId] = @providerEmployeeId,
        [LastUpdatedWhen] = GETDATE()
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
        _q_.[ProviderEmployeeId] AS "ProviderEmployeeId",
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
            p.[ProviderEmployeeId] AS "ProviderEmployeeId",
            e.[HireDate] AS "HireDate",
            1 AS "_EntityType_"
        FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Employee] e
        INNER JOIN [EmployeeWithDependants].[EmployeeBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
                p.[CellPhone_Exchange] AS "CellPhone.Exchange",
                p.[CellPhone_Number] AS "CellPhone.Number",
                p.[ProviderEmployeeId] AS "ProviderEmployeeId",
                NULL AS "HireDate",
                2 AS "_EntityType_"
            FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Person] p
            LEFT OUTER JOIN [EmployeeWithDependants].[EmployeeBoundedContext].[Employee] e
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
        _q_.[ProviderEmployeeId] AS "ProviderEmployeeId",
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
            p.[ProviderEmployeeId] AS "ProviderEmployeeId",
            e.[HireDate] AS "HireDate",
            1 AS "_EntityType_"
        FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Employee] e
        INNER JOIN [EmployeeWithDependants].[EmployeeBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
                p.[CellPhone_Exchange] AS "CellPhone.Exchange",
                p.[CellPhone_Number] AS "CellPhone.Number",
                p.[ProviderEmployeeId] AS "ProviderEmployeeId",
                NULL AS "HireDate",
                2 AS "_EntityType_"
            FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Person] p
            LEFT OUTER JOIN [EmployeeWithDependants].[EmployeeBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @personId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_GetDependants_ForEmployee]
    @providerEmployeeId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[CellPhone.AreaCode] AS "CellPhone.AreaCode",
        _q_.[CellPhone.Exchange] AS "CellPhone.Exchange",
        _q_.[CellPhone.Number] AS "CellPhone.Number",
        _q_.[ProviderEmployeeId] AS "ProviderEmployeeId",
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
            p.[ProviderEmployeeId] AS "ProviderEmployeeId",
            e.[HireDate] AS "HireDate",
            1 AS "_EntityType_"
        FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Employee] e
        INNER JOIN [EmployeeWithDependants].[EmployeeBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
                p.[CellPhone_Exchange] AS "CellPhone.Exchange",
                p.[CellPhone_Number] AS "CellPhone.Number",
                p.[ProviderEmployeeId] AS "ProviderEmployeeId",
                NULL AS "HireDate",
                2 AS "_EntityType_"
            FROM [EmployeeWithDependants].[EmployeeBoundedContext].[Person] p
            LEFT OUTER JOIN [EmployeeWithDependants].[EmployeeBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
        )
    ) _q_
    WHERE _q_.[ProviderEmployeeId] = @providerEmployeeId;

END;
GO



USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'CountryWithCapitalCity'
)
BEGIN
    DROP DATABASE [CountryWithCapitalCity]
END
GO

CREATE DATABASE [CountryWithCapitalCity]
GO

USE [CountryWithCapitalCity]
GO

CREATE SCHEMA [CountryBoundedContext];
GO

CREATE TABLE [CountryWithCapitalCity].[CountryBoundedContext].[Country]
(
    [CountryCode] CHAR(2) NOT NULL,
    [Name] VARCHAR(150) NOT NULL,
    [IsActive] BIT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Country_PK PRIMARY KEY ([CountryCode])
);
GO

CREATE TABLE [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity]
(
    [CapitalCityId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(150) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [CountryCode] CHAR(2) NOT NULL
    CONSTRAINT CapitalCity_PK PRIMARY KEY ([CapitalCityId])
);
GO

ALTER TABLE [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity]
    ADD CONSTRAINT Country_CapitalCity_FK FOREIGN KEY ([CountryCode])
        REFERENCES [CountryWithCapitalCity].[CountryBoundedContext].[Country] ([CountryCode])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Country_CapitalCity_FK_IX]
    ON [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity]
    (
        [CountryCode]
    );
GO

CREATE PROCEDURE [CountryBoundedContext].[pCapitalCity_Delete]
    @capitalCityId INT
AS
BEGIN
    DELETE FROM [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity]
    WHERE [CapitalCityId] = @capitalCityId;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCapitalCity_Insert]
    @name VARCHAR(150),
    @createdBy INT,
    @countryCode CHAR(2)
AS
BEGIN
    DECLARE @capitalCityOutputData TABLE
    (
        [CapitalCityId] INT
    );

    INSERT INTO [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity]
    (
        [Name],
        [CreatedBy],
        [CountryCode]
    )
    OUTPUT
        INSERTED.[CapitalCityId]
        INTO @capitalCityOutputData
    VALUES
    (
        @name,
        @createdBy,
        @countryCode
    );

    SELECT
        [CapitalCityId]
    FROM @capitalCityOutputData;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCapitalCity_Update]
    @capitalCityId INT,
    @name VARCHAR(150),
    @updatedBy INT,
    @countryCode CHAR(2)
AS
BEGIN
    UPDATE [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [CountryCode] = @countryCode,
        [UpdatedDateTime] = GETDATE()
    WHERE [CapitalCityId] = @capitalCityId;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCapitalCity_Get]
AS
BEGIN
    SELECT
        c.[CapitalCityId] AS "Id",
        c.[Name] AS "Name",
        c.[CountryCode] AS "CountryCode"
    FROM [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity] c;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCapitalCity_GetById]
    @capitalCityId INT
AS
BEGIN
    SELECT
        c.[CapitalCityId] AS "Id",
        c.[Name] AS "Name",
        c.[CountryCode] AS "CountryCode"
    FROM [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity] c
    WHERE c.[CapitalCityId] = @capitalCityId;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCapitalCity_GetCapitalCity_ForCountry]
    @countryCode CHAR(2)
AS
BEGIN
    SELECT
        c.[CapitalCityId] AS "Id",
        c.[Name] AS "Name",
        c.[CountryCode] AS "CountryCode"
    FROM [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity] c
    WHERE c.[CountryCode] = @countryCode;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCountry_DeleteCapitalCity]
    @countryCode CHAR(2)
AS
BEGIN
    DELETE FROM [CountryWithCapitalCity].[CountryBoundedContext].[CapitalCity]
    WHERE [CountryCode] = @countryCode;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCountry_Activate]
    @countryCode CHAR(2),
    @isActive BIT,
    @updatedBy INT
AS
BEGIN
    UPDATE [CountryWithCapitalCity].[CountryBoundedContext].[Country]
    SET
        [IsActive] = @isActive,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [CountryCode] = @countryCode;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCountry_Deactivate]
    @countryCode CHAR(2),
    @isActive BIT,
    @updatedBy INT
AS
BEGIN
    UPDATE [CountryWithCapitalCity].[CountryBoundedContext].[Country]
    SET
        [IsActive] = @isActive,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [CountryCode] = @countryCode;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCountry_Insert]
    @countryCode CHAR(2),
    @name VARCHAR(150),
    @isActive BIT,
    @createdBy INT
AS
BEGIN
    INSERT INTO [CountryWithCapitalCity].[CountryBoundedContext].[Country]
    (
        [CountryCode],
        [Name],
        [IsActive],
        [CreatedBy]
    )
    VALUES
    (
        @countryCode,
        @name,
        @isActive,
        @createdBy
    );

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCountry_Update]
    @countryCode CHAR(2),
    @name VARCHAR(150),
    @isActive BIT,
    @updatedBy INT
AS
BEGIN
    UPDATE [CountryWithCapitalCity].[CountryBoundedContext].[Country]
    SET
        [Name] = @name,
        [IsActive] = @isActive,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [CountryCode] = @countryCode;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCountry_Get]
AS
BEGIN
    SELECT
        c.[CountryCode] AS "Id",
        c.[Name] AS "Name",
        c.[IsActive] AS "IsActive"
    FROM [CountryWithCapitalCity].[CountryBoundedContext].[Country] c;

END;
GO

CREATE PROCEDURE [CountryBoundedContext].[pCountry_GetById]
    @countryCode CHAR(2)
AS
BEGIN
    SELECT
        c.[CountryCode] AS "Id",
        c.[Name] AS "Name",
        c.[IsActive] AS "IsActive"
    FROM [CountryWithCapitalCity].[CountryBoundedContext].[Country] c
    WHERE c.[CountryCode] = @countryCode;

END;
GO


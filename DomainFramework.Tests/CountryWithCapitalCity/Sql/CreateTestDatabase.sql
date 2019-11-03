
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
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'c.[CapitalCityId] AS "Id",
        c.[Name] AS "Name",
        c.[CountryCode] AS "CountryCode"',
        @tableName = N'CapitalCity c',
        @count = @count OUTPUT

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
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'c.[CountryCode] AS "Id",
        c.[Name] AS "Name",
        c.[IsActive] AS "IsActive"',
        @tableName = N'Country c',
        @count = @count OUTPUT

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

CREATE PROCEDURE [pExecuteDynamicQuery]
	@$select NVARCHAR(MAX) = NULL,
	@$filter NVARCHAR(MAX) = NULL,
	@$orderby NVARCHAR(MAX) = NULL,
	@$skip NVARCHAR(10) = NULL,
	@$top NVARCHAR(10) = NULL,
	@selectList NVARCHAR(MAX),
	@tableName NVARCHAR(64),
	@count INT OUTPUT
AS
BEGIN

	DECLARE @sqlCommand NVARCHAR(MAX);
	DECLARE @paramDefinition NVARCHAR(100);

	SET @paramDefinition = N'@cnt INT OUTPUT'

	SET @sqlCommand = 
'
	SELECT
		 @cnt = COUNT(1)
	FROM [' + @tableName + ']
';

	IF @$filter IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	WHERE ' + @$filter;
	END

	SET @sqlCommand = @sqlCommand + 
'
	SELECT
	';

	IF @$select = '*'
	BEGIN
		SET @sqlCommand = @sqlCommand + @selectList;
	END
	ELSE
	BEGIN
		SET @sqlCommand = @sqlCommand + @$select;
	END

	SET @sqlCommand = @sqlCommand +
'
	FROM [' + @tableName + '] s
';

	IF @$filter IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	WHERE ' + @$filter;
	END

	IF @$orderby IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	ORDER BY ' + @$orderby;
	END
	ELSE
	BEGIN

	-- At least a dummy order by is required is $skip and $top are provided
		IF @$skip IS NOT NULL OR @$top IS NOT NULL
		BEGIN  
			SET @sqlCommand = @sqlCommand + 
' 
	ORDER BY 1 ASC';
		END
	END

	IF @$skip IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	OFFSET ' + @$skip + ' ROWS';
	END

	IF @$top IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	FETCH NEXT ' + @$top + ' ROWS ONLY';
	END

	EXECUTE sp_executesql @sqlCommand, @paramDefinition, @cnt = @count OUTPUT

END;

GO
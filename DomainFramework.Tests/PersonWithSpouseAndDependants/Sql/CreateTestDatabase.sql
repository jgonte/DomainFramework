
USE master
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

CREATE SCHEMA [PersonBoundedContext];
GO

CREATE TABLE [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
(
    [PersonId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [MarriedPersonId] INT,
    [ProviderPersonId] INT
    CONSTRAINT Person_PK PRIMARY KEY ([PersonId])
);
GO

ALTER TABLE [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
    ADD CONSTRAINT Person_Spouse_FK FOREIGN KEY ([MarriedPersonId])
        REFERENCES [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Spouse_FK_IX]
    ON [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
    (
        [MarriedPersonId]
    );
GO

ALTER TABLE [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
    ADD CONSTRAINT Person_Dependants_FK FOREIGN KEY ([ProviderPersonId])
        REFERENCES [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Dependants_FK_IX]
    ON [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
    (
        [ProviderPersonId]
    );
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_Insert]
    @name VARCHAR(50),
    @createdBy INT,
    @marriedPersonId INT = NULL,
    @providerPersonId INT = NULL
AS
BEGIN
    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
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

CREATE PROCEDURE [PersonBoundedContext].[pPerson_Update]
    @personId INT,
    @name VARCHAR(50),
    @updatedBy INT = NULL,
    @marriedPersonId INT = NULL,
    @providerPersonId INT = NULL
AS
BEGIN
    UPDATE [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [MarriedPersonId] = @marriedPersonId,
        [ProviderPersonId] = @providerPersonId,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_UnlinkDependants]
    @personId INT
AS
BEGIN
    UPDATE [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
    SET
        [ProviderPersonId] = NULL
    WHERE [ProviderPersonId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_UnlinkSpouse]
    @personId INT
AS
BEGIN
    UPDATE [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person]
    SET
        [MarriedPersonId] = NULL
    WHERE [MarriedPersonId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_Get]
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
        @selectList = N'    p.[PersonId] AS "Id",
    p.[Name] AS "Name",
    p.[MarriedPersonId] AS "MarriedPersonId",
    p.[ProviderPersonId] AS "ProviderPersonId"',
        @from = N'[PersonWithSpouseAndDependants].[PersonBoundedContext].[Person] p',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetAll]
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name",
        p.[MarriedPersonId] AS "MarriedPersonId",
        p.[ProviderPersonId] AS "ProviderPersonId"
    FROM [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person] p;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetById]
    @personId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name",
        p.[MarriedPersonId] AS "MarriedPersonId",
        p.[ProviderPersonId] AS "ProviderPersonId"
    FROM [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person] p
    WHERE p.[PersonId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetAllDependants]
    @personId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name",
        p.[MarriedPersonId] AS "MarriedPersonId",
        p.[ProviderPersonId] AS "ProviderPersonId"
    FROM [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person] p
    WHERE p.[ProviderPersonId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetSpouse]
    @personId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name",
        p.[MarriedPersonId] AS "MarriedPersonId",
        p.[ProviderPersonId] AS "ProviderPersonId"
    FROM [PersonWithSpouseAndDependants].[PersonBoundedContext].[Person] p
    WHERE p.[MarriedPersonId] = @personId;

END;
GO

CREATE PROCEDURE [pExecuteDynamicQuery]
	@$select NVARCHAR(MAX) = NULL,
	@$filter NVARCHAR(MAX) = NULL,
	@$orderby NVARCHAR(MAX) = NULL,
	@$skip NVARCHAR(10) = NULL,
	@$top NVARCHAR(10) = NULL,
	@selectList NVARCHAR(MAX),
	@from NVARCHAR(MAX),
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
	FROM ' + @from + '
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

	IF ISNULL(@$select, '*') = '*'
	BEGIN
		SET @sqlCommand = @sqlCommand + @selectList;
	END
	ELSE
	BEGIN
		SET @sqlCommand = @sqlCommand + @$select;
	END

	SET @sqlCommand = @sqlCommand +
'
	FROM ' + @from + '
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


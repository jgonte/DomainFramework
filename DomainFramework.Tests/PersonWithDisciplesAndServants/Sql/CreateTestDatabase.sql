
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'PersonWithDisciplesAndServants'
)
BEGIN
    DROP DATABASE [PersonWithDisciplesAndServants]
END
GO

CREATE DATABASE [PersonWithDisciplesAndServants]
GO

USE [PersonWithDisciplesAndServants]
GO

CREATE SCHEMA [PersonBoundedContext];
GO

CREATE TABLE [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
(
    [PersonId] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(50) NOT NULL,
    [Gender] CHAR(1) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [LeaderId] INT,
    [MasterId] INT
    CONSTRAINT Person_PK PRIMARY KEY ([PersonId])
);
GO

CREATE TABLE [PersonWithDisciplesAndServants].[PersonBoundedContext].[GenderLov]
(
    [Value] CHAR(1),
    [Label] VARCHAR(6)
    CONSTRAINT GenderLov_PK PRIMARY KEY ([Value])
);
GO

ALTER TABLE [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    ADD CONSTRAINT Person_Disciples_FK FOREIGN KEY ([LeaderId])
        REFERENCES [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Disciples_FK_IX]
    ON [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    (
        [LeaderId]
    );
GO

ALTER TABLE [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    ADD CONSTRAINT Person_Servants_FK FOREIGN KEY ([MasterId])
        REFERENCES [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Servants_FK_IX]
    ON [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    (
        [MasterId]
    );
GO

ALTER TABLE [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    ADD CONSTRAINT GenderLov_Person_FK FOREIGN KEY ([Gender])
        REFERENCES [PersonWithDisciplesAndServants].[PersonBoundedContext].[GenderLov] ([Value])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Gender_IX]
    ON [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    (
        [Gender]
    );
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_Insert]
    @name NVARCHAR(50),
    @gender CHAR(1),
    @createdBy INT,
    @leaderId INT = NULL,
    @masterId INT = NULL
AS
BEGIN
    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    (
        [Name],
        [Gender],
        [CreatedBy],
        [LeaderId],
        [MasterId]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @gender,
        @createdBy,
        @leaderId,
        @masterId
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_Update]
    @personId INT,
    @name NVARCHAR(50),
    @gender CHAR(1),
    @updatedBy INT = NULL,
    @leaderId INT = NULL,
    @masterId INT = NULL
AS
BEGIN
    UPDATE [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    SET
        [Name] = @name,
        [Gender] = @gender,
        [UpdatedBy] = @updatedBy,
        [LeaderId] = @leaderId,
        [MasterId] = @masterId,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_UnlinkDisciples]
    @personId INT
AS
BEGIN
    UPDATE [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    SET
        [LeaderId] = NULL
    WHERE [LeaderId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_UnlinkServants]
    @personId INT
AS
BEGIN
    UPDATE [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person]
    SET
        [MasterId] = NULL
    WHERE [MasterId] = @personId;

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
    p.[Gender] AS "Gender",
    p.[LeaderId] AS "LeaderId",
    p.[MasterId] AS "MasterId"',
        @from = N'[PersonWithDisciplesAndServants].[PersonBoundedContext].[Person] p',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetAll]
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name",
        p.[Gender] AS "Gender",
        p.[LeaderId] AS "LeaderId",
        p.[MasterId] AS "MasterId"
    FROM [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person] p;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetById]
    @personId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name",
        p.[Gender] AS "Gender",
        p.[LeaderId] AS "LeaderId",
        p.[MasterId] AS "MasterId"
    FROM [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person] p
    WHERE p.[PersonId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetAllDisciples]
    @personId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name",
        p.[Gender] AS "Gender",
        p.[LeaderId] AS "LeaderId",
        p.[MasterId] AS "MasterId"
    FROM [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person] p
    WHERE p.[LeaderId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetAllServants]
    @personId INT
AS
BEGIN
    SELECT
        p.[PersonId] AS "Id",
        p.[Name] AS "Name",
        p.[Gender] AS "Gender",
        p.[LeaderId] AS "LeaderId",
        p.[MasterId] AS "MasterId"
    FROM [PersonWithDisciplesAndServants].[PersonBoundedContext].[Person] p
    WHERE p.[MasterId] = @personId;

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetDisciples]
    @personId INT,
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    IF @$filter IS NULL
    BEGIN
        SET @$filter = N'p.[LeaderId] = ' + CAST(@personId AS NVARCHAR(10));
    END
    ELSE
    BEGIN
        SET @$filter = N'(' + N'p.[LeaderId] = ' + CAST(@personId AS NVARCHAR(10)) + N') AND (' + @$filter + N')';
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    p.[PersonId] AS "Id",
    p.[Name] AS "Name",
    p.[Gender] AS "Gender",
    p.[LeaderId] AS "LeaderId",
    p.[MasterId] AS "MasterId"',
        @from = N'[PersonWithDisciplesAndServants].[PersonBoundedContext].[Person] p',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [PersonBoundedContext].[pPerson_GetServants]
    @personId INT,
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    IF @$filter IS NULL
    BEGIN
        SET @$filter = N'p.[MasterId] = ' + CAST(@personId AS NVARCHAR(10));
    END
    ELSE
    BEGIN
        SET @$filter = N'(' + N'p.[MasterId] = ' + CAST(@personId AS NVARCHAR(10)) + N') AND (' + @$filter + N')';
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    p.[PersonId] AS "Id",
    p.[Name] AS "Name",
    p.[Gender] AS "Gender",
    p.[LeaderId] AS "LeaderId",
    p.[MasterId] AS "MasterId"',
        @from = N'[PersonWithDisciplesAndServants].[PersonBoundedContext].[Person] p',
        @count = @count OUTPUT

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

INSERT INTO [PersonWithDisciplesAndServants].[PersonBoundedContext].[GenderLov]
(
    [Value],
    [Label]
)
VALUES
(
    'M',
    'Male'
);

INSERT INTO [PersonWithDisciplesAndServants].[PersonBoundedContext].[GenderLov]
(
    [Value],
    [Label]
)
VALUES
(
    'F',
    'Female'
);


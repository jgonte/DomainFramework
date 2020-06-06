
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'SimpleEntityWithAutoGeneratedKey'
)
BEGIN
    DROP DATABASE [SimpleEntityWithAutoGeneratedKey]
END
GO

CREATE DATABASE [SimpleEntityWithAutoGeneratedKey]
GO

USE [SimpleEntityWithAutoGeneratedKey]
GO

CREATE SCHEMA [SimpleEntityWithAutoGeneratedKeyBoundedContext];
GO

CREATE SCHEMA [Audit];
GO

CREATE TABLE [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[TestEntity]
(
    [TestEntityId] INT NOT NULL IDENTITY,
    [Text] VARCHAR(50) NOT NULL,
    [Enumeration1] INT NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [Url_Value] VARCHAR(255) NOT NULL,
    [TypeValue1_DataType] INT NOT NULL,
    [TypeValue1_Data] VARCHAR(200) NOT NULL,
    [Distance_Selected] BIT,
    [Distance_YesNoNotSure] VARCHAR(1),
    [Traffic_Selected] BIT,
    [Traffic_YesNoNotSure] VARCHAR(1),
    [Time_Selected] BIT,
    [Time_YesNoNotSure] VARCHAR(1)
    CONSTRAINT TestEntity_PK PRIMARY KEY ([TestEntityId])
);
GO

CREATE TABLE [SimpleEntityWithAutoGeneratedKey].[Audit].[TestEntityAudit]
(
    [TestEntityAuditId] INT NOT NULL IDENTITY,
    [TestEntityId] INT NOT NULL,
    [Text] VARCHAR(50) NOT NULL,
    [Enumeration1] INT NOT NULL,
    [IsActive] BIT NOT NULL,
    [Url_Value] VARCHAR(255) NOT NULL,
    [TypeValue1_DataType] INT NOT NULL,
    [TypeValue1_Data] VARCHAR(200) NOT NULL,
    [Distance_Selected] BIT,
    [Distance_YesNoNotSure] VARCHAR(1),
    [Traffic_Selected] BIT,
    [Traffic_YesNoNotSure] VARCHAR(1),
    [Time_Selected] BIT,
    [Time_YesNoNotSure] VARCHAR(1),
    [ModifiedWhen] DATETIME NOT NULL,
    [ModifiedBy] INT NOT NULL,
    [Operation] CHAR(1) NOT NULL
    CONSTRAINT TestEntityAudit_PK PRIMARY KEY ([TestEntityAuditId])
);
GO

CREATE TABLE [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[UrlValues]
(
    [Value] VARCHAR(255)
    CONSTRAINT UrlValues_PK PRIMARY KEY ([Value])
);
GO

CREATE TABLE [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[WeightLov]
(
    [Value] VARCHAR(1),
    [Description] VARCHAR(10)
    CONSTRAINT WeightLov_PK PRIMARY KEY ([Value])
);
GO

CREATE TRIGGER TestEntityAuditUpdateTrigger
ON [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[TestEntity]
AFTER UPDATE
AS
BEGIN
    INSERT INTO [SimpleEntityWithAutoGeneratedKey].[Audit].[TestEntityAudit]
    (
        [TestEntityId],
        [Text],
        [Enumeration1],
        [IsActive],
        [Url_Value],
        [TypeValue1_DataType],
        [TypeValue1_Data],
        [Distance_Selected],
        [Distance_YesNoNotSure],
        [Traffic_Selected],
        [Traffic_YesNoNotSure],
        [Time_Selected],
        [Time_YesNoNotSure],
        [ModifiedWhen],
        [ModifiedBy],
        [Operation]
    )
    SELECT
        d.[TestEntityId],
        d.[Text],
        d.[Enumeration1],
        d.[IsActive],
        d.[Url_Value],
        d.[TypeValue1_DataType],
        d.[TypeValue1_Data],
        d.[Distance_Selected],
        d.[Distance_YesNoNotSure],
        d.[Traffic_Selected],
        d.[Traffic_YesNoNotSure],
        d.[Time_Selected],
        d.[Time_YesNoNotSure],
        GETDATE(),
        dbo.GetUserContext(),
        'U'
    FROM [DELETED] d;

END;
GO

CREATE TRIGGER TestEntityAuditDeleteTrigger
ON [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[TestEntity]
AFTER DELETE
AS
BEGIN
    INSERT INTO [SimpleEntityWithAutoGeneratedKey].[Audit].[TestEntityAudit]
    (
        [TestEntityId],
        [Text],
        [Enumeration1],
        [IsActive],
        [Url_Value],
        [TypeValue1_DataType],
        [TypeValue1_Data],
        [Distance_Selected],
        [Distance_YesNoNotSure],
        [Traffic_Selected],
        [Traffic_YesNoNotSure],
        [Time_Selected],
        [Time_YesNoNotSure],
        [ModifiedWhen],
        [ModifiedBy],
        [Operation]
    )
    SELECT
        d.[TestEntityId],
        d.[Text],
        d.[Enumeration1],
        d.[IsActive],
        d.[Url_Value],
        d.[TypeValue1_DataType],
        d.[TypeValue1_Data],
        d.[Distance_Selected],
        d.[Distance_YesNoNotSure],
        d.[Traffic_Selected],
        d.[Traffic_YesNoNotSure],
        d.[Time_Selected],
        d.[Time_YesNoNotSure],
        GETDATE(),
        dbo.GetUserContext(),
        'D'
    FROM [DELETED] d;

END;
GO

CREATE PROCEDURE [SimpleEntityWithAutoGeneratedKeyBoundedContext].[pTestEntity_Delete]
    @testEntityId INT,
    @userId INT
AS
BEGIN

    EXECUTE [dbo].[SetUserContext] @userId;

    DELETE FROM [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[TestEntity]
    WHERE [TestEntityId] = @testEntityId;

END;
GO

CREATE PROCEDURE [SimpleEntityWithAutoGeneratedKeyBoundedContext].[pTestEntity_Insert]
    @text VARCHAR(50),
    @enumeration1 INT,
    @createdBy INT,
    @url_Value VARCHAR(255),
    @typeValue1_DataType INT,
    @typeValue1_Data VARCHAR(200),
    @distance_Selected BIT = NULL,
    @distance_YesNoNotSure VARCHAR(1) = NULL,
    @traffic_Selected BIT = NULL,
    @traffic_YesNoNotSure VARCHAR(1) = NULL,
    @time_Selected BIT = NULL,
    @time_YesNoNotSure VARCHAR(1) = NULL
AS
BEGIN
    DECLARE @testEntityOutputData TABLE
    (
        [TestEntityId] INT,
        [IsActive] BIT
    );

    INSERT INTO [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[TestEntity]
    (
        [Text],
        [Enumeration1],
        [CreatedBy],
        [Url_Value],
        [TypeValue1_DataType],
        [TypeValue1_Data],
        [Distance_Selected],
        [Distance_YesNoNotSure],
        [Traffic_Selected],
        [Traffic_YesNoNotSure],
        [Time_Selected],
        [Time_YesNoNotSure]
    )
    OUTPUT
        INSERTED.[TestEntityId],
        INSERTED.[IsActive]
        INTO @testEntityOutputData
    VALUES
    (
        @text,
        @enumeration1,
        @createdBy,
        @url_Value,
        @typeValue1_DataType,
        @typeValue1_Data,
        @distance_Selected,
        @distance_YesNoNotSure,
        @traffic_Selected,
        @traffic_YesNoNotSure,
        @time_Selected,
        @time_YesNoNotSure
    );

    SELECT
        [TestEntityId],
        [IsActive]
    FROM @testEntityOutputData;

END;
GO

CREATE PROCEDURE [SimpleEntityWithAutoGeneratedKeyBoundedContext].[pTestEntity_Update]
    @testEntityId INT,
    @text VARCHAR(50),
    @enumeration1 INT,
    @isActive BIT,
    @url_Value VARCHAR(255),
    @typeValue1_DataType INT,
    @typeValue1_Data VARCHAR(200),
    @distance_Selected BIT = NULL,
    @distance_YesNoNotSure VARCHAR(1) = NULL,
    @traffic_Selected BIT = NULL,
    @traffic_YesNoNotSure VARCHAR(1) = NULL,
    @time_Selected BIT = NULL,
    @time_YesNoNotSure VARCHAR(1) = NULL,
    @userId INT
AS
BEGIN

    EXECUTE [dbo].[SetUserContext] @userId;

    UPDATE [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[TestEntity]
    SET
        [Text] = @text,
        [Enumeration1] = @enumeration1,
        [IsActive] = @isActive,
        [Url_Value] = @url_Value,
        [TypeValue1_DataType] = @typeValue1_DataType,
        [TypeValue1_Data] = @typeValue1_Data,
        [Distance_Selected] = @distance_Selected,
        [Distance_YesNoNotSure] = @distance_YesNoNotSure,
        [Traffic_Selected] = @traffic_Selected,
        [Traffic_YesNoNotSure] = @traffic_YesNoNotSure,
        [Time_Selected] = @time_Selected,
        [Time_YesNoNotSure] = @time_YesNoNotSure
    WHERE [TestEntityId] = @testEntityId;

END;
GO

CREATE PROCEDURE [SimpleEntityWithAutoGeneratedKeyBoundedContext].[pTestEntity_Get]
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
        @selectList = N'    t.[TestEntityId] AS "Id",
    t.[Text] AS "Text",
    t.[Enumeration1] AS "Enumeration1",
    t.[IsActive] AS "IsActive",
    t.[Url_Value] AS "Url.Value",
    t.[TypeValue1_DataType] AS "TypeValue1.DataType",
    t.[TypeValue1_Data] AS "TypeValue1.Data",
    t.[Distance_Selected] AS "Distance.Selected",
    t.[Distance_YesNoNotSure] AS "Distance.YesNoNotSure",
    t.[Traffic_Selected] AS "Traffic.Selected",
    t.[Traffic_YesNoNotSure] AS "Traffic.YesNoNotSure",
    t.[Time_Selected] AS "Time.Selected",
    t.[Time_YesNoNotSure] AS "Time.YesNoNotSure"',
        @from = N'[SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[TestEntity] t',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [SimpleEntityWithAutoGeneratedKeyBoundedContext].[pTestEntity_GetAll]
AS
BEGIN
    SELECT
        t.[TestEntityId] AS "Id",
        t.[Text] AS "Text",
        t.[Enumeration1] AS "Enumeration1",
        t.[IsActive] AS "IsActive",
        t.[Url_Value] AS "Url.Value",
        t.[TypeValue1_DataType] AS "TypeValue1.DataType",
        t.[TypeValue1_Data] AS "TypeValue1.Data",
        t.[Distance_Selected] AS "Distance.Selected",
        t.[Distance_YesNoNotSure] AS "Distance.YesNoNotSure",
        t.[Traffic_Selected] AS "Traffic.Selected",
        t.[Traffic_YesNoNotSure] AS "Traffic.YesNoNotSure",
        t.[Time_Selected] AS "Time.Selected",
        t.[Time_YesNoNotSure] AS "Time.YesNoNotSure"
    FROM [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[TestEntity] t;

END;
GO

CREATE PROCEDURE [SimpleEntityWithAutoGeneratedKeyBoundedContext].[pTestEntity_GetById]
    @testEntityId INT
AS
BEGIN
    SELECT
        t.[TestEntityId] AS "Id",
        t.[Text] AS "Text",
        t.[Enumeration1] AS "Enumeration1",
        t.[IsActive] AS "IsActive",
        t.[Url_Value] AS "Url.Value",
        t.[TypeValue1_DataType] AS "TypeValue1.DataType",
        t.[TypeValue1_Data] AS "TypeValue1.Data",
        t.[Distance_Selected] AS "Distance.Selected",
        t.[Distance_YesNoNotSure] AS "Distance.YesNoNotSure",
        t.[Traffic_Selected] AS "Traffic.Selected",
        t.[Traffic_YesNoNotSure] AS "Traffic.YesNoNotSure",
        t.[Time_Selected] AS "Time.Selected",
        t.[Time_YesNoNotSure] AS "Time.YesNoNotSure"
    FROM [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[TestEntity] t
    WHERE t.[TestEntityId] = @testEntityId;

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

CREATE PROCEDURE [dbo].[SetUserContext]
    @userId INT
AS
BEGIN

    SET NOCOUNT ON;

    EXEC sys.sp_set_session_context @key = N'UserId', @value = @userId;
END
GO

CREATE FUNCTION [dbo].[GetUserContext]()
RETURNS INT
AS
BEGIN
 
    DECLARE @userId INT;

    SELECT @userId = CONVERT(INT, SESSION_CONTEXT(N'UserId'));
 
    RETURN @userId;
 
END
GO

INSERT INTO [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[UrlValues]
(
    [Value]
)
VALUES
(
    'www.url1.com'
);

INSERT INTO [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[UrlValues]
(
    [Value]
)
VALUES
(
    'www.url2.com'
);

INSERT INTO [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[UrlValues]
(
    [Value]
)
VALUES
(
    'www.url3.com'
);

INSERT INTO [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[WeightLov]
(
    [Value],
    [Description]
)
VALUES
(
    'Y',
    'Yes'
);

INSERT INTO [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[WeightLov]
(
    [Value],
    [Description]
)
VALUES
(
    'N',
    'No'
);

INSERT INTO [SimpleEntityWithAutoGeneratedKey].[SimpleEntityWithAutoGeneratedKeyBoundedContext].[WeightLov]
(
    [Value],
    [Description]
)
VALUES
(
    '?',
    'Don''t know'
);


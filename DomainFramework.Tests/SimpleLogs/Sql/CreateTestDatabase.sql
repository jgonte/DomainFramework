
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'SimpleLogs'
)
BEGIN
    DROP DATABASE [SimpleLogs]
END
GO

CREATE DATABASE [SimpleLogs]
GO

USE [SimpleLogs]
GO

CREATE SCHEMA [SimpleLogsBoundedContext];
GO

CREATE TABLE [SimpleLogs].[SimpleLogsBoundedContext].[SimpleLog]
(
    [SimpleLogId] INT NOT NULL IDENTITY,
    [MessageType] VARCHAR(1) NOT NULL,
    [Message] VARCHAR(50) NOT NULL,
    [When] DATETIME NOT NULL DEFAULT GETDATE()
    CONSTRAINT SimpleLog_PK PRIMARY KEY ([SimpleLogId])
);
GO

CREATE TABLE [SimpleLogs].[SimpleLogsBoundedContext].[MessageTypes]
(
    [Value] VARCHAR(1),
    [Description] VARCHAR(22),
    [Label] VARCHAR(11)
    CONSTRAINT MessageTypes_PK PRIMARY KEY ([Value])
);
GO

ALTER TABLE [SimpleLogs].[SimpleLogsBoundedContext].[SimpleLog]
    ADD CONSTRAINT MessageTypes_SimpleLog_FK FOREIGN KEY ([MessageType])
        REFERENCES [SimpleLogs].[SimpleLogsBoundedContext].[MessageTypes] ([Value])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [SimpleLog_MessageType_IX]
    ON [SimpleLogs].[SimpleLogsBoundedContext].[SimpleLog]
    (
        [MessageType]
    );
GO

CREATE PROCEDURE [SimpleLogsBoundedContext].[pSimpleLog_Delete]
    @simpleLogId INT
AS
BEGIN
    DELETE FROM [SimpleLogs].[SimpleLogsBoundedContext].[SimpleLog]
    WHERE [SimpleLogId] = @simpleLogId;

END;
GO

CREATE PROCEDURE [SimpleLogsBoundedContext].[pSimpleLog_DeleteOlderLogs]
    @when DATETIME
AS
BEGIN
    DELETE FROM [SimpleLogs].[SimpleLogsBoundedContext].[SimpleLog]
    WHERE [When] < @when;

END;
GO

CREATE PROCEDURE [SimpleLogsBoundedContext].[pSimpleLog_Insert]
    @messageType VARCHAR(1),
    @message VARCHAR(50)
AS
BEGIN
    DECLARE @simpleLogOutputData TABLE
    (
        [SimpleLogId] INT,
        [When] DATETIME
    );

    INSERT INTO [SimpleLogs].[SimpleLogsBoundedContext].[SimpleLog]
    (
        [MessageType],
        [Message]
    )
    OUTPUT
        INSERTED.[SimpleLogId],
        INSERTED.[When]
        INTO @simpleLogOutputData
    VALUES
    (
        @messageType,
        @message
    );

    SELECT
        [SimpleLogId],
        [When]
    FROM @simpleLogOutputData;

END;
GO

CREATE PROCEDURE [SimpleLogsBoundedContext].[pSimpleLog_Get]
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
        @selectList = N'    s.[SimpleLogId] AS "Id",
    s.[MessageType] AS "MessageType",
    s.[Message] AS "Message",
    s.[When] AS "When"',
        @from = N'[SimpleLogs].[SimpleLogsBoundedContext].[SimpleLog] s',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [SimpleLogsBoundedContext].[pSimpleLog_GetAll]
AS
BEGIN
    SELECT
        s.[SimpleLogId] AS "Id",
        s.[MessageType] AS "MessageType",
        s.[Message] AS "Message",
        s.[When] AS "When"
    FROM [SimpleLogs].[SimpleLogsBoundedContext].[SimpleLog] s;

END;
GO

CREATE PROCEDURE [SimpleLogsBoundedContext].[pSimpleLog_GetById]
    @simpleLogId INT
AS
BEGIN
    SELECT
        s.[SimpleLogId] AS "Id",
        s.[MessageType] AS "MessageType",
        s.[Message] AS "Message",
        s.[When] AS "When"
    FROM [SimpleLogs].[SimpleLogsBoundedContext].[SimpleLog] s
    WHERE s.[SimpleLogId] = @simpleLogId;

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

INSERT INTO [SimpleLogs].[SimpleLogsBoundedContext].[MessageTypes]
(
    [Value],
    [Description],
    [Label]
)
VALUES
(
    'i',
    'Informational log type',
    'Information'
);

INSERT INTO [SimpleLogs].[SimpleLogsBoundedContext].[MessageTypes]
(
    [Value],
    [Description],
    [Label]
)
VALUES
(
    'd',
    'Debug log type',
    'Debug'
);

INSERT INTO [SimpleLogs].[SimpleLogsBoundedContext].[MessageTypes]
(
    [Value],
    [Description],
    [Label]
)
VALUES
(
    'w',
    'Warning log type',
    'Warning'
);

INSERT INTO [SimpleLogs].[SimpleLogsBoundedContext].[MessageTypes]
(
    [Value],
    [Description],
    [Label]
)
VALUES
(
    'e',
    'Error log type',
    'Error'
);


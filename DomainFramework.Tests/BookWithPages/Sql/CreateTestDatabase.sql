
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'BookWithPages'
)
BEGIN
    DROP DATABASE [BookWithPages]
END
GO

CREATE DATABASE [BookWithPages]
GO

USE [BookWithPages]
GO

CREATE SCHEMA [BookBoundedContext];
GO

CREATE TABLE [BookWithPages].[BookBoundedContext].[Book]
(
    [BookId] INT NOT NULL IDENTITY,
    [Title] VARCHAR(150) NOT NULL,
    [Category] INT NOT NULL,
    [DatePublished] DATETIME NOT NULL,
    [PublisherId] UNIQUEIDENTIFIER NOT NULL,
    [IsHardCopy] BIT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Book_PK PRIMARY KEY ([BookId])
);
GO

CREATE TABLE [BookWithPages].[BookBoundedContext].[Page]
(
    [PageId] INT NOT NULL IDENTITY,
    [Index] INT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [BookId] INT NOT NULL
    CONSTRAINT Page_PK PRIMARY KEY ([PageId])
);
GO

ALTER TABLE [BookWithPages].[BookBoundedContext].[Page]
    ADD CONSTRAINT Book_Pages_FK FOREIGN KEY ([BookId])
        REFERENCES [BookWithPages].[BookBoundedContext].[Book] ([BookId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Book_Pages_FK_IX]
    ON [BookWithPages].[BookBoundedContext].[Page]
    (
        [BookId]
    );
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_DeletePages]
    @bookId INT
AS
BEGIN
    DELETE FROM [BookWithPages].[BookBoundedContext].[Page]
    WHERE [BookId] = @bookId;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_Delete]
    @bookId INT
AS
BEGIN
    DELETE FROM [BookWithPages].[BookBoundedContext].[Book]
    WHERE [BookId] = @bookId;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_Insert]
    @title VARCHAR(150),
    @category INT,
    @datePublished DATETIME,
    @publisherId UNIQUEIDENTIFIER,
    @isHardCopy BIT,
    @createdBy INT
AS
BEGIN
    DECLARE @bookOutputData TABLE
    (
        [BookId] INT
    );

    INSERT INTO [BookWithPages].[BookBoundedContext].[Book]
    (
        [Title],
        [Category],
        [DatePublished],
        [PublisherId],
        [IsHardCopy],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[BookId]
        INTO @bookOutputData
    VALUES
    (
        @title,
        @category,
        @datePublished,
        @publisherId,
        @isHardCopy,
        @createdBy
    );

    SELECT
        [BookId]
    FROM @bookOutputData;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_Update]
    @bookId INT,
    @title VARCHAR(150),
    @category INT,
    @datePublished DATETIME,
    @publisherId UNIQUEIDENTIFIER,
    @isHardCopy BIT,
    @updatedBy INT
AS
BEGIN
    UPDATE [BookWithPages].[BookBoundedContext].[Book]
    SET
        [Title] = @title,
        [Category] = @category,
        [DatePublished] = @datePublished,
        [PublisherId] = @publisherId,
        [IsHardCopy] = @isHardCopy,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [BookId] = @bookId;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_Get]
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
        @selectList = N'b.[BookId] AS "Id",
        b.[Title] AS "Title",
        b.[Category] AS "Category",
        b.[DatePublished] AS "DatePublished",
        b.[PublisherId] AS "PublisherId",
        b.[IsHardCopy] AS "IsHardCopy"',
        @tableName = N'Book b',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_GetByTitle]
    @title VARCHAR(150)
AS
BEGIN
    SELECT
        b.[BookId] AS "Id",
        b.[Title] AS "Title",
        b.[Category] AS "Category",
        b.[DatePublished] AS "DatePublished",
        b.[PublisherId] AS "PublisherId",
        b.[IsHardCopy] AS "IsHardCopy"
    FROM [BookWithPages].[BookBoundedContext].[Book] b
    WHERE b.[Title] = @title;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_GetById]
    @bookId INT
AS
BEGIN
    SELECT
        b.[BookId] AS "Id",
        b.[Title] AS "Title",
        b.[Category] AS "Category",
        b.[DatePublished] AS "DatePublished",
        b.[PublisherId] AS "PublisherId",
        b.[IsHardCopy] AS "IsHardCopy"
    FROM [BookWithPages].[BookBoundedContext].[Book] b
    WHERE b.[BookId] = @bookId;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pPage_Delete]
    @pageId INT
AS
BEGIN
    DELETE FROM [BookWithPages].[BookBoundedContext].[Page]
    WHERE [PageId] = @pageId;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pPage_Insert]
    @index INT,
    @createdBy INT,
    @bookId INT
AS
BEGIN
    DECLARE @pageOutputData TABLE
    (
        [PageId] INT
    );

    INSERT INTO [BookWithPages].[BookBoundedContext].[Page]
    (
        [Index],
        [CreatedBy],
        [BookId]
    )
    OUTPUT
        INSERTED.[PageId]
        INTO @pageOutputData
    VALUES
    (
        @index,
        @createdBy,
        @bookId
    );

    SELECT
        [PageId]
    FROM @pageOutputData;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pPage_Update]
    @pageId INT,
    @index INT,
    @updatedBy INT,
    @bookId INT
AS
BEGIN
    UPDATE [BookWithPages].[BookBoundedContext].[Page]
    SET
        [Index] = @index,
        [UpdatedBy] = @updatedBy,
        [BookId] = @bookId,
        [UpdatedDateTime] = GETDATE()
    WHERE [PageId] = @pageId;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pPage_Get]
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
        @selectList = N'p.[PageId] AS "Id",
        p.[Index] AS "Index",
        p.[BookId] AS "BookId"',
        @tableName = N'Page p',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pPage_GetById]
    @pageId INT
AS
BEGIN
    SELECT
        p.[PageId] AS "Id",
        p.[Index] AS "Index",
        p.[BookId] AS "BookId"
    FROM [BookWithPages].[BookBoundedContext].[Page] p
    WHERE p.[PageId] = @pageId;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pPage_GetPages_ForBook]
    @bookId INT
AS
BEGIN
    SELECT
        p.[PageId] AS "Id",
        p.[Index] AS "Index",
        p.[BookId] AS "BookId"
    FROM [BookWithPages].[BookBoundedContext].[Page] p
    WHERE p.[BookId] = @bookId;

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
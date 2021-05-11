
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
    [DatePublished] DATE NOT NULL,
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
    [BookId] INT
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
    @datePublished DATE,
    @publisherId UNIQUEIDENTIFIER,
    @isHardCopy BIT,
    @createdBy INT,
	@bookId INT OUTPUT
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
        @bookId = [BookId]
    FROM @bookOutputData;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_Update]
    @bookId INT,
    @title VARCHAR(150),
    @category INT,
    @datePublished DATE,
    @publisherId UNIQUEIDENTIFIER,
    @isHardCopy BIT,
    @updatedBy INT = NULL
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

CREATE PROCEDURE [BookBoundedContext].[pBook_UnlinkPages]
    @bookId INT
AS
BEGIN
    UPDATE [BookWithPages].[BookBoundedContext].[Page]
    SET
        [BookId] = NULL
    WHERE [BookId] = @bookId;

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
    @bookId INT = NULL
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
    @updatedBy INT = NULL,
    @bookId INT = NULL
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
        @selectList = N'    b.[BookId] AS "Id",
    b.[Title] AS "Title",
    b.[Category] AS "Category",
    b.[DatePublished] AS "DatePublished",
    b.[PublisherId] AS "PublisherId",
    b.[IsHardCopy] AS "IsHardCopy"',
        @from = N'[BookWithPages].[BookBoundedContext].[Book] b',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_GetAll]
AS
BEGIN
    SELECT
        b.[BookId] AS "Id",
        b.[Title] AS "Title",
        b.[Category] AS "Category",
        b.[DatePublished] AS "DatePublished",
        b.[PublisherId] AS "PublisherId",
        b.[IsHardCopy] AS "IsHardCopy"
    FROM [BookWithPages].[BookBoundedContext].[Book] b;

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

CREATE PROCEDURE [BookBoundedContext].[pPage_GetBook]
    @pageId INT
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
    INNER JOIN [BookWithPages].[BookBoundedContext].[Page] p
        ON b.[BookId] = p.[BookId]
    WHERE p.[PageId] = @pageId;

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
        @selectList = N'    p.[PageId] AS "Id",
    p.[Index] AS "Index",
    p.[BookId] AS "BookId"',
        @from = N'[BookWithPages].[BookBoundedContext].[Page] p',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pPage_GetAll]
AS
BEGIN
    SELECT
        p.[PageId] AS "Id",
        p.[Index] AS "Index",
        p.[BookId] AS "BookId"
    FROM [BookWithPages].[BookBoundedContext].[Page] p;

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

CREATE PROCEDURE [BookBoundedContext].[pBook_GetAllPages]
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

INSERT INTO [BookWithPages].[BookBoundedContext].[Book]
(
    [Title],
    [Category],
    [DatePublished],
    [PublisherId],
    [IsHardCopy],
    [CreatedBy]
)
VALUES
(
    'Book 1',
    1,
    '10/12/1999 12:00:00 AM',
    'fafac13f-f571-4596-a3ad-1ca1776f21a9',
    1,
    1
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
VALUES
(
    'Book 2',
    0,
    '6/23/2010 12:00:00 AM',
    '97c04d97-9527-4730-ab07-364e59dc7272',
    0,
    1
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
VALUES
(
    'Book 3',
    2,
    '9/3/2018 12:00:00 AM',
    '38ee7a02-9b9d-4ad0-949e-cbe89268c570',
    1,
    1
);


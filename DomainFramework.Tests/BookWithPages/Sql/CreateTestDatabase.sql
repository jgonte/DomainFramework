
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
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME
    CONSTRAINT Book_PK PRIMARY KEY ([BookId])
);
GO

CREATE TABLE [BookWithPages].[BookBoundedContext].[Page]
(
    [PageId] INT NOT NULL IDENTITY,
    [Index] INT NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedWhen] DATETIME NOT NULL DEFAULT GETDATE(),
    [LastUpdatedBy] INT,
    [LastUpdatedWhen] DATETIME,
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
    @lastUpdatedBy INT
AS
BEGIN
    UPDATE [BookWithPages].[BookBoundedContext].[Book]
    SET
        [Title] = @title,
        [Category] = @category,
        [DatePublished] = @datePublished,
        [PublisherId] = @publisherId,
        [LastUpdatedBy] = @lastUpdatedBy,
        [LastUpdatedWhen] = GETDATE()
    WHERE [BookId] = @bookId;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pBook_Get]
AS
BEGIN
    SELECT
        b.[BookId] AS "Id",
        b.[Title] AS "Title",
        b.[Category] AS "Category",
        b.[DatePublished] AS "DatePublished",
        b.[PublisherId] AS "PublisherId"
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
        b.[PublisherId] AS "PublisherId"
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
        b.[PublisherId] AS "PublisherId"
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
    @lastUpdatedBy INT,
    @bookId INT
AS
BEGIN
    UPDATE [BookWithPages].[BookBoundedContext].[Page]
    SET
        [Index] = @index,
        [LastUpdatedBy] = @lastUpdatedBy,
        [BookId] = @bookId,
        [LastUpdatedWhen] = GETDATE()
    WHERE [PageId] = @pageId;

END;
GO

CREATE PROCEDURE [BookBoundedContext].[pPage_Get]
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


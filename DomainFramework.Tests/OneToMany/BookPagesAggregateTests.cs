﻿using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    [TestClass]
    public class BookPagesTests
    {
        static readonly string connectionName = "SqlServerTest.DomainFrameworkOneToManyTest.ConnectionString";

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static async Task MyClassInitialize(TestContext testContext)
        {
            // Test script executor (create database)
            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection("master"),
@"
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'DomainFrameworkOneToManyTest'
)
BEGIN
    DROP DATABASE DomainFrameworkOneToManyTest
END
GO

CREATE DATABASE DomainFrameworkOneToManyTest
GO

USE DomainFrameworkOneToManyTest
GO

CREATE TABLE DomainFrameworkOneToManyTest..Book(
    [BookId] INT NOT NULL IDENTITY,
    [Title] VARCHAR(50)
)

ALTER TABLE DomainFrameworkOneToManyTest..Book
ADD CONSTRAINT Book_PK PRIMARY KEY (BookId)
GO

CREATE TABLE DomainFrameworkOneToManyTest..Page(
    [PageId] INT NOT NULL IDENTITY,
    [Index] INT NOT NULL,
    [BookId] INT  NOT NULL
)

ALTER TABLE DomainFrameworkOneToManyTest..Page
ADD CONSTRAINT Page_PK PRIMARY KEY (PageId)

ALTER TABLE DomainFrameworkOneToManyTest..Page
ADD CONSTRAINT Page_Book_FK FOREIGN KEY (BookId) REFERENCES DomainFrameworkOneToManyTest..Book(BookId)
ON DELETE CASCADE;

GO
",
            "^GO");

            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection(connectionName),
            @"
CREATE PROCEDURE [p_Book_Create]
    @title VARCHAR(50)
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [BookId] INT NOT NULL
    );

    INSERT INTO Book
    (
        [Title]
    )
    OUTPUT
        INSERTED.[BookId]
    INTO @outputData
    VALUES
    (
        @title
    );

    SELECT
        [BookId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Page_Create]
    @index INT,
    @bookId INT
AS
BEGIN
    DECLARE @outputData TABLE
    (
        [PageId] INT NOT NULL
    );

    INSERT INTO Page
    (
        [Index],
        [BookId]
    )
    OUTPUT
        INSERTED.[PageId]
    INTO @outputData
    VALUES
    (
        @index,
        @bookId
    );

    SELECT
        [PageId]
    FROM @outputData;

END;
GO

CREATE PROCEDURE [p_Book_Get]
    @bookId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Title]
    FROM [Book]
        WHERE [BookId] = @bookId

END;
GO

CREATE PROCEDURE [p_Page_Get]
    @pageId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [Index],
        [BookId]
    FROM [Page]
        WHERE [PageId] = @pageId

END;
GO

CREATE PROCEDURE [p_Book_GetPages]
    @bookId INT
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [PageId],
        [Index],
        [BookId]
    FROM [Page]
        WHERE [BookId] = @bookId

END;
GO

CREATE PROCEDURE [p_Book_Update]
    @bookId INT,
    @title VARCHAR(50)
AS
BEGIN

    UPDATE Book
    SET
        [Title] = @title
    WHERE [BookId] = @bookId;

END;
GO

CREATE PROCEDURE [p_Page_Update]
    @pageId INT,
    @index INT,
    @bookId INT
AS
BEGIN

    UPDATE Page
    SET
        [Index] = @index,
        [BookId] = @bookId
    WHERE [PageId] = @pageId;

END;
GO

CREATE PROCEDURE [p_Book_Delete]
    @bookId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM [Book]
        WHERE [BookId] = @bookId

END;
GO

",
                        "^GO");
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod]
        public void Book_Aggregate_With_Pages_Book_Only_Tests()
        {
            var bookEntity = new BookEntity(new BookData { Title = "Programming Java" });

            var context = new RepositoryContext("SqlServerTest.DomainFrameworkOneToManyTest.ConnectionString");

            context.RegisterCommandRepository<BookEntity>(new BookCommandRepository());

            context.RegisterCommandRepository<PageEntity>(new PageCommandRepository());

            // Insert

            var bookCommandAggregate = new BookPagesCommandAggregate(context, bookEntity);

            bookCommandAggregate.Save();

            Assert.AreEqual(1, bookEntity.Id);

            var pages = bookCommandAggregate.Pages;

            Assert.AreEqual(0, pages.Count());

            // Read

            context.RegisterQueryRepository<BookEntity>(new BookQueryRepository());

            context.RegisterQueryRepository<PageEntity>(new PageQueryRepository());

            var bookQueryAggregate = new BookPagesQueryAggregate(context, bookEntity);

            bookQueryAggregate.Load(1);

            bookEntity = bookQueryAggregate.RootEntity;

            Assert.AreEqual(1, bookEntity.Id);

            Assert.AreEqual("Programming Java", bookEntity.Data.Title);

            Assert.AreEqual(0, bookQueryAggregate.Pages.Count());

            // Update

            bookEntity = bookCommandAggregate.RootEntity;

            bookEntity.Data.Title = "Programming Java 2nd Ed.";

            bookCommandAggregate.Save();

            // Read after update

            bookQueryAggregate.Load(1);

            bookEntity = bookQueryAggregate.RootEntity;

            Assert.AreEqual(1, bookEntity.Id);

            Assert.AreEqual("Programming Java 2nd Ed.", bookEntity.Data.Title);

            Assert.AreEqual(0, bookQueryAggregate.Pages.Count());

            // Delete

            bookCommandAggregate.Delete();

            bookQueryAggregate.Load(1);

            Assert.IsNull(bookQueryAggregate.RootEntity);
        }

        [TestMethod]
        public void Book_Aggregate_With_Pages_Tests()
        {
            var bookEntity = new BookEntity(new BookData { Title = "Programming C#" });

            var context = new RepositoryContext("SqlServerTest.DomainFrameworkOneToManyTest.ConnectionString");

            context.RegisterCommandRepository<BookEntity>(new BookCommandRepository());

            context.RegisterCommandRepository<PageEntity>(new PageCommandRepository());

            // Insert

            var bookCommandAggregate = new BookPagesCommandAggregate(context, bookEntity);

            bookCommandAggregate.AddPage(new PageEntity(new PageData { Index = 1 }));

            bookCommandAggregate.AddPage(new PageEntity(new PageData { Index = 2 }));

            bookCommandAggregate.AddPage(new PageEntity(new PageData { Index = 3 }));

            bookCommandAggregate.Save();

            Assert.AreEqual(2, bookEntity.Id);

            var pages = bookCommandAggregate.Pages;

            Assert.AreEqual(3, pages.Count());

            var page = pages.ElementAt(0);

            Assert.AreEqual(1, page.Id);

            Assert.AreEqual(1, page.Data.Index);

            Assert.AreEqual(2, page.BookId);

            page = pages.ElementAt(1);

            Assert.AreEqual(2, page.Id);

            Assert.AreEqual(2, page.Data.Index);

            Assert.AreEqual(2, page.BookId);

            page = pages.ElementAt(2);

            Assert.AreEqual(3, page.Id);

            Assert.AreEqual(3, page.Data.Index);

            Assert.AreEqual(2, page.BookId);

            // Read

            context.RegisterQueryRepository<BookEntity>(new BookQueryRepository());

            context.RegisterQueryRepository<PageEntity>(new PageQueryRepository());

            var bookQueryAggregate = new BookPagesQueryAggregate(context, bookEntity);

            bookQueryAggregate.Load(2);

            bookEntity = bookQueryAggregate.RootEntity;

            Assert.AreEqual(2, bookEntity.Id);

            Assert.AreEqual("Programming C#", bookEntity.Data.Title);

            Assert.AreEqual(3, bookQueryAggregate.Pages.Count());

            page = bookQueryAggregate.Pages.ElementAt(0);

            Assert.AreEqual(1, page.Id);

            Assert.AreEqual(1, page.Data.Index);

            Assert.AreEqual(2, page.BookId);

            page = bookQueryAggregate.Pages.ElementAt(1);

            Assert.AreEqual(2, page.Id);

            Assert.AreEqual(2, page.Data.Index);

            Assert.AreEqual(2, page.BookId);

            page = bookQueryAggregate.Pages.ElementAt(2);

            Assert.AreEqual(3, page.Id);

            Assert.AreEqual(3, page.Data.Index);

            Assert.AreEqual(2, page.BookId);

            // Update

            bookEntity = bookCommandAggregate.RootEntity;

            bookEntity.Data.Title = "Programming C# 2nd Ed.";

            pages = bookCommandAggregate.Pages;

            page = pages.ElementAt(0);

            page.Data.Index = 10;

            page = pages.ElementAt(1);

            page.Data.Index = 20;

            page = pages.ElementAt(2);

            page.Data.Index = 30;

            bookCommandAggregate.Save();

            // Read after update

            bookQueryAggregate.Load(2);

            bookEntity = bookQueryAggregate.RootEntity;

            Assert.AreEqual(2, bookEntity.Id);

            Assert.AreEqual("Programming C# 2nd Ed.", bookEntity.Data.Title);

            Assert.AreEqual(3, bookQueryAggregate.Pages.Count());

            page = bookQueryAggregate.Pages.ElementAt(0);

            Assert.AreEqual(1, page.Id);

            Assert.AreEqual(10, page.Data.Index);

            Assert.AreEqual(2, page.BookId);

            page = bookQueryAggregate.Pages.ElementAt(1);

            Assert.AreEqual(2, page.Id);

            Assert.AreEqual(20, page.Data.Index);

            Assert.AreEqual(2, page.BookId);

            page = bookQueryAggregate.Pages.ElementAt(2);

            Assert.AreEqual(3, page.Id);

            Assert.AreEqual(30, page.Data.Index);

            Assert.AreEqual(2, page.BookId);

            // Delete

            bookCommandAggregate.Delete();

            bookQueryAggregate.Load(2);

            Assert.IsNull(bookQueryAggregate.RootEntity);
        }
    }
}
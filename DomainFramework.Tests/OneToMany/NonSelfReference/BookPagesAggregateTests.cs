using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
            await ScriptExecutor.ExecuteScriptAsync(ConnectionManager.GetConnection("Master"),
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

CREATE PROCEDURE [p_Book_GetAll]
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        [BookId],
        [Title]
    FROM [Book]

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

CREATE PROCEDURE [p_Book_RemovePages]
    @bookId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
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

    DELETE FROM [Book]
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
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<BookEntity>(() => new BookCommandRepository());

            context.RegisterCommandRepositoryFactory<PageEntity>(() => new PageCommandRepository());

            // Insert

            var inputDto = new BookPagesInputDto
            {
                Title = "Programming Java"
            };

            var saveAggregate = new SaveBookPagesCommandAggregate(context, inputDto);

            saveAggregate.Save();

            var bookEntity = saveAggregate.RootEntity;

            Assert.IsNotNull(bookEntity.Id);

            var bookId = bookEntity.Id;

            var pages = saveAggregate.Pages;

            Assert.AreEqual(0, pages.Count());

            // Read

            context.RegisterQueryRepository<BookEntity>(new BookQueryRepository());

            context.RegisterQueryRepository<PageEntity>(new PageQueryRepository());

            var bookQueryAggregate = new BookPagesQueryAggregate(context);

            bookQueryAggregate.Get(bookId);

            bookEntity = bookQueryAggregate.RootEntity;

            Assert.AreEqual(bookId, bookEntity.Id);

            Assert.AreEqual("Programming Java", bookEntity.Title);

            Assert.AreEqual(0, bookQueryAggregate.Pages.Count());

            // Update

            bookEntity = saveAggregate.RootEntity;

            bookEntity.Title = "Programming Java 2nd Ed.";

            saveAggregate.Save();

            // Read after update

            bookQueryAggregate.Get(bookId);

            bookEntity = bookQueryAggregate.RootEntity;

            Assert.AreEqual(bookId, bookEntity.Id);

            Assert.AreEqual("Programming Java 2nd Ed.", bookEntity.Title);

            Assert.AreEqual(0, bookQueryAggregate.Pages.Count());

            // Delete
            var deleteAggregate = new DeleteBookPagesCommandAggregate(context, bookId);

            deleteAggregate.Save();

            bookQueryAggregate.Get(bookId);

            Assert.IsNull(bookQueryAggregate.RootEntity);
        }

        [TestMethod]
        public void Book_Aggregate_With_Pages_Tests()
        {
            var context = new RepositoryContext(connectionName);

            context.RegisterCommandRepositoryFactory<BookEntity>(() => new BookCommandRepository());

            context.RegisterCommandRepositoryFactory<PageEntity>(() => new PageCommandRepository());

            // Insert
            var saveAggregate = new SaveBookPagesCommandAggregate(context, new BookPagesInputDto
            {
                Title = "Programming C#",
                Pages = new List<PageInputDto>
                {
                    new PageInputDto
                    {
                        Index = 1
                    },
                    new PageInputDto
                    {
                        Index = 2
                    },
                    new PageInputDto
                    {
                        Index = 3
                    },
                }
            });

            saveAggregate.Save();

            var bookEntity = saveAggregate.RootEntity;

            Assert.IsNotNull(bookEntity.Id);

            var bookId = bookEntity.Id;

            var pages = saveAggregate.Pages;

            Assert.AreEqual(3, pages.Count());

            var page = pages.ElementAt(0);

            Assert.AreEqual(1, page.Id);

            Assert.AreEqual(1, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            page = pages.ElementAt(1);

            Assert.AreEqual(2, page.Id);

            Assert.AreEqual(2, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            page = pages.ElementAt(2);

            Assert.AreEqual(3, page.Id);

            Assert.AreEqual(3, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            // Read

            context.RegisterQueryRepository<BookEntity>(new BookQueryRepository());

            context.RegisterQueryRepository<PageEntity>(new PageQueryRepository());

            var queryAggregate = new BookPagesQueryAggregate(context);

            var outputDto = queryAggregate.Get(bookId);

            Assert.AreEqual(bookId, outputDto.Id);

            Assert.AreEqual("Programming C#", outputDto.Title);

            Assert.AreEqual(3, outputDto.Pages.Count());

            var pageDto = outputDto.Pages.ElementAt(0);

            Assert.AreEqual(1, pageDto.Id);

            Assert.AreEqual(1, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = outputDto.Pages.ElementAt(1);

            Assert.AreEqual(2, pageDto.Id);

            Assert.AreEqual(2, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = outputDto.Pages.ElementAt(2);

            Assert.AreEqual(3, pageDto.Id);

            Assert.AreEqual(3, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            bookEntity = saveAggregate.RootEntity;

            // Update
            saveAggregate = new SaveBookPagesCommandAggregate(context, new BookPagesInputDto
            {
                Id = bookEntity.Id,
                Title = "Programming C# 2nd Ed.",
                Pages = new List<PageInputDto>
                {
                    new PageInputDto
                    {
                        Id = 1,
                        Index = 10
                    },
                    new PageInputDto
                    {
                        Id = 2,
                        Index = 20
                    },
                    new PageInputDto
                    {
                        Id = 3,
                        Index = 30
                    },
                }
            });

            saveAggregate.Save();

            // Read after update

            queryAggregate.Get(bookId);

            bookEntity = queryAggregate.RootEntity;

            Assert.AreEqual(bookId, bookEntity.Id);

            Assert.AreEqual("Programming C# 2nd Ed.", bookEntity.Title);

            Assert.AreEqual(3, queryAggregate.Pages.Count());

            page = queryAggregate.Pages.ElementAt(0);

            Assert.AreEqual(1, page.Id);

            Assert.AreEqual(10, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            page = queryAggregate.Pages.ElementAt(1);

            Assert.AreEqual(2, page.Id);

            Assert.AreEqual(20, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            page = queryAggregate.Pages.ElementAt(2);

            Assert.AreEqual(3, page.Id);

            Assert.AreEqual(30, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            // Add a single page to the book
            var addPageAggregate = new AddPageAggregate(context, bookEntity, new PageInputDto
            {
                Index = 40
            });

            addPageAggregate.Save();

            // Verify the page was added
            queryAggregate.Get(bookId);

            bookEntity = queryAggregate.RootEntity;

            Assert.AreEqual(bookId, bookEntity.Id);

            Assert.AreEqual("Programming C# 2nd Ed.", bookEntity.Title);

            Assert.AreEqual(4, queryAggregate.Pages.Count());

            page = queryAggregate.Pages.ElementAt(0);

            Assert.AreEqual(1, page.Id);

            Assert.AreEqual(10, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            page = queryAggregate.Pages.ElementAt(1);

            Assert.AreEqual(2, page.Id);

            Assert.AreEqual(20, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            page = queryAggregate.Pages.ElementAt(2);

            Assert.AreEqual(3, page.Id);

            Assert.AreEqual(30, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            page = queryAggregate.Pages.ElementAt(3);

            Assert.AreEqual(4, page.Id);

            Assert.AreEqual(40, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            // Replace all the existing pages if the book

            var replacePagesAggregate = new ReplacePagesAggregate(context, bookEntity, new PageInputDto[]
            {
                new PageInputDto
                {
                    Index = 100
                },
                new PageInputDto
                {
                    Index = 200
                },
                new PageInputDto
                {
                    Index = 300
                }
            });

            replacePagesAggregate.Save();

            // Verify the pages were replaced
            queryAggregate.Get(bookId);

            bookEntity = queryAggregate.RootEntity;

            Assert.AreEqual(bookId, bookEntity.Id);

            Assert.AreEqual("Programming C# 2nd Ed.", bookEntity.Title);

            Assert.AreEqual(3, queryAggregate.Pages.Count());

            page = queryAggregate.Pages.ElementAt(0);

            Assert.AreEqual(5, page.Id);

            Assert.AreEqual(100, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            page = queryAggregate.Pages.ElementAt(1);

            Assert.AreEqual(6, page.Id);

            Assert.AreEqual(200, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            page = queryAggregate.Pages.ElementAt(2);

            Assert.AreEqual(7, page.Id);

            Assert.AreEqual(300, page.Index);

            Assert.AreEqual(bookId, page.BookId);

            // Delete
            var deleteAggregate = new DeleteBookPagesCommandAggregate(context, bookId);

            deleteAggregate.Save();

            queryAggregate.Get(bookId);

            Assert.IsNull(queryAggregate.RootEntity);
        }
    }
}

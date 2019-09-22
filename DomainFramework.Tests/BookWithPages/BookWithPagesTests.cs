using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookWithPages.BookBoundedContext
{
    [TestClass]
    public class BookWithPages
    {
        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // Create the test database
            var script = File.ReadAllText(
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\BookWithPages\Sql\CreateTestDatabase.sql");

            ScriptRunner.Run(ConnectionManager.GetConnection("Master").ConnectionString, script);
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
        public void Save_Book_Only_Tests()
        {
            // Insert
            var commandAggregate = new SaveBookCommandAggregate(new SaveBookInputDto
            {
                Title = "Programming C++",
                Category = Book.Categories.Action,
                DatePublished = new DateTime(2019, 9, 8)
            });

            commandAggregate.Save();

            var bookId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new BookQueryAggregate();

            var bookDto = queryAggregate.Get(bookId);

            Assert.AreEqual(bookId, bookDto.BookId);

            Assert.AreEqual("Programming C++", bookDto.Title);

            Assert.AreEqual(Book.Categories.Action, bookDto.Category);

            Assert.AreEqual(new DateTime(2019, 9, 8), bookDto.DatePublished);

            Assert.AreEqual(0, bookDto.Pages.Count());

            // Update
            commandAggregate = new SaveBookCommandAggregate(new SaveBookInputDto
            {
                Id = bookId,
                Title = "Programming C++ 2nd Ed.",
                Category = Book.Categories.Mistery,
                DatePublished = new DateTime(2020, 9, 8)
            });

            commandAggregate.Save();

            // Read
            queryAggregate = new BookQueryAggregate();

            bookDto = queryAggregate.Get(bookId);

            Assert.AreEqual(bookId, bookDto.BookId);

            Assert.AreEqual("Programming C++ 2nd Ed.", bookDto.Title);

            Assert.AreEqual(Book.Categories.Mistery, bookDto.Category);

            Assert.AreEqual(new DateTime(2020, 9, 8), bookDto.DatePublished);

            Assert.AreEqual(0, bookDto.Pages.Count());

            // Delete
            var deleteAggregate = new DeleteBookCommandAggregate(new DeleteBookInputDto
            {
                Id = bookId.Value
            });

            deleteAggregate.Save();

            bookDto = queryAggregate.Get(bookId);

            Assert.IsNull(bookDto);
        }

        [TestMethod]
        public void Save_Book_With_Pages_Tests()
        {
            // Insert
            var commandAggregate = new SaveBookCommandAggregate(new SaveBookInputDto
            {
                Title = "Programming C#",
                Category = Book.Categories.Action,
                DatePublished = new DateTime(2019, 9, 8),
                Pages = new List<SavePageInputDto>
                {
                    new SavePageInputDto
                    {
                        Index = 1
                    },
                    new SavePageInputDto
                    {
                        Index = 2
                    },
                    new SavePageInputDto
                    {
                        Index = 3
                    },
                }
            });

            commandAggregate.Save();

            var bookId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new BookQueryAggregate();

            var bookDto = queryAggregate.Get(bookId);

            Assert.AreEqual(bookId, bookDto.BookId);

            Assert.AreEqual("Programming C#", bookDto.Title);

            Assert.AreEqual(Book.Categories.Action, bookDto.Category);

            Assert.AreEqual(new DateTime(2019, 9, 8), bookDto.DatePublished);

            Assert.AreEqual(3, bookDto.Pages.Count());

            var pageDto = bookDto.Pages.ElementAt(0);

            Assert.AreEqual(1, pageDto.PageId);

            Assert.AreEqual(1, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(1);

            Assert.AreEqual(2, pageDto.PageId);

            Assert.AreEqual(2, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(2);

            Assert.AreEqual(3, pageDto.PageId);

            Assert.AreEqual(3, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            //TODO: Research the update modes for linked entities for one to many relationships
            // Update
            commandAggregate = new SaveBookCommandAggregate(new SaveBookInputDto
            {
                Id = bookId,
                Title = "Programming C# 2nd Ed.",
                Category = Book.Categories.Mistery,
                DatePublished = new DateTime(2020, 9, 8),
                Pages = new List<SavePageInputDto>
                {
                    new SavePageInputDto
                    {
                        //PageId = 1, Ignored
                        Index = 10,
                        BookId = bookId.Value
                    },
                    new SavePageInputDto
                    {
                        //PageId = 2, Ignored
                        Index = 20,
                        BookId = bookId.Value
                    },
                    new SavePageInputDto
                    {
                        //PageId = 3, Ignored
                        Index = 30,
                        BookId = bookId.Value
                    },
                }
            });

            commandAggregate.Save();

            // Read
            queryAggregate = new BookQueryAggregate();

            bookDto = queryAggregate.Get(bookId);

            Assert.AreEqual(bookId, bookDto.BookId);

            Assert.AreEqual("Programming C# 2nd Ed.", bookDto.Title);

            Assert.AreEqual(Book.Categories.Mistery, bookDto.Category);

            Assert.AreEqual(new DateTime(2020, 9, 8), bookDto.DatePublished);

            Assert.AreEqual(3, bookDto.Pages.Count());

            pageDto = bookDto.Pages.ElementAt(0);

            Assert.AreEqual(4, pageDto.PageId);

            Assert.AreEqual(10, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(1);

            Assert.AreEqual(5, pageDto.PageId);

            Assert.AreEqual(20, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(2);

            Assert.AreEqual(6, pageDto.PageId);

            Assert.AreEqual(30, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            // Add a pages to the book
            var addPageAggregate = new AddBookPagesCommandAggregate(new BookAddPagesInputDto
            {
                Id = bookId.Value,
                Pages = new List<AddPageInputDto>
                {
                   new AddPageInputDto
                   {
                       BookId = bookId.Value,
                       Index = 40
                   },
                   new AddPageInputDto
                   {
                       BookId = bookId.Value,
                       Index = 50
                   }
                }
            });

            addPageAggregate.Save();

            // Verify the pages were added
            queryAggregate = new BookQueryAggregate();

            bookDto = queryAggregate.Get(bookId);

            Assert.AreEqual(5, bookDto.Pages.Count());

            pageDto = bookDto.Pages.ElementAt(0);

            Assert.AreEqual(4, pageDto.PageId);

            Assert.AreEqual(10, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(1);

            Assert.AreEqual(5, pageDto.PageId);

            Assert.AreEqual(20, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(2);

            Assert.AreEqual(6, pageDto.PageId);

            Assert.AreEqual(30, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(3);

            Assert.AreEqual(7, pageDto.PageId);

            Assert.AreEqual(40, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(4);

            Assert.AreEqual(8, pageDto.PageId);

            Assert.AreEqual(50, pageDto.Index);

            Assert.AreEqual(bookId, pageDto.BookId);

            // Replace all the existing pages if the book
            //var replacePagesAggregate = new ReplacePagesAggregate(context, bookEntity, new PageInputDto[]
            //{
            //    new PageInputDto
            //    {
            //        Index = 100
            //    },
            //    new PageInputDto
            //    {
            //        Index = 200
            //    },
            //    new PageInputDto
            //    {
            //        Index = 300
            //    }
            //});

            //Assert.AreEqual(3, bookDto.Pages.Count());

            //pageDto = bookDto.Pages.ElementAt(0);

            //Assert.AreEqual(4, pageDto.PageId);

            //Assert.AreEqual(10, pageDto.Index);

            //Assert.AreEqual(bookId, pageDto.BookId);

            //pageDto = bookDto.Pages.ElementAt(1);

            //Assert.AreEqual(5, pageDto.PageId);

            //Assert.AreEqual(20, pageDto.Index);

            //Assert.AreEqual(bookId, pageDto.BookId);

            //pageDto = bookDto.Pages.ElementAt(2);

            //Assert.AreEqual(6, pageDto.PageId);

            //Assert.AreEqual(30, pageDto.Index);

            //Assert.AreEqual(bookId, pageDto.BookId);

            // Delete
            var deleteAggregate = new DeleteBookCommandAggregate(new DeleteBookInputDto
            {
                Id = bookId.Value
            });

            deleteAggregate.Save();

            bookDto = queryAggregate.Get(bookId);

            Assert.IsNull(bookDto);
        }
    }
}

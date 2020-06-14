using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    [TestClass]
    public class BookWithPagesTests
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\BookWithPages\Sql\CreateTestDatabase.sql"
            );

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

        //[TestMethod]
        //public void Save_Book_Only_Tests()
        //{
        //    // Insert
        //    var createCommandAggregate = new SaveBookCommandAggregate(new SaveBookInputDto
        //    {
        //        Id = "us",
        //        Name = "United States"
        //    });

        //    createCommandAggregate.Save();

        //    var countryId = createCommandAggregate.RootEntity.Id;

        //    // Read
        //    var queryAggregate = new BookQueryAggregate();

        //    var countryDto = queryAggregate.Get(countryId);

        //    Assert.AreEqual(countryId, countryDto.Id);

        //    Assert.AreEqual("United States", countryDto.Name);

        //    Assert.IsNull(countryDto.Pages);

        //    // Update
        //    var updateCommandAggregate = new UpdateBookCommandAggregate(new UpdateBookInputDto
        //    {
        //        Id = countryId,
        //        Name = "United States of America"
        //    });

        //    updateCommandAggregate.Save();

        //    // Read
        //    queryAggregate = new BookQueryAggregate();

        //    countryDto = queryAggregate.Get(countryId);

        //    Assert.AreEqual(countryId, countryDto.Id);

        //    Assert.AreEqual("United States of America", countryDto.Name);

        //    Assert.IsNull(countryDto.Pages);

        //    // Delete
        //    //var deleteAggregate = new DeleteBookCommandAggregate(new DeleteBookInputDto
        //    //{
        //    //    Id = countryId.Value
        //    //});

        //    //deleteAggregate.Save();

        //    //countryDto = queryAggregate.Get(countryId);

        //    //Assert.IsNull(countryDto);
        //}

        [TestMethod]
        public void Save_Book_With_Pages_Tests()
        {
            // Insert
            var saveCommandAggregate = new SaveBookCommandAggregate(new SaveBookInputDto
            {
                Title = "Programming C#",
                DatePublished = new DateTime(2001, 11, 13),
                IsHardCopy = true,
                Category = Book.Categories.Science,
                PublisherId = Guid.Parse("15383656-f9ec-42e9-9049-436efd08e76d"),
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
                    }
                }
            });

            saveCommandAggregate.Save();

            var bookId = saveCommandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new GetBookByIdQueryAggregate();

            var bookDto = queryAggregate.Get(bookId);

            Assert.AreEqual(bookId, bookDto.BookId);

            Assert.AreEqual("Programming C#", bookDto.Title);

            Assert.AreEqual(new DateTime(2001, 11, 13), bookDto.DatePublished);

            Assert.AreEqual(Book.Categories.Science, bookDto.Category);

            Assert.AreEqual(Guid.Parse("15383656-f9ec-42e9-9049-436efd08e76d"), bookDto.PublisherId);

            Assert.AreEqual(3, bookDto.Pages.Count());

            var pageDto = bookDto.Pages.ElementAt(0);

            Assert.AreEqual(1, pageDto.Index);

            Assert.AreEqual(1, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(1);

            Assert.AreEqual(2, pageDto.Index);

            Assert.AreEqual(2, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(2);

            Assert.AreEqual(3, pageDto.Index);

            Assert.AreEqual(3, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            // Update
            saveCommandAggregate = new SaveBookCommandAggregate(new SaveBookInputDto
            {
                BookId = bookId,
                Title = "Programming C# 2nd Edition",
                DatePublished = new DateTime(2009, 01, 31),
                IsHardCopy = true,
                Category = Book.Categories.Mistery,
                PublisherId = Guid.Parse("15383656-f9ec-42e9-9049-436efd08e76e"),
                // Pages  are replaced
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
                    }
                }
            });

            saveCommandAggregate.Save();

            // Read update
            queryAggregate = new GetBookByIdQueryAggregate();

            bookDto = queryAggregate.Get(bookId);

            Assert.AreEqual(bookId, bookDto.BookId);

            Assert.AreEqual("Programming C# 2nd Edition", bookDto.Title);

            Assert.AreEqual(new DateTime(2009, 01, 31), bookDto.DatePublished);

            Assert.AreEqual(Book.Categories.Mistery, bookDto.Category);

            Assert.AreEqual(Guid.Parse("15383656-f9ec-42e9-9049-436efd08e76e"), bookDto.PublisherId);

            Assert.AreEqual(3, bookDto.Pages.Count());

            pageDto = bookDto.Pages.ElementAt(0);

            Assert.AreEqual(1, pageDto.Index);

            Assert.AreEqual(4, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(1);

            Assert.AreEqual(2, pageDto.Index);

            Assert.AreEqual(5, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(2);

            Assert.AreEqual(3, pageDto.Index);

            Assert.AreEqual(6, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            // Add pages to the book
            var addPagesCommandAggregate = new AddBookPagesCommandAggregate(new BookAddPagesInputDto
            {
                BookId = bookId.Value,
                Pages = new List<SavePageInputDto>
                {
                    new SavePageInputDto
                    {
                        Index = 4
                    },
                    new SavePageInputDto
                    {
                        Index = 5
                    }
                }
            });

            addPagesCommandAggregate.Save();

            // Read added pages
            queryAggregate = new GetBookByIdQueryAggregate();

            bookDto = queryAggregate.Get(bookId);

            Assert.AreEqual(bookId, bookDto.BookId);

            Assert.AreEqual("Programming C# 2nd Edition", bookDto.Title);

            Assert.AreEqual(new DateTime(2009, 01, 31), bookDto.DatePublished);

            Assert.AreEqual(Book.Categories.Mistery, bookDto.Category);

            Assert.AreEqual(Guid.Parse("15383656-f9ec-42e9-9049-436efd08e76e"), bookDto.PublisherId);

            Assert.AreEqual(5, bookDto.Pages.Count());

            pageDto = bookDto.Pages.ElementAt(0);

            Assert.AreEqual(1, pageDto.Index);

            Assert.AreEqual(4, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(1);

            Assert.AreEqual(2, pageDto.Index);

            Assert.AreEqual(5, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(2);

            Assert.AreEqual(3, pageDto.Index);

            Assert.AreEqual(6, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(3);

            Assert.AreEqual(4, pageDto.Index);

            Assert.AreEqual(7, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            pageDto = bookDto.Pages.ElementAt(4);

            Assert.AreEqual(5, pageDto.Index);

            Assert.AreEqual(8, pageDto.PageId);

            Assert.AreEqual(bookId, pageDto.BookId);

            // Delete
            var deleteAggregate = new DeleteBookCommandAggregate(new DeleteBookInputDto
            {
                BookId = bookId.Value
            });

            deleteAggregate.Save();

            bookDto = queryAggregate.Get(bookId);

            Assert.IsNull(bookDto);


        }

    }
}

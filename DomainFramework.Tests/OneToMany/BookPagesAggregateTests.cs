using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DomainFramework.Tests
{
    [TestClass]
    public class BookPagesTests
    {
        [TestMethod]
        public void Insert_Book_With_Pages_Tests()
        {
            var bookEntity = new BookEntity(new Book { Title = "Programming C#" });

            var context = new RepositoryContext("connectionName");

            context.RegisterCommandRepository<BookEntity>(new BookCommandRepository());

            context.RegisterCommandRepository<PageEntity>(new PageCommandRepository());

            var aggregate = new BookPagesAggregate(context, bookEntity);

            aggregate.AddPage(new PageEntity(new Page { Index = 1 }));

            aggregate.AddPage(new PageEntity(new Page { Index = 2 }));

            aggregate.AddPage(new PageEntity(new Page { Index = 3 }));

            aggregate.Save();

            Assert.AreEqual(1, bookEntity.Id);

            var pages = aggregate.Pages;

            Assert.AreEqual(3, pages.Count());

            var page = pages.ElementAt(0);

            Assert.AreEqual(1, page.Id);

            //var queryRepository = new InMemoryQueryRepository<BookEntity, Book>(context);

            //var entity = queryRepository.GetById(1);

            //Assert.AreEqual(1, entity.Id);

            //Assert.AreEqual("Programming C#", entity.Data.Title);
        }
    }
}

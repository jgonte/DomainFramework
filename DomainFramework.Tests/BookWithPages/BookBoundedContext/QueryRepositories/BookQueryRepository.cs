using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class BookQueryRepository : EntityQueryRepository<Book, int?>
    {
        public override (int, IEnumerable<Book>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Book>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Book>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Book>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Book> GetAll()
        {
            var result = Query<Book>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Book>> GetAllAsync()
        {
            var result = await Query<Book>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public Book GetBookByTitle(string title)
        {
            var result = Query<Book>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_GetByTitle]")
                .Parameters(
                    p => p.Name("title").Value(title)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Book> GetBookByTitleAsync(string title)
        {
            var result = await Query<Book>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_GetByTitle]")
                .Parameters(
                    p => p.Name("title").Value(title)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override Book GetById(int? bookId)
        {
            var result = Query<Book>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_GetById]")
                .Parameters(
                    p => p.Name("bookId").Value(bookId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Book> GetByIdAsync(int? bookId)
        {
            var result = await Query<Book>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_GetById]")
                .Parameters(
                    p => p.Name("bookId").Value(bookId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public Book GetBookForPage(int? pageId)
        {
            var result = Query<Book>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetBook]")
                .Parameters(
                    p => p.Name("pageId").Value(pageId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Book> GetBookForPageAsync(int? pageId)
        {
            var result = await Query<Book>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetBook]")
                .Parameters(
                    p => p.Name("pageId").Value(pageId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Book>(new BookQueryRepository());
        }

    }
}
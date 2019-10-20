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
        public override Book GetById(int? bookId, IAuthenticatedUser user)
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

        public async override Task<Book> GetByIdAsync(int? bookId, IAuthenticatedUser user)
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

        public override IEnumerable<Book> Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Book>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_Get]")
                .QueryParameters(queryParameters)
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Book>> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Book>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_Get]")
                .QueryParameters(queryParameters)
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

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Book>(new BookQueryRepository());
        }

    }
}
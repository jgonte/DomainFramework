using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class PageQueryRepository : EntityQueryRepository<Page, int?>
    {
        public override Page GetById(int? pageId, IAuthenticatedUser user)
        {
            var result = Query<Page>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetById]")
                .Parameters(
                    p => p.Name("pageId").Value(pageId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Page> GetByIdAsync(int? pageId, IAuthenticatedUser user)
        {
            var result = await Query<Page>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetById]")
                .Parameters(
                    p => p.Name("pageId").Value(pageId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override (int, IEnumerable<Page>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Page>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<Page> GetPagesForBook(int? bookId, IAuthenticatedUser user)
        {
            var result = Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetPages_ForBook]")
                .Parameters(
                    p => p.Name("bookId").Value(bookId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Page>> GetPagesForBookAsync(int? bookId, IAuthenticatedUser user)
        {
            var result = await Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetPages_ForBook]")
                .Parameters(
                    p => p.Name("bookId").Value(bookId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Page>(new PageQueryRepository());
        }

    }
}
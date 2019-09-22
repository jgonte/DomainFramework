using DataAccess;
using DomainFramework.Core;
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

        public override IEnumerable<Page> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Page>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_Get]")
                .ExecuteAsync();

            return result.Data;
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
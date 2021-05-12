using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace BookWithPages.BookBoundedContext
{
    public class PageQueryRepository : EntityQueryRepository<Page, int>
    {
        public override (int, IEnumerable<Page>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Page>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Page> GetAll()
        {
            var result = Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Page>> GetAllAsync()
        {
            var result = await Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Page GetById(int pageId)
        {
            var result = Query<Page>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetById]")
                .Parameters(
                    p => p.Name("pageId").Value(pageId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Page> GetByIdAsync(int pageId)
        {
            var result = await Query<Page>
                .Single()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetById]")
                .Parameters(
                    p => p.Name("pageId").Value(pageId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public IEnumerable<Page> GetAllPagesForBook(int bookId)
        {
            var result = Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_GetAllPages]")
                .Parameters(
                    p => p.Name("bookId").Value(bookId)
                )
                .Execute();

            return result.Records;
        }

        public async Task<IEnumerable<Page>> GetAllPagesForBookAsync(int bookId)
        {
            var result = await Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pBook_GetAllPages]")
                .Parameters(
                    p => p.Name("bookId").Value(bookId)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Page>(new PageQueryRepository());
        }

    }
}
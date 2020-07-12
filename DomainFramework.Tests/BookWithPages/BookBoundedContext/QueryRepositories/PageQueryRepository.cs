using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Page>)> GetAsync(CollectionQueryParameters queryParameters)
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

        public override IEnumerable<Page> GetAll()
        {
            var result = Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Page>> GetAllAsync()
        {
            var result = await Query<Page>
                .Collection()
                .Connection(BookWithPagesConnectionClass.GetConnectionName())
                .StoredProcedure("[BookBoundedContext].[pPage_GetAll]")
                .ExecuteAsync();

            return result.Data;
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

            return result.Data;
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

            return result.Data;
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

            return result.Data;
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

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Page>(new PageQueryRepository());
        }

    }
}
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class GetBooksQueryAggregate : QueryAggregateCollection<Book, BookOutputDto, GetBookByIdQueryAggregate>
    {
        public GetBooksQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            BookQueryRepository.Register(context);

            PageQueryRepository.Register(context);
        }

        public (int, IEnumerable<BookOutputDto>) Get(CollectionQueryParameters queryParameters)
        {
            var repository = (BookQueryRepository)RepositoryContext.GetQueryRepository(typeof(Book));

            var (count, entities) = repository.Get(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return Get(data);
        }

        public async Task<(int, IEnumerable<BookOutputDto>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var repository = (BookQueryRepository)RepositoryContext.GetQueryRepository(typeof(Book));

            var (count, entities) = await repository.GetAsync(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return await GetAsync(data);
        }

    }
}
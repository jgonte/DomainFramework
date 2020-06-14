using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class GetSimpleLogsQueryAggregate : QueryAggregateCollection<SimpleLog, SimpleLogOutputDto, GetSimpleLogByIdQueryAggregate>
    {
        public GetSimpleLogsQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(SimpleLogsConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            SimpleLogQueryRepository.Register(context);
        }

        public (int, IEnumerable<SimpleLogOutputDto>) Get(CollectionQueryParameters queryParameters)
        {
            var repository = (SimpleLogQueryRepository)RepositoryContext.GetQueryRepository(typeof(SimpleLog));

            var (count, entities) = repository.Get(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return Get(data);
        }

        public async Task<(int, IEnumerable<SimpleLogOutputDto>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var repository = (SimpleLogQueryRepository)RepositoryContext.GetQueryRepository(typeof(SimpleLog));

            var (count, entities) = await repository.GetAsync(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return await GetAsync(data);
        }

    }
}
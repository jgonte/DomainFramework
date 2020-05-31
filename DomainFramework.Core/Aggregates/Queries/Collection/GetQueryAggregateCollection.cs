using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetQueryAggregateCollection<TEntity, TOutputDto, TAggregate> : QueryAggregateCollection<TEntity, TOutputDto, TAggregate>
        where TAggregate : IQueryAggregate, new()
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public GetQueryAggregateCollection(RepositoryContext context) : base(context)
        {
        }

        public (int, IEnumerable<TOutputDto>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var (count, entities) = ((IEntityQueryRepository)repository).Get(queryParameters);

            foreach (var entity in entities)
            {
                var aggregate = new TAggregate
                {
                    RepositoryContext = RepositoryContext,
                    RootEntity = (TEntity)entity
                };

                aggregate.LoadLinks();

                aggregate.PopulateDto();

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            return (count, Aggregates.Select(a => (TOutputDto)a.OutputDto));
        }

        public async Task<(int, IEnumerable<TOutputDto>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var (count, entities) = await ((IEntityQueryRepository)repository).GetAsync(queryParameters);

            var tasks = new Queue<Task>();

            foreach (var entity in entities)
            {
                var aggregate = new TAggregate
                {
                    RepositoryContext = RepositoryContext,
                    RootEntity = (TEntity)entity
                };

                tasks.Enqueue(
                    aggregate.LoadLinksAsync()
                );

                aggregate.PopulateDto();

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            await Task.WhenAll(tasks);

            return (count, Aggregates.Select(a => (TOutputDto)a.OutputDto));
        }
    }
}

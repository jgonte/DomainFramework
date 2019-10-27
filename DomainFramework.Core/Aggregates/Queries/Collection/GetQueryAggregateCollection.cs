using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetQueryAggregateCollection<TAggregate, TEntity, TOutputDto> : QueryAggregateCollection<TAggregate, TEntity, TOutputDto>
        where TAggregate : IQueryAggregate<TEntity, TOutputDto>, new()
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public GetQueryAggregateCollection() : base(null)
        {
        }

        public GetQueryAggregateCollection(RepositoryContext context) : base(context)
        {
        }

        public (int, IEnumerable<TOutputDto>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var (count, entities) = ((IEntityQueryRepository)repository).Get(queryParameters, user);

            foreach (var entity in entities)
            {
                var aggregate = new TAggregate
                {
                    RepositoryContext = RepositoryContext,
                    RootEntity = (TEntity)entity
                };

                aggregate.LoadLinks();

                aggregate.PopulateDto((TEntity)entity);

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            return (count, Aggregates.Select(a => a.OutputDto));
        }

        public async Task<(int, IEnumerable<TOutputDto>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var (count, entities) = ((IEntityQueryRepository)repository).Get(queryParameters, user);

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

                aggregate.PopulateDto((TEntity)entity);

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            await Task.WhenAll(tasks);

            return (count, Aggregates.Select(a => a.OutputDto));
        }
    }
}

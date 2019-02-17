using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetQueryAggregateCollection<TAggregate, TEntity, TDto> : QueryAggregateCollection<TAggregate, TEntity, TDto>
        where TAggregate : IQueryAggregate<TEntity, TDto>, new()
        where TEntity : IEntity
    {
        public GetQueryAggregateCollection() : base(null)
        {
        }

        public GetQueryAggregateCollection(RepositoryContext context) : base(context)
        {
        }

        public IEnumerable<TDto> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entities = ((IEntityQueryRepository)repository).Get(queryParameters, user).Cast<TEntity>();

            foreach (var entity in entities)
            {
                var aggregate = new TAggregate
                {
                    RepositoryContext = RepositoryContext,
                    RootEntity = entity
                };

                aggregate.LoadLinks();

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            return Aggregates.Select(a => a.GetDataTransferObject());
        }

        public async Task<IEnumerable<TDto>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entities = ((IEntityQueryRepository)repository).Get(queryParameters, user).Cast<TEntity>();

            var tasks = new Queue<Task>();

            foreach (var entity in entities)
            {
                var aggregate = new TAggregate
                {
                    RepositoryContext = RepositoryContext,
                    RootEntity = entity
                };

                tasks.Enqueue(
                    aggregate.LoadLinksAsync()
                );

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            await Task.WhenAll(tasks);

            return Aggregates.Select(a => a.GetDataTransferObject());
        }
    }
}

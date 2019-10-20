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

        public IEnumerable<TOutputDto> Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
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

                aggregate.PopulateDto(entity);

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            return Aggregates.Select(a => a.OutputDto);
        }

        public async Task<IEnumerable<TOutputDto>> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
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

                aggregate.PopulateDto(entity);

                ((List<TAggregate>)Aggregates).Add(aggregate);
            }

            await Task.WhenAll(tasks);

            return Aggregates.Select(a => a.OutputDto);
        }
    }
}

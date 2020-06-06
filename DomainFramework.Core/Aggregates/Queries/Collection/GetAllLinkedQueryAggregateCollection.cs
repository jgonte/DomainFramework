using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetAllLinkedQueryAggregateCollection<TKey, TEntity, TOutputDto, TAggregate> : QueryAggregateCollection<TEntity, TOutputDto, TAggregate>
        where TAggregate : IQueryAggregate, new()
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public Func<IEntityQueryRepository, TKey, IAuthenticatedUser, IEnumerable<IEntity>> GetAllLinkedEntities { get; protected set; }

        public Func<IEntityQueryRepository, TKey, IAuthenticatedUser, Task<IEnumerable<IEntity>>> GetAllLinkedEntitiesAsync { get; protected set; }

        public GetAllLinkedQueryAggregateCollection() : base(null)
        {
        }

        public GetAllLinkedQueryAggregateCollection(RepositoryContext context) : base(context)
        {
        }

        public IEnumerable<TOutputDto> GetAll(TKey id, IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entities = GetAllLinkedEntities((IEntityQueryRepository)repository, id, user);

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

            return Aggregates.Select(a => (TOutputDto)a.OutputDto);
        }

        public async Task<IEnumerable<TOutputDto>> GetAllAsync(TKey id, IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entities = await GetAllLinkedEntitiesAsync((IEntityQueryRepository)repository, id, user);

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

            return Aggregates.Select(a => (TOutputDto)a.OutputDto);
        }
    }
}

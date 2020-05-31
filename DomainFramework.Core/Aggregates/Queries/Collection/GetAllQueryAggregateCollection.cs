using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetAllQueryAggregateCollection<TEntity, TOutputDto, TAggregate> : QueryAggregateCollection<TEntity, TOutputDto, TAggregate>
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
        where TAggregate : IQueryAggregate, new()
    {
        public GetAllQueryAggregateCollection(RepositoryContext context) : base(context)
        {
        }

        public IEnumerable<TOutputDto> GetAll(IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entities = ((IEntityQueryRepository)repository).GetAll();

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

        public async Task<IEnumerable<TOutputDto>> GetAllAsync(IAuthenticatedUser user)
        {
            Aggregates = new List<TAggregate>();

            var repository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entities = await ((IEntityQueryRepository)repository).GetAllAsync();

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

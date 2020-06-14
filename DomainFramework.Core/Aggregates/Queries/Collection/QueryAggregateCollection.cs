using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryAggregateCollection<TEntity, TOutputDto, TAggregate> : IQueryAggregateCollection<TEntity, TAggregate>
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
        where TAggregate : IQueryAggregate, new()       
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public IEnumerable<TAggregate> Aggregates { get; set; }

        public QueryAggregateCollection(RepositoryContext context)
        {
            RepositoryContext = context;
        }

        public (int, IEnumerable<TOutputDto>) Get(Tuple<int, IEnumerable<IEntity>> data)
        {
            Aggregates = new List<TAggregate>();

            var (count, entities) = data;

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

        protected async Task<(int, IEnumerable<TOutputDto>)> GetAsync(Tuple<int, IEnumerable<IEntity>> data)
        {
            Aggregates = new List<TAggregate>();

            var (count, entities) = data;

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

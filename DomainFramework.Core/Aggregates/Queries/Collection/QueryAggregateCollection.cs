using System.Collections.Generic;

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
    }
}

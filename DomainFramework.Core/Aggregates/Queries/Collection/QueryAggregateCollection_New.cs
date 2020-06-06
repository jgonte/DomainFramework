using System.Collections.Generic;

namespace DomainFramework.Core
{
    public abstract class QueryAggregateCollection_New<TEntity, TOutputDto> : IQueryAggregateCollection
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()    
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public IEnumerable<IQueryAggregate> QueryAggregates { get; set; }

        public QueryAggregateCollection_New(RepositoryContext context)
        {
            RepositoryContext = context;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryAggregateCollection<TAggregate, TEntity, TDto> : IQueryAggregateCollection<TAggregate, TEntity>
        where TAggregate : IQueryAggregate<TEntity, TDto>, new()
        where TEntity : IEntity
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public IEnumerable<TAggregate> Aggregates { get; set; }

        public QueryAggregateCollection(RepositoryContext context)
        {
            RepositoryContext = context;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryAggregateCollection<TAggregate, TEntity, TOutputDto> : IQueryAggregateCollection<TAggregate, TEntity>
        where TAggregate : IQueryAggregate<TEntity, TOutputDto>, new()
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public IEnumerable<TAggregate> Aggregates { get; set; }

        public QueryAggregateCollection(RepositoryContext context)
        {
            RepositoryContext = context;
        }
    }
}

using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface IQueryAggregateCollection
    {
        /// <summary>
        /// The aggregates to fully load the entities returned when querying a collection
        /// </summary>
        IEnumerable<IQueryAggregate> QueryAggregates { get; set; }

    }

    public interface IQueryAggregateCollection<TEntity, TAggregate> : IAggregateCollection<TAggregate>
        where TEntity : IEntity
        where TAggregate : IAggregate
    {
    }
}

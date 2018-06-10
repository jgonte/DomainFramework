using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface ICollectionAggregateLink
    {
        IEnumerable<IAggregate> GetLinkedAggregates();
    }

    public interface ICollectionAggregateLink<TEntity, TLinkedAggregate> : ICollectionAggregateLink
        where TEntity : IEntity
        where TLinkedAggregate: IAggregate
    {
        List<TLinkedAggregate> LinkedAggregates { get; set; }
    }
}
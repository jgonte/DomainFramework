namespace DomainFramework.Core
{
    public interface IQueryAggregateCollection<TEntity, TAggregate> : IAggregateCollection<TAggregate>
        where TEntity : IEntity
        where TAggregate : IAggregate<TEntity>
    {
    }
}

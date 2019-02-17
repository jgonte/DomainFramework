namespace DomainFramework.Core
{
    public interface IQueryAggregateCollection<TAggregate, TEntity> : IAggregateCollection<TAggregate>
        where TAggregate : IAggregate<TEntity>
    {
    }
}

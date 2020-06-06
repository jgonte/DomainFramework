namespace DomainFramework.Core
{
    public interface ILinkedAggregateCommandOperation : ICommandOperation
    {
        ICommandAggregate CommandAggregate { get; }
    }
}
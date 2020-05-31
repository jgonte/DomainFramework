using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface IAggregateCollection
    {
        IRepositoryContext RepositoryContext { get; set; }

        IEnumerable<IAggregate> Aggregates { get; set; }
    }

    public interface IAggregateCollection<TAggregate>
        where TAggregate : IAggregate
    {
        IRepositoryContext RepositoryContext { get; set; }

        IEnumerable<TAggregate> Aggregates { get; set; }
    }
}
using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface IAggregateCollection<TAggregate>
        where TAggregate : IAggregate
    {
        IRepositoryContext RepositoryContext { get; set; }

        IEnumerable<TAggregate> Aggregates { get; set; }
    }
}
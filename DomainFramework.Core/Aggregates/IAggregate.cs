using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface IAggregate
    {
        IRepositoryContext RepositoryContext { get; set; }
    }

    public interface IAggregate<TEntity> : IAggregate
    {
        TEntity RootEntity { get; set; }
    }
}
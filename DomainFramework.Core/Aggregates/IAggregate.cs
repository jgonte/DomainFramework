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

        List<IInheritanceEntityLink> InheritanceEntityLinks { get; set; }

        List<ISingleEntityLink> SingleEntityLinks { get; set; }

        List<ICollectionEntityLink> CollectionEntityLinks { get; set; }
    }
}
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
<<<<<<< HEAD
=======

        List<IInheritanceEntityLink> InheritanceEntityLinks { get; set; }

        List<ISingleEntityLink> SingleEntityLinks { get; set; }

        List<ICollectionEntityLink> CollectionEntityLinks { get; set; }
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
    }
}
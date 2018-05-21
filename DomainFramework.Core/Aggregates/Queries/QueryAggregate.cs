using System;
using System.Collections.Generic;

namespace DomainFramework.Core
{
    public class QueryAggregate<TEntity> : IQueryAggregate<TEntity>
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public TEntity RootEntity { get; set; }

        public List<IInheritanceEntityLink> InheritanceEntityLinks { get; set; }

        public List<ISingleEntityLink> SingleEntityLinks { get; set; }

        public List<ICollectionEntityLink> CollectionEntityLinks { get; set; }

        public object Load(IUnitOfWork unitOfWork = null)
        {
            throw new NotImplementedException();
        }
    }
}

<<<<<<< HEAD
﻿using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface IQueryAggregate<TKey, TEntity> : IAggregate<TEntity>
    {
        List<IQueryInheritanceEntityLink> InheritanceEntityLinks { get; set; }

        List<IQuerySingleEntityLink> SingleEntityLinks { get; set; }

        List<IQueryCollectionEntityLink> CollectionEntityLinks { get; set; }

        /// <summary>
        /// Loads this aggregate by the id of its root entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="unitOfWork"></param>
        void Load(TKey id, IUnitOfWork unitOfWork = null);
=======
﻿namespace DomainFramework.Core
{
    public interface IQueryAggregate<TEntity> : IAggregate<TEntity>
    {
        object Load(IUnitOfWork unitOfWork = null);
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
    }
}

<<<<<<< HEAD
﻿using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface ICommandAggregate<TEntity> : IAggregate<TEntity>
    {
        List<ICommandInheritanceEntityLink<TEntity>> InheritanceEntityLinks { get; set; }

        List<ICommandSingleEntityLink<TEntity>> SingleEntityLinks { get; set; }

        List<ICommandCollectionEntityLink<TEntity>> CollectionEntityLinks { get; set; }

=======
﻿namespace DomainFramework.Core
{
    public interface ICommandAggregate<TEntity> : IAggregate<TEntity>
    {
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        void Save(IUnitOfWork unitOfWork = null);

        void Delete(IUnitOfWork unitOfWork = null);
    }
}
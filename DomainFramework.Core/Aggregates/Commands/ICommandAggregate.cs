using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface ICommandAggregate<TEntity> : IAggregate<TEntity>
    {
        List<ICommandInheritanceEntityLink<TEntity>> InheritanceEntityLinks { get; set; }

        List<ICommandSingleEntityLink<TEntity>> SingleEntityLinks { get; set; }

        List<ICommandCollectionEntityLink<TEntity>> CollectionEntityLinks { get; set; }

        void Save(IUnitOfWork unitOfWork = null);

        void Delete(IUnitOfWork unitOfWork = null);
    }
}
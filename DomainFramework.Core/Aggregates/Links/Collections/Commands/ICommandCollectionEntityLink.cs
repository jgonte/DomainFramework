using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface ICommandCollectionEntityLink<TEntity> : ICollectionEntityLink
    {
        void Save(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity);

        Task SaveAsync(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity);
    }

    public interface ICommandCollectionEntityLink<TEntity, TLinkedEntity> : ICommandCollectionEntityLink<TEntity>,
        ICollectionEntityLink<TEntity, TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        void AddEntity(TLinkedEntity entity);
    }
}
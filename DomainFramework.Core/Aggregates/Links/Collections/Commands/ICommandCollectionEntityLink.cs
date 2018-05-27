using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface ICommandCollectionEntityLink<TEntity> : ICollectionEntityLink
    {
        void Save(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity);

        Task SaveAsync(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity);
    }

    public interface ICommandCollectionEntityLink<TEntity, TLinkedEntity> : ICommandCollectionEntityLink<TEntity>,
        ICollectionEntityLink<TLinkedEntity>
    {
        void AddEntity(TLinkedEntity entity);
    }
}
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQueryCollectionEntityLink
    {
        void PopulateEntities(IRepositoryContext repositoryContext, IEntity entity);

        Task PopulateEntitiesAsync(IRepositoryContext repositoryContext, IEntity entity);
    }

    public interface IQueryCollectionEntityLink<TEntity, TLinkedEntity> : IQueryCollectionEntityLink,
        ICollectionEntityLink<TEntity, TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        void PopulateEntities(IRepositoryContext repositoryContext, TEntity entity);

        Task PopulateEntitiesAsync(IRepositoryContext repositoryContext, TEntity entity);
    }
}
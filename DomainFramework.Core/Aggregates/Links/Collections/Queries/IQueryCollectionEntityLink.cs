namespace DomainFramework.Core
{
    public interface IQueryCollectionEntityLink : IEntityLink
    {
        void PopulateEntities(IQueryRepository repository, IEntity entity);
    }

    public interface IQueryCollectionEntityLink<TEntity, TLinkedEntity> : IQueryCollectionEntityLink,
        ICollectionEntityLink<TEntity, TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        void PopulateEntities(IQueryRepository repository, TEntity entity);
    }
}
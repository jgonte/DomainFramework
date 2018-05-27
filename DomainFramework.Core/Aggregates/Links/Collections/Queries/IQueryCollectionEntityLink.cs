namespace DomainFramework.Core
{
    public interface IQueryCollectionEntityLink : ICollectionEntityLink
    {
        void PopulateEntities(IQueryRepository repository, IEntity entity);
    }

    public interface IQueryCollectionEntityLink<TEntity, TLinkedEntity> : IQueryCollectionEntityLink,
        ICollectionEntityLink<TLinkedEntity>
    {
        void PopulateEntities(IQueryRepository repository, TEntity entity);
    }
}
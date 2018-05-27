namespace DomainFramework.Core
{
    public interface IQuerySingleEntityLink : ISingleEntityLink
    {
        void PopulateEntity(IQueryRepository repository, IEntity entity);
    }

    public interface IQuerySingleEntityLink<TEntity, TLinkedEntity> : IQuerySingleEntityLink,
        ISingleEntityLink<TLinkedEntity>
    {
        void PopulateEntity(IQueryRepository repository, TEntity entity);
    }
}
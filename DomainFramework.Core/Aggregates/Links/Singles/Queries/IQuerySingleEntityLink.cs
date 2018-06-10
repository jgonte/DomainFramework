using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQuerySingleEntityLink : ISingleEntityLink
    {
        void PopulateEntity(IRepositoryContext repositoryContext, IEntity entity);

        Task PopulateEntityAsync(IRepositoryContext repositoryContext, IEntity entity);
    }

    public interface IQuerySingleEntityLink<TEntity, TLinkedEntity> : IQuerySingleEntityLink,
        ISingleEntityLink<TLinkedEntity>
        where TLinkedEntity : IEntity
    {
        void PopulateEntity(IRepositoryContext repositoryContext, TEntity entity);

        Task PopulateEntityAsync(IRepositoryContext repositoryContext, TEntity entity);
    }
}
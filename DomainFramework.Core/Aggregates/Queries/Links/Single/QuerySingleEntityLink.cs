using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QuerySingleEntityLink<TEntity, TLinkedEntity> : IQuerySingleEntityLink
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        public TLinkedEntity LinkedEntity { get; set; }

        IEntity IQuerySingleEntityLink.LinkedEntity { get => LinkedEntity; }

        public void PopulateEntity(IRepositoryContext repositoryContext, IEntity entity)
        {
            PopulateEntity(repositoryContext, (TEntity)entity);
        }

        public abstract void PopulateEntity(IRepositoryContext repositoryContext, TEntity entity);

        public async Task PopulateEntityAsync(IRepositoryContext repositoryContext, IEntity entity)
        {
            await PopulateEntityAsync(repositoryContext, (TEntity)entity);
        }

        public abstract Task PopulateEntityAsync(IRepositoryContext repositoryContext, TEntity entity);
    }
}

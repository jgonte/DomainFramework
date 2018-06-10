using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QuerySingleEntityLink<TEntity, TLinkedEntity> : IQuerySingleEntityLink<TEntity, TLinkedEntity>,
        ISingleEntityLink<TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        public Type LinkedEntityType => typeof(TLinkedEntity);

        public TLinkedEntity LinkedEntity { get; set; }

        public IEntity GetLinkedEntity() => LinkedEntity;

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

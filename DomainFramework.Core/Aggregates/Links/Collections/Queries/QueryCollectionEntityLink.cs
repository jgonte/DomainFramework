using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryCollectionEntityLink<TEntity, TLinkedEntity> : IQueryCollectionEntityLink,
        ICollectionEntityLink<TEntity, TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        public List<TLinkedEntity> LinkedEntities { get; set; }

        public IEnumerable<IEntity> GetLinkedEntities() => LinkedEntities.Cast<IEntity>();

        public void PopulateEntities(IRepositoryContext repositoryContext, IEntity entity)
        {
            PopulateEntities(repositoryContext, (TEntity)entity);
        }

        public abstract void PopulateEntities(IRepositoryContext repositoryContext, TEntity entity);

        public async Task PopulateEntitiesAsync(IRepositoryContext repositoryContext, IEntity entity)
        {
            await PopulateEntitiesAsync(repositoryContext, (TEntity)entity);
        }

        public abstract Task PopulateEntitiesAsync(IRepositoryContext repositoryContext, TEntity entity);
    }
}

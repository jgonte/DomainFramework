using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository to retrieve the instances of the entity from the database
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to retrieve</typeparam>
    /// <typeparam name="TKey">The type of the key of the entity</typeparam>
    public abstract class QueryRepository<TEntity, TKey> : IQueryRepository
        where TEntity : Entity<TKey>, new()
    {
        public string ConnectionName { get; set; }

        public abstract TEntity GetById(TKey id);

        public IEntity GetById(object id)
        {
            return GetById((TKey)id);
        }

        public abstract Task<TEntity> GetByIdAsync(TKey id);

        public async Task<IEntity> GetByIdAsync(object id)
        {
            return await GetByIdAsync((TKey)id);
        }

        public abstract IEnumerable<IEntity>Get(QueryParameters parameters);

        public abstract Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters);

        protected TEntity CreateEntity(TKey id)
        {
            return new TEntity
            {
                Id = id
            };
        }
    }
}

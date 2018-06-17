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

        public abstract TEntity GetById(TKey id, IAuthenticatedUser user);

        public IEntity GetById(object id, IAuthenticatedUser user)
        {
            return GetById((TKey)id, user);
        }

        public abstract Task<TEntity> GetByIdAsync(TKey id, IAuthenticatedUser user);

        public async Task<IEntity> GetByIdAsync(object id, IAuthenticatedUser user)
        {
            return await GetByIdAsync((TKey)id, user);
        }

        public abstract IEnumerable<IEntity>Get(QueryParameters parameters, IAuthenticatedUser user);

        public abstract Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters, IAuthenticatedUser user);
    }
}

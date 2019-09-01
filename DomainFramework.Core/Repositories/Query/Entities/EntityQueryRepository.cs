using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository to retrieve the instances of the entity from the database
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to retrieve</typeparam>
    /// <typeparam name="TKey">The type of the key of the entity</typeparam>
    public abstract class EntityQueryRepository<TEntity, TKey> : IEntityQueryRepository
        where TEntity : Entity<TKey>, new()
    {
        public string ConnectionName { get; set; }

        public virtual TEntity GetById(TKey id, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        public IEntity GetById(object id, IAuthenticatedUser user)
        {
            return GetById((TKey)id, user);
        }

        public virtual Task<TEntity> GetByIdAsync(TKey id, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEntity> GetByIdAsync(object id, IAuthenticatedUser user)
        {
            return await GetByIdAsync((TKey)id, user);
        }

        public virtual IEnumerable<TEntity> Get(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IEntity> IEntityQueryRepository.Get(QueryParameters parameters, IAuthenticatedUser user)
        {
            return Get(parameters, user);
        }

        public virtual Task<IEnumerable<TEntity>> GetAsync(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<IEntity>> IEntityQueryRepository.GetAsync(QueryParameters parameters, IAuthenticatedUser user)
        {
            return await GetAsync(parameters, user);
        }
    }
}

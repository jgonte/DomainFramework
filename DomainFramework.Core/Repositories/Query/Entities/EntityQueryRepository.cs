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

        #region GetById

        public virtual TEntity GetById(TKey id)
        {
            throw new NotImplementedException();
        }

        public IEntity GetById(object id)
        {
            return GetById((TKey)id);
        }

        public virtual Task<TEntity> GetByIdAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEntity> GetByIdAsync(object id)
        {
            return await GetByIdAsync((TKey)id);
        }
        
        #endregion

        #region Get

        public virtual (int, IEnumerable<TEntity>) Get(CollectionQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        (int, IEnumerable<IEntity>) IEntityQueryRepository.Get(CollectionQueryParameters parameters)
        {
            return Get(parameters);
        }

        public virtual Task<(int, IEnumerable<TEntity>)> GetAsync(CollectionQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        async Task<(int, IEnumerable<IEntity>)> IEntityQueryRepository.GetAsync(CollectionQueryParameters parameters)
        {
            return await GetAsync(parameters);
        }

        #endregion

        #region GetAll

        public virtual IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        IEnumerable<IEntity> IEntityQueryRepository.GetAll()
        {
            return GetAll();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<IEntity>> IEntityQueryRepository.GetAllAsync()
        {
            return await GetAllAsync();
        }

        #endregion
    }
}

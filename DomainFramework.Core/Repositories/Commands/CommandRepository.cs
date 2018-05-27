<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Repository that performs write operations to a database
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to persist</typeparam>
    /// <typeparam name="TKey">The type of the key of the entity</typeparam>
    public abstract class CommandRepository<TEntity, TKey> : ICommandRepository
        where TEntity : Entity<TKey>
    {
        public string ConnectionName { get; set; }

        public Func<IEnumerable<IEntity>> TransferEntities { get; set; }

        public virtual void Save(TEntity entity, IUnitOfWork unitOfWork = null)
=======
﻿namespace DomainFramework.Core
{
    /// <summary>
    /// Repository that performs write operation on a database
    /// </summary>
    /// <typeparam name="E">The type of the entity</typeparam>
    /// <typeparam name="K">The type of the key of the entity</typeparam>
    /// <typeparam name="T">The type of the data of the entity</typeparam>
    public abstract class CommandRepository<E, K, T> : ICommandRepository
        where E : Entity<K, T>
    {
        public string ConnectionName { get; set; }

        public virtual void Save(E entity, IUnitOfWork unitOfWork = null)
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        {
            if (entity.Id == null)
            {
                Insert(entity, unitOfWork);
            }
            else
            {
                Update(entity, unitOfWork);
            }
        }

<<<<<<< HEAD
        public abstract void Insert(TEntity entity, IUnitOfWork unitOfWork = null);

        public abstract bool Update(TEntity entity, IUnitOfWork unitOfWork = null);

        public abstract bool Delete(TEntity entity, IUnitOfWork unitOfWork = null);

        public virtual async Task SaveAsync(TEntity entity, IUnitOfWork unitOfWork = null)
        {
            if (entity.Id == null)
            {
                await InsertAsync(entity, unitOfWork);
            }
            else
            {
                await UpdateAsync(entity, unitOfWork);
            }
        }

        public abstract Task InsertAsync(TEntity entity, IUnitOfWork unitOfWork = null);

        public abstract Task<bool> UpdateAsync(TEntity entity, IUnitOfWork unitOfWork = null);

        public abstract Task<bool> DeleteAsync(TEntity entity, IUnitOfWork unitOfWork = null);

        protected void UpdateEntityId(TEntity entity, TKey id)
=======
        public abstract void Insert(E entity, IUnitOfWork unitOfWork = null);

        public abstract bool Update(E entity, IUnitOfWork unitOfWork = null);

        public abstract bool Delete(E entity, IUnitOfWork unitOfWork = null);

        protected void UpdateEntityId(E entity, K id)
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        {
            entity.Id = id;
        }

        #region ICommandRepository members

        void ICommandRepository.Save(IEntity entity, IUnitOfWork unitOfWork)
        {
<<<<<<< HEAD
            Save((TEntity)entity, unitOfWork);
=======
            Save((E)entity, unitOfWork);
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        }

        void ICommandRepository.Insert(IEntity entity, IUnitOfWork unitOfWork)
        {
<<<<<<< HEAD
            Insert((TEntity)entity);
=======
            Insert((E)entity);
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        }

        bool ICommandRepository.Update(IEntity entity, IUnitOfWork unitOfWork)
        {
<<<<<<< HEAD
            return Update((TEntity)entity);
=======
            return Update((E)entity);
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        }

        bool ICommandRepository.Delete(IEntity entity, IUnitOfWork unitOfWork)
        {
<<<<<<< HEAD
            return Delete((TEntity)entity);
        }

        public async Task SaveAsync(IEntity entity, IUnitOfWork unitOfWork = null)
        {
            await SaveAsync((TEntity)entity, unitOfWork);
        }

        public async Task InsertAsync(IEntity entity, IUnitOfWork unitOfWork = null)
        {
            await InsertAsync((TEntity)entity, unitOfWork);
        }

        public async Task<bool> UpdateAsync(IEntity entity, IUnitOfWork unitOfWork = null)
        {
            return await UpdateAsync((TEntity)entity, unitOfWork);
        }

        public async Task<bool> DeleteAsync(IEntity entity, IUnitOfWork unitOfWork = null)
        {
            return await DeleteAsync((TEntity)entity, unitOfWork);
=======
            return Delete((E)entity);
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        }

        #endregion
    }
}

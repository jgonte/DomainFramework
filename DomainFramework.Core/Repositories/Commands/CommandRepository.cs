using System;
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
        {
            entity.Id = id;
        }

        #region ICommandRepository members

        void ICommandRepository.Save(IEntity entity, IUnitOfWork unitOfWork)
        {
            Save((TEntity)entity, unitOfWork);
        }

        void ICommandRepository.Insert(IEntity entity, IUnitOfWork unitOfWork)
        {
            Insert((TEntity)entity);
        }

        bool ICommandRepository.Update(IEntity entity, IUnitOfWork unitOfWork)
        {
            return Update((TEntity)entity);
        }

        bool ICommandRepository.Delete(IEntity entity, IUnitOfWork unitOfWork)
        {
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
        }

        #endregion
    }
}

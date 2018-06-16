using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Repository that performs write operations to a database for an entity
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to persist</typeparam>
    public abstract class CommandEntityRepository<TEntity> : ICommandEntityRepository
        where TEntity : IEntity
    {
        public string ConnectionName { get; set; }

        public Func<IEnumerable<IEntity>> TransferEntities { get; set; }

        public virtual void Save(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            if (entity.Id == null)
            {
                Insert(entity, user, unitOfWork);
            }
            else
            {
                Update(entity, user, unitOfWork);
            }
        }

        public abstract void Insert(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract bool Update(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract bool Delete(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public virtual async Task SaveAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            if (entity.Id == null)
            {
                await InsertAsync(entity, user, unitOfWork);
            }
            else
            {
                await UpdateAsync(entity, user, unitOfWork);
            }
        }

        public abstract Task InsertAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task<bool> UpdateAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task<bool> DeleteAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        #region ICommandRepository members

        void ICommandEntityRepository.Save(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            Save((TEntity)entity, user, unitOfWork);
        }

        void ICommandEntityRepository.Insert(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            Insert((TEntity)entity, user, unitOfWork);
        }

        bool ICommandEntityRepository.Update(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return Update((TEntity)entity, user, unitOfWork);
        }

        bool ICommandEntityRepository.Delete(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return Delete((TEntity)entity, user, unitOfWork);
        }

        public async Task SaveAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await SaveAsync((TEntity)entity, user, unitOfWork);
        }

        public async Task InsertAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await InsertAsync((TEntity)entity, user, unitOfWork);
        }

        public async Task<bool> UpdateAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return await UpdateAsync((TEntity)entity, user, unitOfWork);
        }

        public async Task<bool> DeleteAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return await DeleteAsync((TEntity)entity, user, unitOfWork);
        }

        #endregion
    }
}

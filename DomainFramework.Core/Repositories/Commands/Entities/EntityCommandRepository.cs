using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Repository that performs write operations to a database for an entity
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to persist</typeparam>
    public abstract class EntityCommandRepository<TEntity> : IEntityCommandRepository
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
                if (!Update(entity, user, unitOfWork))
                {
                    throw new InvalidCastException($"No update occurred for command repository of entity: '{typeof(TEntity)}'. Id: {entity.Id}");
                }
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
                if (!await UpdateAsync(entity, user, unitOfWork))
                {
                    throw new InvalidCastException($"No update occurred for command repository of entity: '{typeof(TEntity)}'. Id: {entity.Id}");
                }
            }
        }

        public abstract Task InsertAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task<bool> UpdateAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task<bool> DeleteAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        #region ICommandRepository members

        void IEntityCommandRepository.Save(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            Save((TEntity)entity, user, unitOfWork);
        }

        void IEntityCommandRepository.Insert(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            Insert((TEntity)entity, user, unitOfWork);
        }

        bool IEntityCommandRepository.Update(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return Update((TEntity)entity, user, unitOfWork);
        }

        bool IEntityCommandRepository.Delete(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
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

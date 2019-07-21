using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.DataAccess
{
    /// <summary>
    /// Implementation of a command repository to work using the DataAccess library
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityCommandRepository<TEntity> : IEntityCommandRepository
        where TEntity : IEntity
    {
        public string ConnectionName { get; set; }

        // This is needed because the id of the entity might not be available until the entity is inserted
        public Func<IEnumerable<EntityDependency>> Dependencies { get; set; } = EntityDependency.EmptyEntityDependencies;

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

        #region Insert

        public void Insert(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateInsertCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                HandleInsert(command);
            }
        }

        protected virtual Command CreateInsertCommand(TEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected virtual void HandleInsert(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Update

        public bool Update(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateUpdateCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleUpdate(command);
            }
        }

        protected virtual Command CreateUpdateCommand(TEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected virtual bool HandleUpdate(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Delete
    
        public bool Delete(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleDelete(command);
            }
        }

        protected virtual Command CreateDeleteCommand(TEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected virtual bool HandleDelete(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DeleteCollection

        public bool DeleteCollection(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector = null)
        {
            var command = CreateDeleteCollectionCommand(entity, user, selector);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleDeleteCollection(command);
            }
        }

        protected virtual Command CreateDeleteCollectionCommand(TEntity entity, IAuthenticatedUser user, string selector)
        {
            throw new NotImplementedException();
        }

        protected virtual bool HandleDeleteCollection(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

        public async Task SaveAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
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

        #region InsertAsync

        public async Task InsertAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateInsertCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                await HandleInsertAsync(command);
            }
        }

        protected virtual Task HandleInsertAsync(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UpdateAsync

        public async Task<bool> UpdateAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateUpdateCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return await HandleUpdateAsync(command);
            }
        }

        protected virtual Task<bool> HandleUpdateAsync(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DeleteAsync

        public async Task<bool> DeleteAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return await HandleDeleteAsync(command);
            }
        }

        protected virtual Task<bool> HandleDeleteAsync(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DeleteCollectionAsync

        public async Task<bool> DeleteCollectionAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector)
        {
            var command = CreateDeleteCollectionCommand(entity, user, selector);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return await HandleDeleteCollectionAsync(command);
            }
        }

        protected virtual Task<bool> HandleDeleteCollectionAsync(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

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

        bool IEntityCommandRepository.DeleteCollection(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector)
        {
            return DeleteCollection((TEntity)entity, user, unitOfWork, selector);
        }

        async Task IEntityCommandRepository.SaveAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await SaveAsync((TEntity)entity, user, unitOfWork);
        }

        async Task IEntityCommandRepository.InsertAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await InsertAsync((TEntity)entity, user, unitOfWork);
        }

        async Task<bool> IEntityCommandRepository.UpdateAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return await UpdateAsync((TEntity)entity, user, unitOfWork);
        }

        async Task<bool> IEntityCommandRepository.DeleteAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return await DeleteAsync((TEntity)entity, user, unitOfWork);
        }

        async Task<bool> IEntityCommandRepository.DeleteCollectionAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector = null)
        {
            return await DeleteCollectionAsync((TEntity)entity, user, unitOfWork, selector);
        }

        #endregion
    }
}

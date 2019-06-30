using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.DataAccess
{
    public class LinkedValueObjectCommandRepository<TValueObject> : ILinkedValueObjectCommandRepository
        where TValueObject : IValueObject
    {
        public string ConnectionName { get; set; }

        // This is needed because the id of the entity might not be available until the entity is inserted
        public Func<IEnumerable<IEntity>> Dependencies { get; set; }
 
        #region Add

        public void Add(TValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateInsertCommand(valueObject, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                HandleInsert(command);
            }
        }

        protected virtual Command CreateInsertCommand(TValueObject valueObject, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected virtual void HandleInsert(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DeleteCollection

        public bool DeleteCollection(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteCollectionCommand(user);

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

        protected virtual Command CreateDeleteCollectionCommand(IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected virtual bool HandleDeleteCollection(Command command)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region AddAsync

        public async Task AddAsync(TValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateInsertCommand(valueObject, user);

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

        #region DeleteCollectionAsync

        public async Task<bool> DeleteCollectionAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteCollectionCommand(user);

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

        // Adapter to use the interface IEntity and IValueObject
        #region ILinkedValueObjectCommandRepository

        void ILinkedValueObjectCommandRepository.Add(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            Add((TValueObject)valueObject, user, unitOfWork);
        }

        bool ILinkedValueObjectCommandRepository.DeleteCollection(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return DeleteCollection(user, unitOfWork);
        }

        async Task ILinkedValueObjectCommandRepository.AddAsync(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await AddAsync((TValueObject)valueObject, user, unitOfWork);
        }

        async Task<bool> ILinkedValueObjectCommandRepository.DeleteCollectionAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return await DeleteCollectionAsync(user, unitOfWork);
        } 

        #endregion
    }
}

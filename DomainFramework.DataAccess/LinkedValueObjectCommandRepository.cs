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
        public Func<IEnumerable<EntityDependency>> Dependencies { get; set; }
 
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

        #region DeleteLinks

        public bool DeleteLinks(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteLinksCommand(user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleDeleteLinks(command);
            }
        }

        protected virtual Command CreateDeleteLinksCommand(IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected virtual bool HandleDeleteLinks(Command command)
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

        #region DeleteLinksAsync

        public async Task<bool> DeleteLinksAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteLinksCommand(user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return await HandleDeleteLinksAsync(command);
            }
        }

        protected virtual Task<bool> HandleDeleteLinksAsync(Command command)
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

        bool ILinkedValueObjectCommandRepository.DeleteLinks(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return DeleteLinks(user, unitOfWork);
        }

        async Task ILinkedValueObjectCommandRepository.AddAsync(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await AddAsync((TValueObject)valueObject, user, unitOfWork);
        }

        async Task<bool> ILinkedValueObjectCommandRepository.DeleteLinksAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return await DeleteLinksAsync(user, unitOfWork);
        } 

        #endregion
    }
}

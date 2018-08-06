using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class ValueObjectCommandRepository<TValueObject> : IValueObjectCommandRepository
        where TValueObject : IValueObject
    {
        public string ConnectionName { get; set; }

        public Func<IEnumerable<IEntity>> TransferEntities { get; set; }

        public abstract void Insert(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract bool DeleteAll(IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task InsertAsync(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task<bool> DeleteAllAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork);

        #region ICommandValueObjectRepository members

        void IValueObjectCommandRepository.Insert(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            Insert(valueObject, user, unitOfWork);
        }

        bool IValueObjectCommandRepository.DeleteAll(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return DeleteAll(user, unitOfWork);
        }

        async Task IValueObjectCommandRepository.InsertAsync(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await InsertAsync(valueObject, user, unitOfWork);
        }

        Task<bool> IValueObjectCommandRepository.DeleteAllAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return DeleteAllAsync(user, unitOfWork);
        }

        #endregion
    }
}

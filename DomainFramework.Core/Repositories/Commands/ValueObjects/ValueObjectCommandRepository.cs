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

        public abstract void Insert(TValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract bool DeleteAll(IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task InsertAsync(TValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task<bool> DeleteAllAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork);

        #region ICommandValueObjectRepository members

        void IValueObjectCommandRepository.Insert(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            Insert((TValueObject)valueObject, user, unitOfWork);
        }

        bool IValueObjectCommandRepository.DeleteAll(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return DeleteAll(user, unitOfWork);
        }

        async Task IValueObjectCommandRepository.InsertAsync(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await InsertAsync((TValueObject)valueObject, user, unitOfWork);
        }

        Task<bool> IValueObjectCommandRepository.DeleteAllAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return DeleteAllAsync(user, unitOfWork);
        }

        #endregion
    }
}

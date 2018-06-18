using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class CommandValueObjectRepository<TValueObject> : ICommandValueObjectRepository
        where TValueObject : IValueObject
    {
        public string ConnectionName { get; set; }

        public Func<IEnumerable<IEntity>> TransferEntities { get; set; }

        public abstract void Insert(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract bool DeleteAll(IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task InsertAsync(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        public abstract Task<bool> DeleteAllAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork);

        #region ICommandValueObjectRepository members

        void ICommandValueObjectRepository.Insert(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            Insert(valueObject, user, unitOfWork);
        }

        bool ICommandValueObjectRepository.DeleteAll(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return DeleteAll(user, unitOfWork);
        }

        async Task ICommandValueObjectRepository.InsertAsync(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await InsertAsync(valueObject, user, unitOfWork);
        }

        Task<bool> ICommandValueObjectRepository.DeleteAllAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            return DeleteAllAsync(user, unitOfWork);
        }

        #endregion
    }
}

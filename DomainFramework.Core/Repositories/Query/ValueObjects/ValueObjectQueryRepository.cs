using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class ValueObjectQueryRepository<TOwnerId, TValueObject> : IValueObjectQueryRepository
        where TValueObject : IValueObject
    {
        public string ConnectionName { get; set; }

        public abstract IEnumerable<TValueObject> Get(TOwnerId ownerId, IAuthenticatedUser user);

        public abstract Task<IEnumerable<TValueObject>> GetAsync(TOwnerId ownerId, IAuthenticatedUser user);

        #region IQueryValueObjectRepository members

        IEnumerable<IValueObject> IValueObjectQueryRepository.Get(object ownerId, IAuthenticatedUser user)
        {
            return Get((TOwnerId)ownerId, user).Cast<IValueObject>();
        }

        async Task<IEnumerable<IValueObject>> IValueObjectQueryRepository.GetAsync(object ownerId, IAuthenticatedUser user)
        {
            var valueObjects = await GetAsync((TOwnerId)ownerId, user);

            return valueObjects.Cast<IValueObject>();
        }

        #endregion
    }
}

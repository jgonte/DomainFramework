using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class ValueObjectQueryRepository<TOwnerId, TValueObject> : IValueObjectQueryRepository
        where TValueObject : IValueObject
    {
        public string ConnectionName { get; set; }

        public abstract (int, IEnumerable<TValueObject>) Get(TOwnerId ownerId, CollectionQueryParameters queryParameters);

        public abstract Task<(int, IEnumerable<TValueObject>)> GetAsync(TOwnerId ownerId, CollectionQueryParameters queryParameters);

        public abstract IEnumerable<TValueObject> GetAll(TOwnerId ownerId);

        public abstract Task<IEnumerable<TValueObject>> GetAllAsync(TOwnerId ownerId);

        #region IQueryValueObjectRepository members

        (int, IEnumerable<IValueObject>) IValueObjectQueryRepository.Get(object ownerId, CollectionQueryParameters queryParameters)
        {
            (int, IEnumerable<TValueObject>) result = Get((TOwnerId)ownerId, queryParameters);

            return (result.Item1, result.Item2.Cast<IValueObject>());
        }

        async Task<(int, IEnumerable<IValueObject>)> IValueObjectQueryRepository.GetAsync(object ownerId, CollectionQueryParameters queryParameters)
        {
            (int, IEnumerable<TValueObject>) result = await GetAsync((TOwnerId)ownerId, queryParameters);

            return (result.Item1, result.Item2.Cast<IValueObject>());
        }

        IEnumerable<IValueObject> IValueObjectQueryRepository.GetAll(object ownerId)
        {
            var valueObjects = GetAll((TOwnerId)ownerId);

            return valueObjects.Cast<IValueObject>();
        }

        async Task<IEnumerable<IValueObject>> IValueObjectQueryRepository.GetAllAsync(object ownerId)
        {
            var valueObjects = await GetAllAsync((TOwnerId)ownerId);

            return valueObjects.Cast<IValueObject>();
        }

        #endregion
    }
}

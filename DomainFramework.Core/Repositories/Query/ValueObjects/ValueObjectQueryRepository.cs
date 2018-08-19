using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class ValueObjectQueryRepository<TValueObject> : IValueObjectQueryRepository
        where TValueObject : IValueObject
    {
        public string ConnectionName { get; set; }

        public abstract IEnumerable<TValueObject> Get(QueryParameters parameters, IAuthenticatedUser user);

        public abstract Task<IEnumerable<TValueObject>> GetAsync(QueryParameters parameters, IAuthenticatedUser user);

        #region IQueryValueObjectRepository members

        IEnumerable<IValueObject> IValueObjectQueryRepository.Get(QueryParameters parameters, IAuthenticatedUser user)
        {
            return Get(parameters, user).Cast<IValueObject>();
        }

        async Task<IEnumerable<IValueObject>> IValueObjectQueryRepository.GetAsync(QueryParameters parameters, IAuthenticatedUser user)
        {
            var valueObjects = await GetAsync(parameters, user);

            return valueObjects.Cast<IValueObject>();
        }

        #endregion
    }
}

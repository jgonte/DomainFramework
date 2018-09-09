using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository
    /// </summary>
    public interface IValueObjectQueryRepository : IRepository
    {
        IEnumerable<IValueObject> Get(object ownerId, IAuthenticatedUser user);

        Task<IEnumerable<IValueObject>> GetAsync(object ownerId, IAuthenticatedUser user);
    }
}
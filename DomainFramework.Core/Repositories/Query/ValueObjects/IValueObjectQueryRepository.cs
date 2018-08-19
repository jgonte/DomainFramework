using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository
    /// </summary>
    public interface IValueObjectQueryRepository : IRepository
    {
        IEnumerable<IValueObject> Get(QueryParameters parameters, IAuthenticatedUser user);

        Task<IEnumerable<IValueObject>> GetAsync(QueryParameters parameters, IAuthenticatedUser user);
    }
}
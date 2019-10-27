using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository
    /// </summary>
    public interface IEntityQueryRepository : IQueryRepository
    {
        IEntity GetById(object id, IAuthenticatedUser user);

        Task<IEntity> GetByIdAsync(object id, IAuthenticatedUser user);

        (int, IEnumerable<IEntity>) Get(CollectionQueryParameters parameters, IAuthenticatedUser user);

        Task<(int, IEnumerable<IEntity>)> GetAsync(CollectionQueryParameters parameters, IAuthenticatedUser user);
    }
}
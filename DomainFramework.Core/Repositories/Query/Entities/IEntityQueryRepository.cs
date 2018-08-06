using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository
    /// </summary>
    public interface IEntityQueryRepository : IRepository
    {
        IEntity GetById(object id, IAuthenticatedUser user);

        Task<IEntity> GetByIdAsync(object id, IAuthenticatedUser user);

        IEnumerable<IEntity> Get(QueryParameters parameters, IAuthenticatedUser user);

        Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters, IAuthenticatedUser user);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository
    /// </summary>
    public interface IQueryRepository : IRepository
    {
        IEntity GetById(object id);

        Task<IEntity> GetByIdAsync(object id);

        IEnumerable<IEntity> Get(QueryParameters parameters);

        Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters);
    }
}
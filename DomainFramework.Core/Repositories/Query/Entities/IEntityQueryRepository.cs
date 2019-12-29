using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository
    /// </summary>
    public interface IEntityQueryRepository : IQueryRepository
    {
        IEntity GetById(object id);

        Task<IEntity> GetByIdAsync(object id);

        (int, IEnumerable<IEntity>) Get(CollectionQueryParameters parameters);

        Task<(int, IEnumerable<IEntity>)> GetAsync(CollectionQueryParameters parameters);
    }
}
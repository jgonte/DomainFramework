using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQueryCollectionEntityLink
    {
        IEnumerable<IEntity> LinkedEntities { get; }

        void PopulateEntities(IRepositoryContext repositoryContext, IEntity entity);

        Task PopulateEntitiesAsync(IRepositoryContext repositoryContext, IEntity entity);
    }
}
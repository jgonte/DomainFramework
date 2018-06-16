using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQueryCollectionValueObjectLink
    {
        IEnumerable<IValueObject> LinkedValueObjects { get; }

        void PopulateValueObjects(IRepositoryContext repositoryContext, IEntity entity);

        Task PopulateValueObjectsAsync(IRepositoryContext repositoryContext, IEntity entity);
    }
}
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQueryInheritanceEntityLink<TKey>
    {
        void PopulateEntity(IRepositoryContext repositoryContext, TKey derivedEntityId);

        Task PopulateEntityAsync(IRepositoryContext repositoryContext, TKey derivedEntityId);
    }
}
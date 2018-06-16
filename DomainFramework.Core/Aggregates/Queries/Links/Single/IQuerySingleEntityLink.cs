using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQuerySingleEntityLink
    {
        IEntity LinkedEntity { get; }

        void PopulateEntity(IRepositoryContext repositoryContext, IEntity entity);

        Task PopulateEntityAsync(IRepositoryContext repositoryContext, IEntity entity);
    }
}
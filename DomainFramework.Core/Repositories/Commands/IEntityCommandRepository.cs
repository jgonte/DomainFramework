
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a write only repository
    /// </summary>
    public interface IEntityCommandRepository : ICommandRepository
    {
        void Save(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);

        void Insert(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);

        bool Update(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);

        bool Delete(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);

        // Pass the root entity to have access to its parameters if needed
        bool DeleteLinks(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);

        Task SaveAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);

        Task InsertAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);

        Task<bool> UpdateAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);

        Task<bool> DeleteAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);

        // Pass the root entity to have access to its parameters if needed
        Task<bool> DeleteLinksAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork, string selector);
    }
}
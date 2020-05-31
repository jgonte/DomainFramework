using System.Threading.Tasks;
using DomainFramework.Core;

namespace DomainFramework.DataAccess
{
    /// <summary>
    /// The use of a repository of this type is in case when an entity has a collection
    /// of value types that need to be inserted belonging to that entity (identifying relationship) 
    /// and in case of updating the entity will those value types, they will need to be removed 
    /// (therefore we need DeleteLinks or DeleteLinksAsync) and inserted after (Add or AddAsync)
    /// there are no updates for those items
    /// </summary>
    public interface ILinkedValueObjectCommandRepository : ICommandRepository
    {
        void Add(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        bool DeleteLinks(IAuthenticatedUser user, IUnitOfWork unitOfWork);

        Task AddAsync(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        Task<bool> DeleteLinksAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork);
    }
}
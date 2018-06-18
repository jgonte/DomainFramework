using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// The use of a repository of this type is in case when an entity has a collection
    /// of value types that need to be inserted belonging to that entity (identifying relationship) 
    /// and in case of updating the entity will those value types, they will need to be removed 
    /// (therefore we need DeleteAll or DeleteAllAsync) and inserted after (Insert or InsertAsync)
    /// there are no updates for those items
    /// </summary>
    /// <typeparam name="IValueObject"></typeparam>
    public interface ICommandValueObjectRepository : ICommandRepository
    {
        void Insert(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        bool DeleteAll(IAuthenticatedUser user, IUnitOfWork unitOfWork);

        Task InsertAsync(IValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        Task<bool> DeleteAllAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork);
    }
}

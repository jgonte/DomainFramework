using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a write only repository
    /// </summary>
    public interface ICommandRepository : IRepository
    {
        void Save(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        void Insert(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        bool Update(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        bool Delete(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        Task SaveAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        Task InsertAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        Task<bool> UpdateAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        Task<bool> DeleteAsync(IEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        /// <summary>
        /// The entities whose ids are required by the linked and join entities at the time of persistance
        /// </summary>
        Func<IEnumerable<IEntity>> TransferEntities { get; set; }
    }
}
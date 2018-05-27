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
        void Save(IEntity entity, IUnitOfWork unitOfWork = null);

        void Insert(IEntity entity, IUnitOfWork unitOfWork = null);

        bool Update(IEntity entity, IUnitOfWork unitOfWork = null);

        bool Delete(IEntity entity, IUnitOfWork unitOfWork = null);

        Task SaveAsync(IEntity entity, IUnitOfWork unitOfWork = null);

        Task InsertAsync(IEntity entity, IUnitOfWork unitOfWork = null);

        Task<bool> UpdateAsync(IEntity entity, IUnitOfWork unitOfWork = null);

        Task<bool> DeleteAsync(IEntity entity, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// The entities whise ids are required by the linked and join entities at the time of persistance
        /// </summary>
        Func<IEnumerable<IEntity>> TransferEntities { get; set; }
    }
}
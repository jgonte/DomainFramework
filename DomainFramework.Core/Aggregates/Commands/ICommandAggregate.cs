using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface ICommandAggregate : IAggregate
    {
        void Save(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);

        void Delete(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);

        Task SaveAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);

        Task DeleteAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);
    }

    public interface ICommandAggregate<TEntity> : IAggregate<TEntity>, ICommandAggregate
        where TEntity : IEntity
    {
        Queue<ITransactedOperation> TransactedSaveOperations { get; set; }

        Queue<ITransactedOperation> TransactedDeleteOperations { get; set; }
    }
}
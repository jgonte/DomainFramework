using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface ICommandAggregate : IAggregate
    {
        /// <summary>
        /// Initializes the aggregate for the given input DTO
        /// </summary>
        /// <param name="inputDto"></param>
        void Initialize(IInputDataTransferObject inputDto);

        void Save(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);

        Task SaveAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);
    }

    public interface ICommandAggregate<TEntity> : IAggregate<TEntity>, ICommandAggregate
    {
    }
}

using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface ICommandAggregateCollection<TAggregate, TEntity> : IAggregateCollection<TAggregate>
        where TAggregate : IAggregate<TEntity>
    {
        void Save(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);

        Task SaveAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);

        void Delete(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);

        Task DeleteAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);
    }
}
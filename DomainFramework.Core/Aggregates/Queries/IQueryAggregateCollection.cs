using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQueryAggregateCollection<TAggregate, TEntity> : IAggregateCollection<TAggregate>
        where TAggregate : IAggregate<TEntity>
    {
        /// <summary>
        /// Loads a collection of aggregates based on the parameters provided
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <param name="user"></param>
        /// <param name="unitOfWork"></param>
        void Load(QueryParameters queryParameters, IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);

        Task LoadAsync(QueryParameters queryParameters, IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null);
    }
}

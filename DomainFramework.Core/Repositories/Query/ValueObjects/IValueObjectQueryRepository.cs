using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository
    /// </summary>
    public interface IValueObjectQueryRepository : IQueryRepository
    {
        /// <summary>
        /// Retrieves the value objects for the given ownerId that also statisfy the conditions set by the queryParameters
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        (int, IEnumerable<IValueObject>) Get(object ownerId, CollectionQueryParameters queryParameters);

        /// <summary>
        /// Asynchronously retrieves the value objects for the given ownerId that also statisfy the conditions set by the queryParameters
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        Task<(int, IEnumerable<IValueObject>)> GetAsync(object ownerId, CollectionQueryParameters queryParameters);

        /// <summary>
        /// Retrieves all the values for the given ownerId
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        IEnumerable<IValueObject> GetAll(object ownerId);

        /// <summary>
        /// Asynchronously retrieves all the value objects for the given ownerId
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<IEnumerable<IValueObject>> GetAllAsync(object ownerId);
    }
}
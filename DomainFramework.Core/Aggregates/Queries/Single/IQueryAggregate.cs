using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQueryAggregate<TEntity, TDto> : IAggregate<TEntity>
    {
        /// <summary>
        /// Loads the aggregated entities of the root one but assumes that the root one has been already loaded
        /// </summary>
        /// <param name="user"></param>
        void LoadLinks(IAuthenticatedUser user = null);

        Task LoadLinksAsync(IAuthenticatedUser user = null);

        /// <summary>
        /// Retrieves the output data transfer object from the aggregate
        /// </summary>
        /// <returns></returns>
        TDto GetDataTransferObject();
    }
}

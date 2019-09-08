using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQueryAggregate<TEntity, TOuputDto> : IAggregate<TEntity>
        where TEntity : IEntity
        where TOuputDto : IOutputDataTransferObject, new()
    {
        /// <summary>
        /// The output DTO of the aggregate
        /// </summary>
        TOuputDto OutputDto { get; set; }

        /// <summary>
        /// Loads the aggregated entities of the root one but assumes that the root one has been already loaded
        /// </summary>
        /// <param name="user"></param>
        void LoadLinks(IAuthenticatedUser user = null);

        Task LoadLinksAsync(IAuthenticatedUser user = null);

        /// <summary>
        /// Populates the output data transfer object from the aggregate
        /// </summary>
        /// <returns></returns>
        void PopulateDto(TEntity entity);
    }
}

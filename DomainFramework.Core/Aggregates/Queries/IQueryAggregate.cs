using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQueryAggregate<TKey, TEntity> : IAggregate<TEntity>
    {
        /// <summary>
        /// Loads this aggregate by the id of its root entity
        /// </summary>
        /// <param name="rootEntityId"></param>
        /// <param name="user"></param>
        void Load(TKey rootEntityId, IAuthenticatedUser user = null);

        Task LoadAsync(TKey rootEntityId, IAuthenticatedUser user = null);

        /// <summary>
        /// Loads the aggregated entities of the root one but assumes that the root one has been already loaded
        /// </summary>
        /// <param name="user"></param>
        void LoadLinks(IAuthenticatedUser user = null);

        Task LoadLinksAsync(IAuthenticatedUser user = null);
    }
}

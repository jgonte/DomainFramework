using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class PageEntity : ContainerEntity<int?, PageData>
    {
        // Foreing references are added to the entity
        /// <summary>
        /// The id of the owner book
        /// </summary>
        public int BookId { get; internal set; }

        public PageEntity()
        {
        }

        internal PageEntity(PageData data, int? id = null) : base(data, id)
        {
        }
    }
}

using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class PageEntity : Entity<int?, Page>
    {
        // Foreing references are added to the entity
        /// <summary>
        /// The id of the owner book
        /// </summary>
        public int BookId { get; internal set; }

        public PageEntity()
        {
        }

        internal PageEntity(Page data, int? id = null) : base(data, id)
        {
        }
    }
}

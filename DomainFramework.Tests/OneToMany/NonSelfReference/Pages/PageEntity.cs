using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class PageEntity : Entity<int?>
    {
        public int Index { get; set; }

        // Foreing references are added to the entity
        /// <summary>
        /// The id of the owner book
        /// </summary>
        public int BookId { get; internal set; }
    }
}

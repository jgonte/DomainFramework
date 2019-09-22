using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class BookEntity : Entity<int?>
    {
        public string Title { get; set; }
    }
}

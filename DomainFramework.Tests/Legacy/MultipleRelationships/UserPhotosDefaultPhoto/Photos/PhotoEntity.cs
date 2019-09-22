using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class PhotoEntity : Entity<int?>
    {
        public string Description { get; set; }

        public int UserId { get; set; }
    }
}
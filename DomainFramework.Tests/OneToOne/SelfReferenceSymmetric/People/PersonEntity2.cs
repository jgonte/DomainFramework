using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class PersonEntity2 : Entity<int?>
    {
        public string FirstName { get; set; }

        public int? SpouseId { get; set; }
    }
}

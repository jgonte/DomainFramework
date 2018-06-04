using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class PersonEntity3 : Entity<int?>
    {
        public string FirstName { get; set; }

        public int? ManagerId { get; set; }
    }
}
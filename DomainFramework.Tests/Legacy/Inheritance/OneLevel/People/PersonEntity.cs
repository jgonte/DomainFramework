using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class PersonEntity : Entity<int?>
    {
        public string FirstName { get; set; }
    }
}

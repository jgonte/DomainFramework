using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class CountryEntity : Entity<string>
    {
        public string Name { get; set; }
    }
}
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class CapitalCityEntity : Entity<int?>
    {
        public string CountryCode { get; set; }

        public string Name { get; set; }
    }
}
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class CountryEntity : ContainerEntity<string, CountryData>
    {
        public CountryEntity()
        {
        }

        internal CountryEntity(CountryData data, string id = null) : base(data, id)
        {
        }
    }
}
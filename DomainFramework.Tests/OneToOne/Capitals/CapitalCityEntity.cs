using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class CapitalCityEntity : ContainerEntity<int?, CapitalCityData>
    {
        public string CountryCode { get; set; }

        public CapitalCityEntity()
        {
        }

        internal CapitalCityEntity(CapitalCityData data, int? id = null) : base(data, id)
        {
        }
    }
}
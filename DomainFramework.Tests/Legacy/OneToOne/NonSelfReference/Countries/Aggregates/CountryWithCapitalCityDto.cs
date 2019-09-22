namespace DomainFramework.Tests
{
    public class CountryWithCapitalCityDto
    {
        public string CountryCode { get; internal set; }

        public string Name { get; internal set; }

        public CapitalCityDto CapitalCity { get; internal set; }
    }
}
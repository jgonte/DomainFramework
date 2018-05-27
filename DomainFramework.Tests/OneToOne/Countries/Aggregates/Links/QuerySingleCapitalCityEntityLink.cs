using DomainFramework.Core;

namespace DomainFramework.Tests.OneToOne
{
    public class QuerySingleCapitalCityEntityLink : QuerySingleEntityLink<CountryEntity, CapitalCityEntity>
    {
        public override void PopulateEntity(IQueryRepository repository, CountryEntity entity)
        {
            LinkedEntity = ((CapitalCityQueryRepository)repository).GetForCountry(entity.Id);
        }
    }
}
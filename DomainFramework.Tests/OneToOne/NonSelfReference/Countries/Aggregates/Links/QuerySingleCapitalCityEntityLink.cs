using System.Threading.Tasks;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class QuerySingleCapitalCityEntityLink : QuerySingleEntityLink<CountryEntity, CapitalCityEntity>
    {
        public override void PopulateEntity(IRepositoryContext repositoryContext, CountryEntity entity)
        {
            var repository = (CapitalCityQueryRepository)repositoryContext.GetQueryRepository(typeof(CapitalCityEntity));

            LinkedEntity = repository.GetForCountry(entity.Id);
        }

        public override Task PopulateEntityAsync(IRepositoryContext repositoryContext, CountryEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
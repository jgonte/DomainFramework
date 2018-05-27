using DataAccess;

namespace DomainFramework.Tests.OneToOne
{
    class CapitalCityQueryRepository : Core.QueryRepository<CapitalCityEntity, int?>
    {
        public override CapitalCityEntity GetById(int? id)
        {
            var result = Query<CapitalCityEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_CapitalCity_Get")
                .Parameters(
                    p => p.Name("capitalCityId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = id.Value;

                    var capitalCity = new CapitalCityData
                    {
                        Name = reader.GetString(0)
                    };

                    entity.Data = capitalCity;

                    entity.CountryCode = reader.GetString(1);
                })
                .Execute();

            return result.Data;
        }

        public CapitalCityEntity GetForCountry(string countryCode)
        {
            var result = Query<CapitalCityEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Country_GetCapitalCity")
                .Parameters(
                    p => p.Name("countryCode").Value(countryCode)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = reader.GetInt32(0);

                    var capitalCity = new CapitalCityData
                    {
                        Name = reader.GetString(1)
                    };

                    entity.Data = capitalCity;

                    entity.CountryCode = reader.GetString(2);
                })
                .Execute();

            return result.Data;
        }
    }
}
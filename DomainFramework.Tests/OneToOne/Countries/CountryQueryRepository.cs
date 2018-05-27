using DataAccess;

namespace DomainFramework.Tests
{
    class CountryQueryRepository : Core.QueryRepository<CountryEntity, string>
    {
        public override CountryEntity GetById(string id)
        {
            var result = Query<CountryEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Country_Get")
                .Parameters(
                    p => p.Name("countryCode").Value(id)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = id;

                    var country = new CountryData
                    {
                        Name = reader.GetString(0)
                    };

                    entity.Data = country;
                })
                .Execute();

            return result.Data;
        }
    }
}
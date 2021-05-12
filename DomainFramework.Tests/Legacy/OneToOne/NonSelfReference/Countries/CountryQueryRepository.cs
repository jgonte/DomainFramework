using DataAccess;
using DomainFramework.Core;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Tests
{
    class CountryQueryRepository : EntityQueryRepository<CountryEntity, string>
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

                    entity.Name = reader.GetString(0);
                })
                .Execute();

            return result.Record;
        }

        public override async Task<CountryEntity> GetByIdAsync(string id)
        {
            var result = await Query<CountryEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Country_Get")
                .Parameters(
                    p => p.Name("countryCode").Value(id)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = id;

                    entity.Name = reader.GetString(0);
                })
                .ExecuteAsync();

            return result.Record;
        }
    }
}
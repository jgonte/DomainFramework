using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class CapitalCityQueryRepository : Core.EntityQueryRepository<CapitalCityEntity, int?>
    {
        public override (int, IEnumerable<CapitalCityEntity>) Get(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<(int, IEnumerable<CapitalCityEntity>)> GetAsync(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

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

                    entity.Name = reader.GetString(0);

                    entity.CountryCode = reader.GetString(1);
                })
                .Execute();

            return result.Record;
        }

        public override async Task<CapitalCityEntity> GetByIdAsync(int? id)
        {
            var result = await Query<CapitalCityEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_CapitalCity_Get")
                .Parameters(
                    p => p.Name("capitalCityId").Value(id.Value)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = id.Value;

                    entity.Name = reader.GetString(0);

                    entity.CountryCode = reader.GetString(1);
                })
                .ExecuteAsync();

            return result.Record;
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

                    entity.Name = reader.GetString(1);

                    entity.CountryCode = reader.GetString(2);
                })
                .Execute();

            return result.Record;
        }

        public async Task<CapitalCityEntity> GetForCountryAsync(string countryCode)
        {
            var result = await Query<CapitalCityEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Country_GetCapitalCity")
                .Parameters(
                    p => p.Name("countryCode").Value(countryCode)
                )
                .OnRecordRead((reader, entity) =>
                {
                    entity.Id = reader.GetInt32(0);

                    entity.Name = reader.GetString(1);

                    entity.CountryCode = reader.GetString(2);
                })
                .ExecuteAsync();

            return result.Record;
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class CapitalCityQueryRepository : Core.EntityQueryRepository<CapitalCityEntity, int?>
    {
        public override IEnumerable<CapitalCityEntity> Get(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<CapitalCityEntity>> GetAsync(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override CapitalCityEntity GetById(int? id, IAuthenticatedUser user)
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

            return result.Data;
        }

        public override async Task<CapitalCityEntity> GetByIdAsync(int? id, IAuthenticatedUser user)
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

            return result.Data;
        }

        public CapitalCityEntity GetForCountry(string countryCode, IAuthenticatedUser user)
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

            return result.Data;
        }

        public async Task<CapitalCityEntity> GetForCountryAsync(string countryCode, IAuthenticatedUser user)
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

            return result.Data;
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class CountryQueryRepository : Core.QueryRepository<CountryEntity, string>
    {
        public override IEnumerable<IEntity> Get(QueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

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

                    var country = new CountryData
                    {
                        Name = reader.GetString(0)
                    };

                    entity.Data = country;
                })
                .ExecuteAsync();

            return result.Data;
        }
    }
}
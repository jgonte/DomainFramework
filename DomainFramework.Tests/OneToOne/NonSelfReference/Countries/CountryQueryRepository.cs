﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class CountryQueryRepository : Core.EntityQueryRepository<CountryEntity, string>
    {
        public override IEnumerable<CountryEntity> Get(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<CountryEntity>> GetAsync(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override CountryEntity GetById(string id, IAuthenticatedUser user)
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

        public override async Task<CountryEntity> GetByIdAsync(string id, IAuthenticatedUser user)
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
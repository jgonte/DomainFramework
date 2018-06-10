﻿using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonQueryRepository4 : Core.QueryRepository<PersonEntity3, int?>
    {
        public override IEnumerable<IEntity> Get(QueryParameters parameters)
        {
            var result = Query<PersonEntity3>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetAll")
                .Execute();

            return result.Data;
        }

        public override async Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters)
        {
            var result = await Query<PersonEntity3>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetAll")
                .ExecuteAsync();

            return result.Data;
        }

        public async Task<IEnumerable<PersonEntity3>> GetForManagerAsync(int? id)
        {
            var result = await Query<PersonEntity3>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetEmployees")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override PersonEntity3 GetById(int? id)
        {
            var result = Query<PersonEntity3>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .Execute();

            return result.Data;
        }

        public override async Task<PersonEntity3> GetByIdAsync(int? id)
        {
            var result = await Query<PersonEntity3>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }
    }
}
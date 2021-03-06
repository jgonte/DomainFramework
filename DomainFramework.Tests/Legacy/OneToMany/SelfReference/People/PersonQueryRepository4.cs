﻿using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Tests
{
    class PersonQueryRepository4 : Core.EntityQueryRepository<PersonEntity3, int?>
    {
        public override (int, IEnumerable<PersonEntity3>) Get(CollectionQueryParameters parameters)
        {
            var result = Query<PersonEntity3>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetAll")
                .Execute();

            return (result.Count, result.Records);
        }

        public override async Task<(int, IEnumerable<PersonEntity3>)> GetAsync(CollectionQueryParameters parameters)
        {
            var result = await Query<PersonEntity3>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetAll")
                .ExecuteAsync();

            return (result.Count, result.Records);
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

            return result.Records;
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

            return result.Record;
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

            return result.Record;
        }
    }
}
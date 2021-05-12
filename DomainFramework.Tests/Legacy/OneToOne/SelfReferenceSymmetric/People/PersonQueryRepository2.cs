using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Tests
{
    class PersonQueryRepository2 : EntityQueryRepository<PersonEntity2, int?>
    {
        public override (int, IEnumerable<PersonEntity2>) Get(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<(int, IEnumerable<PersonEntity2>)> GetAsync(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override PersonEntity2 GetById(int? id)
        {
            var result = Query<PersonEntity2>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .Execute();

            return result.Record;
        }

        public override async Task<PersonEntity2> GetByIdAsync(int? id)
        {
            var result = await Query<PersonEntity2>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public PersonEntity2 GetForPerson(int? id)
        {
            var result = Query<PersonEntity2>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetSpouse")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .Execute();

            return result.Record;
        }

        public async Task<PersonEntity2> GetForPersonAsync(int? id)
        {
            var result = await Query<PersonEntity2>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetSpouse")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Record;
        }
    }
}
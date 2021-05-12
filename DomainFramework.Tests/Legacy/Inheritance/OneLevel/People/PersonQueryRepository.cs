using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Tests
{
    class PersonQueryRepository : EntityQueryRepository<PersonEntity, int?>
    {
        public override (int, IEnumerable<PersonEntity>) Get(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<(int, IEnumerable<PersonEntity>)> GetAsync(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override PersonEntity GetById(int? id)
        {
            var result = Query<PersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .Execute();

            return result.Record;
        }

        public override async Task<PersonEntity> GetByIdAsync(int? id)
        {
            var result = await Query<PersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public PersonEntity GetForPerson(int? id)
        {
            var result = Query<PersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetSpouse")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .Execute();

            return result.Record;
        }
    }
}
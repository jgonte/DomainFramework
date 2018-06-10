using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonQueryRepository2 : QueryRepository<PersonEntity2, int?>
    {
        public override IEnumerable<IEntity> Get(QueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters)
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

            return result.Data;
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

            return result.Data;
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

            return result.Data;
        }
    }
}
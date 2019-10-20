using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonQueryRepository2 : EntityQueryRepository<PersonEntity2, int?>
    {
        public override IEnumerable<PersonEntity2> Get(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<PersonEntity2>> GetAsync(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override PersonEntity2 GetById(int? id, IAuthenticatedUser user)
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

        public override async Task<PersonEntity2> GetByIdAsync(int? id, IAuthenticatedUser user)
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

        public PersonEntity2 GetForPerson(int? id, IAuthenticatedUser user)
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

        public async Task<PersonEntity2> GetForPersonAsync(int? id, IAuthenticatedUser user)
        {
            var result = await Query<PersonEntity2>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetSpouse")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }
    }
}
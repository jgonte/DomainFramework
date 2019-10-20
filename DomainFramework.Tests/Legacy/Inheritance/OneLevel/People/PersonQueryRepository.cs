using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonQueryRepository : EntityQueryRepository<PersonEntity, int?>
    {
        public override IEnumerable<PersonEntity> Get(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<PersonEntity>> GetAsync(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override PersonEntity GetById(int? id, IAuthenticatedUser user)
        {
            var result = Query<PersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .Execute();

            return result.Data;
        }

        public override async Task<PersonEntity> GetByIdAsync(int? id, IAuthenticatedUser user)
        {
            var result = await Query<PersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public PersonEntity GetForPerson(int? id, IAuthenticatedUser user)
        {
            var result = Query<PersonEntity>
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
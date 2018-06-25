using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonQueryRepository5 : EntityQueryRepository<PersonEntity4, int?>
    {
        public override IEnumerable<IEntity> Get(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override PersonEntity4 GetById(int? id, IAuthenticatedUser user)
        {
            var result = Query<PersonEntity4>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .Execute();

            return result.Data;
        }

        public override async Task<PersonEntity4> GetByIdAsync(int? id, IAuthenticatedUser user)
        {
            var result = await Query<PersonEntity4>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Get")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Phone> GetPhones(int? id, IAuthenticatedUser user)
        {
            var result = Query<Phone>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetPhones")
                .Parameters(
                    p => p.Name("personId").Value(id.Value)
                )
                .Execute();

            return result.Data;
        }
    }
}
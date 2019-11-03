using DataAccess;
using DomainFramework.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonQueryRepository4 : Core.EntityQueryRepository<PersonEntity3, int?>
    {
        public override (int, IEnumerable<PersonEntity3>) Get(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            var result = Query<PersonEntity3>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetAll")
                .Execute();

            return (result.Data.Count, result.Data);
        }

        public override async Task<(int, IEnumerable<PersonEntity3>)> GetAsync(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            var result = await Query<PersonEntity3>
                .Collection()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_GetAll")
                .ExecuteAsync();

            return (result.Data.Count, result.Data);
        }

        public async Task<IEnumerable<PersonEntity3>> GetForManagerAsync(int? id, IAuthenticatedUser user)
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

        public override PersonEntity3 GetById(int? id, IAuthenticatedUser user)
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

        public override async Task<PersonEntity3> GetByIdAsync(int? id, IAuthenticatedUser user)
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
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class ExecutiveQueryRepository : EntityQueryRepository<ExecutiveEntity, int?>
    {
        public override IEnumerable<ExecutiveEntity> Get(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<ExecutiveEntity>> GetAsync(CollectionQueryParameters parameters, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        public override ExecutiveEntity GetById(int? id, IAuthenticatedUser user)
        {
            var result = Query<ExecutiveEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Executive_Get")
                .Parameters(
                    p => p.Name("executiveId").Value(id.Value)
                )
                .Execute();

            return result.Data;
        }

        public override async Task<ExecutiveEntity> GetByIdAsync(int? id, IAuthenticatedUser user)
        {
            var result = await Query<ExecutiveEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Executive_Get")
                .Parameters(
                    p => p.Name("executiveId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }
    }
}
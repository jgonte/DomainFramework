using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class ExecutiveQueryRepository : EntityQueryRepository<ExecutiveEntity, int?>
    {
        public override (int, IEnumerable<ExecutiveEntity>) Get(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<(int, IEnumerable<ExecutiveEntity>)> GetAsync(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override ExecutiveEntity GetById(int? id)
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

        public override async Task<ExecutiveEntity> GetByIdAsync(int? id)
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
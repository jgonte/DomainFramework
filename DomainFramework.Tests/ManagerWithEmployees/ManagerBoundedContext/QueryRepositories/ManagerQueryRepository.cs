using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class ManagerQueryRepository : EntityQueryRepository<Manager, int?>
    {
        public override Manager GetById(int? managerId, IAuthenticatedUser user)
        {
            var result = Query<Manager>
                .Single()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_GetById]")
                .Parameters(
                    p => p.Name("managerId").Value(managerId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Manager> GetByIdAsync(int? managerId, IAuthenticatedUser user)
        {
            var result = await Query<Manager>
                .Single()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_GetById]")
                .Parameters(
                    p => p.Name("managerId").Value(managerId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override (int, IEnumerable<Manager>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Manager>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Manager>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Manager>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Manager>(new ManagerQueryRepository());
        }

    }
}
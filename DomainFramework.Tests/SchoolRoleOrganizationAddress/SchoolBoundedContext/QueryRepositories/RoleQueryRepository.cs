using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class RoleQueryRepository : EntityQueryRepository<Role, int?>
    {
        public override Role GetById(int? roleId, IAuthenticatedUser user)
        {
            var result = Query<Role>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_GetById]")
                .Parameters(
                    p => p.Name("roleId").Value(roleId.Value)
                )
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Role> GetByIdAsync(int? roleId, IAuthenticatedUser user)
        {
            var result = await Query<Role>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_GetById]")
                .Parameters(
                    p => p.Name("roleId").Value(roleId.Value)
                )
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Role> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Role>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_Get]")
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Role>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Role>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_Get]")
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Role>(new RoleQueryRepository());
        }

    }
}
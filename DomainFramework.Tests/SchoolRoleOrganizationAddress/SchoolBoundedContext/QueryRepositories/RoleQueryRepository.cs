using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class RoleQueryRepository : EntityQueryRepository<Role, int?>
    {
        public override (int, IEnumerable<Role>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Role>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Role>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Role>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<Role> GetAll()
        {
            var result = Query<Role>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_GetAll]")
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var result = await Query<Role>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_GetAll]")
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override Role GetById(int? roleId)
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

        public async override Task<Role> GetByIdAsync(int? roleId)
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

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Role>(new RoleQueryRepository());
        }

    }
}
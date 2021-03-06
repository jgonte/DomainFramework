using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class RoleQueryRepository : EntityQueryRepository<Role, int>
    {
        public override (int, IEnumerable<Role>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Role>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Role>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Role>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Role> GetAll()
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

            return result.Records;
        }

        public async override Task<IEnumerable<Role>> GetAllAsync()
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

            return result.Records;
        }

        public override Role GetById(int roleId)
        {
            var result = Query<Role>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_GetById]")
                .Parameters(
                    p => p.Name("roleId").Value(roleId)
                )
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Role> GetByIdAsync(int roleId)
        {
            var result = await Query<Role>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pRole_GetById]")
                .Parameters(
                    p => p.Name("roleId").Value(roleId)
                )
                .MapTypes(
                    2,
                    tm => tm.Type(typeof(School)).Index(1),
                    tm => tm.Type(typeof(Role)).Index(2)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Role>(new RoleQueryRepository());
        }

    }
}
using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class OrganizationRoleQueryRepository : EntityQueryRepository<OrganizationRole, OrganizationRoleId>
    {
        public override (int, IEnumerable<OrganizationRole>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<OrganizationRole>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganizationRole_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<OrganizationRole>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<OrganizationRole>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganizationRole_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<OrganizationRole> GetAll()
        {
            var result = Query<OrganizationRole>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganizationRole_GetAll]")
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<OrganizationRole>> GetAllAsync()
        {
            var result = await Query<OrganizationRole>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganizationRole_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override OrganizationRole GetById(OrganizationRoleId organizationRoleId)
        {
            var result = Query<OrganizationRole>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganizationRole_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationRoleId.OrganizationId.Value),
                    p => p.Name("roleId").Value(organizationRoleId.RoleId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<OrganizationRole> GetByIdAsync(OrganizationRoleId organizationRoleId)
        {
            var result = await Query<OrganizationRole>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganizationRole_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationRoleId.OrganizationId.Value),
                    p => p.Name("roleId").Value(organizationRoleId.RoleId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<OrganizationRole>(new OrganizationRoleQueryRepository());
        }

    }
}
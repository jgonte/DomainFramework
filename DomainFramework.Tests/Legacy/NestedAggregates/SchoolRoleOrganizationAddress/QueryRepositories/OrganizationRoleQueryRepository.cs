using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class OrganizationRoleQueryRepository : EntityQueryRepository<OrganizationRole, OrganizationRoleId>
    {
        public override OrganizationRole GetById(OrganizationRoleId organizationRoleId, IAuthenticatedUser user)
        {
            var result = Query<OrganizationRole>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganizationRole_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationRoleId.OrganizationId.Value),
                    p => p.Name("roleId").Value(organizationRoleId.RoleId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<OrganizationRole> GetByIdAsync(OrganizationRoleId organizationRoleId, IAuthenticatedUser user)
        {
            var result = await Query<OrganizationRole>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganizationRole_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationRoleId.OrganizationId.Value),
                    p => p.Name("roleId").Value(organizationRoleId.RoleId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<OrganizationRole> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<OrganizationRole>
                .Collection()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganizationRole_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<OrganizationRole>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<OrganizationRole>
                .Collection()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganizationRole_Get]")
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<OrganizationRole>(new OrganizationRoleQueryRepository());
        }

    }
}

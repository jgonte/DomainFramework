using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class OrganizationQueryRepository : EntityQueryRepository<Organization, int?>
    {
        public override Organization GetById(int? organizationId, IAuthenticatedUser user)
        {
            var result = Query<Organization>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganization_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Organization> GetByIdAsync(int? organizationId, IAuthenticatedUser user)
        {
            var result = await Query<Organization>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganization_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Organization> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Organization>
                .Collection()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganization_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Organization>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Organization>
                .Collection()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganization_Get]")
                .ExecuteAsync();

            return result.Data;
        }

        public Organization GetOrganizationForOrganizationRole(int? roleId, IAuthenticatedUser user)
        {
            var result = Query<Organization>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganization_GetOrganization_ForOrganizationRole]")
                .Parameters(
                    p => p.Name("roleId").Value(roleId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Organization> GetOrganizationForOrganizationRoleAsync(int? roleId, IAuthenticatedUser user)
        {
            var result = await Query<Organization>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganization_GetOrganization_ForOrganizationRole]")
                .Parameters(
                    p => p.Name("roleId").Value(roleId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Organization>(new OrganizationQueryRepository());
        }

    }
}

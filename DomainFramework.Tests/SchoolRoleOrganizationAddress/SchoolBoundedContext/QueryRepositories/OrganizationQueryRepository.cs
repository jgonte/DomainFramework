using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class OrganizationQueryRepository : EntityQueryRepository<Organization, int?>
    {
        public override (int, IEnumerable<Organization>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Organization>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Organization>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Organization>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Organization> GetAll()
        {
            var result = Query<Organization>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Organization>> GetAllAsync()
        {
            var result = await Query<Organization>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Organization GetById(int? organizationId)
        {
            var result = Query<Organization>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Organization> GetByIdAsync(int? organizationId)
        {
            var result = await Query<Organization>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Organization> GetAllOrganizationForRole(int? roleId)
        {
            var result = Query<Organization>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pGetAll_Organization_For_Role]")
                .Parameters(
                    p => p.Name("roleId").Value(roleId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationForRoleAsync(int? roleId)
        {
            var result = await Query<Organization>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pGetAll_Organization_For_Role]")
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
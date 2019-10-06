using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class AddressQueryRepository : EntityQueryRepository<Address, int?>
    {
        public override Address GetById(int? addressId, IAuthenticatedUser user)
        {
            var result = Query<Address>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_GetById]")
                .Parameters(
                    p => p.Name("addressId").Value(addressId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Address> GetByIdAsync(int? addressId, IAuthenticatedUser user)
        {
            var result = await Query<Address>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_GetById]")
                .Parameters(
                    p => p.Name("addressId").Value(addressId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Address> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Address>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Address>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Address>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_Get]")
                .ExecuteAsync();

            return result.Data;
        }

        public Address GetAddressForOrganization(int? organizationId, IAuthenticatedUser user)
        {
            var result = Query<Address>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_GetAddress_ForOrganization]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Address> GetAddressForOrganizationAsync(int? organizationId, IAuthenticatedUser user)
        {
            var result = await Query<Address>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_GetAddress_ForOrganization]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Address>(new AddressQueryRepository());
        }

    }
}
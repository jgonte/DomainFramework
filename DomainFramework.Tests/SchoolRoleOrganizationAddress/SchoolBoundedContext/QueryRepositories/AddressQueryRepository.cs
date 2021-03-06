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
    public class AddressQueryRepository : EntityQueryRepository<Address, int>
    {
        public override (int, IEnumerable<Address>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Address>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Address>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Address>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Address> GetAll()
        {
            var result = Query<Address>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Address>> GetAllAsync()
        {
            var result = await Query<Address>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Address GetById(int addressId)
        {
            var result = Query<Address>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_GetById]")
                .Parameters(
                    p => p.Name("addressId").Value(addressId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Address> GetByIdAsync(int addressId)
        {
            var result = await Query<Address>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_GetById]")
                .Parameters(
                    p => p.Name("addressId").Value(addressId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public Address GetAddressForOrganization(int organizationId)
        {
            var result = Query<Address>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_GetAddress]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .Execute();

            return result.Record;
        }

        public async Task<Address> GetAddressForOrganizationAsync(int organizationId)
        {
            var result = await Query<Address>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_GetAddress]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Address>(new AddressQueryRepository());
        }

    }
}
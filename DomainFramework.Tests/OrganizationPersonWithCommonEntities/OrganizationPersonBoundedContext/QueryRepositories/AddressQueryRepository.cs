using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class AddressQueryRepository : EntityQueryRepository<Address, int>
    {
        public override (int, IEnumerable<Address>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Address>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pAddress_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Address>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Address>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pAddress_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Address> GetAll()
        {
            var result = Query<Address>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pAddress_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Address>> GetAllAsync()
        {
            var result = await Query<Address>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pAddress_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Address GetById(int addressId)
        {
            var result = Query<Address>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pAddress_GetById]")
                .Parameters(
                    p => p.Name("addressId").Value(addressId)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Address> GetByIdAsync(int addressId)
        {
            var result = await Query<Address>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pAddress_GetById]")
                .Parameters(
                    p => p.Name("addressId").Value(addressId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public Address GetAddressForOrganization(int organizationId)
        {
            var result = Query<Address>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetAddress]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Address> GetAddressForOrganizationAsync(int organizationId)
        {
            var result = await Query<Address>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetAddress]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public Address GetAddressForPerson(int personId)
        {
            var result = Query<Address>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_GetAddress]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Address> GetAddressForPersonAsync(int personId)
        {
            var result = await Query<Address>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_GetAddress]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
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
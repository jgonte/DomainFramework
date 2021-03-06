using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class OrganizationQueryRepository : EntityQueryRepository<Organization, int>
    {
        public override (int, IEnumerable<Organization>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Organization>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Organization>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Organization>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Organization> GetAll()
        {
            var result = Query<Organization>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Organization>> GetAllAsync()
        {
            var result = await Query<Organization>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Organization GetById(int organizationId)
        {
            var result = Query<Organization>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Organization> GetByIdAsync(int organizationId)
        {
            var result = await Query<Organization>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetById]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Organization>(new OrganizationQueryRepository());
        }

    }
}
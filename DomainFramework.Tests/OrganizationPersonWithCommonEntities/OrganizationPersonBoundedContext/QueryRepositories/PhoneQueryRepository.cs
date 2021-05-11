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
    public class PhoneQueryRepository : EntityQueryRepository<Phone, int>
    {
        public override (int, IEnumerable<Phone>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPhone_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Phone>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPhone_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Phone> GetAll()
        {
            var result = Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPhone_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Phone>> GetAllAsync()
        {
            var result = await Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPhone_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Phone GetById(int phoneId)
        {
            var result = Query<Phone>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPhone_GetById]")
                .Parameters(
                    p => p.Name("phoneId").Value(phoneId)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Phone> GetByIdAsync(int phoneId)
        {
            var result = await Query<Phone>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPhone_GetById]")
                .Parameters(
                    p => p.Name("phoneId").Value(phoneId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Phone> GetAllPhonesForOrganization(int organizationId)
        {
            var result = Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetAllPhones]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Phone>> GetAllPhonesForOrganizationAsync(int organizationId)
        {
            var result = await Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetAllPhones]")
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Phone> GetAllPhonesForPerson(int personId)
        {
            var result = Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_GetAllPhones]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Phone>> GetAllPhonesForPersonAsync(int personId)
        {
            var result = await Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_GetAllPhones]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public (int, IEnumerable<Phone>) GetPhonesForOrganization(int organizationId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetPhones]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async Task<(int, IEnumerable<Phone>)> GetPhonesForOrganizationAsync(int organizationId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pOrganization_GetPhones]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("organizationId").Value(organizationId)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public (int, IEnumerable<Phone>) GetPhonesForPerson(int personId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_GetPhones]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async Task<(int, IEnumerable<Phone>)> GetPhonesForPersonAsync(int personId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Phone>
                .Collection()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_GetPhones]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Phone>(new PhoneQueryRepository());
        }

    }
}
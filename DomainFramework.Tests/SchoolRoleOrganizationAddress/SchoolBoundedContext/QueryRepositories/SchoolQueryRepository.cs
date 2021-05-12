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
    public class SchoolQueryRepository : EntityQueryRepository<School, int>
    {
        public override (int, IEnumerable<School>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<School>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<School> GetAll()
        {
            var result = Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<School>> GetAllAsync()
        {
            var result = await Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override School GetById(int schoolId)
        {
            var result = Query<School>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_GetById]")
                .Parameters(
                    p => p.Name("schoolId").Value(schoolId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<School> GetByIdAsync(int schoolId)
        {
            var result = await Query<School>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_GetById]")
                .Parameters(
                    p => p.Name("schoolId").Value(schoolId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<School>(new SchoolQueryRepository());
        }

    }
}
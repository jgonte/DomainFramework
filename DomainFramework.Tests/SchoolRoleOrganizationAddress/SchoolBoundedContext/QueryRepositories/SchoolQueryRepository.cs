using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class SchoolQueryRepository : EntityQueryRepository<School, int?>
    {
        public override School GetById(int? schoolId, IAuthenticatedUser user)
        {
            var result = Query<School>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_GetById]")
                .Parameters(
                    p => p.Name("schoolId").Value(schoolId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<School> GetByIdAsync(int? schoolId, IAuthenticatedUser user)
        {
            var result = await Query<School>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_GetById]")
                .Parameters(
                    p => p.Name("schoolId").Value(schoolId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<School> Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_Get]")
                .QueryParameters(queryParameters)
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<School>> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_Get]")
                .QueryParameters(queryParameters)
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<School>(new SchoolQueryRepository());
        }

    }
}
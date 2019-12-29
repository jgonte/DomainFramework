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
        public override (int, IEnumerable<School>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<School>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<School> GetAll()
        {
            var result = Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_GetAll]")
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<School>> GetAllAsync()
        {
            var result = await Query<School>
                .Collection()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pSchool_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override School GetById(int? schoolId)
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

        public async override Task<School> GetByIdAsync(int? schoolId)
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

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<School>(new SchoolQueryRepository());
        }

    }
}
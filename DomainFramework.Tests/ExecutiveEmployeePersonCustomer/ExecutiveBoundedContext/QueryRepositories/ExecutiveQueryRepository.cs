using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class ExecutiveQueryRepository : EntityQueryRepository<Executive, int?>
    {
        public override Executive GetById(int? executiveId, IAuthenticatedUser user)
        {
            var result = Query<Executive>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_GetById]")
                .Parameters(
                    p => p.Name("executiveId").Value(executiveId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Executive> GetByIdAsync(int? executiveId, IAuthenticatedUser user)
        {
            var result = await Query<Executive>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_GetById]")
                .Parameters(
                    p => p.Name("executiveId").Value(executiveId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override (int, IEnumerable<Executive>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Executive>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Executive>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Executive>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Executive>(new ExecutiveQueryRepository());
        }

    }
}
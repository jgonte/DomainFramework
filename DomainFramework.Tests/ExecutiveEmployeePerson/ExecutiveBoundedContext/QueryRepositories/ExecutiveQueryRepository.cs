using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class ExecutiveQueryRepository : EntityQueryRepository<Executive, int>
    {
        public override (int, IEnumerable<Executive>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Executive>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Executive>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Executive>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Executive> GetAll()
        {
            var result = Query<Executive>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Executive>> GetAllAsync()
        {
            var result = await Query<Executive>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Executive GetById(int executiveId)
        {
            var result = Query<Executive>
                .Single()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_GetById]")
                .Parameters(
                    p => p.Name("executiveId").Value(executiveId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Executive> GetByIdAsync(int executiveId)
        {
            var result = await Query<Executive>
                .Single()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_GetById]")
                .Parameters(
                    p => p.Name("executiveId").Value(executiveId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Executive>(new ExecutiveQueryRepository());
        }

    }
}
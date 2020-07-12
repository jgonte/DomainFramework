using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Executive>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Executive>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Executive> GetAll()
        {
            var result = Query<Executive>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Executive>> GetAllAsync()
        {
            var result = await Query<Executive>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_GetAll]")
                .ExecuteAsync();

            return result.Data;
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

            return result.Data;
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

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Executive>(new ExecutiveQueryRepository());
        }

    }
}
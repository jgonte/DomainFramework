using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class SimpleLogQueryRepository : EntityQueryRepository<SimpleLog, int>
    {
        public override (int, IEnumerable<SimpleLog>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<SimpleLog>
                .Collection()
                .Connection(SimpleLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[SimpleLogsBoundedContext].[pSimpleLog_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<SimpleLog>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<SimpleLog>
                .Collection()
                .Connection(SimpleLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[SimpleLogsBoundedContext].[pSimpleLog_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<SimpleLog> GetAll()
        {
            var result = Query<SimpleLog>
                .Collection()
                .Connection(SimpleLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[SimpleLogsBoundedContext].[pSimpleLog_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<SimpleLog>> GetAllAsync()
        {
            var result = await Query<SimpleLog>
                .Collection()
                .Connection(SimpleLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[SimpleLogsBoundedContext].[pSimpleLog_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override SimpleLog GetById(int simpleLogId)
        {
            var result = Query<SimpleLog>
                .Single()
                .Connection(SimpleLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[SimpleLogsBoundedContext].[pSimpleLog_GetById]")
                .Parameters(
                    p => p.Name("simpleLogId").Value(simpleLogId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<SimpleLog> GetByIdAsync(int simpleLogId)
        {
            var result = await Query<SimpleLog>
                .Single()
                .Connection(SimpleLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[SimpleLogsBoundedContext].[pSimpleLog_GetById]")
                .Parameters(
                    p => p.Name("simpleLogId").Value(simpleLogId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<SimpleLog>(new SimpleLogQueryRepository());
        }

    }
}
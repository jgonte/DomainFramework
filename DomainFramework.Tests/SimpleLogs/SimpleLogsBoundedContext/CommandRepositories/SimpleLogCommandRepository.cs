using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System.Threading.Tasks;
using Utilities;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class SimpleLogCommandRepository : EntityCommandRepository<SimpleLog>
    {
        protected override Command CreateInsertCommand(SimpleLog entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<SimpleLog>
                .Single()
                .Connection(SimpleLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[SimpleLogsBoundedContext].[pSimpleLog_Insert]")
                .Parameters(
                    p => p.Name("messageType").Value(entity.MessageType),
                    p => p.Name("message").Value(entity.Message)
                )
                .RecordInstance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0),
                    p => p.Name("When").Index(1)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<SimpleLog>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<SimpleLog>)command).ExecuteAsync();
        }

        protected override Command CreateDeleteCommand(SimpleLog entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "DeleteOlderLogs": return Command
                    .NonQuery()
                    .Connection(SimpleLogsConnectionClass.GetConnectionName())
                    .StoredProcedure("[SimpleLogsBoundedContext].[pSimpleLog_DeleteOlderLogs]")
                    .Parameters(
                        p => p.Name("when").Value(entity.When)
                    );

                default: return Command
                    .NonQuery()
                    .Connection(SimpleLogsConnectionClass.GetConnectionName())
                    .StoredProcedure("[SimpleLogsBoundedContext].[pSimpleLog_Delete]")
                    .Parameters(
                        p => p.Name("simpleLogId").Value(entity.Id)
                    );
            }
        }

        protected override bool HandleDelete(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class ExecutiveCommandRepository : EntityCommandRepository<Executive>
    {
        protected override Command CreateInsertCommand(Executive entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<Executive>
                .Single()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_Insert]")
                .Parameters(
                    p => p.Name("bonus").Value(entity.Bonus),
                    p => p.Name("hireDate").Value(entity.HireDate),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Executive>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Executive>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Executive entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_Update]")
                .Parameters(
                    p => p.Name("executiveId").Value(entity.Id),
                    p => p.Name("bonus").Value(entity.Bonus),
                    p => p.Name("hireDate").Value(entity.HireDate),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy)
                );
        }

        protected override bool HandleUpdate(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleUpdateAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

        protected override Command CreateDeleteCommand(Executive entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pExecutive_Delete]")
                .Parameters(
                    p => p.Name("executiveId").Value(entity.Id)
                );
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
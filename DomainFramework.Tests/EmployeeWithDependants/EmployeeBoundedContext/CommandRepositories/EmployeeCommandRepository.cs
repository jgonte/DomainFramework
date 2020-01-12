using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class EmployeeCommandRepository : EntityCommandRepository<Employee>
    {
        protected override Command CreateInsertCommand(Employee entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<Employee>
                .Single()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_Insert]")
                .Parameters(
                    p => p.Name("hireDate").Value(entity.HireDate),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy),
                    p => p.Name("cellPhone_AreaCode").Value(entity.CellPhone.AreaCode),
                    p => p.Name("cellPhone_Exchange").Value(entity.CellPhone.Exchange),
                    p => p.Name("cellPhone_Number").Value(entity.CellPhone.Number),
                    p => p.Name("providerEmployeeId").Value(entity.ProviderEmployeeId)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Employee>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Employee>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Employee entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_Update]")
                .Parameters(
                    p => p.Name("employeeId").Value(entity.Id),
                    p => p.Name("hireDate").Value(entity.HireDate),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("cellPhone_AreaCode").Value(entity.CellPhone.AreaCode),
                    p => p.Name("cellPhone_Exchange").Value(entity.CellPhone.Exchange),
                    p => p.Name("cellPhone_Number").Value(entity.CellPhone.Number),
                    p => p.Name("providerEmployeeId").Value(entity.ProviderEmployeeId)
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

        protected override Command CreateDeleteCommand(Employee entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_Delete]")
                .Parameters(
                    p => p.Name("employeeId").Value(entity.Id)
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

        protected override Command CreateDeleteCollectionCommand(Employee entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_DeleteDependants]")
                .Parameters(
                    p => p.Name("providerEmployeeId").Value(entity.Id)
                );
        }

        protected override bool HandleDeleteCollection(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteCollectionAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
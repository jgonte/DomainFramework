using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class EmployeeCommandRepository : EntityCommandRepository<Employee>
    {
        protected override Command CreateInsertCommand(Employee entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<Employee>
                .Single()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_Insert]")
                .Parameters(
                    p => p.Name("hireDate").Value(entity.HireDate),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy),
                    p => p.Name("cellPhone_AreaCode").Value(entity.CellPhone?.AreaCode),
                    p => p.Name("cellPhone_Exchange").Value(entity.CellPhone?.Exchange),
                    p => p.Name("cellPhone_Number").Value(entity.CellPhone?.Number)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var marriedToDependency = (Person)dependencies?.SingleOrDefault()?.Entity;

                    if (marriedToDependency != null)
                    {
                        entity.MarriedToPersonId = marriedToDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("marriedToPersonId").Value(entity.MarriedToPersonId)
                    );
                })
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

        protected override Command CreateUpdateCommand(Employee entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_Update]")
                .Parameters(
                    p => p.Name("hireDate").Value(entity.HireDate),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("cellPhone_AreaCode").Value(entity.CellPhone?.AreaCode),
                    p => p.Name("cellPhone_Exchange").Value(entity.CellPhone?.Exchange),
                    p => p.Name("cellPhone_Number").Value(entity.CellPhone?.Number)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    cmd.Parameters(
                        p => p.Name("employeeId").Value(entity.Id)
                    );

                    var entityForSpouse = (Person)dependencies.SingleOrDefault(d => d.Selector == "Spouse")?.Entity;

                    if (entityForSpouse != null)
                    {
                        entity.MarriedToPersonId = entityForSpouse.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("marriedToPersonId").Value(entity.MarriedToPersonId)
                    );
                });
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

        protected override Command CreateDeleteCommand(Employee entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
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

        protected override Command CreateDeleteLinksCommand(Employee entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pPerson_UnlinkSpouse]")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id)
                );
        }

        protected override bool HandleDeleteLinks(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteLinksAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
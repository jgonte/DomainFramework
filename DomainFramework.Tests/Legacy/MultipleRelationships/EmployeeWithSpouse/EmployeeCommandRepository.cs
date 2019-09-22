using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class EmployeeCommandRepository : EntityCommandRepository<Employee>
    {
        protected override Command CreateInsertCommand(Employee entity, IAuthenticatedUser user, string selector)
        {
            return Query<Employee>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pEmployee_Insert]")
                .Parameters(
                    p => p.Name("hireDate").Value(entity.HireDate),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy),
                    p => p.Name("cellPhone_AreaCode").Value(entity.CellPhone?.AreaCode),
                    p => p.Name("cellPhone_Exchange").Value(entity.CellPhone?.Exchange),
                    p => p.Name("cellPhone_Number").Value(entity.CellPhone?.Number),
                    p => p.Name("marriedToPersonId").Value(entity.MarriedToPersonId)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((NonQueryCommand)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((NonQueryCommand)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Employee entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pEmployee_Update]")
                .Parameters(
                    p => p.Name("hireDate").Value(entity.HireDate),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("lastUpdatedBy").Value(entity.LastUpdatedBy),
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
                        cmd.Parameters(
                            p => p.Name("marriedToPersonId").Value(entityForSpouse.Id)
                        );
                    }
                    else
                    {
                        cmd.Parameters(
                            p => p.Name("marriedToPersonId").Value(entity.MarriedToPersonId)
                        );
                    }
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

        protected override Command CreateDeleteCommand(Employee entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pEmployee_Delete]")
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
            switch (selector)
            {
                case "Spouse": return Command
                    .NonQuery()
                    .Connection(TestConnectionClass.GetConnectionName())
                    .StoredProcedure("[pPerson_DeleteSpouse]")
                    .Parameters(
                        p => p.Name("marriedToPersonId").Value(entity.Id)
                    );

                default: throw new InvalidOperationException();
            }
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

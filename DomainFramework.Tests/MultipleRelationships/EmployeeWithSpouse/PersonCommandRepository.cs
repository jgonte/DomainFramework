using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class PersonCommandRepository : EntityCommandRepository<Person>
    {
        protected override Command CreateInsertCommand(Person entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            return Query<Person>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_Insert]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy),
                    p => p.Name("cellPhone_AreaCode").Value(entity.CellPhone?.AreaCode),
                    p => p.Name("cellPhone_Exchange").Value(entity.CellPhone?.Exchange),
                    p => p.Name("cellPhone_Number").Value(entity.CellPhone?.Number)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

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
                })
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Person>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Person>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Person entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.LastUpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_Update]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("lastUpdatedBy").Value(entity.LastUpdatedBy),
                    p => p.Name("cellPhone_AreaCode").Value(entity.CellPhone.AreaCode),
                    p => p.Name("cellPhone_Exchange").Value(entity.CellPhone.Exchange),
                    p => p.Name("cellPhone_Number").Value(entity.CellPhone.Number)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    cmd.Parameters(
                        p => p.Name("personId").Value(entity.Id)
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

        protected override Command CreateDeleteCommand(Person entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_Delete]")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id)
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

        protected override Command CreateDeleteCollectionCommand(Person entity, IAuthenticatedUser user, string selector)
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

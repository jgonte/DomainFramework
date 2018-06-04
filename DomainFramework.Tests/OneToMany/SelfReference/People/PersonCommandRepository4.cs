using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonCommandRepository4 : DataAccess.CommandRepository<PersonEntity3, int?>
    {
        protected override Command CreateInsertCommand(PersonEntity3 entity, IAuthenticatedUser user)
        {
            Expression<Func<PersonEntity3, object>>[] excludedProperties;

            if (TransferEntities != null)
            {
                excludedProperties = new Expression<Func<PersonEntity3, object>>[]{
                    m => m.Id,
                    m => m.ManagerId
                };
            }
            else
            {
                excludedProperties = new Expression<Func<PersonEntity3, object>>[]{
                    m => m.Id
                };
            }

            var command = Query<PersonEntity3>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: excludedProperties
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map(m => m.Id)//.Index(0),
                );

            command.OnBeforeCommandExecuted(() =>
            {
                if (TransferEntities != null)
                {
                    var e = (PersonEntity3)TransferEntities().Single();

                    entity.ManagerId = e.Id.Value;

                    command.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("ManagerId").Value(entity.ManagerId)
                    );
                }
            });

            return command;
        }

        protected override Command CreateUpdateCommand(PersonEntity3 entity, IAuthenticatedUser user)
        {
            Expression<Func<PersonEntity3, object>>[] excludedProperties;

            if (TransferEntities != null)
            {
                excludedProperties = new Expression<Func<PersonEntity3, object>>[]{
                    m => m.Id,
                    m => m.ManagerId
                };
            }
            else
            {
                excludedProperties = new Expression<Func<PersonEntity3, object>>[]{
                    m => m.Id
                };
            }

            var command = Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Update");

            if (entity.Id.HasValue)
            {
                command.Parameters(
                    p => p.Name("personId").Value(entity.Id.Value)
                );
            }
            else
            {
                command.OnBeforeCommandExecuted(() =>
                {
                    command.Parameters(
                        p => p.Name("personId").Value(entity.Id.Value)
                    );
                });
            }

            command.AutoGenerateParameters(
                qbeObject: entity,
                excludedProperties: excludedProperties
            );

            command.OnBeforeCommandExecuted(() =>
            {
                if (TransferEntities != null)
                {
                    var e = (PersonEntity3)TransferEntities().Single();

                    entity.ManagerId = e.Id.Value;

                    command.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("ManagerId").Value(entity.ManagerId)
                    );
                }
            });

            return command;
        }

        protected override Command CreateDeleteCommand(PersonEntity3 entity, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleDelete(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleUpdate(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
}
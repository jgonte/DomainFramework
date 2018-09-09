using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonCommandRepository3 : DataAccess.EntityCommandRepository<PersonEntity>
    {
        protected override Command CreateInsertCommand(PersonEntity entity, IAuthenticatedUser user)
        {
            return Query<PersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<PersonEntity, object>>[]{
                        m => m.Id
                    }
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<PersonEntity>(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(PersonEntity entity, IAuthenticatedUser user)
        {
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
                command.OnBeforeCommandExecuted(cmd =>
                {
                    command.Parameters(
                        p => p.Name("personId").Value(entity.Id.Value)
                    );
                });
            }

            command.AutoGenerateParameters(
                qbeObject: entity,
                excludedProperties: new Expression<Func<PersonEntity, object>>[]{
                    m => m.Id,
                }
            );

            return command;
        }

        protected override Command CreateDeleteCommand(PersonEntity entity, IAuthenticatedUser user)
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
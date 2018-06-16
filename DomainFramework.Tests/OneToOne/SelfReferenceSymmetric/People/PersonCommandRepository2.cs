using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonCommandRepository2 : DataAccess.CommandEntityRepository<PersonEntity2>
    {
        protected override Command CreateInsertCommand(PersonEntity2 entity, IAuthenticatedUser user)
        {
            Expression<Func<PersonEntity2, object>>[] excludedProperties;

            if (TransferEntities != null)
            {
                excludedProperties = new Expression<Func<PersonEntity2, object>>[]{
                    m => m.Id,
                    m => m.SpouseId
                };
            }
            else
            {
                excludedProperties = new Expression<Func<PersonEntity2, object>>[]{
                    m => m.Id
                };
            }

            var command = Query<PersonEntity2>
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
                if (TransferEntities != null) // It is a self reference for Spouse
                {
                    var e = (PersonEntity2)TransferEntities().Single();

                    entity.SpouseId = e.Id;

                    command.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("SpouseId").Value(entity.SpouseId)
                    );
                }
            });

            command.OnAfterCommandExecuted(() =>  // Handle symmetric relationship
            {
                if (TransferEntities != null) // It is a self reference for Spouse
                {
                    var e = (PersonEntity2)TransferEntities().Single();

                    e.SpouseId = entity.Id; // Update the parent entity with the id of the created child one
                }
            });

            return command;
        }

        protected override Command CreateUpdateCommand(PersonEntity2 entity, IAuthenticatedUser user)
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
                command.OnBeforeCommandExecuted(() =>
                {
                    command.Parameters(
                        p => p.Name("personId").Value(entity.Id.Value)
                    );
                });
            }

            command.AutoGenerateParameters(
                qbeObject: entity,
                excludedProperties: new Expression<Func<PersonEntity2, object>>[]{
                    m => m.Id,
                }
            );

            return command;
        }

        protected override Command CreateDeleteCommand(PersonEntity2 entity, IAuthenticatedUser user)
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
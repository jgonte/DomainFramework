using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonCommandRepository2 : DataAccess.EntityCommandRepository<PersonEntity2>
    {
        protected override Command CreateInsertCommand(PersonEntity2 entity, IAuthenticatedUser user)
        {
            Expression<Func<PersonEntity2, object>>[] excludedProperties;

            if (Dependencies().Any())
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
                    pm => pm.Map<PersonEntity2>(m => m.Id)//.Index(0),
                );

            command.OnBeforeCommandExecuted(cmd =>
            {
                if (Dependencies().Any()) // It is a self reference for Spouse
                {
                    var e = (PersonEntity2)Dependencies().Single().Entity;

                    entity.SpouseId = e.Id;

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("SpouseId").Value(entity.SpouseId)
                    );
                }
            });

            command.OnAfterCommandExecuted(cmd =>  // Handle symmetric relationship
            {
                if (Dependencies().Any()) // It is a self reference for Spouse
                {
                    var e = (PersonEntity2)Dependencies().Single().Entity;

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
                command.OnBeforeCommandExecuted(cmd =>
                {
                    cmd.Parameters(
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
    }
}
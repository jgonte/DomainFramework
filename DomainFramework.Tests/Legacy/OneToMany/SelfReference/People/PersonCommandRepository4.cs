using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonCommandRepository4 : DataAccess.EntityCommandRepository<PersonEntity3>
    {
        protected override Command CreateInsertCommand(PersonEntity3 entity, IAuthenticatedUser user, string selector)
        {
            Expression<Func<PersonEntity3, object>>[] excludedProperties;

            if (Dependencies().Any())
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
                    pm => pm.Map<PersonEntity3>(m => m.Id)//.Index(0),
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    if (Dependencies().Any())
                    {
                        var e = (PersonEntity3)Dependencies().Single().Entity;

                        entity.ManagerId = e.Id.Value;

                        cmd.Parameters( // Map the extra parameters for the foreign key(s)
                            p => p.Name("ManagerId").Value(entity.ManagerId)
                        );
                    }
                });

            return command;
        }

        protected override Command CreateUpdateCommand(PersonEntity3 entity, IAuthenticatedUser user)
        {
            Expression<Func<PersonEntity3, object>>[] excludedProperties;

            if (Dependencies().Any())
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
                command.OnBeforeCommandExecuted(cmd =>
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

            command.OnBeforeCommandExecuted(cmd =>
            {
                if (Dependencies().Any())
                {
                    var e = (PersonEntity3)Dependencies().Single().Entity;

                    entity.ManagerId = e.Id.Value;

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("ManagerId").Value(entity.ManagerId)
                    );
                }
            });

            return command;
        }
    }
}
﻿using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Tests
{
    class EmployeeCommandRepository : DataAccess.EntityCommandRepository<EmployeeEntity>
    {
        protected override Command CreateInsertCommand(EmployeeEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Employee_Create")
                .RecordInstance(entity)
                .AutoGenerateParameters(
                    excludedProperties: new Expression<Func<EmployeeEntity, object>>[]{
                        m => m.Id
                    }
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var entities = Dependencies();

                    var personEntity = (PersonEntity)entities.Single().Entity;

                    entity.Id = personEntity.Id;

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("EmployeeId").Value(entity.Id)
                    );
                });
        }

        protected override Command CreateUpdateCommand(EmployeeEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Employee_Update")
                .Parameters(
                    p => p.Name("employeeId").Value(entity.Id.Value)
                )
                .RecordInstance(entity)
                .AutoGenerateParameters(
                    excludedProperties: new Expression<Func<EmployeeEntity, object>>[]{
                        m => m.Id
                    }
                );
        }

        protected override Command CreateDeleteCommand(EmployeeEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Employee_Delete")
                .Parameters(
                    p => p.Name("employeeId").Value(entity.Id.Value)
                );
        }

        protected override bool HandleDelete(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected override async Task<bool> HandleDeleteAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
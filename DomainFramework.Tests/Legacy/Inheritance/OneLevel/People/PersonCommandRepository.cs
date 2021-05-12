﻿using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Tests
{
    class PersonCommandRepository : DataAccess.EntityCommandRepository<PersonEntity>
    {
        protected override Command CreateInsertCommand(PersonEntity entity, IAuthenticatedUser user, string selector)
        {
            return Query<PersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Create")
                .Record(entity)
                .AutoGenerateParameters(
                    excludedProperties: new Expression<Func<PersonEntity, object>>[]{
                        m => m.Id,
                    }
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<PersonEntity>(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(PersonEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Update")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id.Value)
                )
                .Record(entity)
                .AutoGenerateParameters(
                    excludedProperties: new Expression<Func<PersonEntity, object>>[]{
                        m => m.Id,
                    }
                );
        }
    }
}
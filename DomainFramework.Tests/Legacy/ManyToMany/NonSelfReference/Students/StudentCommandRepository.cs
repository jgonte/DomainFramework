﻿using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class StudentCommandRepository : DataAccess.EntityCommandRepository<StudentEntity>
    {
        protected override Command CreateInsertCommand(StudentEntity entity, IAuthenticatedUser user, string selector)
        {
            return Query<StudentEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Student_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<StudentEntity, object>>[]{
                        m => m.Id,
                    }
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<StudentEntity>(m => m.Id)//.Index(0),
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<StudentEntity>)command).Execute();
        }
    }
}
﻿using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class ClassEnrollmentCommandRepository : DataAccess.EntityCommandRepository<ClassEnrollmentEntity>
    {
        protected override Command CreateInsertCommand(ClassEnrollmentEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_ClassEnrollment_Create")
                .AutoGenerateParameters(
                    qbeObject: entity
                )
                .OnBeforeCommandExecuted(cmd =>
            {
                var entities = TransferEntities();

                var classEntity = (ClassEntity)entities.ElementAt(0);

                var studentEntity = (StudentEntity)entities.ElementAt(1);

                entity.Id = new ClassEnrollmentEntityId
                {
                    ClassId = classEntity.Id.Value,
                    StudentId = studentEntity.Id.Value
                };

                cmd.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("ClassId").Value(classEntity.Id),
                    p => p.Name("StudentId").Value(studentEntity.Id)
                );
            });
        }

        protected override Command CreateUpdateCommand(ClassEnrollmentEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected override Command CreateDeleteCommand(ClassEnrollmentEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleDelete(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleUpdate(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new NotImplementedException();
        }
    }
}

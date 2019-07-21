using DataAccess;
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
                if (!Dependencies().Any())
                {
                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("ClassId").Value(entity.Id.ClassId),
                        p => p.Name("StudentId").Value(entity.Id.StudentId)
                    );
                }
                else
                {
                    var entities = Dependencies();

                    var classEntity = (ClassEntity)entities.ElementAt(0).Entity;

                    var studentEntity = (StudentEntity)entities.ElementAt(1).Entity;

                    entity.Id = new ClassEnrollmentEntityId
                    {
                        ClassId = classEntity.Id.Value,
                        StudentId = studentEntity.Id.Value
                    };

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("ClassId").Value(classEntity.Id),
                        p => p.Name("StudentId").Value(studentEntity.Id)
                    );
                }
            });
        }

        protected override void HandleInsert(Command command)
        {
            ((NonQueryCommand)command).Execute();
        }

        protected override Command CreateDeleteCollectionCommand(ClassEnrollmentEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Class_RemoveStudents")
                .Parameters(
                    p => p.Name("classId").Value(entity.Id.ClassId)
                );
        }

        protected override bool HandleDeleteCollection(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }
    }
}

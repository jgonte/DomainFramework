using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassEnrollmentCommandRepository : EntityCommandRepository<ClassEnrollment>
    {
        protected override Command CreateInsertCommand(ClassEnrollment entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Command
                .NonQuery()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClassEnrollment_Insert]")
                .Parameters(
                    p => p.Name("startedDateTime").Value(entity.StartedDateTime),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    if (!dependencies.Any())
                    {
                        cmd.Parameters(
                            p => p.Name("classId").Value(entity.Id.ClassId),
                            p => p.Name("studentId").Value(entity.Id.StudentId)
                        );
                    }
                    else
                    {
                        switch (selector)
                        {
                            case "Classes":
                                {
                                    var student = (Student)dependencies.ElementAt(0).Entity;

                                    cmd.Parameters(
                                        p => p.Name("classId").Value(entity.Id.ClassId),
                                        p => p.Name("studentId").Value(student.Id)
                                    );
                                }
                                break;

                            case "Students":
                                {
                                    var @class = (Class)dependencies.ElementAt(0).Entity;

                                    cmd.Parameters(
                                        p => p.Name("classId").Value(@class.Id),
                                        p => p.Name("studentId").Value(entity.Id.StudentId)
                                    );
                                }
                                break;

                            default:
                                {
                                    var @class = (Class)dependencies.Single(d => d.Selector == "Classes").Entity;

                                    var student = (Student)dependencies.Single(d => d.Selector == "Students").Entity;

                                    entity.Id = new ClassEnrollmentId
                                    {
                                        ClassId = @class.Id.Value,
                                        StudentId = student.Id.Value
                                    };

                                    cmd.Parameters(
                                        p => p.Name("classId").Value(@class.Id),
                                        p => p.Name("studentId").Value(student.Id)
                                    );
                                }
                                break;
                        }
                    }
                });

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((NonQueryCommand)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((NonQueryCommand)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(ClassEnrollment entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClassEnrollment_Update]")
                .Parameters(
                    p => p.Name("classId").Value(entity.Id.ClassId),
                    p => p.Name("studentId").Value(entity.Id.StudentId),
                    p => p.Name("startedDateTime").Value(entity.StartedDateTime),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy)
                );
        }

        protected override bool HandleUpdate(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleUpdateAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

        protected override Command CreateDeleteCommand(ClassEnrollment entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClassEnrollment_Delete]")
                .Parameters(
                    p => p.Name("classId").Value(entity.Id.ClassId),
                    p => p.Name("studentId").Value(entity.Id.StudentId)
                );
        }

        protected override bool HandleDelete(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
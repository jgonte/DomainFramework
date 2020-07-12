using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWithPreRequisites.CourseBoundedContext
{
    public class PreRequisiteCommandRepository : EntityCommandRepository<PreRequisite>
    {
        protected override Command CreateInsertCommand(PreRequisite entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Command
                .NonQuery()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pPreRequisite_Insert]")
                .Parameters(
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    if (!dependencies.Any())
                    {
                        cmd.Parameters(
                            p => p.Name("requiredCourseId").Value(entity.Id.RequiredCourseId),
                            p => p.Name("courseId").Value(entity.Id.CourseId)
                        );
                    }
                    else
                    {
                        switch (selector)
                        {
                            case "Requires":
                                {
                                    var course = (Course)dependencies.ElementAt(0).Entity;

                                    cmd.Parameters(
                                        p => p.Name("requiredCourseId").Value(course.Id),
                                        p => p.Name("courseId").Value(course.Id)
                                    );
                                }
                                break;

                            case "IsRequiredBy":
                                {
                                    var course = (Course)dependencies.ElementAt(0).Entity;

                                    cmd.Parameters(
                                        p => p.Name("requiredCourseId").Value(course.Id),
                                        p => p.Name("courseId").Value(course.Id)
                                    );
                                }
                                break;

                            default:
                                {
                                    var requiresCourse = (Course)dependencies.Single(d => d.Selector == "Requires").Entity;

                                    var isRequiredByCourse = (Course)dependencies.Single(d => d.Selector == "IsRequiredBy").Entity;

                                    entity.Id = new PreRequisiteId
                                    {
                                        RequiredCourseId = requiresCourse.Id,
                                        CourseId = isRequiredByCourse.Id
                                    };

                                    cmd.Parameters(
                                        p => p.Name("requiredCourseId").Value(requiresCourse.Id),
                                        p => p.Name("courseId").Value(isRequiredByCourse.Id)
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

        protected override Command CreateUpdateCommand(PreRequisite entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pPreRequisite_Update]")
                .Parameters(
                    p => p.Name("requiredCourseId").Value(entity.Id.RequiredCourseId),
                    p => p.Name("courseId").Value(entity.Id.CourseId),
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

        protected override Command CreateDeleteCommand(PreRequisite entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(CourseWithPreRequisitesConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pPreRequisite_Delete]")
                .Parameters(
                    p => p.Name("requiredCourseId").Value(entity.Id.RequiredCourseId),
                    p => p.Name("courseId").Value(entity.Id.CourseId)
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
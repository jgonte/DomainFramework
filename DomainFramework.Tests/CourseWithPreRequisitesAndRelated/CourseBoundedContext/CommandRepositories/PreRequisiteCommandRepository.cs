using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
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
                .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
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
                            p => p.Name("requires_CourseId").Value(entity.Id.Requires_CourseId),
                            p => p.Name("isRequiredBy_CourseId").Value(entity.Id.IsRequiredBy_CourseId)
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
                                        p => p.Name("requires_CourseId").Value(course.Id),
                                        p => p.Name("isRequiredBy_CourseId").Value(course.Id)
                                    );
                                }
                                break;

                            case "IsRequiredBy":
                                {
                                    var course = (Course)dependencies.ElementAt(0).Entity;

                                    cmd.Parameters(
                                        p => p.Name("requires_CourseId").Value(course.Id),
                                        p => p.Name("isRequiredBy_CourseId").Value(course.Id)
                                    );
                                }
                                break;

                            default:
                                {
                                    var requiresCourse = (Course)dependencies.Single(d => d.Selector == "Requires").Entity;

                                    var isRequiredByCourse = (Course)dependencies.Single(d => d.Selector == "IsRequiredBy").Entity;

                                    entity.Id = new PreRequisiteId
                                    {
                                        Requires_CourseId = requiresCourse.Id.Value,
                                        IsRequiredBy_CourseId = isRequiredByCourse.Id.Value
                                    };

                                    cmd.Parameters(
                                        p => p.Name("requires_CourseId").Value(requiresCourse.Id),
                                        p => p.Name("isRequiredBy_CourseId").Value(isRequiredByCourse.Id)
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
                .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pPreRequisite_Update]")
                .Parameters(
                    p => p.Name("requires_CourseId").Value(entity.Id.Requires_CourseId),
                    p => p.Name("isRequiredBy_CourseId").Value(entity.Id.IsRequiredBy_CourseId),
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
                .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pPreRequisite_Delete]")
                .Parameters(
                    p => p.Name("requires_CourseId").Value(entity.Id.Requires_CourseId),
                    p => p.Name("isRequiredBy_CourseId").Value(entity.Id.IsRequiredBy_CourseId)
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
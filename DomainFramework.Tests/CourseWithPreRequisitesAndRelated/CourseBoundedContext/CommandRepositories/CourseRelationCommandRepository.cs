using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    public class CourseRelationCommandRepository : EntityCommandRepository<CourseRelation>
    {
        protected override Command CreateInsertCommand(CourseRelation entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Command
                .NonQuery()
                .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourseRelation_Insert]")
                .Parameters(
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    if (!dependencies.Any())
                    {
                        cmd.Parameters(
                            p => p.Name("relates_CourseId").Value(entity.Id.Relates_CourseId),
                            p => p.Name("isRelatedTo_CourseId").Value(entity.Id.IsRelatedTo_CourseId)
                        );
                    }
                    else
                    {
                        switch (selector)
                        {
                            case "Relates":
                                {
                                    var course = (Course)dependencies.ElementAt(0).Entity;

                                    cmd.Parameters(
                                        p => p.Name("relates_CourseId").Value(course.Id),
                                        p => p.Name("isRelatedTo_CourseId").Value(course.Id)
                                    );
                                }
                                break;

                            case "IsRelatedTo":
                                {
                                    var course = (Course)dependencies.ElementAt(0).Entity;

                                    cmd.Parameters(
                                        p => p.Name("relates_CourseId").Value(course.Id),
                                        p => p.Name("isRelatedTo_CourseId").Value(course.Id)
                                    );
                                }
                                break;

                            default:
                                {
                                    var relatesCourse = (Course)dependencies.Single(d => d.Selector == "Relates").Entity;

                                    var isRelatedToCourse = (Course)dependencies.Single(d => d.Selector == "IsRelatedTo").Entity;

                                    entity.Id = new CourseRelationId
                                    {
                                        Relates_CourseId = relatesCourse.Id,
                                        IsRelatedTo_CourseId = isRelatedToCourse.Id
                                    };

                                    cmd.Parameters(
                                        p => p.Name("relates_CourseId").Value(relatesCourse.Id),
                                        p => p.Name("isRelatedTo_CourseId").Value(isRelatedToCourse.Id)
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

        protected override Command CreateUpdateCommand(CourseRelation entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourseRelation_Update]")
                .Parameters(
                    p => p.Name("relates_CourseId").Value(entity.Id.Relates_CourseId),
                    p => p.Name("isRelatedTo_CourseId").Value(entity.Id.IsRelatedTo_CourseId),
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

        protected override Command CreateDeleteCommand(CourseRelation entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourseRelation_Delete]")
                .Parameters(
                    p => p.Name("relates_CourseId").Value(entity.Id.Relates_CourseId),
                    p => p.Name("isRelatedTo_CourseId").Value(entity.Id.IsRelatedTo_CourseId)
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
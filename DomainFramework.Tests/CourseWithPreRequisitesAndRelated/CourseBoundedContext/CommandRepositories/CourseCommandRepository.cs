using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    public class CourseCommandRepository : EntityCommandRepository<Course>
    {
        protected override Command CreateInsertCommand(Course entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Course>
                .Single()
                .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_Insert]")
                .Parameters(
                    p => p.Name("description").Value(entity.Description),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .RecordInstance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Course>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Course>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Course entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_Update]")
                .Parameters(
                    p => p.Name("courseId").Value(entity.Id),
                    p => p.Name("description").Value(entity.Description),
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

        protected override Command CreateDeleteCommand(Course entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                .StoredProcedure("[CourseBoundedContext].[pCourse_Delete]")
                .Parameters(
                    p => p.Name("courseId").Value(entity.Id)
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

        protected override Command CreateDeleteLinksCommand(Course entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "UnlinkIsRelatedToFromCourse": return Command
                    .NonQuery()
                    .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                    .StoredProcedure("[CourseBoundedContext].[pCourse_UnlinkIsRelatedTo]")
                    .Parameters(
                        p => p.Name("courseId").Value(entity.Id)
                    );

                case "UnlinkIsRequiredByFromCourse": return Command
                    .NonQuery()
                    .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                    .StoredProcedure("[CourseBoundedContext].[pCourse_UnlinkIsRequiredBy]")
                    .Parameters(
                        p => p.Name("courseId").Value(entity.Id)
                    );

                case "UnlinkRelatesFromCourse": return Command
                    .NonQuery()
                    .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                    .StoredProcedure("[CourseBoundedContext].[pCourse_UnlinkRelates]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("courseId").Value(entity.Id)
                    );

                case "UnlinkRequiresFromCourse": return Command
                    .NonQuery()
                    .Connection(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName())
                    .StoredProcedure("[CourseBoundedContext].[pCourse_UnlinkRequires]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("courseId").Value(entity.Id)
                    );

                default: throw new InvalidOperationException();
            }
        }

        protected override bool HandleDeleteLinks(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteLinksAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
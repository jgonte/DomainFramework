using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassCommandRepository : EntityCommandRepository<Class>
    {
        protected override Command CreateInsertCommand(Class entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Class>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_Insert]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Class>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Class>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Class entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_Update]")
                .Parameters(
                    p => p.Name("classId").Value(entity.Id),
                    p => p.Name("name").Value(entity.Name),
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

        protected override Command CreateDeleteCommand(Class entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_Delete]")
                .Parameters(
                    p => p.Name("classId").Value(entity.Id)
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

        protected override Command CreateDeleteCollectionCommand(Class entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_DeleteStudents]")
                .Parameters(
                    p => p.Name("classId").Value(entity.Id)
                );
        }

        protected override bool HandleDeleteCollection(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteCollectionAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}
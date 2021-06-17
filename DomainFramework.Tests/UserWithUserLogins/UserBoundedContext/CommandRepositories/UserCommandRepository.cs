using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace UserWithUserLogins.UserBoundedContext
{
    public class UserCommandRepository : EntityCommandRepository<User>
    {
        protected override Command CreateInsertCommand(User entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<User>
                .Single()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_Insert]")
                .Parameters(
                    p => p.Name("userName").Value(entity.UserName),
                    p => p.Name("normalizedUserName").Value(entity.NormalizedUserName),
                    p => p.Name("email").Value(entity.Email),
                    p => p.Name("normalizedEmail").Value(entity.NormalizedEmail),
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
            ((SingleQuery<User>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<User>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(User entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_Update]")
                .Parameters(
                    p => p.Name("userId").Value(entity.Id),
                    p => p.Name("userName").Value(entity.UserName),
                    p => p.Name("normalizedUserName").Value(entity.NormalizedUserName),
                    p => p.Name("email").Value(entity.Email),
                    p => p.Name("normalizedEmail").Value(entity.NormalizedEmail),
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

        protected override Command CreateDeleteCommand(User entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_Delete]")
                .Parameters(
                    p => p.Name("userId").Value(entity.Id)
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

        protected override Command CreateDeleteLinksCommand(User entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_DeleteUserLogins]")
                .Parameters(
                    p => p.Name("userId").Value(entity.Id)
                );
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
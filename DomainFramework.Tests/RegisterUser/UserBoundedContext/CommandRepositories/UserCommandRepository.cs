using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterUser.UserBoundedContext
{
    public class UserCommandRepository : EntityCommandRepository<User>
    {
        protected override Command CreateInsertCommand(User entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<User>
                .Single()
                .Connection(RegisterUserConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_Insert]")
                .Parameters(
                    p => p.Name("username").Value(entity.Username),
                    p => p.Name("passwordSalt").Value(entity.PasswordSalt),
                    p => p.Name("passwordHash").Value(entity.PasswordHash),
                    p => p.Name("email").Value(entity.Email)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0),
                    p => p.Name("SubjectId").Index(1)
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

        protected override Command CreateDeleteCommand(User entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(RegisterUserConnectionClass.GetConnectionName())
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

    }
}
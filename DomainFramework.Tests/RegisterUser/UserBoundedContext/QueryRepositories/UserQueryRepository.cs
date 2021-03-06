using DataAccess;
using DomainFramework.Core;
using System.Threading.Tasks;
using Utilities;

namespace RegisterUser.UserBoundedContext
{
    public class UserQueryRepository : EntityQueryRepository<User, int>
    {
        public User GetUserByUserName(string username)
        {
            var result = Query<User>
                .Single()
                .Connection(RegisterUserConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetUserByUserName]")
                .Parameters(
                    p => p.Name("username").Value(username)
                )
                .Execute();

            return result.Record;
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            var result = await Query<User>
                .Single()
                .Connection(RegisterUserConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetUserByUserName]")
                .Parameters(
                    p => p.Name("username").Value(username)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<User>(new UserQueryRepository());
        }

    }
}
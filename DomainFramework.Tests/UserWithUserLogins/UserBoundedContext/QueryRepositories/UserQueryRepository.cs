using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserWithUserLogins.UserBoundedContext
{
    public class UserQueryRepository : EntityQueryRepository<User, int?>
    {
        public override (int, IEnumerable<User>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<User>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<User>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<User>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<User> GetAll()
        {
            var result = Query<User>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<User>> GetAllAsync()
        {
            var result = await Query<User>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override User GetById(int? userId)
        {
            var result = Query<User>
                .Single()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetById]")
                .Parameters(
                    p => p.Name("userId").Value(userId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<User> GetByIdAsync(int? userId)
        {
            var result = await Query<User>
                .Single()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetById]")
                .Parameters(
                    p => p.Name("userId").Value(userId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public User GetUserByNormalizedEmail(string email)
        {
            var result = Query<User>
                .Single()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetByNormalizedEmail]")
                .Parameters(
                    p => p.Name("email").Value(email)
                )
                .Execute();

            return result.Data;
        }

        public async Task<User> GetUserByNormalizedEmailAsync(string email)
        {
            var result = await Query<User>
                .Single()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetByNormalizedEmail]")
                .Parameters(
                    p => p.Name("email").Value(email)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public User GetUserByUserLogin(string provider, string userKey)
        {
            var result = Query<User>
                .Single()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetByUserLogin]")
                .Parameters(
                    p => p.Name("provider").Value(provider),
                    p => p.Name("userKey").Value(userKey)
                )
                .Execute();

            return result.Data;
        }

        public async Task<User> GetUserByUserLoginAsync(string provider, string userKey)
        {
            var result = await Query<User>
                .Single()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetByUserLogin]")
                .Parameters(
                    p => p.Name("provider").Value(provider),
                    p => p.Name("userKey").Value(userKey)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<User>(new UserQueryRepository());
        }

    }
}
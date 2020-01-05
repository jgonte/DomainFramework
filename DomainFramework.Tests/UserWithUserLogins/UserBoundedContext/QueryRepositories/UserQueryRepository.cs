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
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (CollectionQuery<User>)cmd;

                    foreach (var entity in query.Data)
                    {
                    }
                })
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
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (CollectionQuery<User>)cmd;

                    foreach (var entity in query.Data)
                    {
                    }
                })
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<User> GetAll()
        {
            var result = Query<User>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetAll]")
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (CollectionQuery<User>)cmd;

                    foreach (var entity in query.Data)
                    {
                    }
                })
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var result = await Query<User>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetAll]")
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (CollectionQuery<User>)cmd;

                    foreach (var entity in query.Data)
                    {
                    }
                })
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
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (SingleQuery<User>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }
                })
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
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (SingleQuery<User>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }
                })
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
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (SingleQuery<User>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }
                })
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
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (SingleQuery<User>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }
                })
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
                .OnAfterCommandExecuted(cmd =>
                {
                    var query = (SingleQuery<User>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }
                })
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
                .OnAfterCommandExecutedAsync(async cmd =>
                {
                    var query = (SingleQuery<User>)cmd;

                    var entity = query.Data;

                    if (entity == null)
                    {
                        return;
                    }
                })
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<User>(new UserQueryRepository());
        }

    }
}
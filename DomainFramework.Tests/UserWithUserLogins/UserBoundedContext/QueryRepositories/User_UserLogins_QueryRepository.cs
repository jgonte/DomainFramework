using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserWithUserLogins.UserBoundedContext
{
    public class User_UserLogins_QueryRepository : ValueObjectQueryRepository<int?, UserLogin>
    {
        public override (int, IEnumerable<UserLogin>) Get(int? userId, CollectionQueryParameters queryParameters)
        {
            var result = Query<UserLogin>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetUserLogins]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("userId").Value(userId.Value)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<UserLogin>)> GetAsync(int? userId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<UserLogin>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetUserLogins]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("userId").Value(userId.Value)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<UserLogin> GetAll(int? userId)
        {
            var result = Query<UserLogin>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetAllUserLogins]")
                .Parameters(
                    p => p.Name("userId").Value(userId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<UserLogin>> GetAllAsync(int? userId)
        {
            var result = await Query<UserLogin>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pUser_GetAllUserLogins]")
                .Parameters(
                    p => p.Name("userId").Value(userId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<RepositoryKey>(new User_UserLogins_QueryRepository());
        }

        public class RepositoryKey
        {
        }
    }
}
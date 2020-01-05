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
            throw new NotImplementedException();
        }

        public async override Task<(int, IEnumerable<UserLogin>)> GetAsync(int? userId, CollectionQueryParameters queryParameters)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<UserLogin> GetAll(int? userId)
        {
            var result = Query<UserLogin>
                .Collection()
                .Connection(UserWithUserLoginsConnectionClass.GetConnectionName())
                .StoredProcedure("[UserBoundedContext].[pGetAll_UserLogins_For_User]")
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
                .StoredProcedure("[UserBoundedContext].[pGetAll_UserLogins_For_User]")
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
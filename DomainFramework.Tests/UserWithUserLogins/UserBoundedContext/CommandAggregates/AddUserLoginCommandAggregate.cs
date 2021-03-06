using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UserWithUserLogins.UserBoundedContext
{
    public class AddUserLoginCommandAggregate : CommandAggregate<User>
    {
        public AddUserLoginCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(UserWithUserLoginsConnectionClass.GetConnectionName()))
        {
        }

        public AddUserLoginCommandAggregate(UserAddUserLoginInputDto user, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(UserWithUserLoginsConnectionClass.GetConnectionName()))
        {
            Initialize(user, dependencies);
        }

        public override void Initialize(IInputDataTransferObject user, EntityDependency[] dependencies)
        {
            Initialize((UserAddUserLoginInputDto)user, dependencies);
        }

        private void Initialize(UserAddUserLoginInputDto user, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<User>(() => new UserCommandRepository());

            RegisterCommandRepositoryFactory<User_UserLogins_CommandRepository.RepositoryKey>(() => new User_UserLogins_CommandRepository());

            RootEntity = new User
            {
                Id = user.UserId
            };

            foreach (var dto in user.UserLogins)
            {
                var userLoginValueObject = new UserLogin
                {
                    Provider = dto.Provider,
                    UserKey = dto.UserKey
                };

                Enqueue(new AddLinkedValueObjectCommandOperation<User, UserLogin, User_UserLogins_CommandRepository.RepositoryKey>(RootEntity, userLoginValueObject));
            }
        }

    }
}
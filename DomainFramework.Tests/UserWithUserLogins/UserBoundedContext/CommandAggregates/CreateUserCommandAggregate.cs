using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UserWithUserLogins.UserBoundedContext
{
    public class CreateUserCommandAggregate : CommandAggregate<User>
    {
        public CreateUserCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(UserWithUserLoginsConnectionClass.GetConnectionName()))
        {
        }

        public CreateUserCommandAggregate(CreateUserInputDto user, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(UserWithUserLoginsConnectionClass.GetConnectionName()))
        {
            Initialize(user, dependencies);
        }

        public override void Initialize(IInputDataTransferObject user, EntityDependency[] dependencies)
        {
            Initialize((CreateUserInputDto)user, dependencies);
        }

        private void Initialize(CreateUserInputDto user, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<User>(() => new UserCommandRepository());

            RegisterCommandRepositoryFactory<User_UserLogins_CommandRepository.RepositoryKey>(() => new User_UserLogins_CommandRepository());

            RootEntity = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                NormalizedUserName = user.UserName.ToUpperInvariant(),
                NormalizedEmail = user.Email.ToUpperInvariant(),
                UserLogins = user.UserLogins.Select(dto => new UserLogin
                {
                    Provider = dto.Provider,
                    UserKey = dto.UserKey
                }).ToList()
            };

            Enqueue(new InsertEntityCommandOperation<User>(RootEntity));

            foreach (var userLogin in RootEntity.UserLogins)
            {
                Enqueue(new AddLinkedValueObjectCommandOperation<User, UserLogin, User_UserLogins_CommandRepository.RepositoryKey>(RootEntity, () => userLogin));
            }
        }

    }
}
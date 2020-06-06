using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Security;

namespace RegisterUser.UserBoundedContext
{
    public class RegisterUserCommandAggregate : CommandAggregate<User>
    {
        public RegisterUserCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(RegisterUserConnectionClass.GetConnectionName()))
        {
        }

        public RegisterUserCommandAggregate(RegisterUserInputDto user, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(RegisterUserConnectionClass.GetConnectionName()))
        {
            Initialize(user, dependencies);
        }

        public override void Initialize(IInputDataTransferObject user, EntityDependency[] dependencies)
        {
            Initialize((RegisterUserInputDto)user, dependencies);
        }

        private void Initialize(RegisterUserInputDto user, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<User>(() => new UserCommandRepository());

            var salt = Password.SaltInBase64();

            RootEntity = new User
            {
                Username = user.Username,
                Email = user.Email,
                PasswordSalt = salt,
                PasswordHash = Password.ToHashBase64(user.Password, salt)
            };

            Enqueue(new InsertEntityCommandOperation<User>(RootEntity, dependencies));
        }

    }
}
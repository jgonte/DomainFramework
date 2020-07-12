using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterUser.UserBoundedContext
{
    public class GetUserByUserNameQueryAggregate : QueryAggregate<User, UserOutputDto>
    {
        public GetUserByUserNameQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(RegisterUserConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            UserQueryRepository.Register(context);
        }

        public UserOutputDto Get(string username)
        {
            var repository = (UserQueryRepository)RepositoryContext.GetQueryRepository(typeof(User));

            RootEntity = repository.GetUserByUserName(username);

            if (RootEntity == null)
            {
                return null;
            }

            LoadLinks(null);

            PopulateDto();

            return OutputDto;
        }

        public async Task<UserOutputDto> GetAsync(string username)
        {
            var repository = (UserQueryRepository)RepositoryContext.GetQueryRepository(typeof(User));

            RootEntity = await repository.GetUserByUserNameAsync(username);

            if (RootEntity == null)
            {
                return null;
            }

            await LoadLinksAsync(null);

            PopulateDto();

            return OutputDto;
        }

        public override void PopulateDto()
        {
            OutputDto.UserId = RootEntity.Id;

            OutputDto.SubjectId = RootEntity.SubjectId;

            OutputDto.Username = RootEntity.Username;

            OutputDto.PasswordSalt = RootEntity.PasswordSalt;

            OutputDto.PasswordHash = RootEntity.PasswordHash;

            OutputDto.Email = RootEntity.Email;
        }

    }
}
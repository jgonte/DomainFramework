using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserWithUserLogins.UserBoundedContext
{
    public class GetUserByUserLoginQueryAggregate : QueryAggregate<User, UserOutputDto>
    {
        public GetCollectionLinkedValueObjectQueryOperation<User, UserLogin, User_UserLogins_QueryRepository.RepositoryKey> GetUserLoginsOperation { get; private set; }

        public GetUserByUserLoginQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(UserWithUserLoginsConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            UserQueryRepository.Register(context);

            User_UserLogins_QueryRepository.Register(context);

            GetUserLoginsOperation = new GetCollectionLinkedValueObjectQueryOperation<User, UserLogin, User_UserLogins_QueryRepository.RepositoryKey>
            {
                GetLinkedValueObjects = (repository, entity, user) => ((User_UserLogins_QueryRepository)repository).GetAll(RootEntity.Id).ToList(),
                GetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((User_UserLogins_QueryRepository)repository).GetAllAsync(RootEntity.Id);

                    return items.ToList();
                }
            };

            QueryOperations.Enqueue(GetUserLoginsOperation);
        }

        public UserOutputDto Get(string provider, string userKey)
        {
            var repository = (UserQueryRepository)RepositoryContext.GetQueryRepository(typeof(User));

            RootEntity = repository.GetUserByUserLogin(provider, userKey);

            if (RootEntity == null)
            {
                return null;
            }

            LoadLinks(null);

            PopulateDto();

            return OutputDto;
        }

        public async Task<UserOutputDto> GetAsync(string provider, string userKey)
        {
            var repository = (UserQueryRepository)RepositoryContext.GetQueryRepository(typeof(User));

            RootEntity = await repository.GetUserByUserLoginAsync(provider, userKey);

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
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.UserName = RootEntity.UserName;

            OutputDto.Email = RootEntity.Email;

            OutputDto.UserLogins = GetUserLoginsDtos();
        }

        public List<UserLoginOutputDto> GetUserLoginsDtos()
        {
            return GetUserLoginsOperation
                .LinkedValueObjects
                .Select(vo => new UserLoginOutputDto
                {
                    Provider = vo.Provider,
                    UserKey = vo.UserKey
                })
                .ToList();
        }

    }
}
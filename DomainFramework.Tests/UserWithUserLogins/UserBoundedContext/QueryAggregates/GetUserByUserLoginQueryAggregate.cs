using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserWithUserLogins.UserBoundedContext
{
    public class GetUserByUserLoginQueryAggregate : QueryAggregate<User, UserOutputDto>
    {
        public SetCollectionLinkedValueObjectQueryOperation<User, User_UserLogins_QueryRepository.RepositoryKey> UserLogins { get; private set; }

        public GetUserByUserLoginQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(UserWithUserLoginsConnectionClass.GetConnectionName());

            UserQueryRepository.Register(context);

            User_UserLogins_QueryRepository.Register(context);

            RepositoryContext = context;

            UserLogins = new SetCollectionLinkedValueObjectQueryOperation<User, User_UserLogins_QueryRepository.RepositoryKey>
            {
                SetLinkedValueObjects = (repository, entity, user) => entity.UserLogins = ((User_UserLogins_QueryRepository)repository)
                    .GetAll(RootEntity.Id)
                    .ToList(),
                SetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((User_UserLogins_QueryRepository)repository).GetAllAsync(RootEntity.Id);

                    entity.UserLogins = items.ToList();
                }
            };

            QueryOperations.Enqueue(UserLogins);
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

            PopulateDto(RootEntity);

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

            PopulateDto(RootEntity);

            return OutputDto;
        }

        public List<UserLoginOutputDto> GetUserLoginsDtos(User user)
        {
            return user
                .UserLogins
                .Select(vo => new UserLoginOutputDto
                {
                    Provider = vo.Provider,
                    UserKey = vo.UserKey
                })
                .ToList();
        }

        public override void PopulateDto(User entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.UserName = entity.UserName;

            OutputDto.Email = entity.Email;

            OutputDto.UserLogins = GetUserLoginsDtos(entity);
        }

    }
}
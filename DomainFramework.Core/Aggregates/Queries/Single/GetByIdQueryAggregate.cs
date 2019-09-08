using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class GetByIdQueryAggregate<TEntity, TKey, TOutputDto> : QueryAggregate<TEntity, TOutputDto>
        where TEntity : Entity<TKey>
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public GetByIdQueryAggregate()
        {
        }

        public GetByIdQueryAggregate(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public TOutputDto Get(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var repository = (IEntityQueryRepository)RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)repository.GetById(rootEntityId, user);

            if (RootEntity == null) // Not found
            {
                return default(TOutputDto);
            }

            LoadLinks(user);

            PopulateDto(RootEntity);

            return OutputDto;
        }

        public async Task<TOutputDto> GetAsync(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var repository = (IEntityQueryRepository)RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)await repository.GetByIdAsync(rootEntityId, user);

            if (RootEntity == null) // Not found
            {
                return default(TOutputDto);
            }

            await LoadLinksAsync(user);

            PopulateDto(RootEntity);

            return OutputDto;
        }
    }
}

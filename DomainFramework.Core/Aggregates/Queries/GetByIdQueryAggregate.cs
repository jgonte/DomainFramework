using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class GetByIdQueryAggregate<TEntity, TKey, TDto> : QueryAggregate<TEntity, TDto>
        where TEntity : Entity<TKey>
    {
        public GetByIdQueryAggregate()
        {
        }

        public GetByIdQueryAggregate(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public TDto Get(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var repository = (IEntityQueryRepository)RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)repository.GetById(rootEntityId, user);

            if (RootEntity == null) // Not found
            {
                return default(TDto);
            }

            LoadLinks(user);

            return GetDataTransferObject();
        }

        public async Task<TDto> GetAsync(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var repository = (IEntityQueryRepository)RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)await repository.GetByIdAsync(rootEntityId, user);

            if (RootEntity == null) // Not found
            {
                return default(TDto);
            }

            await LoadLinksAsync(user);

            return GetDataTransferObject();
        }
    }
}

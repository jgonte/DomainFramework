using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class GetByIdQueryAggregate<TEntity, TKey, TOutputDto> : QueryAggregate<TEntity, TOutputDto>
        where TEntity : Entity<TKey>
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public HashSet<(string, IEntity)> ProcessedEntities { get; set; }

        public GetByIdQueryAggregate(RepositoryContext repositoryContext, HashSet<(string, IEntity)> processedEntities) : base(repositoryContext)
        {
            ProcessedEntities = processedEntities;

            if (ProcessedEntities == null)
            {
                ProcessedEntities = new HashSet<(string, IEntity)>();
            }
        }

        public TOutputDto Get(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var repository = (IEntityQueryRepository)RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)repository.GetById(rootEntityId);

            if (RootEntity == null) // Not found
            {
                return default(TOutputDto);
            }

            LoadLinks(user);

            PopulateDto();

            return OutputDto;
        }

        public async Task<TOutputDto> GetAsync(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var repository = (IEntityQueryRepository)RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)await repository.GetByIdAsync(rootEntityId);

            if (RootEntity == null) // Not found
            {
                return default(TOutputDto);
            }

            await LoadLinksAsync(user);

            PopulateDto();

            return OutputDto;
        }
    }
}

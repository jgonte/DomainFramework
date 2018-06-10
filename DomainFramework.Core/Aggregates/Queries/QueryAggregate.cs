using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class QueryAggregate<TKey, TEntity> : IQueryAggregate<TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public TEntity RootEntity { get; set; }

        public List<IQueryInheritanceEntityLink<TKey>> InheritanceEntityLinks { get; set; }

        public List<IQuerySingleEntityLink> SingleEntityLinks { get; set; }

        public List<IQueryCollectionEntityLink> CollectionEntityLinks { get; set; }

        public QueryAggregate()
        {
        }

        public QueryAggregate(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public void Load(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var rootRepository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)rootRepository.GetById(rootEntityId);

            if (RootEntity == null)
            {
                return;
            }

            LoadAggregatedEntities(user);
        }

        public async Task LoadAsync(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var rootRepository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)rootRepository.GetById(rootEntityId);

            if (RootEntity == null)
            {
                return;
            }

            await LoadAggregatedEntitiesAsync(user);
        }

        public void LoadAggregatedEntities(IAuthenticatedUser user = null)
        {
            if (InheritanceEntityLinks != null)
            {
                foreach (var link in InheritanceEntityLinks)
                {
                    link.PopulateEntity(RepositoryContext, RootEntity.Id); // The id must be the same for the inheritance chain
                }
            }

            if (SingleEntityLinks != null)
            {
                foreach (var link in SingleEntityLinks)
                {
                    link.PopulateEntity(RepositoryContext, RootEntity);
                }
            }

            if (CollectionEntityLinks != null)
            {
                foreach (var link in CollectionEntityLinks)
                {
                    link.PopulateEntities(RepositoryContext, RootEntity);
                }
            }
        }

        public async Task LoadAggregatedEntitiesAsync(IAuthenticatedUser user = null)
        {
            if (InheritanceEntityLinks != null)
            {
                foreach (var link in InheritanceEntityLinks)
                {
                    await link.PopulateEntityAsync(RepositoryContext, RootEntity.Id); // The id must be the same for the inheritance chain
                }
            }

            if (SingleEntityLinks != null)
            {
                foreach (var link in SingleEntityLinks)
                {
                    await link.PopulateEntityAsync(RepositoryContext, RootEntity);
                }
            }

            if (CollectionEntityLinks != null)
            {
                foreach (var link in CollectionEntityLinks)
                {
                    await link.PopulateEntitiesAsync(RepositoryContext, RootEntity);
                }
            }
        }
    }
}

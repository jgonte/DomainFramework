using System.Collections.Generic;

namespace DomainFramework.Core
{
    public class QueryAggregate<TKey, TEntity> : IQueryAggregate<TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public TEntity RootEntity { get; set; }

        public List<IQueryInheritanceEntityLink> InheritanceEntityLinks { get; set; }

        public List<IQuerySingleEntityLink> SingleEntityLinks { get; set; }

        public List<IQueryCollectionEntityLink> CollectionEntityLinks { get; set; }

        public QueryAggregate(RepositoryContext context, TEntity entity)
        {
            RepositoryContext = context;

            RootEntity = entity;
        }

        public void Load(TKey id, IUnitOfWork unitOfWork = null)
        {
            var rootRepository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)rootRepository.GetById(id);

            if (RootEntity == null)
            {
                return;
            }

            if (SingleEntityLinks != null)
            {
                foreach (var link in SingleEntityLinks)
                {
                    var repository = RepositoryContext.GetQueryRepository(link.LinkedEntityType);

                    link.PopulateEntity(repository, RootEntity);
                }
            }

            if (CollectionEntityLinks != null)
            {
                foreach (var link in CollectionEntityLinks)
                {
                    var repository = RepositoryContext.GetQueryRepository(link.LinkedEntityType);

                    link.PopulateEntities(repository, RootEntity);
                }
            }
        }
    }
}

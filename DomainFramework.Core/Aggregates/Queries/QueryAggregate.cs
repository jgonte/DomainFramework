using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryAggregate<TEntity, TKey, TDto> : IQueryAggregate<TEntity, TKey, TDto>
        where TEntity : Entity<TKey>
        where TDto: class
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public TEntity RootEntity { get; set; }

        public Queue<ILoadOperation> LoadOperations { get; set; } = new Queue<ILoadOperation>();

        public QueryAggregate()
        {
        }

        public QueryAggregate(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public abstract TDto GetDataTransferObject();

        public TDto GetById(TKey rootEntityId, IAuthenticatedUser user)
        {
            Load(rootEntityId, user);

            if (RootEntity == null)
            {
                return null;
            }

            return GetDataTransferObject();
        }

        public async Task<TDto> GetByIdAsync(TKey rootEntityId, IAuthenticatedUser user)
        {
            await LoadAsync(rootEntityId, user);

            if (RootEntity == null)
            {
                return null;
            }

            return GetDataTransferObject();
        }

        public void Load(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var rootRepository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            RootEntity = (TEntity)rootRepository.GetById(rootEntityId, user);

            if (RootEntity == null) // Not found
            {
                return;
            }

            LoadLinks(user);
        }

        public void LoadLinks(IAuthenticatedUser user = null)
        {
            foreach (var operation in LoadOperations)
            {
                operation.Execute(RepositoryContext, RootEntity, user);
            }
        }

        public async Task LoadAsync(TKey rootEntityId, IAuthenticatedUser user = null)
        {
            var rootRepository = RepositoryContext.GetQueryRepository(typeof(TEntity));

            var entity = await rootRepository.GetByIdAsync(rootEntityId, user);

            if (entity == null) // Not found
            {
                return;
            }

            RootEntity = (TEntity)entity;

            await LoadLinksAsync(user);
        }

        public async Task LoadLinksAsync(IAuthenticatedUser user = null)
        {
            var tasks = new Queue<Task>();

            foreach (var operation in LoadOperations)
            {
                tasks.Enqueue(
                    operation.ExecuteAsync(RepositoryContext, RootEntity, user)
                );
            }

            await Task.WhenAll(tasks);
        }
    }
}

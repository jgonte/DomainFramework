using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryAggregate<TEntity, TDto> : IQueryAggregate<TEntity, TDto>
        where TEntity : IEntity
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

        public void LoadLinks(IAuthenticatedUser user = null)
        {
            foreach (var operation in LoadOperations)
            {
                operation.Execute(RepositoryContext, RootEntity, user);
            }
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

        public abstract TDto GetDataTransferObject();
    }
}

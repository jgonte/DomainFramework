using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryAggregate<TEntity, TOutputDto> : IQueryAggregate<TEntity, TOutputDto>
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public TEntity RootEntity { get; set; }

        // It needs to be created here so the nested query aggregates (if any) can use it
        public TOutputDto OutputDto { get; set; } = new TOutputDto();

        public Queue<IQueryOperation> QueryOperations { get; set; } = new Queue<IQueryOperation>();

        public QueryAggregate()
        {
        }

        public QueryAggregate(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public void LoadLinks(IAuthenticatedUser user = null)
        {
            foreach (var operation in QueryOperations)
            {
                operation.Execute(RepositoryContext, RootEntity, user);
            }
        }
    
        public async Task LoadLinksAsync(IAuthenticatedUser user = null)
        {
            var tasks = new Queue<Task>();

            foreach (var operation in QueryOperations)
            {
                tasks.Enqueue(
                    operation.ExecuteAsync(RepositoryContext, RootEntity, user)
                );
            }

            await Task.WhenAll(tasks);
        }

        public abstract void PopulateDto(TEntity entity);
    }
}

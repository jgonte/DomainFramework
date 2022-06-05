using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryAggregate<TEntity, TOutputDto> : IQueryAggregate
        where TEntity : IEntity
        where TOutputDto : IOutputDataTransferObject, new()
    {
        public IRepositoryContext RepositoryContext { get; set; }

        IEntity IAggregate.RootEntity { get; set; }

        public TEntity RootEntity
        {
            get
            {
                return (TEntity)((IAggregate)this).RootEntity;
            }

            set
            {
                ((IAggregate)this).RootEntity = value;
            }
        }

        IOutputDataTransferObject IQueryAggregate.OutputDto { get; set; } = new TOutputDto();

        public TOutputDto OutputDto
        {
            get
            {
                return (TOutputDto)((IQueryAggregate)this).OutputDto;
            }

            set
            {
                ((IQueryAggregate)this).OutputDto = value;
            }
        }

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
            foreach (var operation in QueryOperations)
            {
                await operation.ExecuteAsync(RepositoryContext, RootEntity, user);
            }
        }

        public abstract void PopulateDto();
    }
}

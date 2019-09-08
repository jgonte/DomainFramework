using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class AddLinkedAggregateQueryOperation<TEntity, TLinkedAggregate, TOutputDto> : IQueryOperation
        where TEntity : IEntity
        where TLinkedAggregate : IQueryAggregate<TEntity, TOutputDto>, new()
        where TOutputDto : IOutputDataTransferObject, new()
    {
        private TLinkedAggregate _queryAggregate;

        public Action<TLinkedAggregate, IAuthenticatedUser> Query { get; set; }

        public Func<TLinkedAggregate, IAuthenticatedUser, Task> QueryAsync { get; set; }

        public AddLinkedAggregateQueryOperation()
        {
            _queryAggregate = new TLinkedAggregate();
        }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user) => Query(_queryAggregate, user);

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user) => await QueryAsync(_queryAggregate, user);
    }
}

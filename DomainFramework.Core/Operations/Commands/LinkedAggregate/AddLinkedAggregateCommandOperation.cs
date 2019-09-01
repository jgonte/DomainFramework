using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class AddLinkedAggregateCommandOperation<TEntity, TLinkedAggregate> : CommandOperation<TEntity>
        where TEntity : IEntity
        where TLinkedAggregate : IAggregate
    {
        public AddLinkedAggregateCommandOperation(TEntity entity) : base(entity)
        {
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }

        public override Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }
    }
}

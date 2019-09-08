using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class AddLinkedAggregateCommandOperation<TEntity, TLinkedAggregate, TInputDto> : CommandOperation<TEntity>
        where TEntity : IEntity
        where TLinkedAggregate : ICommandAggregate, new()
        where TInputDto : IInputDataTransferObject
    {
        public TLinkedAggregate CommandAggregate { get; private set; }

        public AddLinkedAggregateCommandOperation(TEntity entity, TInputDto inputDto) : base(entity)
        {
            CommandAggregate = new TLinkedAggregate();

            CommandAggregate.Initialize(inputDto);
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            CommandAggregate.Save(user, unitOfWork);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await CommandAggregate.SaveAsync(user, unitOfWork);
        }
    }
}

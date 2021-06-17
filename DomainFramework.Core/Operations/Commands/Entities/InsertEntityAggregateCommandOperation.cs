using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class InsertEntityAggregateCommandOperation<TEntity, TLinkedAggregate, TInputDto> : ICommandOperation,
        ILinkedAggregateCommandOperation
        where TEntity : IEntity
        where TLinkedAggregate : ICommandAggregate, new()
        where TInputDto : IInputDataTransferObject
    {
        public TLinkedAggregate CommandAggregate { get; private set; }

        ICommandAggregate ILinkedAggregateCommandOperation.CommandAggregate => CommandAggregate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryContext">The repository context of the parent aggregate to keep using the shared data of the commands of the parent</param>
        /// <param name="inputDto"></param>
        /// <param name="dependencies"></param>
        public InsertEntityAggregateCommandOperation(RepositoryContext repositoryContext, TInputDto inputDto, EntityDependency[] dependencies = null)
        {
            CommandAggregate = new TLinkedAggregate();

            CommandAggregate.RepositoryContext = repositoryContext; // Set the parent context

            CommandAggregate.Initialize(inputDto, dependencies);
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            CommandAggregate.Save(user, unitOfWork);

            repositoryContext.Dependencies.Add(new EntityDependency
            {
                Entity = CommandAggregate.RootEntity
            });
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            await CommandAggregate.SaveAsync(user, unitOfWork);

            repositoryContext.Dependencies.Add(new EntityDependency
            {
                Entity = CommandAggregate.RootEntity
            });
        }
    }
}

using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class DeleteEntityCollectionCommandOperation<TEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
    {
        public string Selector { get; private set; }

        public DeleteEntityCollectionCommandOperation(TEntity entity, string selector) : base(entity)
        {
            Selector = selector;
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            repository.DeleteCollection(Entity, user, unitOfWork, Selector);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            await repository.DeleteCollectionAsync(Entity, user, unitOfWork, Selector);
        }
    }
}

using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class DeleteLinksCommandOperation<TEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
    {
        public string Selector { get; private set; }

        public DeleteLinksCommandOperation(TEntity entity, string selector) : base(entity)
        {
            Selector = selector;
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            repository.DeleteLinks(Entity, user, unitOfWork, Selector);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            await repository.DeleteLinksAsync(Entity, user, unitOfWork, Selector);
        }
    }
}

using DomainFramework.DataAccess;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class DeleteLinkedValueObjectCommandOperation<TEntity, TLinkedValueObject, TRepositoryKey> : CommandOperation<TEntity>
        where TEntity : IEntity
        where TLinkedValueObject : IValueObject
    {
        public DeleteLinkedValueObjectCommandOperation(TEntity entity) : base(entity)
        {
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (ILinkedValueObjectCommandRepository)repositoryContext.CreateCommandRepository(typeof(TRepositoryKey));

            repository.Dependencies = () => new EntityDependency[]
            {
                new EntityDependency
                {
                    Entity = Entity,
                    Selector = null
                }
            };

            repository.DeleteLinks(user, unitOfWork);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (ILinkedValueObjectCommandRepository)repositoryContext.CreateCommandRepository(typeof(TRepositoryKey));

            repository.Dependencies = () => new EntityDependency[]
            {
                new EntityDependency
                {
                    Entity = Entity,
                    Selector = null
                }
            };

            await repository.DeleteLinksAsync(user, unitOfWork);
        }
    }
}

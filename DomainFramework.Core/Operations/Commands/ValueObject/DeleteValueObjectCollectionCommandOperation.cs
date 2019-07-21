using DomainFramework.DataAccess;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class DeleteValueObjectCollectionCommandOperation<TEntity, TLinkedValueObject, TRepositoryKey> : CommandOperation<TEntity>
        where TEntity : IEntity
        where TLinkedValueObject : IValueObject
    {
        public DeleteValueObjectCollectionCommandOperation(TEntity entity) : base(entity)
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

            repository.DeleteCollection(user, unitOfWork);
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

            await repository.DeleteCollectionAsync(user, unitOfWork);
        }
    }
}

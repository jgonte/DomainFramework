using DomainFramework.DataAccess;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class AddLinkedValueObjectCommandOperation<TEntity, TLinkedValueObject, TRepositoryKey> :
        CommandOperation<TEntity>
        where TEntity : IEntity
        where TLinkedValueObject : IValueObject
    {
        private TLinkedValueObject _linkedValueObject;

        public AddLinkedValueObjectCommandOperation(TEntity entity, TLinkedValueObject linkedValueObject) : base(entity)
        {
            _linkedValueObject = linkedValueObject;
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            if (_linkedValueObject != null)
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

                repository.Add(_linkedValueObject, user, unitOfWork);
            }
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            if (_linkedValueObject != null)
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

                await repository.AddAsync(_linkedValueObject, user, unitOfWork);
            }
        }
    }
}

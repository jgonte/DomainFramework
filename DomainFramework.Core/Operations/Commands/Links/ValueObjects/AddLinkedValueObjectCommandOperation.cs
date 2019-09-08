using DomainFramework.DataAccess;
using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class AddLinkedValueObjectCommandOperation<TEntity, TLinkedValueObject, TRepositoryKey> :
        CommandOperation<TEntity>
        where TEntity : IEntity
        where TLinkedValueObject : IValueObject
    {
        private Func<TLinkedValueObject> _getLinkedValueObject;

        public AddLinkedValueObjectCommandOperation(TEntity entity, Func<TLinkedValueObject> getLinkedValueObject) : base(entity)
        {
            _getLinkedValueObject = getLinkedValueObject;
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var linkedValueObject = _getLinkedValueObject();

            if (linkedValueObject != null)
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

                repository.Add(linkedValueObject, user, unitOfWork);
            }
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var linkedValueObject = _getLinkedValueObject();

            if (linkedValueObject != null)
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

                await repository.AddAsync(linkedValueObject, user, unitOfWork);
            }
        }
    }
}

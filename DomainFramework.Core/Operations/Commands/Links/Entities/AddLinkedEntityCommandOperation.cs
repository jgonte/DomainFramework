using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class AddLinkedEntityCommandOperation<TEntity, TLinkedEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        private Func<TLinkedEntity> _getLinkedEntity;

        private string _selector;

        public AddLinkedEntityCommandOperation(TEntity entity, Func<TLinkedEntity> getLinkedEntity, string selector = null) : base(entity)
        {
            _getLinkedEntity = getLinkedEntity;

            _selector = selector;
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var linkedEntity = _getLinkedEntity();

            if (linkedEntity == null)
            {
                throw new ArgumentNullException(nameof(linkedEntity));
            }

            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

            repository.Dependencies = () => new EntityDependency[] 
            {
                new EntityDependency
                {
                    Entity = Entity,
                    Selector = _selector
                }
            };

            repository.Insert(linkedEntity, user, unitOfWork, _selector);
        }

        public async override Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var linkedEntity = _getLinkedEntity();

            if (linkedEntity == null)
            {
                throw new ArgumentNullException(nameof(linkedEntity));
            }

            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

            repository.Dependencies = () => new EntityDependency[]
            {
                new EntityDependency
                {
                    Entity = Entity,
                    Selector = _selector
                }
            };

            await repository.InsertAsync(linkedEntity, user, unitOfWork, _selector);
        }
    }
}

using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class AddLinkedEntityCommandOperation<TEntity, TLinkedEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        private Func<TLinkedEntity> _getLinkedEntity;

        public AddLinkedEntityCommandOperation(TEntity entity, Func<TLinkedEntity> getLinkedEntity) : base(entity)
        {
            _getLinkedEntity = getLinkedEntity;
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var linkedEntity = _getLinkedEntity();

            if (linkedEntity == null)
            {
                throw new ArgumentNullException(nameof(linkedEntity));
            }

            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

            repository.Dependencies = () => new IEntity[] { Entity };

            repository.Insert(linkedEntity, user, unitOfWork);
        }

        public async override Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var linkedEntity = _getLinkedEntity();

            if (linkedEntity == null)
            {
                throw new ArgumentNullException(nameof(linkedEntity));
            }

            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

            repository.Dependencies = () => new IEntity[] { Entity };

            await repository.InsertAsync(linkedEntity, user, unitOfWork);
        }
    }
}
